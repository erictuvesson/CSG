namespace CSG.Algorithms
{
    using CSG.Exceptions;
    using System.Collections.Generic;

    class BSPNode
    {
        // Throw an exception with the max depth is reached.
        // Have to look into what is the most optimal max depth.
        // 
        // This can also be an issue when computing very big shapes using BSP.
        const int MAX_DEPTH = 1000;

        public List<Polygon> Polygons;

        public readonly BSPNode Parent;
        public readonly int Depth;

        public BSPNode Front;
        public BSPNode Back;

        public Plane Plane;

        public BSPNode(BSPNode parent)
        {
            this.Parent = parent;
            this.Depth = parent != null ? parent.Depth + 1 : 0;
            this.Front = null;
            this.Back = null;
        }

        public BSPNode(IEnumerable<Polygon> list)
        {
            this.Parent = null;
            this.Depth = 0;

            Build(new List<Polygon>(list));
        }

        public BSPNode(BSPNode parent, List<Polygon> list, Plane plane, BSPNode front, BSPNode back)
        {
            this.Parent = parent;
            this.Depth = parent != null ? parent.Depth + 1 : 0;
            this.Polygons = list;
            this.Plane = plane;
            this.Front = front;
            this.Back = back;
        }

        public BSPNode Clone(BSPNode parent = null)
        {
            return new BSPNode(parent, this.Polygons, this.Plane, this.Front, this.Back);
        }

        public void ClipTo(BSPNode other)
        {
            this.Polygons = other.ClipPolygons(this.Polygons);
            this.Front?.ClipTo(other);
            this.Back?.ClipTo(other);
        }

        public void Invert()
        {
            for (int i = 0; i < this.Polygons.Count; i++)
            {
                this.Polygons[i].Flip();
            }

            this.Plane.Flip();
            this.Front?.Invert();
            this.Back?.Invert();

            var tmp = this.Front;
            this.Front = this.Back;
            this.Back = tmp;
        }

        public void Build(List<Polygon> list)
        {
            if (list.Count < 1)
            {
                return;
            }

            if (this.Plane == null || !this.Plane.Valid())
            {
                this.Plane = new Plane
                {
                    Normal = list[0].Plane.Normal,
                    W = list[0].Plane.W
                };
            }

            if (this.Polygons == null)
            {
                this.Polygons = new List<Polygon>();
            }

            // TODO: Optimize!
            var list_front = new List<Polygon>();
            var list_back = new List<Polygon>();

            for (int i = 0; i < list.Count; i++)
            {
                this.Plane.SplitPolygon(list[i], this.Polygons, this.Polygons, list_front, list_back);
            }
            
            if (list_front.Count > 0)
            {
                if (this.Front == null)
                    this.Front = new BSPNode(this);

                var depth = this.Front.Depth;
                if (depth >= MAX_DEPTH)
                    throw new BSPNodeMaxDepthException();

                this.Front.Build(list_front);
            }

            if (list_back.Count > 0)
            {
                if (this.Back == null)
                {
                    this.Back = new BSPNode(this);
                }

                var depth = this.Back.Depth;
                if (depth >= MAX_DEPTH)
                    throw new BSPNodeMaxDepthException();

                this.Back.Build(list_back);
            }
        }

        public List<Polygon> ClipPolygons(List<Polygon> list)
        {
            if (!this.Plane.Valid())
                return list;

            var list_front = new List<Polygon>();
            var list_back = new List<Polygon>();

            for (int i = 0; i < list.Count; i++)
            {
                this.Plane.SplitPolygon(list[i], list_front, list_back, list_front, list_back);
            }

            if (this.Front != null)
            {
                list_front = this.Front.ClipPolygons(list_front);
            }

            if (this.Back != null)
            {
                list_back = this.Back.ClipPolygons(list_back);
            }
            else
            {
                list_back.Clear();
            }

            list_front.AddRange(list_back);

            return list_front;
        }

        public List<Polygon> AllPolygons()
        {
            var list = this.Polygons ?? new List<Polygon>();
            var list_front = new List<Polygon>();
            var list_back = new List<Polygon>();

            if (this.Front != null)
            {
                list_front = this.Front.AllPolygons();
            }

            if (this.Back != null)
            {
                list_back = this.Back.AllPolygons();
            }

            list.AddRange(list_front);
            list.AddRange(list_back);

            return list;
        }


        public static BSPNode Intersect(BSPNode a1, BSPNode b1)
        {
            var a = a1.Clone();
            var b = b1.Clone();

            a.ClipTo(b);
            b.ClipTo(a);
            b.Invert();
            b.ClipTo(a);
            b.Invert();
            a.Build(b.AllPolygons());

            return new BSPNode(a.AllPolygons());
        }

        public static BSPNode Subtract(BSPNode a1, BSPNode b1)
        {
            var a = a1.Clone();
            var b = b1.Clone();

            a.Invert();
            a.ClipTo(b);
            b.ClipTo(a);
            b.Invert();
            b.ClipTo(a);
            b.Invert();
            a.Build(b.AllPolygons());

            return new BSPNode(a.AllPolygons());
        }

        public static BSPNode Union(BSPNode a1, BSPNode b1)
        {
            var a = a1.Clone();
            var b = b1.Clone();

            a.Invert();
            b.ClipTo(a);
            b.Invert();
            a.ClipTo(b);
            b.ClipTo(a);
            a.Build(b.AllPolygons());
            a.Invert();

            return new BSPNode(a.AllPolygons());
        }
    }
}
