namespace josearias210.DDD.Unit.Tests
{
    using System;
    using Xunit;
    using DDD.Events;

    public class EntityTests
    {
        [Fact]
        public void PublishAllEventsSuccess()
        {
            // Arrange
            var entity = new EntityStub();

            // Act
            entity.PublishAllEvents();

            // Asserts
            Assert.Collection(entity.DomainEvents,
                e =>
                {
                    Assert.True(e.IsPublished);
                },
                e =>
                {
                    Assert.True(e.IsPublished);
                });
        }
    }

    class EntityStub : Entity<Guid>
    {
        public EntityStub()
        {
            AddDomainEvent(new CreateDomainEvent());
            AddDomainEvent(new UpdateDomainEvent());
        }
    }

    class CreateDomainEvent : DomainEvent
    {
    }

    class UpdateDomainEvent : DomainEvent
    {
    }
}