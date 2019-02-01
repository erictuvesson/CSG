namespace CSG
{
    using CSG.Algorithms;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public enum PolygonType
    {
        Coplanar,
        Front,
        Back,
        Spanning = Front + Back
    }

    public class Polygon
    {
        public List<Vertex> Vertices;
        public Plane Plane;

        public Polygon(IEnumerable<Vertex> vertices)
        {
            Debug.Assert(vertices.Count() >= 3, "Polygon must have at least 3 vertices.");

            this.Vertices = new List<Vertex>(vertices);
            this.Plane = new Plane(
                    Vertices[0].Position,
                    Vertices[1].Position,
                    Vertices[2].Position
                );
        }

        public void Flip()
        {
            this.Vertices.Reverse();
            this.Vertices.ForEach(vertex => vertex.Flip());
            Plane.Flip();
        }

        public override string ToString() => $"Vertices: {Vertices.Count}, Plane: {Plane}";
    }
}
