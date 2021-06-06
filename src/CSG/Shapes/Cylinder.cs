namespace CSG.Shapes
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Numerics;
    using System.Runtime.Serialization;

    [Serializable]
    public class Cylinder : Shape, IEquatable<Cylinder>
    {
        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        public Vector3 Start
        {
            get => this.start;
            set
            {
                if (this.start != value)
                {
                    this.start = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the end position.
        /// </summary>
        public Vector3 End
        {
            get => this.end;
            set
            {
                if (this.end != value)
                {
                    this.end = value;
                    Invalidate();
                }
            }
        }

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

        private Vector3 start;
        private Vector3 end;
        private int tessellation = 32;
        private float radius = 1.0f;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cylinder"/> class.
        /// </summary>
        public Cylinder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cylinder"/> class.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="radius"></param>
        /// <param name="tessellation"></param>
        public Cylinder(Vector3? start = null, Vector3? end = null,
            float radius = 1, int tessellation = 32)
        {
            this.Start = start ?? Vector3.UnitY;
            this.End = end ?? -Vector3.UnitY;
            this.Radius = radius;
            this.Tessellation = tessellation;
        }

        public Cylinder(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Start = (Vector3)info.GetValue("start", typeof(Vector3));
            this.End = (Vector3)info.GetValue("end", typeof(Vector3));
            this.Radius = info.GetSingle("radius");
            this.Tessellation = info.GetInt32("tessellation");
        }

        protected override void OnBuild(IShapeBuilder builder)
        {
            Geometry.Cylinder.CreateSolid(builder, this.Start, this.End, this.Radius, this.tessellation);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("start", Start);
            info.AddValue("end", End);
            info.AddValue("radius", Radius);
            info.AddValue("tessellation", Tessellation);
        }

        public bool Equals(Cylinder other)
        {
            return base.Equals(other as Shape) &&
                   Start == other.Start && End == other.End &&
                   Radius == other.Radius && Tessellation == other.Tessellation;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals(obj as Cylinder);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                   Start.GetHashCode() ^ End.GetHashCode() ^
                   Radius.GetHashCode() ^ Tessellation.GetHashCode();
        }

        private static Vector3 GetCircleVector(int i, int tessellation)
        {
            float angle = i * Algorithms.Helpers.TwoPi / tessellation;

            float dx = (float)Math.Cos(angle);
            float dz = (float)Math.Sin(angle);

            return new Vector3(dx, 0, dz);
        }
    }
}
