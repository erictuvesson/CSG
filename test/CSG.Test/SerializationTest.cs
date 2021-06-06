﻿//namespace CSG
//{
//    using CSG.Serialization;
//    using CSG.Shapes;
//    using System.Linq;
//    using System.Numerics;
//    using Xunit;
//    using Xunit.Abstractions;

//    public class SerializationTest : Test
//    {
//        public SerializationTest(ITestOutputHelper output)
//            : base(output)
//        {

//        }

//        [Fact]
//        public void SerializeSimpleCube()
//        {
//            var cube = new Cube(new Vector3(0, 1, 0), new Vector3(1));

//            var serializer = new SerializerStreamXml(null);

//            var content = serializer.SerializeContent(cube);
//            output.WriteLine(content);

//            var result = serializer.DeserializeContent<Cube>(content);

//            Assert.Equal(cube, result);
//        }

//        [Fact]
//        public void SerializeSimpleGroup()
//        {
//            var group = new Group(ShapeOperation.Union,
//                new Cube(new Vector3(0, 0, 0), new Vector3(1)),
//                new Cube(new Vector3(0, 0, 0), new Vector3(1, 0, 0))
//            );

//            var serializer = new SerializerStreamXml();

//            var content = serializer.SerializeContent(group);
//            output.WriteLine(content);

//            var result = serializer.DeserializeContent<Group>(content);

//            Assert.Equal(group, result);
//        }

//        [Fact]
//        public void SerializableTypes()
//        {
//            var shapeTypes = SerializerHelper.GetShapeTypes().Select((type) => type.Name);
//            var shapeTypeNames = string.Join(", ", shapeTypes);
//            output.WriteLine($"Shape Types: {shapeTypeNames}");

//            var serializableTypes = SerializerHelper.GetSerializableTypes().Select((type) => type.Name);
//            var serializableTypeNames = string.Join(", ", serializableTypes);
//            output.WriteLine($"Serializable Types: {serializableTypeNames}");
//        }
//    }
//}
