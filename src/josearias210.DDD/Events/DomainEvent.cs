namespace josearias210.DDD.Events
{
    using System;

    public abstract class DomainEvent
    {
        public Guid Id { get; }
        public bool IsPublished { get; private set; }

        public DateTime OccurredOn { get; }

        public string Type { get { return this.GetType().Name; } }

        public DomainEvent()
        {
            this.Id = Guid.NewGuid();
            this.OccurredOn = DateTime.UtcNow;
        }

        public void Published()
        {
            this.IsPublished = true;
        }
    }
}