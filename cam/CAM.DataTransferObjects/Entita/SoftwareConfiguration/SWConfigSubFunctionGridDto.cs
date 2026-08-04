using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.Entita.SoftwareConfiguration
{
    public class SWConfigSubFunctionGridDto
    {
        public int SubFunctionId { get; set; }
        [Default]
        public string SubFunctionName { get; set;}
        public int SubFunctionAreaId { get; set; }
        [Default]
        public string SubFuncAreaName { get; set; }
        [Default]
        public string SubFuncAreaDescription { get; set; }
        public string CreationUser { get; set; }
        [DateRangeGrid]
        public DateTime CreationDate { get; set; }
        public string ModificationUser { get; set; }
        [DateRangeGrid]
        public DateTime ModificationDate {  get; set; }
    }
}
