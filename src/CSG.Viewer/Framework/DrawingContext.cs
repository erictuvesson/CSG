namespace CSG.Viewer.Framework
{
    using CSG.Viewer.Framework.Materials;
    using Veldrid;

    public class DrawingContext
    {
        public GraphicsDevice GraphicsDevice { get; private set; }
        public ResourceFactory ResourceFactory { get; private set; }
        public Swapchain MainSwapchain => GraphicsDevice.MainSwapchain;

        public Material PreviousMaterial { get; internal set; } = null;

        public DrawingContext(GraphicsDevice graphicsDevice, ResourceFactory resourceFactory)
        {
            this.GraphicsDevice = graphicsDevice;
            this.ResourceFactory = resourceFactory;
        }
    }
}
