namespace CSG
{
    using Veldrid;
    using Veldrid.Rendering;

    public class ShapeGeometry : Geometry<Vertex, ushort>
    {
        public Shape Shape
        {
            get => shape;
            set
            {
                if (shape != value)
                {
                    this.shape = value;
                    Rebuild();
                }
            }
        }

        private Shape shape;

        public ShapeGeometry(DrawingContext drawingContext, Shape shape)
            : base(drawingContext, shape.Vertices, shape.Indices)
        {

        }

        public void Rebuild()
        {
            this.Update(shape.Vertices, shape.Indices);
        }
    }
}
