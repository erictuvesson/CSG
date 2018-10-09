namespace CSG
{
    using CSG.Serialization;
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

            var ser = new SerializerStreamXml(null);

            var content = ser.SerializeContent(cube);
            output.WriteLine(content);

            var result = ser.DeserializeContent<Cube>(content);

            Assert.Equal(cube, result);
        }

        [Fact]
        public void SerializeSimpleGroup()
        {
            var group = new Group(ShapeOperation.Union,
                new Cube(new Vector3(0, 0, 0), new Vector3(1)),
                new Cube(new Vector3(0, 0, 0), new Vector3(1, 0, 0))
            );

            var ser = new SerializerStreamXml(null);

            var content = ser.SerializeContent(group);
            output.WriteLine(content);

            var result = ser.DeserializeContent<Group>(content);

            Assert.Equal(group, result);
        }
    }
}
