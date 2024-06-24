using System.Net;

namespace CardTracker.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public T Data { get; set; }

    public static Result<T> SuccessResult(T data)
    {
        return new Result<T> { IsSuccess = true, Data = data };
    }

    public static Result<T> ErrorResult(string message)
    {
        return new Result<T> { IsSuccess = false, ErrorMessage = message };
    }
}