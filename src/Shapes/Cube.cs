namespace CSG.Shapes
{
    using System.Numerics;

    public class Cube : Shape
    {
        public Vector3 Position { get; }
        public Vector3 Size { get; set; }

        public Cube(Vector3? position = null, Vector3? size = null)
        {
            this.Position = position ?? Vector3.Zero;
            this.Size = size ?? Vector3.One;

            Build();
        }

        protected override void OnBuild()
        {
            var normals = new Vector3[]
            {
                new Vector3(+0, +0, +1),
                new Vector3(+0, +0, -1),
                new Vector3(+1, +0, +0),
                new Vector3(-1, +0, +0),
                new Vector3(+0, +1, +0),
                new Vector3(+0, -1, +0),
            };

            for (int i = 0; i < normals.Length; i++)
            {
                var normal = normals[i];
                
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);
                
                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 1);
                AddIndex(CurrentVertex + 2);

                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 2);
                AddIndex(CurrentVertex + 3);

                AddVertex(new Vertex(Position + ((normal - side1 - side2) / 2) * Size, normal, Vector2.Zero));
                AddVertex(new Vertex(Position + ((normal - side1 + side2) / 2) * Size, normal, Vector2.UnitX));
                AddVertex(new Vertex(Position + ((normal + side1 + side2) / 2) * Size, normal, Vector2.One));
                AddVertex(new Vertex(Position + ((normal + side1 - side2) / 2) * Size, normal, Vector2.UnitY));
            }
        }
    }
}
