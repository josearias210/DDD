namespace josearias210.DDD.Exceptions;

public interface IBusinessRule
{
    bool IsBroken();
    string Message { get; }
}