namespace CSG.Shapes
{
    using System.Numerics;
    using Xunit;

    public class SphereTest
    {
        [Fact]
        public void Intersect()
        {
            var shape1 = new Sphere(new Vector3(0, 0, 0));
            var shape2 = new Sphere(new Vector3(1, 1, 0));
            var result = shape1.Intersect(shape2);

            Assert.True(result.Vertices.Length > 0);
            Assert.True(result.Indices.Length > 0);
        }

        [Fact]
        public void Subtract()
        {
            var shape1 = new Sphere(new Vector3(0, 0, 0));
            var shape2 = new Sphere(new Vector3(1, 1, 0));
            var result = shape1.Subtract(shape2);

            Assert.True(result.Vertices.Length > 0);
            Assert.True(result.Indices.Length > 0);
        }

        [Fact]
        public void Union()
        {
            var shape1 = new Sphere(new Vector3(0, 0, 0));
            var shape2 = new Sphere(new Vector3(1, 1, 0));
            var result = shape1.Union(shape2);

            Assert.True(result.Vertices.Length > 0);
            Assert.True(result.Indices.Length > 0);
        }
    }
}
