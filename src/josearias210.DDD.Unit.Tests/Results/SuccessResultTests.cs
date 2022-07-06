namespace josearias210.DDD.Unit.Tests.Results
{
    using Xunit;
    using josearias210.DDD.Results;

    public class SuccessResultTests
    {
        [Fact]
        public void SuccessResult()
        {
            // Act
            var result = new SuccessResult();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}