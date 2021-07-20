using System.Collections.Generic;
using System.Linq;

namespace CSG
{
    static class LinqExtensions
    {
        // https://github.com/dotnet/runtime/issues/27449
        // https://stackoverflow.com/questions/13731796/create-batches-in-linq
        public static IEnumerable<IEnumerable<TSource>> Chunk<TSource>(this IEnumerable<TSource> source, int size)
        {
            TSource[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                    bucket = new TSource[size];

                bucket[count++] = item;
                if (count != size)
                    continue;

                yield return bucket;

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
                yield return bucket.Take(count).ToArray();
        }
    }
}
