namespace CSG.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization.Json;

    public class SerializerStreamJson : SerializerStreamDataContract<DataContractJsonSerializer>
    {
        public SerializerStreamJson(IEnumerable<Type> knownTypes)
            : base(knownTypes)
        {

        }
    }
}
