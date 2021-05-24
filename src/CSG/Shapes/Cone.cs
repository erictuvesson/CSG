namespace CSG.Shapes
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Numerics;
    using System.Runtime.Serialization;

    [Serializable]
    public class Cone : Shape, IEquatable<Cone>
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

        private int tessellation = 32;

        public Cone(int tessellation = 32)
        {
            this.Tessellation = tessellation;
        }

        public Cone(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Tessellation = info.GetInt32("tessellation");
        }

        protected override void OnBuild(IShapeBuilder builder)
        {
            var offset = new Vector3(0, -0.25f, 0);

            builder.AddVertex(Vector3.UnitY + offset, Vector3.UnitY);
            builder.AddVertex(Vector3.Zero + offset, -Vector3.UnitY);

            for (int i = 0; i < Tessellation; ++i)
            {
                Vector3 normal = GetCircleVector(i, Tessellation);

                builder.AddVertex(normal + offset, normal);

                builder.AddIndex(0);
                builder.AddIndex(2 + i);
                builder.AddIndex(2 + (i + 1) % Tessellation);

                builder.AddIndex(1);
                builder.AddIndex(2 + (i + 1) % Tessellation);
                builder.AddIndex(2 + i);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("tessellation", Tessellation);
        }

        public bool Equals(Cone other)
        {
            return base.Equals(other as Shape) &&
                   Tessellation == other.Tessellation;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals(obj as Cone);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Tessellation.GetHashCode();
        }

        // <summary>
        /// Helper method computes a point on a circle.
        /// </summary>
        static Vector3 GetCircleVector(int i, int tessellation)
        {
            float angle = i * Algorithms.Helpers.TwoPi / tessellation;

            float dx = (float)Math.Cos(angle);
            float dz = (float)Math.Sin(angle);

            return new Vector3(dx, 0, dz);
        }
    }
}
