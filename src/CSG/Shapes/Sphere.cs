namespace CSG.Shapes
{
    using System;
    using System.Numerics;
    using System.Runtime.Serialization;

    [DataContract]
    public class Sphere : Shape
    {
        [DataMember]
        public Vector3 Position { get; set; }

        [DataMember]
        public float Radius { get; set; }

        /// <summary>
        /// Gets or sets the tessellation of this primitive.
        /// </summary>
        [DataMember]
        public int Tessellation
        {
            get => tessellation;
            set
            {
                if (value < 3) throw new ArgumentOutOfRangeException(nameof(tessellation));
                tessellation = value;
            }
        }
        private int tessellation;

        public Sphere(Vector3? position = null, float radius = 1, int tessellation = 12)
        {
            this.Position = position ?? Vector3.Zero;
            this.Radius = radius;
            this.Tessellation = tessellation;
        }

        protected override void OnBuild(IShapeBuilder builder)
        {
            int verticalSegments = Tessellation;
            int horizontalSegments = Tessellation * 2;

            // Start with a single vertex at the bottom of the sphere.
            builder.AddVertex(Position - Vector3.UnitY, -Vector3.UnitY);

            // Create rings of vertices at progressively higher latitudes.
            for (int i = 0; i < verticalSegments - 1; ++i)
            {
                float latitude = ((i + 1) * Algorithms.Helpers.Pi
                                            / verticalSegments) - Algorithms.Helpers.PiOver2;

                float dy = (float)Math.Sin(latitude);
                float dxz = (float)Math.Cos(latitude);

                // Create a single ring of vertices at this latitude.
                for (int j = 0; j < horizontalSegments; j++)
                {
                    float longitude = j * Algorithms.Helpers.TwoPi / horizontalSegments;

                    float dx = (float)Math.Cos(longitude) * dxz;
                    float dz = (float)Math.Sin(longitude) * dxz;

                    Vector3 normal = new Vector3(dx, dy, dz) * Radius;

                    builder.AddVertex(Position + normal, normal);
                }
            }

            // Finish with a single vertex at the top of the sphere.
            builder.AddVertex(Position + Vector3.UnitY, Vector3.UnitY);

            // Create a fan connecting the bottom vertex to the bottom latitude ring.
            for (int i = 0; i < horizontalSegments; ++i)
            {
                builder.AddIndex(0);
                builder.AddIndex(1 + ((i + 1) % horizontalSegments));
                builder.AddIndex(1 + i);
            }

            // Fill the sphere body with triangles joining each pair of latitude rings.
            for (int i = 0; i < verticalSegments - 2; ++i)
            {
                for (int j = 0; j < horizontalSegments; j++)
                {
                    int nextI = i + 1;
                    int nextJ = (j + 1) % horizontalSegments;

                    builder.AddIndex(1 + (i * horizontalSegments) + j);
                    builder.AddIndex(1 + (i * horizontalSegments) + nextJ);
                    builder.AddIndex(1 + (nextI * horizontalSegments) + j);

                    builder.AddIndex(1 + (i * horizontalSegments) + nextJ);
                    builder.AddIndex(1 + (nextI * horizontalSegments) + nextJ);
                    builder.AddIndex(1 + (nextI * horizontalSegments) + j);
                }
            }

            // Create a fan connecting the top vertex to the top latitude ring.
            for (int i = 0; i < horizontalSegments; ++i)
            {
                builder.AddIndex(builder.CurrentVertex - 1);
                builder.AddIndex(builder.CurrentVertex - 2 - ((i + 1) % horizontalSegments));
                builder.AddIndex(builder.CurrentVertex - 2 - i);
            }
        }
    }
}
