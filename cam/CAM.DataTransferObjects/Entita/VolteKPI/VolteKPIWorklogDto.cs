using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.VolteKPI
{
    public class VolteKPIWorklogDto : GridDtoBase
    {
        [IgnoreGrid]
        public long? VolteKPIWorklogId { get; set; }
        //Chiave VolteKPI
        [IgnoreGrid]
        public long VolteKPIId { get; set; }
        //Chiave Naturale
        [IgnoreGrid]
        public short OpCoId { get; set; }
        [IgnoreGrid]
        public short Month { get; set; }
        [IgnoreGrid]
        public short Year { get; set; }
        [IgnoreGrid]
        public int VolteKPIType { get; set; }

        [OrderGrid(Order = 1)]
        [Default]
        public string OpCo { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        public string KpiIdName { get; set; }
        [OrderGrid(Order = 3)]
        public string MonthYear { get; set; }
        [OrderGrid(Order = 4)]
        [Default]
        public decimal? TargetMonthlyValueOld { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        public decimal? TargetMonthlyValueProposed { get; set; }
        [OrderGrid(Order = 6)]
        [Default]
        public decimal? TargetMonthlyValueNew { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        public decimal? EoyTargetOld { get; set; }
        [OrderGrid(Order = 8)]
        [Default]
        public decimal? EoyTargetNew { get; set; }

        [OrderGrid(Order = 9)]
        [Default]
        public decimal? ActualMonthlyValueOld { get; set; }
        [OrderGrid(Order = 10)]
        [Default]
        public decimal? ActualMonthlyValueNew { get; set; }
        [OrderGrid(Order = 11)]
        [Default]
        public decimal? ActualNumberOfRegisteredOld { get; set; }
        [OrderGrid(Order = 12)]
        [Default]
        public decimal? ActualNumberOfRegisteredNew { get; set; }
        [OrderGrid(Order = 13)]
        [Default]
        public decimal? ActualNumberOfProvisionedOld { get; set; }
        [OrderGrid(Order = 14)]
        [Default]
        public decimal? ActualNumberOfProvisionedNew { get; set; }
        [OrderGrid(Order = 15)]
        [TextTooltip]
        [Default]
        public string Comments { get; set; }
        [OrderGrid(Order = 16)]
        [DisplayName("(REJECT/APPROVAL SWITCHBOX)")]
        [Default]
        public bool? Approved { get; set; }
        [OrderGrid(Order = 17)]
        [DisplayName("Submission Date")]
        [DateRangeGrid]
        [Default]
        public DateTime SubmissionDate { get; set; }
        [OrderGrid(Order = 18)]
        [DisplayName("Submitted By")]
        [MailTo]
        [Default]
        public string SubmittedBy { get; set; }
        [OrderGrid(Order = 19)]
        [DisplayName("(SAVE BUTTON)")]
        [Default]
        public bool IsStored { get; set; }

        [IgnoreGrid]
        public DateTime? LastModified { get; set; }
        [IgnoreGrid]

        public string LastModifiedBy { get; set; }

    }
    public class VolteKPIWorklogQueryDto : QueryObject
    {
        public List<int> OpCo { get; set; }
        public List<int> KpiIdName { get; set; }
        public List<string> MonthYear { get; set; }
        public List<bool> TargetMonthlyValueOld { get; set; }
        public List<bool> TargetMonthlyValueProposed { get; set; }
        public List<bool> TargetMonthlyValueNew { get; set; }
        public List<bool> EoyTargetOld { get; set; }
        public List<bool> EoyTargetNew { get; set; }
        public List<bool> ActualMonthlyValueOld { get; set; }
        public List<bool> ActualMonthlyValueNew { get; set; }
        public List<bool> ActualNumberOfRegisteredOld { get; set; }
        public List<bool> ActualNumberOfRegisteredNew { get; set; }
        public List<bool> ActualNumberOfProvisionedOld { get; set; }
        public List<bool> ActualNumberOfProvisionedNew { get; set; }
        public List<string> Comments { get; set; }
        public List<int> Approved { get; set; }
        public DateFilter SubmissionDate { get; set; }
        public List<string> SubmittedBy { get; set; }
        public List<int> IsStored { get; set; }
    }

}
