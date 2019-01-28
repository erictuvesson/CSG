namespace Veldrid.Host
{
    using System;
    using Veldrid.Content;

    public abstract class Application : IDisposable
    {
        public IApplicationWindow Window { get; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public ResourceFactory ResourceFactory { get; private set; }
        public Swapchain MainSwapchain { get; private set; }

        public readonly ContentProvider ContentProvider;
        public readonly TextureLoader TextureLoader;

        public Application(IApplicationWindow window = null)
        {
            Window = window ?? new VeldridStartupWindow(GetType().Name);
            Window.Resized += HandleWindowResize;
            Window.GraphicsDeviceCreated += OnGraphicsDeviceCreated;
            Window.GraphicsDeviceDestroyed += OnDeviceDestroyed;
            Window.Rendering += Draw;
            Window.KeyPressed += OnKeyDown;

            ContentProvider = new ContentProvider();
            TextureLoader = new TextureLoader(ContentProvider);
        }

        public void Run()
        {
            Window.Run();
        }

        public void OnGraphicsDeviceCreated(GraphicsDevice gd, ResourceFactory factory, Swapchain sc)
        {
            GraphicsDevice = gd;
            ResourceFactory = factory;
            MainSwapchain = sc;
            CreateResources(factory);
            CreateSwapchainResources(factory);
        }

        protected virtual void OnDeviceDestroyed()
        {
            GraphicsDevice = null;
            ResourceFactory = null;
            MainSwapchain = null;
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
