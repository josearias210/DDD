namespace josearias210.DDD.Results
{
    public class Error
    {
        public Error(string details) : this(null, details)
        {
        }

#if NULLABLE
        public Error(string? code, string? details)
#else
        public Error(string code, string details)
#endif
        {
            Code = code;
            Details = details;
        }

#if NULLABLE
        public string? Code { get; }
        public string? Details { get; }
#else
        public string Code { get; }
        public string Details { get; }
#endif
    }
}