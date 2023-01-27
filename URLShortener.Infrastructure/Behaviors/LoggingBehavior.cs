using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace URLShortener.Infrastructure.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling {typeof(TRequest).Name}");
        try
        {
            var response = await next();
            return response;
        }
        catch (System.Exception ex)
        {

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            _logger.LogError(ex, $"Error occurred while handling {typeof(TRequest).Name}");
            throw;
        }
        finally
        {
            _logger.LogDebug($"Handled {typeof(TResponse).Name}");
        }
    }
}