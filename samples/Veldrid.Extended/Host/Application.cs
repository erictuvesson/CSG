namespace Veldrid.Host
{
    using System;
    using Veldrid.Content;

    public abstract class Application : IDisposable
    {
        public IApplicationWindow Window { get; }
        public DrawingContext DrawingContext { get; private set; }

        public readonly ContentProvider ContentProvider;
        public readonly TextureLoader TextureLoader;

        public Application(IApplicationWindow window = null)
        {
            Window = window ?? new VeldridStartupWindow(GetType().Name);
            Window.Resized += HandleWindowResize;
            Window.DrawingContextCreated += OnGraphicsDeviceCreated;
            
            Window.Rendering += Draw;
            Window.KeyPressed += OnKeyDown;

            ContentProvider = new ContentProvider();
            TextureLoader = new TextureLoader(ContentProvider);
        }

        public void Run()
        {
            Window.Run();
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

        protected virtual void HandleWindowResize()
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
