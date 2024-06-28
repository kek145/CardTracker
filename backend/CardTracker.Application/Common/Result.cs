namespace CardTracker.Application.Common;

public class Result
{
    public bool IsSuccess { get; private set; }
    public string ErrorMessage { get; private set; } = string.Empty;
    

    public static Result Success()
    {
        return new Result { IsSuccess = true };
    }

    public static Result Failure(string message)
    {
        return new Result { IsSuccess = false, ErrorMessage = message };
    }
}

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public string ErrorMessage { get; private set; } = string.Empty;
    public T Data { get; private set; }

    public static Result<T> Success(T data)
    {
        return new Result<T> { IsSuccess = true, Data = data };
    }

    public static Result<T> Failure(string message)
    {
        return new Result<T> { IsSuccess = false, ErrorMessage = message };
    }
}