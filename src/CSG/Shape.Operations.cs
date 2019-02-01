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
        public GeneratedShape Do(ShapeOperation operation, Shape other)
        {
            switch (operation)
            {
                default:
                case ShapeOperation.Intersect:  return Intersect(other);
                case ShapeOperation.Subtract:   return Subtract(other);
                case ShapeOperation.Union:      return Union(other);
            }
        }

        public GeneratedShape Union(Shape other) => Union(this, other);
        public GeneratedShape Subtract(Shape other) => Subtract(this, other);
        public GeneratedShape Intersect(Shape other) => Intersect(this, other);

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
