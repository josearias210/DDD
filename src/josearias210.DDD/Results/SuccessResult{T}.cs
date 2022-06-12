namespace josearias210.DDD.Results;

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T data) : base(data)
    {
        IsSuccess = true;
    }
}
