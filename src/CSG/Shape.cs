namespace CSG
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.Serialization;
    using CSG.Serialization;

    [Serializable]
    public abstract partial class Shape : IEquatable<Shape>, ISerializable
    {
        public string Name { get; set; }

        public Vector4 Color
        {
            get => color;
            set
            {
                if (this.color != value)
                {
                    this.color = value;
                    Invalidate();
                }
            }
        }

        public Vertex[] Vertices => Cache.Vertices;
        public ushort[] Indices => Cache.Indices;

        public ShapeCache Cache => cache ?? (cache = BuildCache()).Value;

        private ShapeCache? cache = null;
        private Vector4 color = Vector4.One;

        protected Shape()
        {

        }

        protected Shape(SerializationInfo info, StreamingContext context)
        {
            this.Name = info.GetString("name");
            this.Color = info.GetValue<Vector4>("color");
        }

        /// <summary>
        /// Create polygons of the <see cref="Shape"/>.
        /// </summary>
        public virtual Polygon[] CreatePolygons() => Cache.CreatePolygons();

        /// <summary>
        /// Invalidates the cache. This will force it to rebuild next time it is accessed.
        /// </summary>
        public void Invalidate()
        {
            this.cache = null;
        }
        
        protected abstract void OnBuild(IShapeBuilder builder);

        private ShapeCache BuildCache()
        {
            var builder = ShapeBuilderPool.CurrentBuilder();

            // Make sure that it is clean.
            builder.Clear();

            // Build the shape.
            OnBuild(builder);

            // Generate the shape cache.
            var cache = builder.CreateCache();

            // Clear the currently builder so we dont waste memory.
            builder.Clear();

            return cache;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("color", color);
        }

        public bool Equals(Shape other) => Name == other.Name && Color == other.Color;
    }
}
