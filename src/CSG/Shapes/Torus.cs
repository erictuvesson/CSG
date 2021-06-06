namespace CSG.Shapes
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Numerics;
    using System.Runtime.Serialization;

    [Serializable]
    public class Torus : Shape, IEquatable<Torus>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Torus"/> class.
        /// </summary>
        public Torus()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Torus"/> class.
        /// </summary>
        /// <param name="tessellation"></param>
        public Torus(int tessellation)
        {
            this.Tessellation = tessellation;
        }

        public Torus(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Tessellation = info.GetInt32("tessellation");
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("tessellation", Tessellation);
        }

        /// <inheritdoc />
        public bool Equals(Torus other)
        {
            return base.Equals(other as Shape) &&
                   Tessellation == other.Tessellation;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals(obj as Torus);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Tessellation.GetHashCode();
        }

        /// <inheritdoc />
        protected override void OnBuild(IShapeBuilder builder)
        {
            Geometry.Torus.CreateSolid(builder, this.Tessellation);
        }
    }
}
