namespace CSG
{
    using System.Runtime.Serialization;

    public class Prefab : ISerializable
    {
        public string Name { get; private set; }

        public Prefab(string name = "")
        {
            this.Name = name;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
        }
    }
}
