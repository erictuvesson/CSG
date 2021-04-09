namespace CSG.Viewer
{
    using CSG.Shapes;
    using CSG.Viewer.Framework;
    using CSG.Viewer.Framework.Materials;
    using ImGuiNET;
    using System.Numerics;
    using Veldrid;

    class App : Application
    {
        private CommandList commandList;
        private BasicMaterial basicMaterial;
        private ShapeGeometry shapeGeometry;

        private float _ticks;

        protected override void CreateResources(ResourceFactory factory)
        {
            var texture = TextureLoader.Load("v:checker").GetAwaiter().GetResult();
            basicMaterial = new BasicMaterial(DrawingContext, texture, true);

            // var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            // var shape2 = new Cube(new Vector3(0.8f, 0.8f, 0), new Vector3(1, 1, 1));
            // var shape = shape1.Do(ShapeOperation.Intersect, shape2);

            var shape = new Teapot();

            shapeGeometry = new ShapeGeometry(DrawingContext, shape);

            commandList = factory.CreateCommandList();
        }

        protected override void Draw(float deltaSeconds)
        {
            ImGuiRenderer.Update(deltaSeconds, this.Host.InputSnapshot);

            if (ImGui.Begin(
                "Stats",
                ImGuiWindowFlags.NoSavedSettings
              | ImGuiWindowFlags.NoTitleBar
              | ImGuiWindowFlags.NoResize
              | ImGuiWindowFlags.NoMove
              | ImGuiWindowFlags.NoBackground
                ))
            {
                ImGui.SetWindowSize(new Vector2(200, 100));
                ImGui.SetWindowPos(new Vector2(10, 10));

                ImGui.TextColored(new Vector4(0, 0, 0, 1), "Vertices: " + this.shapeGeometry.Shape.Vertices.Length);
                ImGui.TextColored(new Vector4(0, 0, 0, 1), "Indices: " + this.shapeGeometry.Shape.Indices.Length);
                ImGui.TextColored(new Vector4(0, 0, 0, 1), "Triangles: " + this.shapeGeometry.Shape.Indices.Length / 3);
            }

            _ticks += deltaSeconds * 1000f;
            commandList.Begin();

            basicMaterial.Projection = Matrix4x4.CreatePerspectiveFieldOfView(1.0f, HostAspectRatio, 0.5f, 100f);
            basicMaterial.View = Matrix4x4.CreateLookAt(Vector3.UnitZ * 2.5f, Vector3.Zero, Vector3.UnitY);
            basicMaterial.World = Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, (_ticks / 1000f)) *
                                  Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, (_ticks / 3000f));

            commandList.SetFramebuffer(DrawingContext.MainSwapchain.Framebuffer);
            commandList.ClearColorTarget(0, RgbaFloat.CornflowerBlue);
            commandList.ClearDepthStencil(1f);

            basicMaterial.Apply(commandList);
            shapeGeometry.Draw(commandList);

            ImGuiRenderer.Render(DrawingContext.GraphicsDevice, commandList);

            commandList.End();

            DrawingContext.GraphicsDevice.SubmitCommands(commandList);
            DrawingContext.GraphicsDevice.SwapBuffers(DrawingContext.MainSwapchain);
            DrawingContext.GraphicsDevice.WaitForIdle();
        }
    }
}
