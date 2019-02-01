namespace CSG
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    class ShapeBuilder : IShapeBuilder
    {
        public readonly List<Vertex> Vertices;
        public readonly List<ushort> Indices;

        public int CurrentVertex => Vertices.Count;

        public ShapeBuilder(int initialVertexCount = 256, int initialIndexCount = 512)
        {
            this.Vertices = new List<Vertex>(initialIndexCount);
            this.Indices = new List<ushort>(initialIndexCount);
        }

        public void AddVertex(Vertex vertex) => Vertices.Add(vertex);
        public void AddVertex(Vector3 position, Vector3 normal) => AddVertex(position, normal, Vector4.One);
        public void AddVertex(Vector3 position, Vector3 normal, Vector4 color) => AddVertex(new Vertex(position, normal, CalculateTexCoordsFromNormal(normal), color));
        public void AddVertex(Vector3 position, Vector3 normal, Vector2 texCoords, Vector4 color) => AddVertex(new Vertex(position, normal, texCoords, color));

        public void AddIndex(int index) => Indices.Add((ushort)index);

        public void Clear()
        {
            Vertices.Clear();
            Indices.Clear();
        }

        public ShapeCache CreateCache() => new ShapeCache(Vertices.ToArray(), Indices.ToArray());

        private static Vector2 CalculateTexCoordsFromNormal(Vector3 normal)
        {
            return new Vector2((float)((Math.Asin(normal.X) / Algorithms.Helpers.Pi) + 0.5),
                               (float)((Math.Asin(normal.X) / Algorithms.Helpers.Pi) + 0.5));
        }
    }
}
