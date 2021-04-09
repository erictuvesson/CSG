namespace CSG.Content.STL
{
    using System.Globalization;
    using System.Numerics;
    using System.Text;

    public class StlContentWriter : ContentWriter
    {
        public override byte[] Write(Shape shape)
        {
            var stlShape = StlHelper.ToStlShape(shape);

            var sb = new StringBuilder();

            sb.AppendLine($"solid {stlShape.Name}");

            foreach (var triangle in stlShape.Triangles)
            {
                sb.AppendLine("\tfacet normal " + Ascii_Vector3Format(triangle.Normal));
                sb.AppendLine("\t\touter loop");

                foreach (var position in triangle.Positions)
                {
                    sb.AppendLine("\t\t\tvertex " + Ascii_Vector3Format(position));
                }

                sb.AppendLine("\t\tendloop");
                sb.AppendLine("\tendfacet");
            }

            sb.AppendLine("endsolid");

            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        private static readonly CultureInfo ciEnUS = new CultureInfo("en-us");
        private static string Ascii_Vector3Format(Vector3 value)
        {
            return string.Format(ciEnUS, "{0} {1} {2}", value.X, value.Y, value.Z);
        }
    }
}
