namespace CSG
{
    using System.Collections.Generic;

    class ShapeBuilder
    {
        public readonly List<Vertex> Vertices;
        public readonly List<ushort> Indices;

        public ShapeBuilder()
        {
            this.Vertices = new List<Vertex>();
            this.Indices = new List<ushort>();
        }

        public void Clear()
        {
            Vertices.Clear();
            Indices.Clear();
        }

        public ShapeCache CreateCache() => new ShapeCache(Vertices.ToArray(), Indices.ToArray());


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
