namespace CSG
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract partial class Shape : IEquatable<Shape>
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
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

        public Vertex[] Vertices => vertices.ToArray();
        public ushort[] Indices => indices.ToArray();

        public ShapeCache Cache => cache ?? (cache = BuildCache()).Value;

        private ShapeCache? cache = null;
        private Vector4 color = Vector4.One;
        private readonly List<Vertex> vertices = new List<Vertex>();
        private readonly List<ushort> indices = new List<ushort>();

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

        public bool Equals(Shape other) => Name == other.Name && Color == other.Color;
    }
}
