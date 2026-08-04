using CAM.Infrastucture.Enums;
using System.Collections.Generic;

namespace CAM.Infrastucture.QueryResult
{
    public class GenericReportInfrastructureGrid<T>
    {
        public long DynamicReportId { get; set; }
        public string ReportName { get; set; }
        public bool Published { get; set; }       
        public List<GenericReportInfrastructureDto> Render { get; set; }
        public List<int> OpcoId { get; set; }
        #region //Ticket 852 - Insert or update the details for the automation report process from the UI
        public string? ExportFilePath { get; set; }
        public string? ExportFileFormat { get; set; }
        public int? ScheduledDate { get; set; }
        public int? ExportType { get; set; }
        public bool isExportReport { get; set; }
        public bool isScheduled { get; set; }
        public string? ScheduledDayInWeek { get; set; }
        public int? ScheduledType { get; set; }
        public bool? IsTestNodeRequired { get; set; }
        #endregion

    }


}