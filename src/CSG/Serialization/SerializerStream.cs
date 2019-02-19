namespace CSG.Serialization
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public abstract class SerializerStream
    {
        public readonly IEnumerable<Type> KnownTypes;

        protected SerializerStream(IEnumerable<Type> knownTypes)
        {
            this.KnownTypes = knownTypes.Concat(SerializerHelper.DependencyTypes());
        }

        public abstract T Deserialize<T>(byte[] value);
        public abstract T DeserializeContent<T>(string value);

        public abstract byte[] Serialize<T>(T value);
        public abstract string SerializeContent<T>(T value);
    }
}
