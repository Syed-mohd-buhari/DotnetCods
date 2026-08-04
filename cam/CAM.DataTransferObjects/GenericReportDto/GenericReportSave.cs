using CAM.Enum;
using CAM.Infrastucture.QueryResult;
using System;
using System.Collections.Generic;


namespace CAM.DataTransferObjects.GenericReportDto
{
    public class GenericReportSave : GenericQueryDto
    {
        public List<GenericReportInfrastructureDto> Render { get; set; }

        public List<long> DynamicReportId { get; set; }
        public List<string> ReportName { get; set; }       
        public List<bool> Published { get; set; }
        public ReportViewMode ViewMode { get; set; }

        public List<int> TsrOpcoId { get; set; }
        #region //Ticket 852 - Insert or update the details for the automation report process from the UI
        public string? ExportFilePath { get; set; }
        public string? ExportFileFormat { get; set; }
        public string? ScheduledDate { get; set; }
        public string? ExportType { get; set; }
        public bool isExportReport { get; set; }
        public string? ScheduledDayInWeek { get; set; }
        public string? ScheduledType { get; set; }
        #endregion

        public bool IsTestNodeRequired { get; set; }

    }
    public class GenericReportSchedulerDto
    {
        public long DynamicReportId { get; set; }
        public string ReportName { get; set; }
        public bool Published { get; set; }
        public bool ViewMode { get; set; }

        public string TsrOpcoId { get; set; }

        public string ExportFilePath { get; set; }
        public string ExportFileFormat { get; set; }
        public int ScheduledDate { get; set; }
        public bool? Isscheduled { get; set; }
        public short ExportType { get; set; }

        public bool isExportReport { get; set; }
        public string? ScheduledDayInWeek { get; set; }
        public short? ScheduledType { get; set; }
    }
}
