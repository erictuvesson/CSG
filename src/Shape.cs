namespace CSG
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract partial class Shape : IEquatable<Shape>
    {
        [DataMember]
        public string Name { get; set; }

        public Vertex[] Vertices => vertices.ToArray();
        public ushort[] Indices => indices.ToArray();

        protected int CurrentVertex => vertices.Count;

        private readonly List<Vertex> vertices = new List<Vertex>();
        private readonly List<ushort> indices = new List<ushort>();

        public void Build()
        {
            this.vertices.Clear();
            this.indices.Clear();

            OnBuild();
        }

        public Polygon[] ToPolygons()
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
            AddVertex(new Vertex()
            {
                Position = position,
                Normal = normal,
                TexCoords = new Vector2(
                    (float)((Math.Asin(normal.X) / Algorithms.Helpers.Pi) + 0.5),
                    (float)((Math.Asin(normal.X) / Algorithms.Helpers.Pi) + 0.5)),
            });
        }

        protected void AddIndex(int index)
        {
            this.indices.Add((ushort)index);
        }

        public void Rotate(Quaternion quaternion)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                var newVertex = vertices[i];
                newVertex.Position = Vector3.Transform(newVertex.Position, quaternion);
                vertices[i] = newVertex;
            }
        }

        public bool Equals(Shape other)
        {
            return Name == other.Name;
        }
    }
}
