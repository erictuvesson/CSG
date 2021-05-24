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
        /// Initializes a new instance of the <see cref="GeneratedShape"/> class.
        /// </summary>
        /// <param name="polygons"></param>
        public GeneratedShape(IEnumerable<Polygon> polygons)
        {
            this.Polygons = polygons.ToArray();
        }

        /// <inheritdoc />
        public override Polygon[] CreatePolygons()
            => this.Polygons;

        /// <inheritdoc />
        protected override void OnBuild(IShapeBuilder builder)
        {
            int p = 0;
            for (int i = 0; i < Polygons.Length; i++)
            {
                var poly = Polygons[i];
                for (int j = 2; j < poly.Vertices.Count; j++)
                {
                    builder.AddVertex(poly.Vertices[0]);
                    builder.AddIndex(p++);

                    builder.AddVertex(poly.Vertices[j - 1]);
                    builder.AddIndex(p++);

                    builder.AddVertex(poly.Vertices[j]);
                    builder.AddIndex(p++);
                }
            }
        }
    }
}
