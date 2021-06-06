namespace CSG.Geometry
{
    using System;
    using System.Numerics;

    public static class Cylinder
    {
        public static void CreateSolid(this IGeometryBuilder builder, float radius)
            => CreateSolid(builder, Vector3.UnitY, -Vector3.UnitY, radius, 32);

        public static void CreateSolid(this IGeometryBuilder builder, float radius, int tessellation)
            => CreateSolid(builder, Vector3.UnitY, -Vector3.UnitY, radius, tessellation);

        public static void CreateSolid(this IGeometryBuilder builder, Vector3 start, Vector3 end, float radius, int tessellation)
        {
            builder.AddVertex(start * 0.5f, start);
            builder.AddVertex(-end * 0.5f, -end);

            float diameter = radius / 2.0f;
            for (int i = 0; i < tessellation; ++i)
            {
                float angle = i * Algorithms.Helpers.TwoPi / tessellation;

                float dx = MathF.Cos(angle);
                float dz = MathF.Sin(angle);

                Vector3 normal = new Vector3(dx, 0.0f, dz);

                builder.AddVertex(normal + (diameter * start), normal);
                builder.AddVertex(normal - (diameter * start), normal);

                builder.AddIndex(0);
                builder.AddIndex(2 + (i * 2));
                builder.AddIndex(2 + (((i * 2) + 2) % (tessellation * 2)));

                builder.AddIndex(2 + (i * 2));
                builder.AddIndex(2 + (i * 2) + 1);
                builder.AddIndex(2 + (((i * 2) + 2) % (tessellation * 2)));

                builder.AddIndex(1);
                builder.AddIndex(2 + (((i * 2) + 3) % (tessellation * 2)));
                builder.AddIndex(2 + (i * 2) + 1);

                builder.AddIndex(2 + (i * 2) + 1);
                builder.AddIndex(2 + (((i * 2) + 3) % (tessellation * 2)));
                builder.AddIndex(2 + (((i * 2) + 2) % (tessellation * 2)));
            }
        }
    }
}
