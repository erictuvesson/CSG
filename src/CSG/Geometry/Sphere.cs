namespace CSG.Geometry
{
    using System;
    using System.Numerics;

    public static class Sphere
    {
        public static void CreateSolid(this IGeometryBuilder builder)
            => CreateSolid(builder, 1.0f, 12);

        public static void CreateSolid(this IGeometryBuilder builder, float radius)
            => CreateSolid(builder, radius, 12);

        public static void CreateSolid(this IGeometryBuilder builder, float radius, int tessellation)
        {
            int verticalSegments = tessellation;
            int horizontalSegments = tessellation * 2;

            // Start with a single vertex at the bottom of the sphere.
            builder.AddVertex(Vector3.UnitY, -Vector3.UnitY);

            // Create rings of vertices at progressively higher latitudes.
            for (int i = 0; i < verticalSegments - 1; ++i)
            {
                float latitude = ((i + 1.0f) * Algorithms.Helpers.Pi / verticalSegments) - Algorithms.Helpers.PiOver2;

                float dy = MathF.Sin(latitude);
                float dxz = MathF.Cos(latitude);

                // Create a single ring of vertices at this latitude.
                for (int j = 0; j < horizontalSegments; j++)
                {
                    float longitude = j * Algorithms.Helpers.TwoPi / horizontalSegments;

                    float dx = MathF.Cos(longitude) * dxz;
                    float dz = MathF.Sin(longitude) * dxz;

                    Vector3 normal = new Vector3(dx, dy, dz) * radius;

                    builder.AddVertex(normal, normal);
                }
            }

            // Finish with a single vertex at the top of the sphere.
            builder.AddVertex(Vector3.UnitY, Vector3.UnitY);

            // Create a fan connecting the bottom vertex to the bottom latitude ring.
            for (int i = 0; i < horizontalSegments; ++i)
            {
                builder.AddIndex(0);
                builder.AddIndex(1 + ((i + 1) % horizontalSegments));
                builder.AddIndex(1 + i);
            }

            // Fill the sphere body with triangles joining each pair of latitude rings.
            for (int i = 0; i < verticalSegments - 2; ++i)
            {
                for (int j = 0; j < horizontalSegments; j++)
                {
                    int nextI = i + 1;
                    int nextJ = (j + 1) % horizontalSegments;

                    builder.AddIndex(1 + (i * horizontalSegments) + j);
                    builder.AddIndex(1 + (i * horizontalSegments) + nextJ);
                    builder.AddIndex(1 + (nextI * horizontalSegments) + j);

                    builder.AddIndex(1 + (i * horizontalSegments) + nextJ);
                    builder.AddIndex(1 + (nextI * horizontalSegments) + nextJ);
                    builder.AddIndex(1 + (nextI * horizontalSegments) + j);
                }
            }

            // Create a fan connecting the top vertex to the top latitude ring.
            for (int i = 0; i < horizontalSegments; ++i)
            {
                builder.AddIndex(builder.CurrentVertex - 1);
                builder.AddIndex(builder.CurrentVertex - 2 - ((i + 1) % horizontalSegments));
                builder.AddIndex(builder.CurrentVertex - 2 - i);
            }
        }
    }
}
