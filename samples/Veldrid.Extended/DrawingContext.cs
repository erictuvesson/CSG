namespace Veldrid
{
    using Veldrid.Materials;

    public class DrawingContext
    {
        public GraphicsDevice GraphicsDevice { get; private set; }
        public ResourceFactory ResourceFactory { get; private set; }
        public Swapchain Swapchain { get; private set; }

        public CommandList CommandList { get; private set; }

        public Material PreviousMaterial { get; internal set; }
    }
}
