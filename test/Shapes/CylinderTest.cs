namespace CSG.Shapes
{
    using System.Numerics;
    using Xunit;
    using Xunit.Abstractions;

    public class CylinderTest : Test
    {
        public CylinderTest(ITestOutputHelper output)
            : base(output)
        {

        }

        [Theory]
        [InlineData(ShapeOperation.Intersect)]
        [InlineData(ShapeOperation.Subtract)]
        [InlineData(ShapeOperation.Union)]
        public void ShapeOperations(ShapeOperation operation)
        {
            var result = RunMemoryTest("", () =>
            {
                var shape1 = new Cylinder(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
                var shape2 = new Cylinder(new Vector3(1, 1, 0), new Vector3(0, 1, 0));
                return shape1.Do(operation, shape2);
            });

            Assert.True(result.Cache.Vertices.Length > 0);
            Assert.True(result.Cache.Indices.Length > 0);

            output.WriteLine($"Result Cache: {result.Cache.ToString()}");
        }

        [Theory]
        [InlineData(ShapeOperation.Intersect)]
        [InlineData(ShapeOperation.Subtract)]
        [InlineData(ShapeOperation.Union)]
        public void ShapeOperationsOverlapping(ShapeOperation operation)
        {
            var result = RunMemoryTest("", () =>
            {
                var shape1 = new Cylinder(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
                var shape2 = new Cylinder(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
                return shape1.Do(operation, shape2);
            });

            Assert.True(result.Cache.Vertices.Length > 0);
            Assert.True(result.Cache.Indices.Length > 0);

            output.WriteLine($"Result Cache: {result.Cache.ToString()}");
        }
    }
}

