namespace CSG.Shapes
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Numerics;
    using System.Runtime.Serialization;

    [Serializable]
    public class Sphere : Shape, IEquatable<Sphere>
    {
        /// <summary>
        /// Gets or sets the tessellation of this primitive.
        /// </summary>
        /// <remarks>
        /// Minimum value is 3.
        /// </remarks>
        [Category("Shape")]
        [Range(3, int.MaxValue)]
        public int Tessellation
        {
            get => this.tessellation;
            set
            {
                var newValue = Algorithms.Helpers.Clamp(value, 3);
                if (this.tessellation != newValue)
                {
                    this.tessellation = newValue;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        [Category("Shape")]
        [Range(0.1f, float.MaxValue)]
        public float Radius
        {
            get => this.radius;
            set
            {
                if (this.radius != value)
                {
                    this.radius = value;
                    Invalidate();
                }
            }
        }

        private int tessellation = 12;
        private float radius = 1.0f;

        public Sphere(Vector3? position = null, float radius = 1, int tessellation = 12)
        {
            this.Position = position ?? Vector3.Zero;
            this.Radius = radius;
            this.Tessellation = tessellation;
        }

        public Sphere(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Position = (Vector3)info.GetValue("position", typeof(Vector3));
            this.Radius = info.GetInt32("radius");
            this.Tessellation = info.GetInt32("tessellation");
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

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("position", Position);
            info.AddValue("radius", Radius);
            info.AddValue("tessellation", Tessellation);
        }

        public bool Equals(Sphere other)
        {
            return base.Equals(other as Shape) &&
                   Position == other.Position &&
                   Radius == other.Radius &&
                   Tessellation == other.Tessellation;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals(obj as Sphere);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Position.GetHashCode() ^
                   Radius.GetHashCode() ^ Tessellation.GetHashCode();
        }
    }
}
