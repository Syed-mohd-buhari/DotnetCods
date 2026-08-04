using CAM.DataAttributes.Grid;
using CAM.Entities.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CAM.DataTransferObjects.Entita.PlannedActivity
{
    public class ArchivedPlannedActivityDtoGrid
    {
        [OrderGrid(Order = 1)]
        [DisplayName("Planned Activity Type")]
        [Default]
        public string PlannedActivityResourceId { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("OpCo")]
        [Default]
        public string OpCo { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("DCF Name")]
        [Default]
        public string DesignComponentFamilyName { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("Current Bag Name")]
        public string CurrentBuildBagDescription { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("Planned Bag Name")]
        public string PlannedBuildBagDescription { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("Planned Activity Index")]
        [Default]
        public long PlannedActivityId { get; set; }

        [OrderGrid(Order = 7)]
        [DisplayName("Original Design Component")]
        [Default]
        public string OriginalDesignComponent { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Planned Design Component")]
        [Default]
        public string PlannedActivityDesignComponentId { get; set; }

        [OrderGrid(Order = 9)]
        [DisplayName("Activity Details")]
        [Default]
        public string ActivityDetails { get; set; }

        [OrderGrid(Order = 10)]
        [DisplayName("Implementation Year")]
        [Default]
        public string PlannedImplementationYear { get; set; }

        [OrderGrid(Order = 11)]
        [DateRangeGrid]
        [DisplayName("Start Date")]
        [Default]
        public string StartDateValue { get; set; }

        [OrderGrid(Order = 12)]
        [DateRangeGrid]
        [DisplayName("Planned Completion")]
        [Default]
        public string PlannedCompletion { get; set; }

        [OrderGrid(Order = 13)]
        [DisplayName("Archived")]
        public bool? Archived { get; set; } = false;

        [OrderGrid(Order = 14)]
        [DisplayName("Vertical Name")]

        public string VerticalName { get; set; }

        [IgnoreGrid]
        public List<string> VerticalNameId { get; set; }

        ///Ticket 793 PPM ID value should present for Archived PAs too in LCM export
        [OrderGrid(Order = 15)]
        [DisplayName("PPM ID")]
        public string DeliveryProjectPpmId { get; set; }

        [IgnoreGrid]
        public List<FilterValueDtoKeyValueList> VerticalFilterDto { get; set; }
        [DateRangeGrid]
        public DateTime ModificationDate { get; set; }
    }
}
