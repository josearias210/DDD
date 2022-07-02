namespace josearias210.DDD.Unit.Tests;

using josearias210.DDD.Results;

public class ErrorResultTTests
{
    [Fact]
    public void ErrorResult()
    {
        // Arrange
        string message = "error message";

        // Act
        var result = new ErrorResult<string>(message);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Errors);
        Assert.False(result.IsSuccess);
        Assert.Equal(message, result.Message);
    }

    [Fact]
    public void ErrorResultWithErrors()
    {
        // Arrange
        string code = "code";
        string error = "error";
        string message = "message";

        var errors = new List<Error>()
        {
            new Error(code, error)
        };

        // Act
        var result = new ErrorResult<string>(message, errors);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal(message, result.Message);
        Assert.Collection(result.Errors, c =>
        {
            Assert.Equal(code, c.Code);
            Assert.Equal(error, c.Details);
        });
    }
}