namespace CSG.Shapes
{
    using System;
    using System.ComponentModel;
    using System.Numerics;
    using System.Runtime.Serialization;

    [Serializable]
    public class Cube : Shape, IEquatable<Cube>
    {
        /// <summary>
        /// Gets or sets the cube size.
        /// </summary>
        [Category("Transform")]
        public Vector3 Size
        {
            get => this.size;
            set
            {
                if (this.size != value)
                {
                    this.size = value;
                    Invalidate();
                }
            }
        }
        
        private Vector3 size = Vector3.One;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cube"/> class.
        /// </summary>
        public Cube()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cube"/> class.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public Cube(Vector3? position = null, Vector3? size = null)
        {
            this.Position = position ?? Vector3.Zero;
            this.Size = size ?? Vector3.One;
        }

        public Cube(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Size = (Vector3)info.GetValue("size", typeof(Vector3));
        }

        /// <inheritdoc />
        protected override void OnBuild(IShapeBuilder builder)
        {
            Geometry.Cube.CreateSolid(builder, this.Size);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("size", this.Size);
        }

        /// <inheritdoc />
        public bool Equals(Cube other)
        {
            return base.Equals(other) &&
                   Size == other.Size;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals(obj as Cube);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Size.GetHashCode();
        }
    }
}
