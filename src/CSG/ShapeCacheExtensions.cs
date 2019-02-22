namespace CSG
{
    using System;
    using System.ComponentModel;
    using System.Numerics;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class ShapeCacheExtensions
    {
        public static ShapeCache Rotate(this ShapeCache shape, Quaternion quaternion)
        {
            throw new NotImplementedException();

            // for (int i = 0; i < vertices.Count; i++)
            // {
            //     var vertex = vertices[i];
            //     var newPosition = Vector3.Transform(vertex.Position, quaternion);
            //     vertices[i] = new Vertex(newPosition, vertex.Normal, vertex.TexCoords, vertex.Color);
            // }
        }

        public static ShapeCache Translate(this ShapeCache shape, Vector3 position)
        {
            throw new NotImplementedException();
        }

        public static ShapeCache Transform(this ShapeCache shape, Matrix4x4 transform)
        {
            throw new NotImplementedException();
        }
    }
}
