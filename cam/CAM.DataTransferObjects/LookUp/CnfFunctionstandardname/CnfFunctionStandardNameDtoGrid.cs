using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.CnfFunctionstandardname
{
    public class CnfFunctionStandardNameDtoGrid
    {
        [Default]
        public long FunctionStandardNameId { get; set; }
        [Default]
        public string FunctionName { get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }
    }
}
