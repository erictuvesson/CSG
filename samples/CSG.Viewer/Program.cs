using CSG.Viewer.Core;

namespace CSG.Viewer
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new VeldridStartupWindow("CSG");
            var app = new App(window);
            window.Run();
        }
    }
}