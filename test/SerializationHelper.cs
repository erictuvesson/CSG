namespace CSG
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    public static class SerializationHelper
    {
        public static T DeserializeContent<T>(string value)
        {
            var byteArray = Encoding.UTF8.GetBytes(value);
            using (var fs = new MemoryStream(byteArray))
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(fs);
            }
        }

        public static string SerializeContent<T>(T value)
        {
            using (var fs = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(fs, value);
                return Encoding.ASCII.GetString(fs.ToArray());
            }
        }
    }
}
