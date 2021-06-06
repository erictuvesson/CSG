namespace CSG.Geometry
{
    using System;
    using System.Numerics;

    public static class Cone
    {
        public static void CreateSolid(this IGeometryBuilder builder)
            => CreateSolid(builder, 32);

        public static void CreateSolid(this IGeometryBuilder builder, int tessellation)
        {
            Vector3 offset = new Vector3(0, -0.25f, 0);

            builder.AddVertex(Vector3.UnitY + offset, Vector3.UnitY);
            builder.AddVertex(Vector3.Zero + offset, -Vector3.UnitY);

            for (int i = 0; i < tessellation; ++i)
            {
                float angle = i * Algorithms.Helpers.TwoPi / tessellation;

                float dx = MathF.Cos(angle);
                float dz = MathF.Sin(angle);

                Vector3 normal = new Vector3(dx, 0, dz);

                builder.AddVertex(normal + offset, normal);

                builder.AddIndex(0);
                builder.AddIndex(2 + i);
                builder.AddIndex(2 + (i + 1) % tessellation);

                builder.AddIndex(1);
                builder.AddIndex(2 + (i + 1) % tessellation);
                builder.AddIndex(2 + i);
            }
        }
    }
}
