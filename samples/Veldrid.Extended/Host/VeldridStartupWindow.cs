namespace Veldrid.Host
{
    using System;
    using System.Diagnostics;
    using Veldrid.Sdl2;
    using Veldrid.StartupUtilities;
    using Veldrid.Utilities;

    public class VeldridStartupWindow : IApplicationWindow
    {
        public uint Width => (uint)window.Width;
        public uint Height => (uint)window.Height;

        public PlatformType PlatformType => PlatformType.Desktop;

        public event Action<float> Rendering;
        public event Action<GraphicsDevice, ResourceFactory, Swapchain> GraphicsDeviceCreated;
        public event Action GraphicsDeviceDestroyed;
        public event Action Resized;
        public event Action<KeyEvent> KeyPressed;

        private readonly Sdl2Window window;
        private GraphicsDevice graphicsDevice;
        private DisposeCollectorResourceFactory resourceFactory;
        private bool windowResized = true;

        public VeldridStartupWindow(string title)
        {
            WindowCreateInfo wci = new WindowCreateInfo
            {
                X = 100,
                Y = 100,
                WindowWidth = 1280,
                WindowHeight = 720,
                WindowTitle = title,
            };

            window = VeldridStartup.CreateWindow(ref wci);
            window.Resized += () => windowResized = true;
            window.KeyDown += OnKeyDown;
        }

        public void Run()
        {
            var options = new GraphicsDeviceOptions(
                debug: false,
                swapchainDepthFormat: PixelFormat.R16_UNorm,
                syncToVerticalBlank: true,
                resourceBindingModel: ResourceBindingModel.Improved,
                preferDepthRangeZeroToOne: true,
                preferStandardClipSpaceYDirection: true);
#if DEBUG
            options.Debug = true;
#endif

            graphicsDevice = VeldridStartup.CreateGraphicsDevice(window, options);
            resourceFactory = new DisposeCollectorResourceFactory(graphicsDevice.ResourceFactory);
            GraphicsDeviceCreated?.Invoke(graphicsDevice, resourceFactory, graphicsDevice.MainSwapchain);

            var sw = Stopwatch.StartNew();
            var previousElapsed = sw.Elapsed.TotalSeconds;

            while (window.Exists)
            {
                double newElapsed = sw.Elapsed.TotalSeconds;
                float deltaSeconds = (float)(newElapsed - previousElapsed);

                InputSnapshot inputSnapshot = window.PumpEvents();
                //InputTracker.UpdateFrameInput(inputSnapshot);

                if (window.Exists)
                {
                    previousElapsed = newElapsed;
                    if (windowResized)
                    {
                        windowResized = false;
                        graphicsDevice.ResizeMainWindow((uint)window.Width, (uint)window.Height);
                        Resized?.Invoke();
                    }

                    Rendering?.Invoke(deltaSeconds);
                }
            }

            graphicsDevice.WaitForIdle();
            resourceFactory.DisposeCollector.DisposeAll();
            graphicsDevice.Dispose();

            GraphicsDeviceDestroyed?.Invoke();
        }

        protected void OnKeyDown(KeyEvent keyEvent)
        {
            KeyPressed?.Invoke(keyEvent);
        }
    }
}
