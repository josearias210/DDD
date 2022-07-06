namespace josearias210.DDD.Results
{
    using System.Collections.Generic;

    public interface IErrorResult
    {
        string Message { get; }
        IReadOnlyCollection<Error> Errors { get; }
    }
}