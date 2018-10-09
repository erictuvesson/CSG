namespace CSG.Serialization
{
    public abstract class SerializerStream
    {
        public abstract T Deserialize<T>(byte[] value);
        public abstract T DeserializeContent<T>(string value);

        public abstract byte[] Serialize<T>(T value);
        public abstract string SerializeContent<T>(T value);
    }
}
