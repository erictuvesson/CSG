namespace CSG
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A <see cref="GeneratedShape"/> is a processed shape built from other <see cref="Shape"/>'s.
    /// </summary>
    public class GeneratedShape : Shape
    {
        /// <summary>
        /// <see cref="Polygon"/>'s that was generated from the other <see cref="Shape"/>'s.
        /// </summary>
        public readonly Polygon[] Polygons;

        /// <summary>
        /// Initialize a new <see cref="GeneratedShape"/>.
        /// </summary>
        /// <param name="polygons"></param>
        public GeneratedShape(IEnumerable<Polygon> polygons)
        {
            this.Polygons = polygons.ToArray();
        }

        public override Polygon[] CreatePolygons() => Polygons;

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
