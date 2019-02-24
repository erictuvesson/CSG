namespace CSG.Shapes
{
    using System;
    using System.Numerics;

    class Dome : Shape, IEquatable<Dome>
    {
        protected override void OnBuild(IShapeBuilder builder)
        {
            throw new System.NotImplementedException();
        }

        public bool Equals(Dome other)
        {
            return base.Equals(other as Shape);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals(obj as Dome);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
