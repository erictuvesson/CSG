namespace CSG.Serialization
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Xml;

    public abstract class SerializerStreamDataContract<TSerializer> : SerializerStream
        where TSerializer : XmlObjectSerializer
    {
        public override T Deserialize<T>(byte[] value)
        {
            using (var fs = new MemoryStream(value))
            {
                var reader = XmlDictionaryReader.CreateTextReader(fs,
                    new XmlDictionaryReaderQuotas());
                var ser = CreateSerializer<T>();
                var obj = (T)ser.ReadObject(reader);
                reader.Close();
                return obj;
            }
        }

        public override T DeserializeContent<T>(string value)
        {
            return default(T);
        }

        public override byte[] Serialize<T>(T value)
        {
            using (var fs = new MemoryStream())
            {
                var writer = XmlDictionaryWriter.CreateBinaryWriter(fs);
                var ser = CreateSerializer<T>();
                ser.WriteObject(writer, value);
                writer.Close();
                return fs.ToArray();
            }
        }

        public override string SerializeContent<T>(T value)
        {
            using (var fs = new MemoryStream())
            {
                var ser = CreateSerializer<T>();
                ser.WriteObject(fs, value);
                return Encoding.ASCII.GetString(fs.ToArray());
            }
        }

        private TSerializer CreateSerializer<T>()
        {
            return (TSerializer)Activator.CreateInstance(typeof(TSerializer), typeof(T));
        }
    }

    public class SerializerStreamJson : SerializerStreamDataContract<DataContractJsonSerializer> {}
    public class SerializerStreamXml : SerializerStreamDataContract<DataContractSerializer> { }
}
