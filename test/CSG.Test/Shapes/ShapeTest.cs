namespace CSG.Shapes
{
    using System.Numerics;
    using Xunit;
    using Xunit.Abstractions;

    public class ShapeTest : Test
    {
        public ShapeTest(ITestOutputHelper output)
            : base(output)
        {

        }

        /// <summary>
        /// This is the exampel given at https://evanw.github.io/csg.js/.
        /// </summary>
        [Fact]
        public void CsgJsExample1()
        {
            var result = RunMemoryTest("", () =>
            {
                var a = new Cube();
                var b = new Sphere(radius: 1.35f, tessellation: 12);
                var c = new Cylinder(radius: 0.7f, start: new Vector3(-1, 0, 0), end: new Vector3(1, 0, 0));
                var d = new Cylinder(radius: 0.7f, start: new Vector3(0, -1, 0), end: new Vector3(0, 1, 0));
                var e = new Cylinder(radius: 0.7f, start: new Vector3(0, 0, -1), end: new Vector3(0, 0, 1));
                return a.Intersect(b).Subtract(c.Union(d).Union(e));
            });

            Assert.True(result.Cache.Vertices.Length > 0);
            Assert.True(result.Cache.Indices.Length > 0);

            output.WriteLine($"Result Cache: {result.Cache}");
        }

        [Fact]
        public void CsgJsExample2()
        {
            var result = RunMemoryTest("", () =>
            {
                var a = new Cube();
                var b = new Sphere(radius: 1.35f);
                a.Color = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
                b.Color = new Vector4(0.0f, 0.5f, 1.0f, 1.0f);
                return a.Subtract(b);
            });

            Assert.True(result.Cache.Vertices.Length > 0);
            Assert.True(result.Cache.Indices.Length > 0);

            output.WriteLine($"Result Cache: {result.Cache}");
        }
    }
}
