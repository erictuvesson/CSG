namespace CSG.Shapes
{
    using System;
    using System.Numerics;

    // TODO: Center, Radius
    public class Sphere : Shape
    {
        readonly Vector3 XAxis = new Vector3(+1, +0, +0);
        readonly Vector3 YAxis = new Vector3(+0, -1, +0);
        readonly Vector3 ZAxis = new Vector3(+0, +0, +1);

        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        public int Tessellation { get; set; }

        public Sphere(Vector3 center, float radius = 1, int tessellation = 12)
        {
            this.Center = center;
            this.Radius = radius;
            this.Tessellation = tessellation;

            Build();
        }

        protected override void OnBuild()
        {
            int verticalSegments = Tessellation;
            int horizontalSegments = Tessellation * 2;

            // Start with a single vertex at the bottom of the sphere.
            AddVertex(-Vector3.UnitY, -Vector3.UnitY);

            // Create rings of vertices at progressively higher latitudes.
            for (int i = 0; i < verticalSegments - 1; ++i)
            {
                float latitude = ((i + 1) * Algorithms.Helpers.Pi /
                                            verticalSegments) - Algorithms.Helpers.PiOver2;

                float dy = (float)Math.Sin(latitude);
                float dxz = (float)Math.Cos(latitude);

                // Create a single ring of vertices at this latitude.
                for (int j = 0; j < horizontalSegments; j++)
                {
                    float longitude = j * Algorithms.Helpers.TwoPi / horizontalSegments;

                    float dx = (float)Math.Cos(longitude) * dxz;
                    float dz = (float)Math.Sin(longitude) * dxz;

                    Vector3 normal = new Vector3(dx, dy, dz);

                    AddVertex(normal, normal);
                }
            }

            // Finish with a single vertex at the top of the sphere.
            AddVertex(Vector3.UnitY, Vector3.UnitY);

            // Create a fan connecting the bottom vertex to the bottom latitude ring.
            for (int i = 0; i < horizontalSegments; ++i)
            {
                AddIndex(0);
                AddIndex(1 + (i + 1) % horizontalSegments);
                AddIndex(1 + i);
            }

            // Fill the sphere body with triangles joining each pair of latitude rings.
            for (int i = 0; i < verticalSegments - 2; ++i)
            {
                for (int j = 0; j < horizontalSegments; j++)
                {
                    int nextI = i + 1;
                    int nextJ = (j + 1) % horizontalSegments;

                    AddIndex(1 + i * horizontalSegments + j);
                    AddIndex(1 + i * horizontalSegments + nextJ);
                    AddIndex(1 + nextI * horizontalSegments + j);

                    AddIndex(1 + i * horizontalSegments + nextJ);
                    AddIndex(1 + nextI * horizontalSegments + nextJ);
                    AddIndex(1 + nextI * horizontalSegments + j);
                }
            }

            // Create a fan connecting the top vertex to the top latitude ring.
            for (int i = 0; i < horizontalSegments; ++i)
            {
                AddIndex(CurrentVertex - 1);
                AddIndex(CurrentVertex - 2 - (i + 1) % horizontalSegments);
                AddIndex(CurrentVertex - 2 - i);
            }
        }
    }
}
