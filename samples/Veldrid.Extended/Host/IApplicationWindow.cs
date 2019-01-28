namespace Veldrid.Host
{
    using System;

    public enum PlatformType
    {
        Desktop,
        Mobile,
    }

    public interface IApplicationWindow
    {
        PlatformType PlatformType { get; }

        event Action<float> Rendering;
        event Action<GraphicsDevice, ResourceFactory, Swapchain> GraphicsDeviceCreated;
        event Action GraphicsDeviceDestroyed;
        event Action Resized;
        event Action<KeyEvent> KeyPressed;

        uint Width { get; }
        uint Height { get; }

        void Run();
    }
}
