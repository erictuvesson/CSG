namespace CSG
{
    using System.Collections.Generic;
    using System.Linq;

    class GeneratedShape : Shape
    {
        public readonly Polygon[] Polygons;

        public GeneratedShape(IEnumerable<Polygon> polygons)
        {
            this.Polygons = polygons.ToArray();

            Build();
        }

        protected override void OnBuild()
        {
            int p = 0;
            for (int i = 0; i < Polygons.Length; i++)
            {
                var poly = Polygons[i];
                for (int j = 2; j < poly.Vertices.Count; j++)
                {
                    AddVertex(poly.Vertices[0]);
                    AddIndex(p++);

                    AddVertex(poly.Vertices[j - 1]);
                    AddIndex(p++);

                    AddVertex(poly.Vertices[j]);
                    AddIndex(p++);
                }
            }
        }
    }
}
