namespace CSG.Algorithms
{
    using Xunit;

    public class HelpersTest
    {
        [Theory]
        // Min
        [InlineData(10, 0, int.MaxValue, 10)]
        [InlineData(10, 100, int.MaxValue, 100)]
        // Max
        [InlineData(5, 0, 10, 5)]
        [InlineData(100, 0, 10, 10)]
        public void Clamp(int value, int min, int max, int expected)
        {
            var result = Helpers.Clamp(value, min, max);
            Assert.Equal(expected, result);
        }
    }
}
