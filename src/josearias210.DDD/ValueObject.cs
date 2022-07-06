namespace josearias210.DDD
{
    using System.Linq;
    using System.Collections.Generic;
    using josearias210.DDD.Exceptions;

    public abstract class ValueObject
    {
#if NULLABLE
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
#if NULLABLE
            return ReferenceEquals(left, null) || left.Equals(right!);
#else
      return ReferenceEquals(left, null) || left.Equals(right);
#endif
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right) => !(EqualOperator(left, right));

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}