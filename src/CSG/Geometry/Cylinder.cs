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
            var dir = start - end;
            if (dir == Vector3.Zero)
            {
                dir = new Vector3(1, 0, 0);
            }
            dir = Vector3.Normalize(dir);

            builder.AddVertex(start, dir);
            builder.AddVertex(end, -dir);

            var sTop = 0;
            var sBottom = 1;
            var sEdgeTop = 2;
            var sEdgeBottom = 3;

            var stride = 4;

            int getIndex(int i, int offset)
            {
                return 2 + (((i * stride) + offset) % (tessellation * stride));
            }

            for (int i = 0; i < tessellation; ++i)
            {
                float angle = i * Algorithms.Helpers.TwoPi / tessellation;

                var mat = Matrix4x4.CreateFromAxisAngle(dir, angle);
                var normal = Vector3.Transform(Vector3.UnitZ, mat);
                var rotated = normal * radius;

                builder.AddVertex(rotated + start, dir); // Top surface
                builder.AddVertex(rotated + end, -dir); // Bottom surface
                builder.AddVertex(rotated + start, normal); // Top edge
                builder.AddVertex(rotated + end, normal); // Bottom edge

                // Top face
                builder.AddIndex(0);
                builder.AddIndex(getIndex(i + 1, sTop));
                builder.AddIndex(getIndex(i, sTop));

                // Bottom face
                builder.AddIndex(1);
                builder.AddIndex(getIndex(i, sBottom));
                builder.AddIndex(getIndex(i + 1, sBottom));

                // Side face 1
                builder.AddIndex(getIndex(i, sEdgeTop));
                builder.AddIndex(getIndex(i + 1, sEdgeBottom));
                builder.AddIndex(getIndex(i, sEdgeBottom));

                // Side face 2
                builder.AddIndex(getIndex(i + 1, sEdgeBottom));
                builder.AddIndex(getIndex(i, sEdgeTop));
                builder.AddIndex(getIndex(i + 1, sEdgeTop));
            }
        }
    }
}
