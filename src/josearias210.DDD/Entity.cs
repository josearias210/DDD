namespace josearias210.DDD
{
    using josearias210.DDD.Exceptions;

    public abstract class Entity<TId>
    {
        public TId Id { get; set; }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}