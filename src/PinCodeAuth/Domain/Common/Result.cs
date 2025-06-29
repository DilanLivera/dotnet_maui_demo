using System.Diagnostics;

namespace PinCodeAuth.Domain.Common;

public class Result<T>
{
    public T Value { get; }
    public Error Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(T value, Error error, bool isSuccess)
    {
        Value = value;
        Error = error;
        IsSuccess = isSuccess;
    }

    public static Result<T> Success(T value) => new(value, default!, true);
    public static Result<T> Failure(Error error) => new(default!, error, false);

    public Result<TNew> Map<TNew>(Func<T, TNew> mapper)
    {
        Debug.Assert(mapper != null, "Mapper function cannot be null");

        return IsSuccess ? Result<TNew>.Success(mapper(Value)) : Result<TNew>.Failure(Error);
    }

    public Result<TNew> Bind<TNew>(Func<T, Result<TNew>> binder)
    {
        Debug.Assert(binder != null, "Binder function cannot be null");

        return IsSuccess ? binder(Value) : Result<TNew>.Failure(Error);
    }
}

public sealed record Error(string Code, string Description, ErrorType Type)
{
    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);
}

public enum ErrorType
{
    Failure,
    Validation,
    NotFound
}