namespace CSG.Content.STL
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;

    readonly struct StlTriangle
    {
        public readonly Vector3 Normal;
        public readonly Vector3[] Positions;
        public readonly ushort Attribute;

        public StlTriangle(
            Vector3 normal,
            Vector3[] positions,
            ushort attribute)
        {
            Debug.Assert(positions.Length == 3);

            this.Normal = normal;
            this.Positions = positions;
            this.Attribute = attribute;
        }
    }

    readonly struct StlShape
    {
        public readonly string Name;
        public readonly StlTriangle[] Triangles;

        public StlShape(string name, StlTriangle[] triangles)
        {
            this.Name = name;
            this.Triangles = triangles;
        }
    }

    static class StlHelper
    {
        public static StlShape ToStlShape(Shape shape)
        {
            Debug.Assert((shape.Indices.Length % 3) == 0);

            var triangles = new List<StlTriangle>();
            for (int i = 0; i < shape.Indices.Length; i += 3)
            {
                var i1 = shape.Indices[i + 0];
                var i2 = shape.Indices[i + 1];
                var i3 = shape.Indices[i + 2];

                var v1 = shape.Vertices[i1];
                var v2 = shape.Vertices[i2];
                var v3 = shape.Vertices[i3];

                var normal = (v1.Normal + v2.Normal + v3.Normal) / 3;

                triangles.Add(new StlTriangle(
                    normal,
                    new Vector3[3]
                    {
                        v1.Position,
                        v2.Position,
                        v3.Position
                    },
                    0
                ));
            }

            return new StlShape(shape.Name, triangles.ToArray());
        }

        public static Shape ToShape(StlShape stlShape)
        {
            var polygons = stlShape.Triangles.Select(item => new Polygon(new []
            {
                new Vertex(item.Positions[0], item.Normal, Vector2.Zero, Vector4.One),
                new Vertex(item.Positions[1], item.Normal, Vector2.Zero, Vector4.One),
                new Vertex(item.Positions[2], item.Normal, Vector2.Zero, Vector4.One)
            }));

            return new GeneratedShape(polygons);
        }
    }
}
