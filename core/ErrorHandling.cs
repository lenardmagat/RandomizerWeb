namespace PracticeWeb.ErrorHandling;
public class Result<T>
{
    public bool IsSuccess {get;}
    public T? Value {get;}
    public string? Error {get;}
    public int StatusCode{get;}
    public Result(T? value, bool isSuccess, string? error, int statusCode){
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }
    public static Result<T> Success(T value) => new(value, true, null, 200);
    public static Result<T> Failure(string error, int statusCode) => new(default, false, error, statusCode);
}