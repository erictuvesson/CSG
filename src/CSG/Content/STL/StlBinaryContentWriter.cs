namespace CSG.Content.STL
{
    using System.IO;

    public class StlBinaryContentWriter : ContentWriter
    {
        public override byte[] Write(Shape shape)
        {
            var stlShape = StlHelper.ToStlShape(shape);

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            byte[] header = new byte[80];
            uint triangleCount = (uint)stlShape.Triangles.Length;

            writer.Write(header);
            writer.Write(triangleCount);

            foreach (var triangle in stlShape.Triangles)
            {
                writer.Write(triangle.Normal.X);
                writer.Write(triangle.Normal.Y);
                writer.Write(triangle.Normal.Z);

                foreach (var position in triangle.Positions)
                {
                    writer.Write(position.X);
                    writer.Write(position.Y);
                    writer.Write(position.Z);
                }

                writer.Write(triangle.Attribute);
            }

            return stream.ToArray();
        }
    }
}
