namespace CSG.Algorithms
{
    using CSG.Exceptions;
    using System.Collections.Generic;
    using System.Numerics;

    public class Plane
    {
        const float EPSILON = 0.00001f;

        public Vector3 Normal { get; set; }
        public float W { get; set; }

        public Plane()
        {
            this.Normal = Vector3.Zero;
            this.W = 0f;
        }

        public Plane(Vector3 a, Vector3 b, Vector3 c)
        {
            this.Normal = Vector3.Cross(b - a, c - a);
            this.W = Vector3.Dot(this.Normal, a);
        }

        public bool Valid()
        {
            return this.Normal.Magnitude() > 0f;
        }

        public void Flip()
        {
            this.Normal *= -1f;
            this.W *= -1f;
        }

        public void SplitPolygon(Polygon polygon, 
            List<Polygon> coplanarFront, 
            List<Polygon> coplanarBack, 
            List<Polygon> front, 
            List<Polygon> back)
        {
            var (type, types) = PossibleTypes(polygon);
            switch (type)
            {
            default:
            case PolygonType.Coplanar:
                {
                    if (Vector3.Dot(this.Normal, polygon.Plane.Normal) > 0)
                    {
                        coplanarFront.Add(polygon);
                    }
                    else
                    {
                        coplanarBack.Add(polygon);
                    }
                }
                break;

            case PolygonType.Front:
                {
                    front.Add(polygon);
                }
                break;

            case PolygonType.Back:
                {
                    back.Add(polygon);
                }
                break;

            case PolygonType.Spanning:
                {
                    List<Vertex> f = new List<Vertex>();
                    List<Vertex> b = new List<Vertex>();

                    for (int i = 0; i < polygon.Vertices.Count; i++)
                    {
                        int j = (i + 1) % polygon.Vertices.Count;

                        var ti = types[i];
                        var tj = types[j];
                        var vi = polygon.Vertices[i];
                        var vj = polygon.Vertices[j];

                        if (ti != PolygonType.Back)
                        {
                            f.Add(vi);
                        }

                        if (ti != PolygonType.Front)
                        {
                            b.Add(vi);
                        }

                        if ((ti | tj) == PolygonType.Spanning)
                        {
                            var t = (this.W - Vector3.Dot(this.Normal, vi.Position)) / 
                                    Vector3.Dot(this.Normal, vj.Position - vi.Position);

                            var vertex = Helpers.Interpolate(vi, vj, t);
                            f.Add(vertex);
                            b.Add(vertex);
                        }
                    }

                    if (f.Count >= 3) front.Add(new Polygon(f));
                    if (b.Count >= 3) back.Add(new Polygon(b));
                }
                break;
            }
        }
        
        private (PolygonType, PolygonType[]) PossibleTypes(Polygon polygon)
        {
            PolygonType result = 0;
            var types = new List<PolygonType>();
            for (int i = 0; i < polygon.Vertices.Count; i++)
            {
                float t = Vector3.Dot(this.Normal, polygon.Vertices[i].Position) - this.W;
                var type = (t < -EPSILON) ? PolygonType.Back : 
                           ((t > EPSILON) ? PolygonType.Front : PolygonType.Coplanar);
                types.Add(type);
                result |= type;
            }
            return (result, types.ToArray());
        }

        public override string ToString()
        {
            return $"Normal: {Normal}, W: {W}";
        }
    }
}
