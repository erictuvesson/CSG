namespace CSG
{
    using System.ComponentModel;
    using System.Numerics;

    public interface IShapeBuilder : IGeometryBuilder
    {
        /// <summary>
        /// Gets or sets the default color.
        /// </summary>
        Vector4 DefaultColor { get; set; }

        /// <summary>
        /// Gets or sets the local position.
        /// </summary>
        Vector3 LocalPosition { get; set; }

        /// <summary>
        /// Gets or sets the local scale.
        /// </summary>
        Vector3 LocalScale { get; set; }

        /// <summary>
        /// Clear the current vertices and indicies.
        /// </summary>
        void Clear();

        /// <summary>
        /// Create a <see cref="ShapeCache"/> from the current state.
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        ShapeCache CreateCache();
    }
}
