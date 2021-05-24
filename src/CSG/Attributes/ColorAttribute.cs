namespace CSG.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Specifies that a data field value is a color.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ColorAttribute : DataTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorAttribute"/> class.
        /// </summary>
        public ColorAttribute()
            : base(DataType.Custom)
        {
        }
    }
}
