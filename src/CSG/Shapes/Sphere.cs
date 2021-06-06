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

        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere"/> class.
        /// </summary>
        public Sphere()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere"/> class.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="tessellation"></param>
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

        /// <inheritdoc />
        protected override void OnBuild(IShapeBuilder builder)
        {
            Geometry.Sphere.CreateSolid(builder, this.Radius, this.Tessellation);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("position", Position);
            info.AddValue("radius", Radius);
            info.AddValue("tessellation", Tessellation);
        }

        /// <inheritdoc />
        public bool Equals(Sphere other)
        {
            return base.Equals(other as Shape) &&
                   Position == other.Position &&
                   Radius == other.Radius &&
                   Tessellation == other.Tessellation;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals(obj as Sphere);
        }
        
        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Position.GetHashCode() ^
                   Radius.GetHashCode() ^ Tessellation.GetHashCode();
        }
    }
}
