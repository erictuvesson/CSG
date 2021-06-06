namespace CSG.Geometry
{
    using System.Numerics;
    
    public static class Cube
    {
        private static readonly Vector3[] normals = new[]
        {
            new Vector3(+0, +0, +1),
            new Vector3(+0, +0, -1),
            new Vector3(+1, +0, +0),
            new Vector3(-1, +0, +0),
            new Vector3(+0, +1, +0),
            new Vector3(+0, -1, +0),
        };

        public static void CreateSolid(this IGeometryBuilder builder, Vector3 size)
        {
            for (int i = 0; i < normals.Length; i++)
            {
                var normal = normals[i];

                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                builder.AddIndex(builder.CurrentVertex + 0);
                builder.AddIndex(builder.CurrentVertex + 1);
                builder.AddIndex(builder.CurrentVertex + 2);

                builder.AddIndex(builder.CurrentVertex + 0);
                builder.AddIndex(builder.CurrentVertex + 2);
                builder.AddIndex(builder.CurrentVertex + 3);

                builder.AddVertex(((normal - side1 - side2) / 2.0f) * size, normal, Vector2.Zero);
                builder.AddVertex(((normal - side1 + side2) / 2.0f) * size, normal, Vector2.UnitX);
                builder.AddVertex(((normal + side1 + side2) / 2.0f) * size, normal, Vector2.One);
                builder.AddVertex(((normal + side1 - side2) / 2.0f) * size, normal, Vector2.UnitY);
            }
        }
    }
}
