namespace CSG
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    public abstract partial class Shape
    {
        public Vertex[] Vertices => vertices.ToArray();
        public ushort[] Indices => indices.ToArray();

        protected int CurrentVertex => vertices.Count;

        private List<Vertex> vertices = new List<Vertex>();
        private List<ushort> indices = new List<ushort>();

        public void Build()
        {
            this.vertices.Clear();
            this.indices.Clear();

            OnBuild();
        }

        /// <summary>
        /// Create polygons of the <see cref="Shape"/>.
        /// </summary>
        public virtual Polygon[] CreatePolygons()
        {
            var result = new Polygon[this.indices.Count / 3];
            for (int i = 0, vi = 0; vi < this.indices.Count; i++, vi += 3)
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

        protected abstract void OnBuild();

        protected void AddVertex(Vertex vertex)
        {
            this.vertices.Add(vertex);
        }

        protected void AddVertex(Vector3 position, Vector3 normal)
        {
            var texCoords = new Vector2((float)(Math.Asin(normal.X) / Algorithms.Helpers.Pi + 0.5),
                                        (float)(Math.Asin(normal.X) / Algorithms.Helpers.Pi + 0.5));

            AddVertex(new Vertex(position, normal, texCoords, Vector4.One));
        }

        protected void AddIndex(int index)
        {
            this.indices.Add((ushort)index);
        }

        public void Rotate(Quaternion quaternion)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                var newPosition = Vector3.Transform(vertex.Position, quaternion);
                vertices[i] = new Vertex(newPosition, vertex.Normal, vertex.TexCoords, vertex.Color);
            }
        }
    }
}
