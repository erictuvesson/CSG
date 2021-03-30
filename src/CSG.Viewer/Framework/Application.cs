namespace CSG.Viewer.Framework
{
    using CSG.Viewer.Framework.Content;
    using CSG.Viewer.Framework.Host;
    using System;
    using Veldrid;

    public abstract class Application : IDisposable
    {
        public float HostAspectRatio => (float)Host.Width / Host.Height;

        public IGraphicsHost Host { get; }
        public DrawingContext DrawingContext { get; private set; }

        public readonly ContentProvider ContentProvider;
        public readonly TextureLoader TextureLoader;

        public Application(IGraphicsHost host = null)
        {
            Host = host ?? new VeldridHost(GetType().Name);
            Host.Resized += HandleHostResize;
            Host.DrawingContextCreated += OnGraphicsDeviceCreated;

            Host.Rendering += Draw;
            Host.KeyPressed += OnKeyDown;

            ContentProvider = new ContentProvider();
            TextureLoader = new TextureLoader(ContentProvider);
        }

        public void Run()
        {
            Host.Run();
        }

        public void OnGraphicsDeviceCreated(DrawingContext drawingContext)
        {
            this.DrawingContext = drawingContext;
            CreateResources(DrawingContext.ResourceFactory);
            CreateSwapchainResources(DrawingContext.ResourceFactory);
        }

        protected abstract void CreateResources(ResourceFactory factory);

        protected virtual void CreateSwapchainResources(ResourceFactory factory)
        {

        }

        protected abstract void Draw(float deltaSeconds);

        protected virtual void HandleHostResize()
        {

        }

        protected virtual void OnKeyDown(KeyEvent ke)
        {

        }

        public void Dispose()
        {

        }
    }
}
