namespace CSG.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;

    public static class SerializerHelper
    {
        public static IEnumerable<Type> GetShapeTypes(Assembly assembly = null)
        {
            return (assembly ?? ThisAssembly()).FindDerivedTypes<Shape>();
        }

        public static IEnumerable<Type> GetSerializableTypes(Assembly assembly = null)
        {
            return (assembly ?? ThisAssembly()).FindByAttribute<DataContractAttribute>();
        }

        private static Assembly ThisAssembly() => Assembly.GetAssembly(typeof(Shape));
    }
}
