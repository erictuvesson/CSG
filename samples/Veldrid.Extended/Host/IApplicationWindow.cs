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

        uint Width { get; }
        uint Height { get; }

        DrawingContext DrawingContext { get; }

        event Action<DrawingContext> DrawingContextCreated;
        event Action DrawingContextDestroyed;

        event Action Resized;
        event Action<KeyEvent> KeyPressed;
        event Action<float> Rendering;

        void Run();
    }
}
