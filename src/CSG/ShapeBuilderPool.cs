namespace CSG
{
    using System.Collections.Concurrent;

    static class ShapeBuilderPool
    {
        private static readonly ConcurrentDictionary<int, ShapeBuilder> builders = new ConcurrentDictionary<int, ShapeBuilder>();
        private static int CurrentThread => System.Threading.Thread.CurrentThread.ManagedThreadId;

        public static ShapeBuilder CurrentBuilder()
        {
            if (!builders.ContainsKey(CurrentThread))
                builders[CurrentThread] = new ShapeBuilder();
            return builders[CurrentThread];
        }
    }
}
