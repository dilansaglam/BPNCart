using BPNCart.Domain.Responses.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BPNCart.Application.Behaviors;
public class ExceptionHandlingAndLogging<TRequest, TResponse>(ILogger<ExceptionHandlingAndLogging<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : BaseResponse, new()
{
    private readonly ILogger<ExceptionHandlingAndLogging<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Executing {typeof(TRequest).Name}. Request: {JsonSerializer.Serialize(request)}");

        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error executing {typeof(TRequest).Name}. Exception: {ex.InnerException?.Message ?? ex.Message}");

            return new TResponse
            {
                Result = false,
                Message = ex.InnerException?.Message ?? ex.Message
            };

        }
    }
}
