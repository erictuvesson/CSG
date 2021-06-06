namespace CSG.Content.STL
{
    using CSG.Shapes;
    using System.IO;
    using System.Numerics;
    using Xunit;

    public class StlBinaryContentTest
    {
        [Theory]
        [InlineData("./assets/models/3DBenchy.stl", 677118, 677118)]
        [InlineData("./assets/models/shape_cone.stl", 192, 192)]
        [InlineData("./assets/models/shape_cube.stl", 36, 36)]
        [InlineData("./assets/models/shape_cylinder.stl", 384, 384)]
        [InlineData("./assets/models/shape_sphere.stl", 1584, 1584)]
        [InlineData("./assets/models/shape_torus.stl", 6144, 6144)]
        public void Read(string modelPath, int expectedVertices, int expectedIndicies)
        {
            // Arrange
            var reader = new StlBinaryContentReader();
            var file = File.ReadAllBytes(modelPath);
            var stream = new MemoryStream(file);

            // Act
            var result = reader.Read(stream);

            // Assert
            Assert.Null(result.Name);
            Assert.Equal(expectedVertices, result.Vertices.Length);
            Assert.Equal(expectedIndicies, result.Indices.Length);
            Assert.Equal(Vector4.One, result.Color);
        }

        [Theory]
        [InlineData("./assets/models/3DBenchy.stl")]
        [InlineData("./assets/models/shape_cone.stl")]
        [InlineData("./assets/models/shape_cube.stl")]
        [InlineData("./assets/models/shape_cylinder.stl")]
        [InlineData("./assets/models/shape_sphere.stl")]
        [InlineData("./assets/models/shape_torus.stl")]
        public void ReadWrite(string modelPath)
        {
            // Arrange
            var reader = new StlBinaryContentReader();
            var writer = new StlBinaryContentWriter();
            var file = File.ReadAllBytes(modelPath);
            var stream = new MemoryStream(file);

            // Act
            var input = reader.Read(stream);
            var output = writer.Write(input);

            // Assert
            Assert.Equal(file.Length, output.Length);
        }

        [Fact]
        public void Write_Operation_Intersect()
        {
            // Arrange
            var reader = new StlBinaryContentReader();
            var writer = new StlBinaryContentWriter();

            var file = File.ReadAllBytes("./assets/models/shape_operation_intersect.stl");
            var stream = new MemoryStream(file);
            var input = reader.Read(stream);

            // Act
            var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            var shape2 = new Cube(new Vector3(0.8f, 0.8f, 0), new Vector3(1, 1, 1));
            var shape = shape1.Do(ShapeOperation.Intersect, shape2);

            var output = writer.Write(shape);

            // Assert
            Assert.Equal(file.Length, output.Length);
            Assert.Null(input.Name);
            Assert.Equal(input.Vertices.Length, shape.Vertices.Length);
            Assert.Equal(input.Indices.Length, shape.Indices.Length);
        }

        [Fact]
        public void Write_Operation_Subtract()
        {
            // Arrange
            var reader = new StlBinaryContentReader();
            var writer = new StlBinaryContentWriter();

            var file = File.ReadAllBytes("./assets/models/shape_operation_subtract.stl");
            var stream = new MemoryStream(file);
            var input = reader.Read(stream);

            // Act
            var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            var shape2 = new Cube(new Vector3(0.8f, 0.8f, 0), new Vector3(1, 1, 1));
            var shape = shape1.Do(ShapeOperation.Subtract, shape2);

            var output = writer.Write(shape);

            // Assert
            Assert.Equal(file.Length, output.Length);
            Assert.Null(input.Name);
            Assert.Equal(input.Vertices.Length, shape.Vertices.Length);
            Assert.Equal(input.Indices.Length, shape.Indices.Length);
        }

        [Fact]
        public void Write_Operation_Union()
        {
            // Arrange
            var reader = new StlBinaryContentReader();
            var writer = new StlBinaryContentWriter();

            var file = File.ReadAllBytes("./assets/models/shape_operation_union.stl");
            var stream = new MemoryStream(file);
            var input = reader.Read(stream);

            // Act
            var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            var shape2 = new Cube(new Vector3(0.8f, 0.8f, 0), new Vector3(1, 1, 1));
            var shape = shape1.Do(ShapeOperation.Union, shape2);

            var output = writer.Write(shape);

            // Assert
            Assert.Equal(file.Length, output.Length);
            Assert.Null(input.Name);
            Assert.Equal(input.Vertices.Length, shape.Vertices.Length);
            Assert.Equal(input.Indices.Length, shape.Indices.Length);
        }
    }
}
