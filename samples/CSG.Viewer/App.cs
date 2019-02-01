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
        private CommandList _cl;
        private BasicMaterial basicMaterial;
        private Geometry geometry;

        private float _ticks;

        protected override void CreateResources(ResourceFactory factory)
        {
            var texture = TextureLoader.Load("v:checker").GetAwaiter().GetResult();
            basicMaterial = new BasicMaterial(DrawingContext, texture);

            // var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            // var shape2 = new Cube(new Vector3(0.8f, 0.8f, 0), new Vector3(1, 1, 1));
            // var shape = shape1.Do(ShapeOperation.Intersect, shape2);
            
            var shape = new Teapot();

            geometry = new Geometry(DrawingContext, shape.Cache.Vertices, shape.Cache.Indices);

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

            basicMaterial.Apply(_cl);
            geometry.Draw(_cl);

            _cl.End();

            DrawingContext.GraphicsDevice.SubmitCommands(_cl);
            DrawingContext.GraphicsDevice.SwapBuffers(DrawingContext.MainSwapchain);
            DrawingContext.GraphicsDevice.WaitForIdle();
        }
    }
}
