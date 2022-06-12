namespace josearias210.DDD.Unit.Tests;

using josearias210.DDD.Results;

public class ErrorResultTests
{
    [Fact]
    public void ErrorResultWithMessage()
    {
        // Arrange
        string message = "Message error";

        // Act
        var result = new ErrorResult(message);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal(message, result.Message);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ErrorResultWithMessageAndErrors()
    {
        // Arrange
        string message = "Message error";
        Error error = new("Code1", "Details1");
        List<Error> errors = new()
        {
            error
        };

        // Act
        var result = new ErrorResult(message, errors);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal(message, result.Message);
        Assert.Collection(result.Errors, e =>
        {
            Assert.Equal(error.Code, e.Code);
            Assert.Equal(error.Details, e.Details);
        });
    }
}