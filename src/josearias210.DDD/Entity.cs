namespace josearias210.DDD
{
    using System.Collections.Generic;
    using josearias210.DDD.Events;
    using josearias210.DDD.Exceptions;

    public abstract class Entity<TId>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TId Id { get; protected set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        private readonly List<DomainEvent> domainEvents = new List<DomainEvent>();
        public IReadOnlyCollection<DomainEvent> DomainEvents => domainEvents.AsReadOnly();

        public void PublishAllEvents()
        {
            foreach (DomainEvent @event in DomainEvents)
            {
                @event.Published();
            }
        }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }

        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            this.domainEvents.Add(domainEvent);
        }
    }
}