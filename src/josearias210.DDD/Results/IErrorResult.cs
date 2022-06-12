namespace josearias210.DDD.Results;

public interface IErrorResult
{
    string Message { get; }
    IReadOnlyCollection<Error> Errors { get; }
}
