namespace CSG
{
    using System.Collections.Generic;

    static class ShapeBuilderPool
    {
        private static Dictionary<int, ShapeBuilder> builders = new Dictionary<int, ShapeBuilder>();
        private static int CurrentThread => System.Threading.Thread.CurrentThread.ManagedThreadId;

        public static ShapeBuilder CurrentBuilder()
        {
            if (!builders.ContainsKey(CurrentThread))
                builders[CurrentThread] = new ShapeBuilder();
            return builders[CurrentThread];
        }
    }
}
