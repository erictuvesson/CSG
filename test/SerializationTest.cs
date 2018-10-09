namespace CSG
{
    using CSG.Shapes;
    using System.Numerics;
    using Xunit;
    using Xunit.Abstractions;

    public class SerializationTest
    {
        private readonly ITestOutputHelper output;

        public SerializationTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SerializeSimpleCube()
        {
            var cube = new Cube(new Vector3(0, 1, 0), new Vector3(1));
            var content = SerializationHelper.SerializeContent(cube);
            var result = SerializationHelper.DeserializeContent<Cube>(content);

            output.WriteLine(content);
            Assert.Equal(cube, result);
        }
    }
}
