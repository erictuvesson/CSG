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
        /// Default: 32.
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Cone"/> class.
        /// </summary>
        public Cone()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cone"/> class.
        /// </summary>
        /// <param name="tessellation"></param>
        public Cone(int tessellation)
        {
            this.Tessellation = tessellation;
        }

        public Cone(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Tessellation = info.GetInt32("tessellation");
        }

        /// <inheritdoc />
        protected override void OnBuild(IShapeBuilder builder)
        {
            Geometry.Cone.CreateSolid(builder, this.Tessellation);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("tessellation", Tessellation);
        }

        /// <inheritdoc />
        public bool Equals(Cone other)
        {
            return base.Equals(other as Shape) &&
                   Tessellation == other.Tessellation;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals(obj as Cone);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Tessellation.GetHashCode();
        }

        // <summary>
        /// Helper method computes a point on a circle.
        /// </summary>
        private static Vector3 GetCircleVector(int i, int tessellation)
        {
            float angle = i * Algorithms.Helpers.TwoPi / tessellation;

            float dx = (float)Math.Cos(angle);
            float dz = (float)Math.Sin(angle);

            return new Vector3(dx, 0, dz);
        }
    }
}
