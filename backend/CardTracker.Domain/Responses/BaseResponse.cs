using System.Collections.Generic;
using System.Net;
using CardTracker.Domain.Abstractions;

namespace CardTracker.Domain.Responses;

public class BaseResponse<T> : IBaseResponse<T>
{
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public T Data { get; set; }

    public BaseResponse<T> Success(string message, HttpStatusCode statusCode, T data)
    {
        return new BaseResponse<T> { Message = message, StatusCode = statusCode, Data = data };
    }

    public BaseResponse<T> Error(string message, HttpStatusCode statusCode)
    {
        return new BaseResponse<T> { Message = message, StatusCode = statusCode };
    }
}