namespace josearias210.DDD.Unit.Tests.Events
{
    using Xunit;
    using System;
    using DDD.Events;

    public class DomainEventTests
    {
        [Fact]
        public void DomainEventCreatedTest()
        {
            // Act
            var domainEvent = new DomainEventStub();

            // Asserts
            Assert.False(domainEvent.IsPublished);
            Assert.NotEqual(default, domainEvent.OccurredOn);
            Assert.Equal(nameof(DomainEventStub), domainEvent.Type);
        }

        [Fact]
        public void DomainEventPublishedTest()
        {
            // Arrange
            var domainEvent = new DomainEventStub();

            // Act
            domainEvent.Published();

            // Asserts
            Assert.True(domainEvent.IsPublished);
        }
    }

    class DomainEventStub : DomainEvent
    {
    }
}
