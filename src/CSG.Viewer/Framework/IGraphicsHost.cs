namespace CSG.Viewer.Framework
{
    using System;
    using Veldrid;

    public enum PlatformType
    {
        Desktop,
        Mobile,
    }

    public interface IGraphicsHost
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
