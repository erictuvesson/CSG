using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CSG.Viewer
{
    static class Test
    {
        private static Vertex[] GetCubeVertices()
        {
            Vertex[] vertices = new Vertex[]
            {
                // Top
                new Vertex(new Vector3(-0.5f, +0.5f, -0.5f), new Vector2(0, 0), Vector4.One),
                new Vertex(new Vector3(+0.5f, +0.5f, -0.5f), new Vector2(1, 0), Vector4.One),
                new Vertex(new Vector3(+0.5f, +0.5f, +0.5f), new Vector2(1, 1), Vector4.One),
                new Vertex(new Vector3(-0.5f, +0.5f, +0.5f), new Vector2(0, 1), Vector4.One),
                // Bottom                                                             
                new Vertex(new Vector3(-0.5f,-0.5f, +0.5f),  new Vector2(0, 0), Vector4.One),
                new Vertex(new Vector3(+0.5f,-0.5f, +0.5f),  new Vector2(1, 0), Vector4.One),
                new Vertex(new Vector3(+0.5f,-0.5f, -0.5f),  new Vector2(1, 1), Vector4.One),
                new Vertex(new Vector3(-0.5f,-0.5f, -0.5f),  new Vector2(0, 1), Vector4.One),
                // Left                                                               
                new Vertex(new Vector3(-0.5f, +0.5f, -0.5f), new Vector2(0, 0), Vector4.One),
                new Vertex(new Vector3(-0.5f, +0.5f, +0.5f), new Vector2(1, 0), Vector4.One),
                new Vertex(new Vector3(-0.5f, -0.5f, +0.5f), new Vector2(1, 1), Vector4.One),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 1), Vector4.One),
                // Right                                                              
                new Vertex(new Vector3(+0.5f, +0.5f, +0.5f), new Vector2(0, 0), Vector4.One),
                new Vertex(new Vector3(+0.5f, +0.5f, -0.5f), new Vector2(1, 0), Vector4.One),
                new Vertex(new Vector3(+0.5f, -0.5f, -0.5f), new Vector2(1, 1), Vector4.One),
                new Vertex(new Vector3(+0.5f, -0.5f, +0.5f), new Vector2(0, 1), Vector4.One),
                // Back                                                               
                new Vertex(new Vector3(+0.5f, +0.5f, -0.5f), new Vector2(0, 0), Vector4.One),
                new Vertex(new Vector3(-0.5f, +0.5f, -0.5f), new Vector2(1, 0), Vector4.One),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(1, 1), Vector4.One),
                new Vertex(new Vector3(+0.5f, -0.5f, -0.5f), new Vector2(0, 1), Vector4.One),
                // Front                                                              
                new Vertex(new Vector3(-0.5f, +0.5f, +0.5f), new Vector2(0, 0), Vector4.One),
                new Vertex(new Vector3(+0.5f, +0.5f, +0.5f), new Vector2(1, 0), Vector4.One),
                new Vertex(new Vector3(+0.5f, -0.5f, +0.5f), new Vector2(1, 1), Vector4.One),
                new Vertex(new Vector3(-0.5f, -0.5f, +0.5f), new Vector2(0, 1), Vector4.One),
            };

            return vertices;
        }

        private static ushort[] GetCubeIndices()
        {
            ushort[] indices =
            {
                0,1,2, 0,2,3,
                4,5,6, 4,6,7,
                8,9,10, 8,10,11,
                12,13,14, 12,14,15,
                16,17,18, 16,18,19,
                20,21,22, 20,22,23,
            };

            return indices;
        }

        private const string VertexCode = @"
#version 450
layout(set = 0, binding = 0) uniform ProjectionBuffer
{
    mat4 Projection;
};

layout(set = 0, binding = 1) uniform ViewBuffer
{
    mat4 View;
};

layout(set = 1, binding = 0) uniform WorldBuffer
{
    mat4 World;
};

layout(location = 0) in vec3 Position;
layout(location = 1) in vec3 Normal;
layout(location = 2) in vec2 TexCoords;
layout(location = 3) in vec4 Color;

layout(location = 0) out vec2 out_TexCoords;
layout(location = 1) out vec4 out_Color;

void main()
{
    //vec4 worldPosition = World * vec4(Position, 1);
    //vec4 viewPosition = View * worldPosition;
    //vec4 clipPosition = Projection * viewPosition;
    //gl_Position = clipPosition;

    gl_Position = Projection * View * vec4(Position, 1);
    //gl_Position = vec4(Position, 1);

    out_TexCoords = TexCoords;
    out_Color = Color;
}";

        private const string FragmentCode = @"
#version 450

layout(location = 0) in vec2 TexCoords;
layout(location = 1) in vec4 Color;

layout(location = 0) out vec4 out_color;

layout(set = 1, binding = 1) uniform texture2D SurfaceTexture;
layout(set = 1, binding = 2) uniform sampler SurfaceSampler;

void main()
{
    // out_color = vec4(1, 0, 0, 1); // Color;
    out_color = vec4(TexCoords, 0, 1);
}";

    }
}
