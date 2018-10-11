namespace CSG
{
    using CSG.Serialization;
    using CSG.Shapes;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Group : IEquatable<Group>
    {
        [DataMember]
        public ShapeOperation Operation { get; set; }

        [DataMember]
        public Shape Shape1 { get; set; }

        [DataMember]
        public Shape Shape2 { get; set; }

        public Group(ShapeOperation operation, Cube shape1, Cube shape2)
        {
            this.Operation = operation;
            this.Shape1 = shape1;
            this.Shape2 = shape2;
        }

        public Group(SerializationInfo info, StreamingContext context)
        {
            Enum.TryParse<ShapeOperation>(info.GetString("operation"), out var operation);
            this.Operation = operation;

            this.Shape1 = info.GetValue<Shape>("shape1");
            this.Shape2 = info.GetValue<Shape>("shape2");
        }

        public GeneratedShape Do()
        {
            if (Shape1 == null || Shape2 == null)
                return null;

            return Shape1.Do(Operation, Shape2);
        }

        #region ISerializable
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("operation", Operation.ToString());
            info.AddValue("shape1", Shape1);
            info.AddValue("shape2", Shape2);
        }
        #endregion

        public bool Equals(Group other)
        {
            return Shape1.Equals(other.Shape1) && Shape2.Equals(other.Shape2)
                   && Operation == other.Operation;
        }
    }
}
