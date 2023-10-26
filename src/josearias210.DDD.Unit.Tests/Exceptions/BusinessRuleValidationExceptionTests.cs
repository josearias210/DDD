namespace josearias210.DDD.Unit.Tests.Exceptions
{
    using Xunit;
    using josearias210.DDD.Exceptions;

    public class BusinessRuleValidationExceptionTests
    {
        [Fact]
        public void ToStringTests()
        {
            // Arrange
            var mockBusinessRule = new BusinessRuleStub();
            var businessRuleValidation = new BusinessRuleValidationException(mockBusinessRule);

            // Act
            var messageResult = businessRuleValidation.ToString();

            // Assert
            Assert.NotNull(businessRuleValidation);
            Assert.Equal($"{mockBusinessRule.GetType()}: {mockBusinessRule.Message}", messageResult);
        }

        [Fact]
        public void DetailTests()
        {
            // Arrange
            var mockBusinessRule = new BusinessRuleStub();

            // Act
            var businessRuleValidation = new BusinessRuleValidationException(mockBusinessRule);

            // Assert
            Assert.NotNull(businessRuleValidation);
            Assert.Equal(mockBusinessRule.Message, businessRuleValidation.Details);
        }
    }

    class BusinessRuleStub : IBusinessRule
    {
        public string Message => "Message error";
        public bool IsBroken() => true;
    }
}