using System;
using System.Collections.Generic;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp.Component
{

    public class ComponentDto
    {
        [OrderGrid(Order = 1)]
        [Default]
        public string OpCo { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        public string Oem { get; set; }
        [OrderGrid(Order = 3)]
        [Default]
        public string ElementName { get; set; }
        [OrderGrid(Order = 7)]
        [Default]
        public string MainSoftwareVersion { get; set; }
        [OrderGrid(Order = 8)]
        [Default]
        public string ComponentName { get; set; }
        [OrderGrid(Order = 9)]
        [Default]
        [DateRangeGrid]
        public DateTime? ProductionDate { get; set; }
        [OrderGrid(Order = 10)]
        [Default]
        public string ProductionNumber { get; set; }
        [OrderGrid(Order = 11)]
        [Default]
        public string ProductionRevision { get; set; }
        [OrderGrid(Order = 16)]
        [MailTo]
        public string CreationUser { get; set; }
        [OrderGrid(Order = 17)]
        [DateRangeGrid]
        public DateTime CreationDate { get; set; }
        [OrderGrid(Order = 18)]
        [MailTo]
        public string ModificationUser { get; set; }
        [OrderGrid(Order = 19)]
        [DateRangeGrid]
        public DateTime ModificationDate { get; set; }
    }
    public class SoftwareComponentsDtoGrid : ComponentDto
    {
        [OrderGrid(Order =6)]
        public string NetworkElementId { get; set; }

        [OrderGrid(Order = 5)]
        public long SoftwareComponentId { get; set; }
        [OrderGrid(Order = 4)]
        public long ComponentId { get; set; }

    }
}