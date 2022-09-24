using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WorkTimer.Blazor.Middleware;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleUnhandledError(context, ex);
        }
    }

    private Task HandleUnhandledError(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unhandled Error.");

        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(string.Empty);
    }

    private Task HandleObjectDisposedError(HttpContext context, ObjectDisposedException exception)
    {
        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

        return Task.CompletedTask;
    }
}
