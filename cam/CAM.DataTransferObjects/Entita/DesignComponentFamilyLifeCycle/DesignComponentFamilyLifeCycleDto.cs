using CAM.DataAttributes.Grid;
using System;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.DesignComponentFamilyLifeCycle
{
    public class DesignComponentFamilyLifeCycleDto
    {

        [OrderGrid(Order = 2)]
        [Default]
        public string DesignComponentFamilyName { get; set; }

        [OrderGrid(Order = 3)]
        [Default]
        public string ResourceKey { get; set; }

        [OrderGrid(Order = 4)]
        [IgnoreGrid]
        public string DesignComponentName { get; set; }

        [OrderGrid(Order = 5)]
        public long DcfId { get; set; }

        [OrderGrid(Order = 6)]
        [Default]
        public string PreviousResourceKey { get; set; }

        [OrderGrid(Order = 7)]
        [DisplayName("Current Entity")]
        public string CurrentDetail { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Planned Entity")]
        [IgnoreGrid]
        public string PlannedDetail { get; set; }

        [OrderGrid(Order = 9)]
        [Default]
        [DisplayName("Element/Component Name")]
        public string EventName { get; set; }

        [OrderGrid(Order = 10)]
        [Default]
        [DisplayName("Category Type")]
        public string CategoryType { get; set; }

        [OrderGrid(Order = 11)]
        public long DcId { get; set; }


        [OrderGrid(Order = 12)]
        public long? EventId { get; set; }
       
        [OrderGrid(Order = 13)]
        public string Notes { get; set; }
        [OrderGrid(Order = 14)]
        public string CreationUser { get; set; }
        [OrderGrid(Order = 15)]
        [DateRangeGrid]
        public DateTime CreationDate { get; set; }
        [OrderGrid(Order = 16)]
        public string ModificationUser { get; set; }
        [OrderGrid(Order = 17)]
        [DateRangeGrid]
        public DateTime ModificationDate { get; set; }
        [OrderGrid(Order = 18)]
        [IgnoreGrid]
        public long NodeIndex { get; set; }
      
    }
    public class DesignComponentFamilyLifeCycleDtoGrid : DesignComponentFamilyLifeCycleDto
    {
        [OrderGrid(Order = 9)]
        public long DcfLifeCycleId { get; set; }

        [IgnoreGrid]
        public short OpCoId { get; set; }

        [OrderGrid(Order = 1)]
        [Default]
        public string OpCo { get; set; }




    }
}
