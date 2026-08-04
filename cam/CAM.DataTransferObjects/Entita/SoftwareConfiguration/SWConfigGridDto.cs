using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.Entita.SoftwareConfiguration
{
    public class SWConfigGridDto
    {

        public decimal SoftwareConfigurationId { get; set; }
        [Default]
        [OrderGrid(Order= 3)]
        public string ElementName { get; set; }
        [Default]
        [OrderGrid(Order = 1)]

        public string OpCo { get; set; }
        [Default]
        [OrderGrid(Order = 2)]

        public string Oem { get; set; }

        public string creationUser { get; set; }
        [DateRangeGrid]
        public DateTime creationDate { get; set; }       
        public string modificationUser { get; set; }
        [DateRangeGrid]
        public DateTime modificationDate { get; set; }

        public decimal FunctionId { get; set; }

        [Default]
        public string FunctionName { get; set; }
        public decimal swConfigFunctionAreaId { get; set; }
        [Default]
        public string FunctionAreaName { get; set; }
        [Default]
        public string FunctionAreaDescription { get; set; }
    }
}
