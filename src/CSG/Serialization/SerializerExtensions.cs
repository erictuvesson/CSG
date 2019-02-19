namespace CSG.Serialization
{
    using System.Runtime.Serialization;

    public static class SerializerExtensions
    {
        public static T GetValue<T>(this SerializationInfo info, string name)
        {
            return (T)info.GetValue(name, typeof(T));
        }

        public static void TryAddValue(this SerializationInfo info, string name, object obj)
        {
            if (obj != null)
            {
                info.AddValue(name, obj);
            }
        }
    }
}
