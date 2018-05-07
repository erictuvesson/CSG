namespace CSG
{
    /// <summary>
    /// Stores all the vertices and indicies that are generated from a <see cref="Shape"/>.
    /// </summary>
    public struct ShapeCache
    {
        public readonly Vertex[] Vertices;
        public readonly ushort[] Indices;

        public ShapeCache(Vertex[] vertices, ushort[] indices)
        {
            this.Vertices = vertices;
            this.Indices = indices;
        }

        public Polygon[] CreatePolygons()
        {
            var result = new Polygon[Indices.Length / 3];
            for (int i = 0, vi = 0; vi < Indices.Length; i++, vi += 3)
            {
                result[i] = new Polygon(new[]
                {
                    Vertices[Indices[vi+0]],
                    Vertices[Indices[vi+1]],
                    Vertices[Indices[vi+2]]
                });
            }
            return result;
        }

        public override string ToString() => $"Vertices: {Vertices.Length}, Indices: {Indices.Length}";
    }
}
