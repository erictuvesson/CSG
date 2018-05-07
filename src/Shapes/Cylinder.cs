namespace CSG.Shapes
{
    using System;
    using System.Numerics;

    public class Cylinder : Shape
    {
        public Vector3 Start { get; set; }
        public Vector3 End { get; set; }
        public float RadiusStart { get; set; }
        public float RadiusEnd { get; set; }
        public float SectorAngle { get; set; }
        public int Tessellation { get; set; }

        public Cylinder(Vector3 start, Vector3 end, 
            float radiusStart = 1, float radiusEnd = 1, 
            float sectorAngle = 360, int resolution = 12, 
            int tessellation = 32)
        {
            this.Start = start;
            this.End = end;
            this.RadiusStart = radiusStart;
            this.RadiusEnd = radiusEnd;
            this.SectorAngle = sectorAngle;
            this.Tessellation = tessellation;

            Build();
        }

        protected override void OnBuild()
        {
            AddVertex(Vector3.UnitY * 0.5f, Vector3.UnitY);
            AddVertex(-Vector3.UnitY * 0.5f, -Vector3.UnitY);

            for (int i = 0; i < this.Tessellation; ++i)
            {
                Vector3 normal = GetCircleVector(i, Tessellation);

                AddVertex(normal + 0.5f * Vector3.UnitY, normal);
                AddVertex(normal - 0.5f * Vector3.UnitY, normal);

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
