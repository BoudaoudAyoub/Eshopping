using Eshopping.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Eshopping.API.Configuration.Filters;

public class CustomExceptionFilter(ILogger<CustomExceptionFilter> exceptionFilterLogger) : IExceptionFilter
{
    private readonly ILogger<CustomExceptionFilter> _exceptionFilterLogger = exceptionFilterLogger;

    public void OnException(ExceptionContext context)
    {
        if (context is null || context.ExceptionHandled || context.Exception is NullReferenceException) return;

        ExceptionRoot exceptionRoot = new()
        {
            ExceptionLogTime = DateTime.UtcNow,
            Instance = context.HttpContext.Request.Path,
            ActionName = context.RouteData.Values["action"]?.ToString() ?? string.Empty,
            ControllerName = context.RouteData.Values["controller"]?.ToString() ?? string.Empty
        };

        if (context.Exception is DomainException exception)
        {
            exceptionRoot.Errors = exception.Errors == default! || exception.Errors.Count == 0 ?
            [
                new()
                {
                    StatusCode = exception.StatusCode,
                    ErrorMessages = [ exception.Message ]
                }
            ]
            :
            ExtractExceptionErrors(exception);
        }
        else
        {
            exceptionRoot.Errors =
            [
                new() { StatusCode = 500, ErrorMessages = [ string.Format($"Internal server error : {context.Exception.Message}") ] }
            ];
        }

        context.ExceptionHandled = true;
        context.Result = new ObjectResult(new
        {
            Failures = exceptionRoot
        });

        _exceptionFilterLogger.LogError(new EventId(context.Exception.HResult), string.Format("@exceptionRoot", exceptionRoot));
    }

    private static List<Error> ExtractExceptionErrors(DomainException exception)
    {
        return exception.Errors?.Select(exception => new Error()
        {
            StatusCode = exception.Key,
            ErrorMessages = [.. exception.Value]

        }).ToList()!;
    }
}