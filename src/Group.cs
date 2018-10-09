namespace CSG
{
    using CSG.Shapes;
    using System.Runtime.Serialization;

    [DataContract]
    public class Group
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

        public GeneratedShape Do()
        {
            if (Shape1 == null || Shape2 == null)
                return null;

            return Shape1.Do(Operation, Shape2);
        }
    }
}
