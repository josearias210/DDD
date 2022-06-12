namespace josearias210.DDD.Unit.Tests;

using josearias210.DDD.Results;

public class SuccessResultTTests
{
    [Fact]
    public void SuccessResult()
    {
        // Arrange
        string data = "data";
        Type type = data.GetType();

        // Act
        var result = new SuccessResult<string>(data);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.IsAssignableFrom(type, result.Data);
    }
}