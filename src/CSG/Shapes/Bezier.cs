namespace CSG.Shapes
{
    using System;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class Bezier : Shape
    {
        protected Bezier()
        {
        }

        protected Bezier(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Creates indices for a patch that is tessellated at the specified level.
        /// </summary>
        protected void CreatePatchIndices(IShapeBuilder builder, int tessellation, bool isMirrored)
        {
            int stride = tessellation + 1;

            for (int i = 0; i < tessellation; ++i)
            {
                for (int j = 0; j < tessellation; j++)
                {
                    // Make a list of six index values (two triangles).
                    int[] indices =
                    {
                        i * stride + j,
                        (i + 1) * stride + j,
                        (i + 1) * stride + j + 1,

                        i * stride + j,
                        (i + 1) * stride + j + 1,
                        i * stride + j + 1,
                    };

                    // If this patch is mirrored, reverse the
                    // indices to keep the correct winding order.
                    if (isMirrored)
                    {
                        Array.Reverse(indices);
                    }

                    // Create the indices.
                    foreach (int index in indices)
                    {
                        builder.AddIndex(builder.CurrentVertex + index);
                    }
                }
            }
        }

        /// <summary>
        /// Creates vertices for a patch that is tessellated at the specified level.
        /// </summary>
        protected void CreatePatchVertices(IShapeBuilder builder, Vector3[] patch, int tessellation, bool isMirrored)
        {
            Debug.Assert(patch.Length == 16);

            for (int i = 0; i <= tessellation; ++i)
            {
                float ti = (float)i / tessellation;

                for (int j = 0; j <= tessellation; j++)
                {
                    float tj = (float)j / tessellation;

                    // Perform four horizontal bezier interpolations
                    // between the control points of this patch.
                    Vector3 p1 = BezierInterpolate(patch[0], patch[1], patch[2], patch[3], ti);
                    Vector3 p2 = BezierInterpolate(patch[4], patch[5], patch[6], patch[7], ti);
                    Vector3 p3 = BezierInterpolate(patch[8], patch[9], patch[10], patch[11], ti);
                    Vector3 p4 = BezierInterpolate(patch[12], patch[13], patch[14], patch[15], ti);

                    // Perform a vertical interpolation between the results of the
                    // previous horizontal interpolations, to compute the position.
                    Vector3 position = BezierInterpolate(p1, p2, p3, p4, tj);

                    // Perform another four bezier interpolations between the control
                    // points, but this time vertically rather than horizontally.
                    Vector3 q1 = BezierInterpolate(patch[0], patch[4], patch[8], patch[12], tj);
                    Vector3 q2 = BezierInterpolate(patch[1], patch[5], patch[9], patch[13], tj);
                    Vector3 q3 = BezierInterpolate(patch[2], patch[6], patch[10], patch[14], tj);
                    Vector3 q4 = BezierInterpolate(patch[3], patch[7], patch[11], patch[15], tj);

                    // Compute vertical and horizontal tangent vectors.
                    Vector3 tangentA = BezierTangent(p1, p2, p3, p4, tj);
                    Vector3 tangentB = BezierTangent(q1, q2, q3, q4, ti);

                    // Cross the two tangent vectors to compute the normal.
                    Vector3 normal = Vector3.Cross(tangentA, tangentB);

                    if (normal.Length() > 0.0001f)
                    {
                        normal = Vector3.Normalize(normal);

                        // If this patch is mirrored, we must invert the normal.
                        if (isMirrored)
                        {
                            normal = -normal;
                        }
                    }
                    else
                    {
                        // In a tidy and well constructed bezier patch, the preceding
                        // normal computation will always work. But the classic teapot
                        // model is not tidy or well constructed! At the top and bottom
                        // of the teapot, it contains degenerate geometry where a patch
                        // has several control points in the same place, which causes
                        // the tangent computation to fail and produce a zero normal.
                        // We 'fix' these cases by just hard-coding a normal that points
                        // either straight up or straight down, depending on whether we
                        // are on the top or bottom of the teapot. This is not a robust
                        // solution for all possible degenerate bezier patches, but hey,
                        // it's good enough to make the teapot work correctly!

                        if (position.Y > 0)
                        {
                            normal = Vector3.UnitY;
                        }
                        else
                        {
                            normal = -Vector3.UnitY;
                        }
                    }

                    // Create the vertex.
                    builder.AddVertex(position, normal);
                }
            }
        }

        /// <summary>
        /// Performs a cubic bezier interpolation between four scalar control
        /// points, returning the value at the specified time (t ranges 0 to 1).
        /// </summary>
        static float BezierInterpolate(float p1, float p2, float p3, float p4, float t)
        {
            return p1 * (1 - t) * (1 - t) * (1 - t) +
                   p2 * 3 * t * (1 - t) * (1 - t) +
                   p3 * 3 * t * t * (1 - t) +
                   p4 * t * t * t;
        }

        /// <summary>
        /// Performs a cubic bezier interpolation between four Vector3 control
        /// points, returning the value at the specified time (t ranges 0 to 1).
        /// </summary>
        static Vector3 BezierInterpolate(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
        {
            Vector3 result = new Vector3
            {
                X = BezierInterpolate(p1.X, p2.X, p3.X, p4.X, t),
                Y = BezierInterpolate(p1.Y, p2.Y, p3.Y, p4.Y, t),
                Z = BezierInterpolate(p1.Z, p2.Z, p3.Z, p4.Z, t)
            };

            return result;
        }

        /// <summary>
        /// Computes the tangent of a cubic bezier curve at the specified time,
        /// when given four scalar control points.
        /// </summary>
        static float BezierTangent(float p1, float p2, float p3, float p4, float t)
        {
            return p1 * (-1 + 2 * t - t * t) +
                   p2 * (1 - 4 * t + 3 * t * t) +
                   p3 * (2 * t - 3 * t * t) +
                   p4 * (t * t);
        }

        /// <summary>
        /// Computes the tangent of a cubic bezier curve at the specified time,
        /// when given four Vector3 control points. This is used for calculating
        /// normals (by crossing the horizontal and vertical tangent vectors).
        /// </summary>
        static Vector3 BezierTangent(Vector3 p1, Vector3 p2,
                                     Vector3 p3, Vector3 p4, float t)
        {
            Vector3 result = new Vector3
            {
                X = BezierTangent(p1.X, p2.X, p3.X, p4.X, t),
                Y = BezierTangent(p1.Y, p2.Y, p3.Y, p4.Y, t),
                Z = BezierTangent(p1.Z, p2.Z, p3.Z, p4.Z, t)
            };

            result = Vector3.Normalize(result);

            return result;
        }
    }
}