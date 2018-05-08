namespace CSG.Shapes
{
    using System;
    using System.Numerics;

    public class Cylinder : Shape
    {
        public Vector3 Start { get; set; }
        public Vector3 End { get; set; }
        public float Radius { get; set; }
        public int Tessellation { get; set; }

        public Cylinder(Vector3? start = null, Vector3? end = null, 
            float radius = 1, int tessellation = 32)
        {
            this.Start = start ?? Vector3.UnitY;
            this.End = end ?? -Vector3.UnitY;
            this.Radius = radius;
            this.Tessellation = tessellation;
        }

        protected override void OnBuild()
        {
            AddVertex(Start * 0.5f, Start);
            AddVertex(-End * 0.5f, -End);

            float diameter = Radius / 2;
            for (int i = 0; i < this.Tessellation; ++i)
            {
                Vector3 normal = GetCircleVector(i, Tessellation);

                AddVertex(normal + diameter * Start, normal);
                AddVertex(normal - diameter * Start, normal);

                AddIndex(0);
                AddIndex(2 + i * 2);
                AddIndex(2 + (i * 2 + 2) % (Tessellation * 2));

                AddIndex(2 + i * 2);
                AddIndex(2 + i * 2 + 1);
                AddIndex(2 + (i * 2 + 2) % (Tessellation * 2));

                AddIndex(1);
                AddIndex(2 + (i * 2 + 3) % (Tessellation * 2));
                AddIndex(2 + i * 2 + 1);

                AddIndex(2 + i * 2 + 1);
                AddIndex(2 + (i * 2 + 3) % (Tessellation * 2));
                AddIndex(2 + (i * 2 + 2) % (Tessellation * 2));
            }
        }

        static Vector3 GetCircleVector(int i, int tessellation)
        {
            float angle = i * Algorithms.Helpers.TwoPi / tessellation;

            float dx = (float)Math.Cos(angle);
            float dz = (float)Math.Sin(angle);

            return new Vector3(dx, 0, dz);
        }
    }
}
