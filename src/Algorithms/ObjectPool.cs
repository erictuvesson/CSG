namespace CSG.Algorithms
{
    using System;
    using System.Collections.Concurrent;

    /// <remarks>
    /// https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/how-to-create-an-object-pool
    /// </remarks>
    class ObjectPool<T>
    {
        private ConcurrentBag<T> objects;
        private Func<T> objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            this.objects = new ConcurrentBag<T>();
            this.objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
        }

        public T GetObject()
        {
            T item;
            if (this.objects.TryTake(out item)) return item;
            return this.objectGenerator();
        }

        public void PutObject(T item)
        {
            this.objects.Add(item);
        }
    }
}
