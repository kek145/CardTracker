using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CardTracker.Domain.Responses.Common;

namespace CardTracker.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        string message;
        HttpStatusCode statusCode;

        var exceptionType = ex.GetType();
        
        if (exceptionType == typeof(NotImplementedException))
        {
            message = $"{ex.Message}";
            statusCode = HttpStatusCode.NotImplemented;
        }
        else if(exceptionType == typeof(UnauthorizedAccessException))
        {
            message = $"{ex.Message}";
            statusCode = HttpStatusCode.Unauthorized;
        }
        else if (exceptionType == typeof(DbUpdateException))
        {
            message = $"{ex.Message}";
            statusCode = HttpStatusCode.InternalServerError;
        }
        else
        {
            message = $"{ex.Message}";
            statusCode = HttpStatusCode.InternalServerError;
        }
        
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var response = new BaseResponse
        {
            StatusCode = statusCode,
            Message = message,
        };

        var exceptionResult = JsonSerializer.Serialize(response, options);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(exceptionResult);
    }
}