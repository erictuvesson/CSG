namespace CSG.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Xml;

    public abstract class SerializerStreamDataContract<TSerializer> : SerializerStream
        where TSerializer : XmlObjectSerializer
    {
        protected SerializerStreamDataContract(IEnumerable<Type> knownTypes)
            : base(knownTypes)
        {

        }

        public override T Deserialize<T>(byte[] value)
        {
            using (var stream = new MemoryStream(value))
            {
                T result = default(T);
                using (var reader = XmlDictionaryReader.CreateTextReader(stream,
                    new XmlDictionaryReaderQuotas()))
                {
                    var ser = CreateSerializer<T>();
                    result = ReadObject<T>(ser, reader);
                }
                return result;
            }
        }

        public override T DeserializeContent<T>(string value)
        {
            var byteArray = Encoding.UTF8.GetBytes(value);
            using (var stream = new MemoryStream(byteArray))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
            }
        }

        public override byte[] Serialize<T>(T value)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlDictionaryWriter.CreateBinaryWriter(stream))
                {
                    var serializer = CreateSerializer<T>();
                    WriteObject(serializer, writer, value);
                }
                return stream.ToArray();
            }
        }

        public override string SerializeContent<T>(T value)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = CreateSerializer<T>();
                serializer.WriteObject(stream, value);
                return Encoding.ASCII.GetString(stream.ToArray());
            }
        }

        protected virtual T ReadObject<T>(TSerializer serializer, XmlDictionaryReader stream)
        {
            return (T)serializer.ReadObject(stream);
        }

        protected virtual void WriteObject<T>(TSerializer serializer,
            XmlDictionaryWriter stream, T value)
        {
            serializer.WriteObject(stream, value);
        }

        protected virtual TSerializer CreateSerializer<T>()
        {
            if (KnownTypes != null)
            {
                return (TSerializer)Activator.CreateInstance(typeof(TSerializer),
                    typeof(T), KnownTypes);
            }
            else
            {
                return (TSerializer)Activator.CreateInstance(typeof(TSerializer), typeof(T));
            }
        }
    }
}
