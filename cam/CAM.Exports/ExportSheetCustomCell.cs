using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Exports
{
    public class ExportSheetCustomCell
    {
        public object Value { get; set; }
        public int? ForeColor { get; set; }
        public int? BackgroundColor { get; set; }
        public bool? Bold { get; set; }
        public ExportDataTypeEnum? FormatTypeInfo { get; set; }
        public string FormatInfo { get; set; }
        public string CellColor { get; set; }
    }
}
