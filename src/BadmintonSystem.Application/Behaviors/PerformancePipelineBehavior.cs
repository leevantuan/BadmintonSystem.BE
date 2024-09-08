﻿using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BadmintonSystem.Application.Behaviors;
public class PerformancePipelineBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;

    public PerformancePipelineBehavior(ILogger<TRequest> logger)
    {
        _timer = new Stopwatch();
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Purpose: Đo thử thực thi trong bao lâu
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        // If bé hơn thì pass
        // Reverse log tên này - bao nhiêu giây - thông tin xử lý là gì
        if (elapsedMilliseconds <= 5000)
            return response;

        var requestName = typeof(TRequest).Name;
        _logger.LogWarning("Long Time Running - Request Details: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
            requestName, elapsedMilliseconds, request);

        return response;
    }
}