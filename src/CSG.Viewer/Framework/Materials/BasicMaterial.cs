namespace CSG.Viewer.Framework.Materials
{
    using System.Numerics;
    using System.Text;
    using Veldrid;
    using Veldrid.SPIRV;

    public class BasicMaterial : Material
    {
        public Matrix4x4 Projection { get; set; } = Matrix4x4.Identity;
        public Matrix4x4 View { get; set; } = Matrix4x4.Identity;
        public Matrix4x4 World { get; set; } = Matrix4x4.Identity;

        private Texture _surfaceTexture;
        private TextureView _surfaceTextureView;
        private Pipeline _pipeline;
        private ResourceSet _projViewSet;
        private ResourceSet _worldTextureSet;

        private DeviceBuffer _projectionBuffer;
        private DeviceBuffer _viewBuffer;
        private DeviceBuffer _worldBuffer;

        public BasicMaterial(DrawingContext context, Texture2D texture)
            : base(context)
        {
            _projectionBuffer = context.ResourceFactory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            _viewBuffer = context.ResourceFactory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            _worldBuffer = context.ResourceFactory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));

            _surfaceTexture = texture.CreateDeviceTexture(context.GraphicsDevice, context.ResourceFactory, TextureUsage.Sampled);
            _surfaceTextureView = context.ResourceFactory.CreateTextureView(_surfaceTexture);

            ShaderSetDescription shaderSet = new ShaderSetDescription(
                new[]
                {
                    new VertexLayoutDescription(
                        new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
                        new VertexElementDescription("Normal", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
                        new VertexElementDescription("TexCoords", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
                        new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4)
                    )
                },
                context.ResourceFactory.CreateFromSpirv(
                    new ShaderDescription(ShaderStages.Vertex, Encoding.UTF8.GetBytes(VertexCode), "main"),
                    new ShaderDescription(ShaderStages.Fragment, Encoding.UTF8.GetBytes(FragmentCode), "main")));

            ResourceLayout projViewLayout = context.ResourceFactory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("Projection", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription("View", ResourceKind.UniformBuffer, ShaderStages.Vertex)));

            ResourceLayout worldTextureLayout = context.ResourceFactory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("World", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription("SurfaceTexture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("SurfaceSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

            _pipeline = context.ResourceFactory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
                BlendStateDescription.SingleOverrideBlend,
                DepthStencilStateDescription.DepthOnlyLessEqual,
                RasterizerStateDescription.Default,
                PrimitiveTopology.TriangleList,
                shaderSet,
                new[] { projViewLayout, worldTextureLayout },
                context.MainSwapchain.Framebuffer.OutputDescription));

            _projViewSet = context.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                projViewLayout,
                _projectionBuffer,
                _viewBuffer));

            _worldTextureSet = context.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                worldTextureLayout,
                _worldBuffer,
                _surfaceTextureView,
                context.GraphicsDevice.Aniso4xSampler));

        }

        public void Apply(CommandList cl)
        {
            cl.UpdateBuffer(_projectionBuffer, 0, Projection);
            cl.UpdateBuffer(_viewBuffer, 0, View);
            cl.UpdateBuffer(_worldBuffer, 0, World);

            cl.SetPipeline(_pipeline);
            cl.SetGraphicsResourceSet(0, _projViewSet);
            cl.SetGraphicsResourceSet(1, _worldTextureSet);
        }

        protected override void OnBeginApply(CommandList commandList, DrawingContext context, Material previousMaterial)
        {
            commandList.UpdateBuffer(_projectionBuffer, 0, Projection);
            commandList.UpdateBuffer(_viewBuffer, 0, View);
            commandList.UpdateBuffer(_worldBuffer, 0, World);

            commandList.SetPipeline(_pipeline);
            commandList.SetGraphicsResourceSet(0, _projViewSet);
            commandList.SetGraphicsResourceSet(1, _worldTextureSet);
        }

        protected override void OnEndApply(CommandList commandList, DrawingContext context)
        {

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

layout(location = 0) out vec2 fsin_texCoords;

void main()
{
    vec4 worldPosition = World * vec4(Position, 1);
    vec4 viewPosition = View * worldPosition;
    vec4 clipPosition = Projection * viewPosition;
    gl_Position = clipPosition;
    fsin_texCoords = TexCoords;
}";

        private const string FragmentCode = @"
#version 450

layout(location = 0) in vec2 fsin_texCoords;
layout(location = 0) out vec4 fsout_color;

layout(set = 1, binding = 1) uniform texture2D SurfaceTexture;
layout(set = 1, binding = 2) uniform sampler SurfaceSampler;

void main()
{
    fsout_color =  texture(sampler2D(SurfaceTexture, SurfaceSampler), fsin_texCoords);
}";
    }
}
