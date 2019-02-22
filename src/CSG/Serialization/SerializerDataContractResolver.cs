namespace CSG.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Linq;

    public class SerializerDataContractResolver : DataContractResolver
    {
        private readonly IEnumerable<Assembly> assemblies;

        public SerializerDataContractResolver(IEnumerable<Assembly> assemblies = null)
        {
            this.assemblies = (assemblies ?? new Assembly[0]).Concat(SerializerHelper.DependencyAssemblies());
        }

        public override Type ResolveName(string typeName, string typeNamespace,
            Type declaredType, DataContractResolver knownTypeResolver)
        {
            return TryGetType($"{typeNamespace}.{typeName}");
        }

        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver,
            out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            if (TryGetType($"{type.Namespace}.{type.Name}") != null)
            {
                typeName = new XmlDictionaryString(XmlDictionary.Empty, type.Name, 0);
                typeNamespace = new XmlDictionaryString(XmlDictionary.Empty, type.Namespace, 0);
                return true;
            }
            else
            {
                typeName = null;
                typeNamespace = null;
                return false;
            }
        }

        private Type TryGetType(string typeName)
        {
            foreach (var assembly in this.assemblies)
            {
                Type type = assembly.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }
    }
}
