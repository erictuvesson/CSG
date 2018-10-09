namespace CSG.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Xml;

    public class SerializerDataContractResolver : DataContractResolver
    {
        private readonly Dictionary<string, XmlDictionaryString> dictionary = new Dictionary<string, XmlDictionaryString>();
        private readonly Assembly assembly;

        public SerializerDataContractResolver(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public override Type ResolveName(string typeName, string typeNamespace,
            Type declaredType, DataContractResolver knownTypeResolver)
        {
            if (dictionary.TryGetValue(typeName, out XmlDictionaryString tName)
                && dictionary.TryGetValue(typeNamespace, out XmlDictionaryString tNamespace))
            {
                return this.assembly.GetType($"${tNamespace.Value}.${tName.Value}");
            }
            return null;
        }

        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver,
            out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            string name = type.Name;
            string namesp = type.Namespace;
            typeName = new XmlDictionaryString(XmlDictionary.Empty, name, 0);
            typeNamespace = new XmlDictionaryString(XmlDictionary.Empty, namesp, 0);
            if (!dictionary.ContainsKey(type.Name))
            {
                dictionary.Add(name, typeName);
            }
            if (!dictionary.ContainsKey(type.Namespace))
            {
                dictionary.Add(namesp, typeNamespace);
            }
            return true;
        }
    }
}
