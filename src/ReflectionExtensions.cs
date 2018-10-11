namespace CSG
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> FindDerivedTypes<T>(this Assembly assembly)
            => assembly.FindDerivedTypes(typeof(T));

        public static IEnumerable<Type> FindDerivedTypes(this Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(t => t != baseType && baseType.IsAssignableFrom(t));
        }

        public static IEnumerable<Type> FindByAttribute<T>(this Assembly assembly)
            => assembly.FindByAttribute(typeof(T));

        public static IEnumerable<Type> FindByAttribute(this Assembly assembly, Type findType)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(findType, true).Length > 0)
                {
                    yield return type;
                }
            }
        }
    }
}
