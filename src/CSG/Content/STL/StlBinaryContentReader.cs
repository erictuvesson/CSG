namespace CSG.Content.STL
{
    using System.Collections.Generic;
    using System.IO;
    using System.Numerics;

    public class StlBinaryContentReader : ContentReader
    {
        public override Shape Read(MemoryStream stream)
        {
            using var reader = new BinaryReader(stream);

            byte[] header = reader.ReadBytes(80);
            uint triangleCount = reader.ReadUInt32();

            var triangles = new List<StlTriangle>();
            for (int i = 0; i < triangleCount; i++)
            {
                var normalX = reader.ReadSingle();
                var normalY = reader.ReadSingle();
                var normalZ = reader.ReadSingle();

                var v1X = reader.ReadSingle();
                var v1Y = reader.ReadSingle();
                var v1Z = reader.ReadSingle();

                var v2X = reader.ReadSingle();
                var v2Y = reader.ReadSingle();
                var v2Z = reader.ReadSingle();

                var v3X = reader.ReadSingle();
                var v3Y = reader.ReadSingle();
                var v3Z = reader.ReadSingle();

                var attribute = reader.ReadUInt16();

                triangles.Add(new StlTriangle(
                    new Vector3(normalX, normalY, normalZ),
                    new Vector3[]
                    {
                        new Vector3(v1X, v1Y, v1Z),
                        new Vector3(v2X, v2Y, v2Z),
                        new Vector3(v3X, v3Y, v3Z)
                    },
                    attribute
                ));
            }

            var stlShape = new StlShape(string.Empty, triangles.ToArray());
            return StlHelper.ToShape(stlShape);
        }
    }
}
