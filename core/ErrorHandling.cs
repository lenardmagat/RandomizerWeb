namespace PracticeWeb.ErrorHandling;
public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public int StatusCode { get; }
    protected Result(bool isSuccess, string? error, int statusCode)
    {
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result Success(int statusCode = 200) => new(true, null, statusCode);
    public static Result Failure(string error, int statusCode) => new(false, error, statusCode);
}
public class Result<T> : Result
{

    public T? Value {get;}    
    public Result(T? value, bool isSuccess, string? error, int statusCode) : base(isSuccess, error, statusCode  )
    {
        Value = value;
    }
    public static Result<T> Success(T value) => new(value, true, null, 200);
    public static new Result<T> Failure(string error, int statusCode) => new(default, false, error, statusCode);
}