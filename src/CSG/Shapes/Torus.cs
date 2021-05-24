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
            // First we loop around the main ring of the torus.
            for (int i = 0; i < Tessellation; ++i)
            {
                float outerAngle = i * Algorithms.Helpers.TwoPi / Tessellation;

                // Create a transform matrix that will align geometry to
                // slice perpendicularly though the current ring position.
                var transform = Matrix4x4.CreateTranslation(1, 0, 0) *
                                Matrix4x4.CreateRotationY(outerAngle);

                // Now we loop along the other axis, around the side of the tube.
                for (int j = 0; j < Tessellation; j++)
                {
                    float innerAngle = j * Algorithms.Helpers.TwoPi / Tessellation;

                    var dx = (float)Math.Cos(innerAngle);
                    var dy = (float)Math.Sin(innerAngle);

                    // Create a vertex.
                    var normal = new Vector3(dx, dy, 0);
                    var position = normal / 2;

                    position = Vector3.Transform(position, transform);
                    normal = Vector3.TransformNormal(normal, transform);

                    builder.AddVertex(position, normal);

                    // And create indices for two triangles.
                    int nextI = (i + 1) % Tessellation;
                    int nextJ = (j + 1) % Tessellation;

                    builder.AddIndex(i * Tessellation + j);
                    builder.AddIndex(i * Tessellation + nextJ);
                    builder.AddIndex(nextI * Tessellation + j);

                    builder.AddIndex(i * Tessellation + nextJ);
                    builder.AddIndex(nextI * Tessellation + nextJ);
                    builder.AddIndex(nextI * Tessellation + j);
                }
            }
        }
    }
}
