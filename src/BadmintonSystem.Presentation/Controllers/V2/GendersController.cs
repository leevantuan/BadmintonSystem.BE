using Asp.Versioning;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V2.Gender;
using BadmintonSystem.Presentation.Abstractions;
using BadmintonSystem.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSystem.Presentation.Controllers.V2;

[ApiVersion(2)]
public class GendersController : ApiController
{
    public GendersController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost(Name = "Create")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] Command.CreateGenderCommand CreateGender)
    {
        // Step 2: If Middleware return next() ==>
        // Using ISend vận chuyển các Request tới Handler " Root to Hadler "
        // Application layer ==> UseCase ================> Application.UseCase
        var result = await Sender.Send(CreateGender);

        // Custom Result Failure
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    // ================================= Test Authorize =====================================
    [HttpGet("GetGenders")]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.GenderResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllGender(string? searchTerm = null,
                                                  string? sortColumn = null,
                                                  string? sortOrder = null,
                                                  string? sortColumnAndOrder = null,
                                                  int pageIndex = 1,
                                                  int pageSize = 10)
    {
        return Ok(await Sender.Send(new Query.GetAllGender(searchTerm, sortColumn, SortOrderExtension.ConvertStringToSortOrder(sortOrder), SortOrderExtension.ConvertStringToSortOrderV2(sortColumnAndOrder), pageIndex, pageSize)));
    }

    [HttpGet("GetGenders-Admin")]
    [BadmintonSystemAuthorizeAttribute("Admin", "Allowed")]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.GenderResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllGenderAdmin(string? searchTerm = null,
                                                  string? sortColumn = null,
                                                  string? sortOrder = null,
                                                  string? sortColumnAndOrder = null,
                                                  int pageIndex = 1,
                                                  int pageSize = 10)
    {
        return Ok(await Sender.Send(new Query.GetAllGender(searchTerm, sortColumn, SortOrderExtension.ConvertStringToSortOrder(sortOrder), SortOrderExtension.ConvertStringToSortOrderV2(sortColumnAndOrder), pageIndex, pageSize)));
    }

    [HttpGet("GetGenders-Guest")]
    [BadmintonSystemAuthorizeAttribute("Guest", "Allowed")]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.GenderResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllGenderGuest(string? searchTerm = null,
                                                  string? sortColumn = null,
                                                  string? sortOrder = null,
                                                  string? sortColumnAndOrder = null,
                                                  int pageIndex = 1,
                                                  int pageSize = 10)
    {
        return Ok(await Sender.Send(new Query.GetAllGender(searchTerm, sortColumn, SortOrderExtension.ConvertStringToSortOrder(sortOrder), SortOrderExtension.ConvertStringToSortOrderV2(sortColumnAndOrder), pageIndex, pageSize)));
    }

    [HttpGet("GetGenders-Manager")]
    [BadmintonSystemAuthorizeAttribute("Manager", "Allowed")]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.GenderResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllGenderManager(string? searchTerm = null,
                                                  string? sortColumn = null,
                                                  string? sortOrder = null,
                                                  string? sortColumnAndOrder = null,
                                                  int pageIndex = 1,
                                                  int pageSize = 10)
    {
        return Ok(await Sender.Send(new Query.GetAllGender(searchTerm, sortColumn, SortOrderExtension.ConvertStringToSortOrder(sortOrder), SortOrderExtension.ConvertStringToSortOrderV2(sortColumnAndOrder), pageIndex, pageSize)));
    }

    // ================================= Test Authorize =====================================
    [HttpGet("{genderId}")]
    [ProducesResponseType(typeof(Result<Response.GenderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdGender(Guid genderId)
    {
        return Ok(await Sender.Send(new Query.GetGenderByIdQuery(genderId)));
    }

    [HttpDelete("{genderId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid genderId)
    {
        var result = await Sender.Send(new Command.DeleteGenderCommand(genderId));
        return Ok(result);
    }

    [HttpPut("{genderId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid genderId, [FromBody] Command.UpdateGenderCommand updateProduct)
    {
        var updateProductCommand = new Command.UpdateGenderCommand(genderId, updateProduct.Name);
        return Ok(await Sender.Send(updateProductCommand));
    }
}
