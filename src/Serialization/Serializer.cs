namespace CSG.Serialization
{
    using System;
    using System.Collections.Generic;

    public class Serializer
    {
        private readonly SerializerStream stream;
        private readonly Dictionary<string, object> dictionary;

        public Serializer(IEnumerable<Type> knownTypes = null)
        {
            stream = new SerializerStreamJson(knownTypes);
            dictionary = new Dictionary<string, object>();
        }

        public void Write(string key, object value)
        {
            dictionary.Add(key, value);
        }

        public byte[] Serialize() => stream.Serialize(dictionary);
        public string SerializeContent() => stream.SerializeContent(dictionary);
    }
}
