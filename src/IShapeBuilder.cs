namespace CSG
{
    using System.ComponentModel;
    using System.Numerics;

    public interface IShapeBuilder
    {
        int CurrentVertex { get; }

        void AddVertex(Vertex vertex);

        void AddVertex(Vector3 position, Vector3 normal);
        void AddVertex(Vector3 position, Vector3 normal, Vector4 color);
        void AddVertex(Vector3 position, Vector3 normal, Vector2 texCoords, Vector4 color);

        void AddIndex(int index);

        void Clear();

        /// <summary>
        /// Create a <see cref="ShapeCache"/> from the current state.
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        ShapeCache CreateCache();
    }
}
