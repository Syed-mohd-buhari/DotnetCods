using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.LcmEngineering
{
    public class ArchivedLcmengineeringDtoGrid : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [DisplayName("OpCo")]
        [Default]
        public string OpCo { get; set; }



        [OrderGrid(Order = 2)]
        [DisplayName("Resource Key")]
        [Default]
        public string ResourceKey { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Design Component")]
        [Default]
        public string DesignComponent { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("DC Index")]
        [Default]
        public long DesignComponentId { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("LCM Engineering Index")]
        [Default]
        public long LcmEngineeringId { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("Design Component Family")]
        [Default]
        public string DesignComponentFamily { get; set; }



        [OrderGrid(Order = 7)]
        [DisplayName("Vodafone Name")]
        [Default]
        public string VodafoneName { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Subnetwork Boundary")]
        [Default]
        public string SubnetworkBoundary { get; set; }

        [IgnoreGrid]
        public DateTime? StartDate { get; set; }

        [OrderGrid(Order = 9)]
        [DateRangeGrid]
        [DisplayName("Start Date")]
        [Default]
        public string StartDateValue { get; set; }

        [OrderGrid(Order = 10)]
        [DateRangeGrid]
        [DisplayName("End Date")]
        [Default]
        public string EndDateValue { get; set; }

        [IgnoreGrid]
        public DateTime? EndDate { get; set; }

        [OrderGrid(Order = 11)]
        [DisplayName("Archived")]
        public bool Archived { get; set; }

        [OrderGrid(Order = 12)]
        [DateRangeGrid]
        [DisplayName("Last Modified")]
        [Default]
        public string LastModifiedValue { get; set; }

        [IgnoreGrid]
        public DateTime? LastModified { get; set; }

        [OrderGrid(Order = 13)]
        [DisplayName("Lcm Deployment Status")]
        [Default]
        public string LcmDeploymentStatus { get; set; }

        [OrderGrid(Order = 14)]
        [DisplayName("Vertical Name")]
        [Default]
        public string VerticalName { get; set; }

        [DisplayName("Vertical Id")]
        [IgnoreGrid]
        public int VerticalId { get; set; }

        [OrderGrid(Order = 15)]
        [DisplayName("N° Nodes In Prod")]
        [Default]
        public int? NumberOfNodes { get; set; }


        [OrderGrid(Order = 16)]
        [DisplayName("N° Nodes In Lab")]
        [Default]
        public int? NumberOfNodesInLab { get; set; }

        [OrderGrid(Order = 17)]
        [DisplayName("Bag Name")]
        [Default]
        public string BuildBagDescription { get; set; }
    }
}
