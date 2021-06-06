namespace CSG
{
    using System.Numerics;

    public interface IGeometryBuilder
    {
        /// <summary>
        /// Gets the current vertex index.
        /// </summary>
        int CurrentVertex { get; }

        /// <summary>
        /// Add a new index.
        /// </summary>
        /// <param name="index">The index.</param>
        void AddIndex(int index);

        /// <summary>
        /// Add a new vertex.
        /// </summary>
        /// <param name="vertex"></param>
        void AddVertex(Vertex vertex);

        /// <summary>
        /// Add a new vertex with position and normal.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        /// <param name="normal">The vertex normal.</param>
        void AddVertex(Vector3 position, Vector3 normal);

        /// <summary>
        /// Add a new vertex with position, normal and color.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        /// <param name="normal">The vertex normal.</param>
        /// <param name="color">The vertex color.</param>
        void AddVertex(Vector3 position, Vector3 normal, Vector4 color);

        /// <summary>
        /// Add a new vertex with position, normal, texture coordinate.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        /// <param name="normal">The vertex normal.</param>
        /// <param name="texCoords">The vertex texture coordinate.</param>
        void AddVertex(Vector3 position, Vector3 normal, Vector2 texCoords);

        /// <summary>
        /// Add a new vertex with position, normal, texture coordinate and color.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        /// <param name="normal">The vertex normal.</param>
        /// <param name="texCoords">The vertex texture coordinate.</param>
        /// <param name="color">The vertex color.</param>
        void AddVertex(Vector3 position, Vector3 normal, Vector2 texCoords, Vector4 color);
    }
}
