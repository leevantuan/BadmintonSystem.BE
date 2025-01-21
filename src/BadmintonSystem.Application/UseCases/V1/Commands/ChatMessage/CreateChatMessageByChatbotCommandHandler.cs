using System.Text;
using System.Text.Json;
using BadmintonSystem.Application.DependencyInjection.Options;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.ChatMessage;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace BadmintonSystem.Application.UseCases.V1.Commands.ChatMessage;

public sealed class CreateChatMessageByChatbotCommandHandler
    : ICommandHandler<Command.CreateChatMessageByChatbotCommand, Response.ChatbotResponse>
{
    private readonly ChatbotOption _chatbotOption = new();

    public CreateChatMessageByChatbotCommandHandler(IConfiguration configuration)
    {
        configuration.GetSection(nameof(ChatbotOption)).Bind(_chatbotOption);
    }

    public async Task<Result<Response.ChatbotResponse>> Handle
    (
        Command.CreateChatMessageByChatbotCommand request,
        CancellationToken cancellationToken)
    {
        var httpClient = new HttpClient();

        // Create a request body
        var requestBody = new
        {
            message = request.Data.Content
        };

        string jsonRequestBody = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

        // Send the POST request
        HttpResponseMessage response =
            await httpClient.PostAsync(_chatbotOption.WebhookUrl, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        // Read the response content
        string jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        Response.ChatbotServerResponse[]? responseObjects =
            JsonSerializer.Deserialize<Response.ChatbotServerResponse[]>(jsonResponse);

        if (responseObjects == null || responseObjects.Length == 0)
        {
            throw new ChatMessageException.ChatBotBadRequestException("No response from chatbot rasa");
        }

        string? firstText = null;
        string? firstImage = null;

        foreach (Response.ChatbotServerResponse responseObject in responseObjects)
        {
            if (firstText == null && !string.IsNullOrEmpty(responseObject.text))
            {
                firstText = responseObject.text;
            }

            if (firstImage == null && !string.IsNullOrEmpty(responseObject.image))
            {
                firstImage = responseObject.image;
            }

            if (firstText != null && firstImage != null)
            {
                break;
            }
        }

        var chatbotResponse = new Response.ChatbotResponse
        {
            Id = Guid.NewGuid(),
            Content = firstText!,
            ImageUrl = firstImage,
            IsFromUser = false,
            CreatedDate = DateTime.Now
        };

        return Result.Success(chatbotResponse);
    }
}
