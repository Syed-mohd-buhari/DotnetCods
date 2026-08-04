using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.LookUp.LcmDeploymentStatus;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Enum;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.DeliveryTracking
{
    public class DeliveryTrackingDtoGrid : DeliveryTrackingDto
    {
        [DateRangeGrid]
        [OrderGrid(Order = 26)]
        [DisplayName("Last Modified Date")]
        [Default]
        public string LastModifiedValue { get; set; }

        [IgnoreGrid]
        public DateTime? LastModified { get; set; }


    }
    public class DeliveryTrackingDto : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [DisplayName("Delivery Tracking Index")]
        [Default]
        public short Id { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Planned Activity Index")]
        [Default]
        public long? PlannedActivityId { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Activity")]
        [Default]
        public string Activity { get; set; }
      
        [OrderGrid(Order = 4)]
        [DisplayName("MS1: Event Type")]
        [Default]
        public string Ms1EventType { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("MS1: BaseLine Date")]
        [Default]
        [DateRangeGrid]
        public DateTime? Ms1BaseLineDate { get; set; }


        [OrderGrid(Order = 6)]
        [DisplayName("MS1: Latest Planning Date")]
        [Default]
        [DateRangeGrid]
        public DateTime? Ms1LatestPlanningDate { get; set; }

        [OrderGrid(Order = 7)]
        [DisplayName("MS1: Status")]
        [Default]
        public string Ms1Status { get; set; }       


        [OrderGrid(Order = 8)]
        [DisplayName("MS2: Event Type")]
        [Default]
        public string Ms2EventType { get; set; }

        [OrderGrid(Order = 9)]
        [DisplayName("MS2: BaseLine Date")]
        [Default]
        [DateRangeGrid]
        public DateTime? Ms2BaseLineDate { get; set; }


        [OrderGrid(Order = 10)]
        [DisplayName("MS2: Latest Planning Date")]
        [Default]
        [DateRangeGrid]
        public DateTime? Ms2LatestPlanningDate { get; set; }

        [OrderGrid(Order = 11)]
        [DisplayName("MS2: Status")]
        [Default]
        public string Ms2Status { get; set; }

        [OrderGrid(Order = 12)]
        [DisplayName("MS3: Event Type")]
        [Default]
        public string Ms3EventType { get; set; }

        [OrderGrid(Order = 13)]
        [DisplayName("MS3: BaseLine Date")]
        [Default]
        [DateRangeGrid]
        public DateTime? Ms3BaseLineDate { get; set; }


        [OrderGrid(Order = 14)]
        [DisplayName("MS3: Latest Planning Date")]
        [Default]
        [DateRangeGrid]
        public DateTime? Ms3LatestPlanningDate { get; set; }

        [OrderGrid(Order = 15)]
        [DisplayName("MS3: Status")]
        [Default]
        public string Ms3Status { get; set; }

        [OrderGrid(Order = 16)]
        [DisplayName("MS4: Event Type")]
        [Default]
        public int? Ms4EventType { get; set; }

        [OrderGrid(Order = 17)]
        [DisplayName("MS4: BaseLine Date")]
        [Default]
        [DateRangeGrid]
        public DateTime? Ms4BaseLineDate { get; set; }


        [OrderGrid(Order = 18)]
        [DisplayName("MS4: Latest Planning Date")]
        [Default]
        [DateRangeGrid]
        public DateTime? Ms4LatestPlanningDate { get; set; }

        [OrderGrid(Order = 19)]
        [DisplayName("MS4: Status")]
        [Default]
        public string Ms4Status { get; set; }

        [OrderGrid(Order = 20)]
        [DisplayName("PPM Import Date")]
        [Default]
        [DateRangeGrid]
        public DateTime? PpmImportDate { get; set; }

        [OrderGrid(Order = 21)]
        [DisplayName("Notes 1")]
        [Default]
        public string Notes1 { get; set; }


        [OrderGrid(Order = 22)]
        [DisplayName("Notes 2")]
        [Default]
        public string Notes2 { get; set; }

        [OrderGrid(Order = 23)]
        [DisplayName("PPM ID")]
        [Default]
        public string PpmID { get; set; }

        [OrderGrid(Order = 24)]
        [DisplayName("Description")]
        [Default]
        public string Description { get; set; }

        [OrderGrid(Order = 25)]
        [DisplayName("Activity Index")]
        [Default]
        public string ActivityIndex { get; set; }

        [IgnoreGrid]
        public string Opco { get; set; }
        [IgnoreGrid]
        public short OpcoId { get; set; }
        [IgnoreGrid]
        public string VerticalName { get; set; }
    }
}