namespace CSG
{
    using System.Numerics;

    public struct Vertex
    {
        public const uint SizeInBytes = 48;

        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoords;
        public Vector4 Color;

        public Vertex(Vector3 position, Vector4 color)
            : this(position, Vector3.Zero, Vector2.Zero, color)
        {

        }

        public Vertex(Vector3 position, Vector2 texCoords, Vector4 color)
            : this(position, Vector3.Zero, texCoords, color)
        {

        }

        public Vertex(Vector3 position, Vector3 normal, Vector2 texCoords)
            : this(position, normal, texCoords, Vector4.One)
        {

        }

        public Vertex(Vector3 position, Vector3 normal, Vector2 texCoords, Vector4 color)
        {
            this.Position = position;
            this.Normal = normal;
            this.TexCoords = texCoords;
            this.Color = color;
        }

        /// <summary>
        /// Flip the <see cref="Vertex"/> normal.
        /// </summary>
        public void Flip()
        {
            this.Normal *= -1f;
        }

        public override string ToString()
        {
            return $"Position: {Position}, Normal: {Normal}, TexCoords: {TexCoords}, Color: {Color}";
        }
    }
}
