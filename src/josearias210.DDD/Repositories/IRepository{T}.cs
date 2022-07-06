namespace josearias210.DDD.Repositories
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
    }
}