namespace CSG.Content
{
    using System.IO;
    
    public abstract class ContentReader
    {
        public abstract Shape Read(MemoryStream stream);
    }
}
