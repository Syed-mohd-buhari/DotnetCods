using System;
using ClosedXML.Excel;

namespace CAM.DataAttributes.Export
{
    public class FormatAttribute : Attribute
    {
        public string FormatType { get; set; }

        public string Format { get; set; }
    }  
     
    public class FormatClosetXmlAttribute : Attribute
    {
        public XLDataType Type { get; set; }
    }  
    

}
