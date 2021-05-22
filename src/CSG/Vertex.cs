namespace CSG
{
    using System;
    using System.Numerics;

    /// <summary>
    /// Vertex with Position, Normal, Texture Coordinates and Color. 
    /// </summary>
    public struct Vertex : IEquatable<Vertex>
    {
        public static readonly uint SizeInBytes = 48;

        public static readonly int PositionOffset = 0;
        public static readonly int NormalOffset = 12;
        public static readonly int TexCoordsOffset = 24;
        public static readonly int ColorOffset = 32;

        public readonly Vector3 Position;
        public readonly Vector3 Normal;
        public readonly Vector2 TexCoords;
        public readonly Vector4 Color;

        public Vertex(Vector3 position, Vector4 color)
            : this(position, Vector3.Zero, Vector2.Zero, color)
        {
        }

        public Vertex(Vector3 position, Vector2 texCoords, Vector4 color)
            : this(position, Vector3.Zero, texCoords, color)
        {
        }

        public Vertex(Vector3 position, Vector3 normal, Vector2 texCoords, Vector4 color)
        {
            this.Position = position;
            this.Normal = normal;
            this.TexCoords = texCoords;
            this.Color = color;
        }

        public bool Equals(Vertex other)
        {
            return this.Position == other.Position &&
                   this.Normal == other.Normal &&
                   this.TexCoords == other.TexCoords &&
                   this.Color == other.Color;
        }

        /// <summary>
        /// Return a flipped <see cref="Vertex"/> normal.
        /// </summary>
        public Vertex FlipNormal()
            => new Vertex(Position, Normal * -1, TexCoords, Color);

        public override string ToString()
            => $"Position: {Position}, Normal: {Normal}, TexCoords: {TexCoords}, Color: {Color}";
    }
}
