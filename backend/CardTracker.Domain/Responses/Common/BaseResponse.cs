using System.Net;
using System.Collections.Generic;

namespace CardTracker.Domain.Responses.Common;

public class BaseResponse
{
    public string Message { get; init; } = string.Empty;
    public HttpStatusCode StatusCode { get; init; }
    public List<string> Errors { get; private set; } = [];
    
    public static BaseResponse Success(string message, HttpStatusCode statusCode)
    {
        return new BaseResponse { Message = message, StatusCode = statusCode, };
    }

    public static BaseResponse Failure(string message, HttpStatusCode statusCode, List<string> errors)
    {
        return new BaseResponse { Message = message, StatusCode = statusCode, Errors = errors };
    }
}

public class BaseResponse<T>
{
    public string Message { get; private set; } = string.Empty;
    public HttpStatusCode StatusCode { get; private set; }
    public T? Data { get; private set; }
    public List<string> Errors { get; set; } = [];

    public static BaseResponse<T> Success(string message, HttpStatusCode statusCode, T data)
    {
        return new BaseResponse<T> { Message = message, StatusCode = statusCode, Data = data };
    }

    public static BaseResponse<T> Failure(string message, HttpStatusCode statusCode, List<string> errors)
    {
        return new BaseResponse<T> { Message = message, StatusCode = statusCode, Errors = errors };
    }
}