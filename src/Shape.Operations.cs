namespace CSG
{
    using CSG.Algorithms;

    public enum ShapeOperation
    {
        Intersect,
        Subtract,
        Union
    }

    public partial class Shape
    {
        public Shape Do(ShapeOperation operation, Shape other)
        {
            switch (operation)
            {
                default:
                case ShapeOperation.Intersect: return this.Intersect(other);
                case ShapeOperation.Subtract: return this.Subtract(other);
                case ShapeOperation.Union: return this.Union(other);
            }
        }

        public Shape Union(Shape other) => Union(this, other);
        public Shape Subtract(Shape other) => Subtract(this, other);
        public Shape Intersect(Shape other) => Intersect(this, other);

        public static Shape Union(Shape lhs, Shape rhs)
        {
            var a = new BSPNode(lhs.ToPolygons());
            var b = new BSPNode(rhs.ToPolygons());
            var polygons = BSPNode.Union(a, b).AllPolygons();
            return new GeneratedShape(polygons);
        }

        public static Shape Subtract(Shape lhs, Shape rhs)
        {
            var a = new BSPNode(lhs.ToPolygons());
            var b = new BSPNode(rhs.ToPolygons());
            var polygons = BSPNode.Subtract(a, b).AllPolygons();
            return new GeneratedShape(polygons);
        }

        public static Shape Intersect(Shape lhs, Shape rhs)
        {
            var a = new BSPNode(lhs.ToPolygons());
            var b = new BSPNode(rhs.ToPolygons());
            var polygons = BSPNode.Intersect(a, b).AllPolygons();
            return new GeneratedShape(polygons);
        }
    }
}
