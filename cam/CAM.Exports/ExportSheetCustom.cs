using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Exports
{
    public class ExportSheetCustom
    {
        public String TabName { get; set; }
        public List<List<ExportSheetCustomHeader>> CustomHeaders { get; set; }
        public List<List<ExportSheetCustomCell>> Data { get; set; }
    }
}
