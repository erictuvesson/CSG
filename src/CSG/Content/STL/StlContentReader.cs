namespace CSG.Content.STL
{
    using System;
    using System.IO;

    public class StlContentReader
    {
        public virtual Shape Read(MemoryStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
