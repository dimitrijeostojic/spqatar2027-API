
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SPQatar2027.Middlewares;

public sealed class GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            ProblemDetails problemDetails = new()
            {
                Status = context.Response.StatusCode,
                Type = "Server error",
                Title = "Server error",
                Detail = "An internal server has occured"
            };

            var json = JsonSerializer.Serialize(problemDetails);


            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(json);
        }
    }
}
