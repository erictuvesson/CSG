namespace CSG.Viewer
{
    using System.Numerics;
    using System.Text;
    using Veldrid;
    using Veldrid.Host;
    using Veldrid.Materials;
    using Veldrid.SPIRV;
    using CSG.Shapes;

    class App : Application
    {
        private readonly Texture2D _stoneTexData;

        private readonly Vertex[] _vertices;
        private readonly ushort[] _indices;
        
        private DeviceBuffer _vertexBuffer;
        private DeviceBuffer _indexBuffer;
        private CommandList _cl;
        private BasicMaterial basicMaterial;
        private float _ticks;

        private Shape shape;

        public App()
        {
            _stoneTexData = TextureLoader.Load("v:checker").GetAwaiter().GetResult();

            var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            var shape2 = new Cube(new Vector3(0.8f, 0.8f, 0), new Vector3(1, 1, 1));
            shape = shape1.Do(ShapeOperation.Intersect, shape2);
            
            _vertices = shape.Cache.Vertices;
            _indices = shape.Cache.Indices;
        }

        protected override void CreateResources(ResourceFactory factory)
        {
            _vertexBuffer = factory.CreateBuffer(new BufferDescription((uint)(Vertex.SizeInBytes * _vertices.Length), BufferUsage.VertexBuffer));
            _indexBuffer = factory.CreateBuffer(new BufferDescription(sizeof(ushort) * (uint)_indices.Length, BufferUsage.IndexBuffer));

            DrawingContext.GraphicsDevice.UpdateBuffer(_vertexBuffer, 0, _vertices);
            DrawingContext.GraphicsDevice.UpdateBuffer(_indexBuffer, 0, _indices);

            var texture = TextureLoader.Load("v:checker").GetAwaiter().GetResult();
            basicMaterial = new BasicMaterial(DrawingContext, texture);

            _cl = factory.CreateCommandList();
        }

        protected override void Draw(float deltaSeconds)
        {
            _ticks += deltaSeconds * 1000f;
            _cl.Begin();

            basicMaterial.Projection = Matrix4x4.CreatePerspectiveFieldOfView(1.0f, (float)Window.Width / Window.Height, 0.5f, 100f);
            basicMaterial.View = Matrix4x4.CreateLookAt(Vector3.UnitZ * 2.5f, Vector3.Zero, Vector3.UnitY);
            basicMaterial.World = Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, (_ticks / 1000f)) * 
                                  Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, (_ticks / 3000f));

            _cl.SetFramebuffer(DrawingContext.MainSwapchain.Framebuffer);
            _cl.ClearColorTarget(0, RgbaFloat.Black);
            _cl.ClearDepthStencil(1f);

            _cl.SetVertexBuffer(0, _vertexBuffer);
            _cl.SetIndexBuffer(_indexBuffer, IndexFormat.UInt16);

            basicMaterial.Apply(_cl);
            _cl.DrawIndexed((uint)_vertices.Length, 1, 0, 0, 0);

            _cl.End();

            DrawingContext.GraphicsDevice.SubmitCommands(_cl);
            DrawingContext.GraphicsDevice.SwapBuffers(DrawingContext.MainSwapchain);
            DrawingContext.GraphicsDevice.WaitForIdle();
        }
    }
}
