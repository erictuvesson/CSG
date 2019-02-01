namespace CSG.Shapes
{
    using System;
    using System.Numerics;
    using System.Runtime.Serialization;

    [DataContract]

    public class Cone : Shape
    {
        [DataMember]
        public int Tessellation { get; set; }

        public Cone(int tessellation = 32)
        {
            this.Tessellation = tessellation;
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
