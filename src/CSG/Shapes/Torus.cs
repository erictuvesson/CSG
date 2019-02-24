namespace CSG.Shapes
{
    using System;
    using System.Numerics;
    using System.Runtime.Serialization;

    [Serializable]
    public class Torus : Shape, IEquatable<Torus>
    {
        /// <summary>
        /// Gets or sets the tessellation of this primitive.
        /// </summary>
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

        public Torus(int tessellation = 32)
        {
            this.Tessellation = tessellation;
        }

        public Torus(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Tessellation = info.GetInt32("tessellation");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("tessellation", Tessellation);
        }

        public bool Equals(Torus other)
        {
            return base.Equals(other as Shape) && 
                   Tessellation == other.Tessellation;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals(obj as Torus);
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Tessellation.GetHashCode();
        }

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
