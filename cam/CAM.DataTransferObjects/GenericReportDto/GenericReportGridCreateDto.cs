using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.GenericReportDto
{
    public class GenericReportGridCreateDto 
    {
        [OrderGrid(Order = 4)]
        public long DynamicReportsId { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        public int UserId { get; set; }
        [OrderGrid(Order = 1)]
        [Default]
        public string ReportName { get; set; }
        //[OrderGrid(Order = 6)]
        //[IgnoreGrid]
        //public string JsonGridCustomizationData { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        public string Published { get; set; }
        [OrderGrid(Order = 3)]
        [Default]      
        public bool? IsScheduled { get; set; }
       
        #region //Ticket 852 - Insert or update the details for the automation report process from the UI

        [OrderGrid(Order = 7)]
        [IgnoreGrid ]
        public string ExportFilePath { get; set; }
        [OrderGrid(Order = 8)]
       
        public string ExportFileFormat { get; set; }
        [OrderGrid(Order = 9)]
        [DisplayName("Schedule Day")]
        public string ScheduledDate { get; set; }
        [OrderGrid(Order = 10)]
         
        public string ExportType { get; set; }
        #endregion

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
        [OrderGrid(Order = 11)]
        [DisplayName("Scheduled Day in week")]
        public string ScheduledDayInWeek { get; set; }
        [OrderGrid(Order = 12)]
        [DisplayName("Scheduled Type")]
        public string ScheduledType { get; set; }
        [OrderGrid(Order = 13)]
        [Default]
        public bool? IsTestNodeRequired { get; set; }
    }
}
