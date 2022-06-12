namespace josearias210.DDD.Unit.Tests;

using josearias210.DDD.Results;

public class ErrorTests
{
    [Fact]
    public void ErrorWithDetailsTests()
    {
        // Arrange
        string details = "Message error";

        // Act
        var error = new Error(details);

        // Assert
        Assert.NotNull(error);
        Assert.Equal(details, error.Details);
        Assert.Null(error.Code);
    }

    [Fact]
    public void ErrorWithCodeAndDetailsTests()
    {
        // Arrange
        string code = "code";
        string details = "Message error";

        // Act
        var error = new Error(code, details);

        // Assert
        Assert.NotNull(error);
        Assert.Equal(details, error.Details);
        Assert.Equal(code, error.Code);
    }
}