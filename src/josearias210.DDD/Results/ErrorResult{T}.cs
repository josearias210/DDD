namespace josearias210.DDD.Results
{
    using System;
    using System.Collections.Generic;

    public class ErrorResult<T> : Result<T>, IErrorResult
    {
        public ErrorResult(string message) : this(message, Array.Empty<Error>())
        {
        }

#if NULLABLE
        public ErrorResult(string message, IReadOnlyCollection<Error> errors) : base(default!)
#else
        public ErrorResult(string message, IReadOnlyCollection<Error> errors) : base(default)
#endif

        {
            Message = message;
            IsSuccess = false;
            Errors = errors ?? Array.Empty<Error>();
        }

        public string Message { get; }
        public IReadOnlyCollection<Error> Errors { get; }
    }
}