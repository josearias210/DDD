namespace josearias210.DDD.Unit.Tests;

using josearias210.DDD.Exceptions;

public class BusinessRuleValidationExceptionTests
{
    [Fact]
    public void ToStringTests()
    {
        // Arrange
        var mockBusinessRule = new BusinessRuleMock();
        var businessRuleValidation = new BusinessRuleValidationException(mockBusinessRule);

        // Act
        var messageResult = businessRuleValidation.ToString();

        // Assert
        Assert.NotNull(businessRuleValidation);
        Assert.Equal($"{mockBusinessRule.GetType()}: {mockBusinessRule.Message}", messageResult);
    }

    [Fact]
    public void DtailTests()
    {
        // Arrange
        var mockBusinessRule = new BusinessRuleMock();

        // Act
        var businessRuleValidation = new BusinessRuleValidationException(mockBusinessRule);

        // Assert
        Assert.NotNull(businessRuleValidation);
        Assert.Equal(mockBusinessRule.Message, businessRuleValidation.Details);
    }
}

public class BusinessRuleMock : IBusinessRule
{
    public string Message => "Message error";
    public bool IsBroken() => true;
}
