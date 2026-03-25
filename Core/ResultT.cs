namespace Core;

public sealed class Result<T> : Result
{
    public T? Value { get; }
    public Error? Error { get; }
    public bool IsSuccess => Error == Error.None;

    private Result(T value) : base(true, Error.None)
    {
        Value = value;
        Error = Error.None;
    }

    private Result(Error error) : base(false, error)
    {
        Error = error;
        Value = default!;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);

}
