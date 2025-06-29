namespace LifeTrack.Core.Models.Contracts;

public class Result<TResult>
{
    public string ErrorMessage { get; set; }
    public TResult? Data { get; set; }
    public bool IsSuccess { get; set; }

    private Result(TResult? data)
    {
        Data = data;
        IsSuccess = true;
        ErrorMessage = "";
    }

    private Result(string errorMessage)
    {
        ErrorMessage = errorMessage;
        IsSuccess = false;
        Data = default(TResult);
    }

    public static Result<TResult> Success(TResult data) => new(data: data);
    public static Result<TResult> Failure(string errorMessage) => new(errorMessage: errorMessage);
}