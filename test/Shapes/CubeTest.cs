namespace CSG.Shapes
{
    using System.Numerics;
    using Xunit;

    public class CubeTest
    {
        [Fact]
        public void Intersect()
        {
            var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(0.50f, 0.50f, 0.50f));
            var shape2 = new Cube(new Vector3(1, 1, 0), new Vector3(0.50f, 0.50f, 0.50f));
            var result = shape1.Intersect(shape2);

            Assert.True(result.Vertices.Length > 0);
            Assert.True(result.Indices.Length > 0);
        }

        [Fact]
        public void Subtract()
        {
            var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(0.50f, 0.50f, 0.50f));
            var shape2 = new Cube(new Vector3(1, 1, 0), new Vector3(0.50f, 0.50f, 0.50f));
            var result = shape1.Subtract(shape2);

            Assert.True(result.Vertices.Length > 0);
            Assert.True(result.Indices.Length > 0);
        }

        [Fact]
        public void Union()
        {
            var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(0.50f, 0.50f, 0.50f));
            var shape2 = new Cube(new Vector3(1, 1, 0), new Vector3(0.50f, 0.50f, 0.50f));
            var result = shape1.Union(shape2);

            Assert.True(result.Vertices.Length > 0);
            Assert.True(result.Indices.Length > 0);
        }

        [Fact]
        public void Intersect_PolygonOverlapping()
        {
            var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(1.00f, 0.50f, 1.00f));
            var shape2 = new Cube(new Vector3(0, 0, 0), new Vector3(0.50f, 0.50f, 0.50f));
            var result = shape1.Intersect(shape2);

            Assert.True(result.Vertices.Length > 0);
            Assert.True(result.Indices.Length > 0);
        }

        [Fact]
        public void Subtract_PolygonOverlapping()
        {
            var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(1.00f, 0.50f, 1.00f));
            var shape2 = new Cube(new Vector3(0, 0, 0), new Vector3(0.50f, 0.50f, 0.50f));
            var result = shape1.Subtract(shape2);

            Assert.True(result.Vertices.Length > 0);
            Assert.True(result.Indices.Length > 0);
        }

        [Fact]
        public void Union_PolygonOverlapping()
        {
            var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(1.00f, 0.50f, 1.00f));
            var shape2 = new Cube(new Vector3(0, 0, 0), new Vector3(0.50f, 0.50f, 0.50f));
            var result = shape1.Union(shape2);

            Assert.True(result.Vertices.Length > 0);
            Assert.True(result.Indices.Length > 0);
        }
    }
}
