namespace CSG
{
    public enum ShapeOperation
    {
        /// <summary>
        /// Operation to get the volume the shapes share.
        /// </summary>
        Intersect,

        /// <summary>
        /// Operation to get the volume subtraction of one object from another.
        /// </summary>
        Subtract,

        /// <summary>
        /// Operation to get the combined volume of shapes.
        /// </summary>
        Union
    }
}
