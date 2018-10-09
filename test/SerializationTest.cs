namespace CSG
{
    using CSG.Shapes;
    using System.Numerics;
    using Xunit;
    using Xunit.Abstractions;

    public class SerializationTest : Test
    {
        public SerializationTest(ITestOutputHelper output)
            : base(output)
        {

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

        [Fact]
        public void SerializeSimpleGroup()
        {
            var group = new Group(ShapeOperation.Union,
                new Cube(new Vector3(0, 0, 0), new Vector3(1)),
                new Cube(new Vector3(0, 0, 0), new Vector3(1, 0, 0))
            );

            var content = SerializationHelper.SerializeContent(group);
            var result = SerializationHelper.DeserializeContent<Group>(content);

            output.WriteLine(content);
            Assert.Equal(group, result);
        }
    }
}
