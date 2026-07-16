using System.Diagnostics;
using EmployeeRequestManager.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRequestManager;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            NotFoundRequestException or NotFoundEmployeeException =>
                (StatusCodes.Status404NotFound, "Ресурс не найден"),
            InvalidStatusTransitionException or InvalidStatusException =>
                (StatusCodes.Status400BadRequest, "Некорректный запрос"),
            _ => (StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Unhandled exception");

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };
        problem.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}