namespace CSG.Geometry
{
    using System;
    using System.Numerics;

    public static class Torus
    {
        public static void CreateSolid(this IGeometryBuilder builder)
            => CreateSolid(builder, 32);

        public static void CreateSolid(this IGeometryBuilder builder, int tessellation)
        {
            // First we loop around the main ring of the torus.
            for (int i = 0; i < tessellation; ++i)
            {
                float outerAngle = i * Algorithms.Helpers.TwoPi / tessellation;

                // Create a transform matrix that will align geometry to
                // slice perpendicularly though the current ring position.
                var transform = Matrix4x4.CreateTranslation(1, 0, 0) *
                                Matrix4x4.CreateRotationY(outerAngle);

                // Now we loop along the other axis, around the side of the tube.
                for (int j = 0; j < tessellation; j++)
                {
                    float innerAngle = j * Algorithms.Helpers.TwoPi / tessellation;

                    float dx = MathF.Cos(innerAngle);
                    float dy = MathF.Sin(innerAngle);

                    // Create a vertex.
                    Vector3 normal = new Vector3(dx, dy, 0);
                    Vector3 position = normal / 2.0f;

                    position = Vector3.Transform(position, transform);
                    normal = Vector3.TransformNormal(normal, transform);

                    builder.AddVertex(position, normal);

                    // And create indices for two triangles.
                    int nextI = (i + 1) % tessellation;
                    int nextJ = (j + 1) % tessellation;

                    builder.AddIndex(i * tessellation + j);
                    builder.AddIndex(i * tessellation + nextJ);
                    builder.AddIndex(nextI * tessellation + j);

                    builder.AddIndex(i * tessellation + nextJ);
                    builder.AddIndex(nextI * tessellation + nextJ);
                    builder.AddIndex(nextI * tessellation + j);
                }
            }
        }
    }
}
