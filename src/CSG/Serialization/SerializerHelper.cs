namespace CSG.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;

    public static class SerializerHelper
    {
        public static IEnumerable<Assembly> DependencyAssemblies()
        {
            return new Assembly[] {
                Assembly.GetEntryAssembly(),
                typeof(SerializerHelper).Assembly,
                typeof(System.Numerics.Vector2).Assembly,
                typeof(System.String).Assembly
            };
        }

        public static IEnumerable<Type> DependencyTypes()
        {
            return new Type[] {
                typeof(System.Numerics.Vector2),
                typeof(System.Numerics.Vector3),
                typeof(System.Numerics.Vector4),
                typeof(System.Numerics.Matrix3x2),
                typeof(System.Numerics.Matrix4x4),
                typeof(System.Numerics.Quaternion),
                typeof(System.Numerics.Plane)
            };
        }

        public static IEnumerable<Type> GetShapeTypes(Assembly assembly = null)
        {
            return (assembly ?? ThisAssembly()).FindDerivedTypes<Shape>();
        }

        public static IEnumerable<Type> GetSerializableTypes(Assembly assembly = null)
        {
            return (assembly ?? ThisAssembly()).FindByAttribute<SerializableAttribute>();
        }

        internal static Assembly ThisAssembly() => Assembly.GetAssembly(typeof(Shape));
    }
}
