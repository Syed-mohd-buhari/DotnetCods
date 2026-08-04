using System;

namespace CAM.DataAttributes.Export
{
    public class IgnoreAttribute : Attribute
    {
        public bool Ignore { get; set; }
    }
}
