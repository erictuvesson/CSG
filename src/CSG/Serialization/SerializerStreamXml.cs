namespace CSG.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Xml;

    public class SerializerStreamXml : SerializerStreamDataContract<DataContractSerializer>
    {
        public readonly DataContractResolver Resolver;

        public SerializerStreamXml(IEnumerable<Assembly> assemblies = null)
            : this(new SerializerDataContractResolver(assemblies))
        {
            
        }

        public SerializerStreamXml(DataContractResolver resolver)
            : base(null)
        {
            this.Resolver = resolver;
        }

        protected override T ReadObject<T>(DataContractSerializer serializer,
            XmlDictionaryReader stream)
        {
            return (T)serializer.ReadObject(stream, true, Resolver);
        }

        protected override void WriteObject<T>(DataContractSerializer serializer,
            XmlDictionaryWriter stream, T value)
        {
            serializer.WriteObject(stream, value, Resolver);
        }
    }
}
