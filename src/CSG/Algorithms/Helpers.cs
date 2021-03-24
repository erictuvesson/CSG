namespace CSG.Algorithms
{
    using System.Numerics;

    public static class Helpers
    {
        public const float Pi = 3.141593f;
        public const float TwoPi = 6.28318530718f;
        public const float PiOver2 = 1.570796f;

        /// <summary>
        /// Create a new linear interpolated <see cref="Vertex"/>.
        /// </summary>
        /// <param name="vertex1"></param>
        /// <param name="vertex2"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static Vertex Interpolate(Vertex vertex1, Vertex vertex2, float delta)
        {
            return new Vertex(
                Vector3.Lerp(vertex1.Position, vertex2.Position, delta),
                Vector3.Lerp(vertex1.Normal, vertex2.Normal, delta),
                Vector2.Lerp(vertex1.TexCoords, vertex2.TexCoords, delta),
                (vertex1.Color + vertex2.Color) / 2f
            );
        }

        public static int Clamp(int value, int min, int max = int.MaxValue)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static float Magnitude(this Vector3 value)
        {
            return value.X * value.X + value.Y * value.Y + value.Z * value.Z;
        }

        public static Vector3 Forward(this Matrix4x4 mat)
        {
            return new Vector3(-mat.M31, -mat.M32, -mat.M33);
        }

        public static Vector3 TransformPoint(this Matrix4x4 matrix, Vector3 value)
        {
            return matrix.Translation + value;
        }

        public static Vector3 TransformDirection(this Matrix4x4 matrix, Vector3 value)
        {
            return matrix.Forward() + value;
        }
    }
}
