namespace CSG
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    class ShapeBuilder : IShapeBuilder
    {
        private readonly List<Vertex> Vertices;
        private readonly List<uint> Indices;

        /// <inheritdoc />
        public int CurrentVertex => Vertices.Count;

        public ShapeBuilder(int initialVertexCount = 256, int initialIndexCount = 512)
        {
            this.Vertices = new List<Vertex>(initialVertexCount);
            this.Indices = new List<uint>(initialIndexCount);
        }

        /// <inheritdoc />
        public void AddVertex(Vertex vertex)
            => Vertices.Add(vertex);

        /// <inheritdoc />
        public void AddVertex(Vector3 position, Vector3 normal)
            => AddVertex(position, normal, Vector4.One);

        /// <inheritdoc />
        public void AddVertex(Vector3 position, Vector3 normal, Vector4 color)
            => AddVertex(new Vertex(position, normal, CalculateTexCoordsFromNormal(normal), color));

        /// <inheritdoc />
        public void AddVertex(Vector3 position, Vector3 normal, Vector2 texCoords, Vector4 color)
            => AddVertex(new Vertex(position, normal, texCoords, color));

        /// <inheritdoc />
        public void AddIndex(int index)
            => Indices.Add((uint)index);

        /// <inheritdoc />
        public void Clear()
        {
            Vertices.Clear();
            Indices.Clear();
        }

        /// <inheritdoc />
        public ShapeCache CreateCache()
            => new ShapeCache(Vertices.ToArray(), Indices.ToArray());

        /// <inheritdoc />
        public ShapeCache CreateCache(Matrix4x4 transform)
        {
            for (int i = 0; i < this.Vertices.Count; i++)
            {
                var newPosition = Vector3.Transform(this.Vertices[i].Position, transform);
                if (newPosition != this.Vertices[i].Position)
                {
                    this.Vertices[i] = new Vertex(
                        newPosition,
                        this.Vertices[i].Normal,
                        this.Vertices[i].TexCoords,
                        this.Vertices[i].Color
                    );
                }
            }

            return new ShapeCache(Vertices.ToArray(), Indices.ToArray());
        }

        private static Vector2 CalculateTexCoordsFromNormal(Vector3 normal)
        {
            return new Vector2((float)((Math.Asin(normal.X) / Algorithms.Helpers.Pi) + 0.5),
                               (float)((Math.Asin(normal.X) / Algorithms.Helpers.Pi) + 0.5));
        }
    }
}
