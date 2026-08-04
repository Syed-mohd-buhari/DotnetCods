using System;
using System.Collections.Generic;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp.SubFunction
{

    public class SubFunctionDto
    {
        [OrderGrid(Order = 4)]
        [Default]
        public string OpCo { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        public string Oem { get; set; }
        [OrderGrid(Order = 6)]
        [Default]
        public string ElementName { get; set; }
        [OrderGrid(Order = 7)]
        [Default]
        public string mainSoftwareVersion { get; set; }
        [OrderGrid(Order = 8)]
        [Default]
        public string componentName { get; set; }
        [OrderGrid(Order = 9)]
        [Default]
        [DateRangeGrid]
        public DateTime? productionDate { get; set; }
        [OrderGrid(Order = 10)]
        [Default]
        public string productionNumber { get; set; }
        [OrderGrid(Order = 11)]
        [Default]
        public string productionRevision { get; set; }
        [OrderGrid(Order = 12)]
        public string creationUser { get; set; }
        [OrderGrid(Order = 13)]
        [DateRangeGrid]
        public DateTime? creationDate { get; set; }
        [OrderGrid(Order = 14)]
        public string modificationUser { get; set; }
        [OrderGrid(Order = 15)]
        [DateRangeGrid]
        public DateTime modificationDate { get; set; }
        [OrderGrid(Order = 16)]
        [Default]
        public string componentCreationUser { get; set; }
        [OrderGrid(Order = 17)]
        [Default]
        [DateRangeGrid]
        public DateTime componentCreationDate { get; set; }
        [OrderGrid(Order = 18)]
        [Default]
        public string componentModificationUser { get; set; }
        [OrderGrid(Order = 19)]
        [Default]
        [DateRangeGrid]
        public DateTime componentModificationDate { get; set; }
    }
    public class SubFunctionDtoGrid : SubFunctionDto
    {
        [OrderGrid(Order =1)]
        public string networkElementId { get; set; }

        [OrderGrid(Order = 2)]
        public long softwareComponentId { get; set; }
        [OrderGrid(Order = 3)]
        public long componentId { get; set; }

    }
}