namespace CSG.Viewer.Core
{
    using System;
    using Veldrid;

    public enum SamplePlatformType
    {
        Desktop,
        Mobile,
    }

    public interface IApplicationWindow
    {
        SamplePlatformType PlatformType { get; }

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
