using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.PAT
{
    public class PATExportQuery : QueryObject
    {
        public List<short> OpCoId { get; set; }
        public List<long> DCFId { get; set; }
        
        public List<short> VendorIds { get; set; }
        public List<string> ProductName { get; set; }

        public int? BuildConstruction { get; set; }

        public List<int> VerticalNameId { get; set; }
        public List<string> VerticalName  { get; set; }

        public bool IsEosDateEnable { get; set; }
    }
}
