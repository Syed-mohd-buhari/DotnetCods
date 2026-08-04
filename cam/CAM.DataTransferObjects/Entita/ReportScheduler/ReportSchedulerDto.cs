using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.ReportScheduler
{
    public class ReportSchedulerDto : GridDtoBase
    {
        [IgnoreGrid]
        public decimal ReportSchedulerId { get; set; }

        [OrderGrid(Order = 1)]
        [Default]
        [DisplayName("Report Name")]
        public string ReportName { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        [DisplayName("Is Scheduled")]
        public string IsScheduled { get; set; }

        [IgnoreGrid]
        [OrderGrid(Order = 3)]
        [DisplayName("Opco Name")]
        public string OpcoId { get; set; }
        
        [IgnoreGrid]
        public string ReportVertical { get; set; }

        [OrderGrid(Order = 4)]
        [Default]
        [DisplayName("Export File Path")]
        public string ExportFilePath { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        [DisplayName("Export File Format")]
        public string ExportFileFormat { get; set; }

        [OrderGrid(Order = 6)]
        [Default]
        [DisplayName("Scheduled Date")]
        public int? ScheduledDate { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        [DisplayName("Scheduled Day In Week")]
        public string ScheduledDayInWeek { get; set; }

    }

}
