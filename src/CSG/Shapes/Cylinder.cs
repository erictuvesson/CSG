namespace CSG.Shapes
{
    using System;
    using System.Numerics;
    using System.Runtime.Serialization;

    [Serializable]
    public class Cylinder : Shape
    {
        public Vector3 Start { get; set; }

        public Vector3 End { get; set; }
        
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

        public float Radius { get; set; }

        public Cylinder(Vector3? start = null, Vector3? end = null,
            float radius = 1, int tessellation = 32)
        {
            this.Start = start ?? Vector3.UnitY;
            this.End = end ?? -Vector3.UnitY;
            this.Radius = radius;
            this.Tessellation = tessellation;
        }
        
        protected override void OnBuild(IShapeBuilder builder)
        {
            builder.AddVertex(Start * 0.5f, Start);
            builder.AddVertex(-End * 0.5f, -End);

            float diameter = Radius / 2;
            for (int i = 0; i < this.Tessellation; ++i)
            {
                Vector3 normal = GetCircleVector(i, Tessellation);

                builder.AddVertex(normal + (diameter * Start), normal);
                builder.AddVertex(normal - (diameter * Start), normal);

                builder.AddIndex(0);
                builder.AddIndex(2 + (i * 2));
                builder.AddIndex(2 + (((i * 2) + 2) % (Tessellation * 2)));

                builder.AddIndex(2 + (i * 2));
                builder.AddIndex(2 + (i * 2) + 1);
                builder.AddIndex(2 + (((i * 2) + 2) % (Tessellation * 2)));

                builder.AddIndex(1);
                builder.AddIndex(2 + (((i * 2) + 3) % (Tessellation * 2)));
                builder.AddIndex(2 + (i * 2) + 1);

                builder.AddIndex(2 + (i * 2) + 1);
                builder.AddIndex(2 + (((i * 2) + 3) % (Tessellation * 2)));
                builder.AddIndex(2 + (((i * 2) + 2) % (Tessellation * 2)));
            }
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
