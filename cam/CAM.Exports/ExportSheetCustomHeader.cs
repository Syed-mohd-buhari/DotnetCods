using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Exports
{
    public class ExportSheetCustomHeader
    {
        public string Name { get; set; }
        //public int RowSpan { get; set; }
        public int ColSpan { get; set; }
        public bool Show { get; set; }
        public int? ForeColor { get; set; }
        public int? BackgroundColor { get; set; }
        public TextAlignmentEnum Alignment { get; set; }
    }
}
