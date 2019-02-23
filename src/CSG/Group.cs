namespace CSG
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class Group : IEquatable<Group>
    {
        public ShapeOperation Operation { get; set; }

        public Shape Value1 { get; set; }

        public Shape Value2 { get; set; }

        public Group(ShapeOperation operation, Shape value1, Shape value2)
        {
            this.Operation = operation;
            this.Value1 = value1;
            this.Value2 = value2;
        }
        
        public GeneratedShape Do()
        {
            if (Value1 == null || Value2 == null)
            {
                return null;
            }

            return Value1.Do(Operation, Value2);
        }

        public bool Equals(Group other)
        {
            return Value1.Equals(other.Value1) && 
                   Value2.Equals(other.Value2) && 
                   Operation == other.Operation;
        }
    }
}
