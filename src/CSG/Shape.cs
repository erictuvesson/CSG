﻿namespace CSG
{
    using CSG.Algorithms;
    using CSG.Attributes;
    using System;
    using System.ComponentModel;
    using System.Numerics;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class Shape : IEquatable<Shape>, ISerializable
    {
        [Category("Default")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the default color.
        /// </summary>
        [Category("Material")]
        [Color]
        public Vector4 Color
        {
            get => this.color;
            set
            {
                if (this.color != value)
                {
                    this.color = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the shape position.
        /// </summary>
        [Category("Transform")]
        public Vector3 Position
        {
            get => this.position;
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the shape scale.
        /// </summary>
        [Category("Transform")]
        public Vector3 Scale
        {
            get => this.scale;
            set
            {
                if (this.scale != value)
                {
                    this.scale = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the vertices from the <see cref="Shape"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Vertex[] Vertices => Cache.Vertices;

        /// <summary>
        /// Gets the indices from the <see cref="Shape"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public uint[] Indices => Cache.Indices;

        /// <summary>
        /// Gets the <see cref="ShapeCache"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ShapeCache Cache => cache ?? (cache = BuildCache()).Value;

        private ShapeCache? cache = null;
        private Vector4 color = Vector4.One;
        private Vector3 position = Vector3.Zero;
        private Vector3 scale = Vector3.One;

        protected Shape()
        {
        }

        protected Shape(SerializationInfo info, StreamingContext context)
        {
            this.Name = info.GetString("name");
            this.Position = (Vector3)info.GetValue("position", typeof(Vector3));
            this.Scale = (Vector3)info.GetValue("scale", typeof(Vector3));
            this.Color = (Vector4)info.GetValue("color", typeof(Vector4));
        }

        public GeneratedShape Do(ShapeOperation operation, Shape other)
            => Do(operation, this, other);

        public GeneratedShape Union(Shape other)
            => Union(this, other);

        public GeneratedShape Subtract(Shape other)
            => Subtract(this, other);

        public GeneratedShape Intersect(Shape other)
            => Intersect(this, other);

        /// <summary>
        /// Create polygons of the <see cref="Shape"/>.
        /// </summary>
        public virtual Polygon[] CreatePolygons()
            => Cache.CreatePolygons();

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
            builder.DefaultColor = this.Color;
            builder.LocalPosition = this.Position;
            builder.LocalScale = this.Scale;

            // Make sure that it is clean.
            builder.Clear();

            // Build the shape.
            OnBuild(builder);

            // Generate the shape cache.
            var tmpCache = builder.CreateCache();

            // Clear the currently builder so we dont waste memory.
            builder.Clear();

            return tmpCache;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", this.Name);
            info.AddValue("position", this.Position);
            info.AddValue("scale", this.Scale);
            info.AddValue("color", this.color);
        }

        public bool Equals(Shape other)
        {
            return this.Name == other.Name &&
                   this.Position == other.Position &&
                   this.Scale == other.Scale &&
                   this.Color == other.Color;
        }

        public static GeneratedShape Do(ShapeOperation operation, Shape lhs, Shape rhs)
        {
            switch (operation)
            {
                default:
                case ShapeOperation.Intersect:
                    return Intersect(lhs, rhs);

                case ShapeOperation.Subtract:
                    return Subtract(lhs, rhs);

                case ShapeOperation.Union:
                    return Union(lhs, rhs);
            }
        }

        public static GeneratedShape Union(Shape lhs, Shape rhs)
        {
            var a = new BSPNode(lhs.CreatePolygons());
            var b = new BSPNode(rhs.CreatePolygons());
            var polygons = BSPNode.Union(a, b).AllPolygons();
            return new GeneratedShape(polygons);
        }

        public static GeneratedShape Subtract(Shape lhs, Shape rhs)
        {
            var a = new BSPNode(lhs.CreatePolygons());
            var b = new BSPNode(rhs.CreatePolygons());
            var polygons = BSPNode.Subtract(a, b).AllPolygons();
            return new GeneratedShape(polygons);
        }

        public static GeneratedShape Intersect(Shape lhs, Shape rhs)
        {
            var a = new BSPNode(lhs.CreatePolygons());
            var b = new BSPNode(rhs.CreatePolygons());
            var polygons = BSPNode.Intersect(a, b).AllPolygons();
            return new GeneratedShape(polygons);
        }
    }
}
