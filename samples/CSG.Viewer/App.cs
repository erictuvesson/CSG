namespace CSG.Viewer
{
    using System.IO;
    using System.Text;
    using System.Numerics;
    using Veldrid;
    using Veldrid.Sdl2;
    using Veldrid.StartupUtilities;
    using System;
    using CSG.Viewer.Core;
    using Veldrid.SPIRV;

    class App : SampleApplication
    {
        private readonly Vertex[] _vertices;
        private readonly ushort[] _indices;
        private DeviceBuffer _projectionBuffer;
        private DeviceBuffer _viewBuffer;
        private DeviceBuffer _worldBuffer;
        private DeviceBuffer _vertexBuffer;
        private DeviceBuffer _indexBuffer;
        private CommandList _cl;

        private Pipeline _pipeline;
        private ResourceSet _projViewSet;
        private ResourceSet _worldSet;
        private float _ticks;

        public App(IApplicationWindow window)
            : base(window)
        {
            _vertices = GetCubeVertices();
            _indices = GetCubeIndices();
        }

        protected override void CreateResources(ResourceFactory factory)
        {
            _projectionBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            _viewBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            _worldBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));

            _vertexBuffer = factory.CreateBuffer(new BufferDescription((uint)(Vertex.SizeInBytes * _vertices.Length), BufferUsage.VertexBuffer));
            GraphicsDevice.UpdateBuffer(_vertexBuffer, 0, _vertices);

            _indexBuffer = factory.CreateBuffer(new BufferDescription(sizeof(ushort) * (uint)_indices.Length, BufferUsage.IndexBuffer));
            GraphicsDevice.UpdateBuffer(_indexBuffer, 0, _indices);

            ShaderSetDescription shaderSet = new ShaderSetDescription(
                new[]
                {
                    new VertexLayoutDescription(
                        new VertexElementDescription("Position",    VertexElementSemantic.Position,             VertexElementFormat.Float3),
                        new VertexElementDescription("Normal",      VertexElementSemantic.Normal,               VertexElementFormat.Float3),
                        new VertexElementDescription("TexCoords",   VertexElementSemantic.TextureCoordinate,    VertexElementFormat.Float2),
                        new VertexElementDescription("Color",       VertexElementSemantic.Color,                VertexElementFormat.Float4)
                    )
                },
                factory.CreateFromSpirv(
                    new ShaderDescription(ShaderStages.Vertex, Encoding.UTF8.GetBytes(VertexCode), "main"),
                    new ShaderDescription(ShaderStages.Fragment, Encoding.UTF8.GetBytes(FragmentCode), "main")));

            ResourceLayout projViewLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("Projection", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription("View", ResourceKind.UniformBuffer, ShaderStages.Vertex)
                )
            );

            ResourceLayout worldLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("World", ResourceKind.UniformBuffer, ShaderStages.Vertex)
                )
            );

            _pipeline = factory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
                BlendStateDescription.SingleOverrideBlend,
                DepthStencilStateDescription.DepthOnlyLessEqual,
                RasterizerStateDescription.Default,
                PrimitiveTopology.TriangleList,
                shaderSet,
                new[] { projViewLayout, worldLayout },
                MainSwapchain.Framebuffer.OutputDescription));

            _projViewSet = factory.CreateResourceSet(new ResourceSetDescription(projViewLayout, _projectionBuffer, _viewBuffer));
            _worldSet = factory.CreateResourceSet(new ResourceSetDescription(worldLayout, _worldBuffer));

            _cl = factory.CreateCommandList();
        }

        protected override void OnDeviceDestroyed()
        {
            base.OnDeviceDestroyed();
        }

        protected override void Draw(float deltaSeconds)
        {
            _ticks += deltaSeconds * 1000f;
            _cl.Begin();

            var proj = Matrix4x4.CreatePerspectiveFieldOfView(2f, (float)Window.Width / Window.Height, 0.05f, 1000f);
            var view = Matrix4x4.CreateLookAt(Vector3.One * 10, Vector3.Zero, Vector3.UnitY);

            _cl.UpdateBuffer(_projectionBuffer, 0, proj);
            _cl.UpdateBuffer(_viewBuffer, 0, view);

            Matrix4x4 rotation = Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, (_ticks / 1000f)) *
                                 Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, (_ticks / 3000f));
            _cl.UpdateBuffer(_worldBuffer, 0, ref rotation);

            _cl.SetFramebuffer(MainSwapchain.Framebuffer);
            _cl.ClearColorTarget(0, RgbaFloat.White);
            _cl.ClearDepthStencil(1f);

            _cl.SetPipeline(_pipeline);
            _cl.SetVertexBuffer(0, _vertexBuffer);
            _cl.SetIndexBuffer(_indexBuffer, IndexFormat.UInt16);
            _cl.SetGraphicsResourceSet(0, _projViewSet);
            _cl.SetGraphicsResourceSet(1, _worldSet);
            _cl.DrawIndexed(36, 1, 0, 0, 0);

            _cl.End();
            GraphicsDevice.SubmitCommands(_cl);
            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }

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
layout(set = 0, binding = 0) uniform ProjectionBuffer { mat4 Projection; };
layout(set = 0, binding = 1) uniform ViewBuffer { mat4 View; };

layout(set = 1, binding = 0) uniform WorldBuffer { mat4 World; };

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

void main()
{
    out_color = vec4(1, 0, 0, 1); // Color;
}";

    }
}
