namespace josearias210.DDD.Results
{
    public abstract class Result<T> : Result
    {
        public T Data { get; set; }

        protected Result(T data)
        {
            Data = data;
        }
    }
}