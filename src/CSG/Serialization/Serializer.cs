namespace CSG.Serialization
{
    using System;
    using System.IO;

    public enum SerializeFormat
    {
        Json,
        Xml
    }

    public class Serializer
    {
        public SerializeFormat Format { get; private set; }

        public SerializerStream SerializerStream { get; private set; }

        public Serializer(SerializeFormat format = SerializeFormat.Xml)
        {
            this.Format = format;
            this.SerializerStream = CreateSerializer(format);
        }

        public void Serialize<T>(string filepath, T value, bool binary = false)
        {
            if (binary)
            {
                // TODO: Add gzip
                
                Stream stream = new FileStream(filepath, FileMode.Create);
                SerializerStream.Serialize(value, ref stream);
                stream.Close();
            }
            else
            {
                var content = SerializerStream.SerializeContent(value);
                File.WriteAllText(filepath, content);
            }
        }

        private static SerializerStream CreateSerializer(SerializeFormat format)
        {
            switch (format)
            {
                default:
                case SerializeFormat.Xml: return new SerializerStreamXml();
                case SerializeFormat.Json: return new SerializerStreamJson();
            }
        }
    }
}
