using System;

namespace CAM.DataAttributes.Export
{
    public class ColorAttribute : Attribute
    {
        public Int32 BackgroundColor { get; set; }
        public Int32 FontColor { get; set; }

    }
}
