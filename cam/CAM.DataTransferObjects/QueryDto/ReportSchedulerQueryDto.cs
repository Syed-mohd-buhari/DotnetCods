using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ReportSchedulerQueryDto : QueryObject
    {
        public List<decimal> ReportSchedulerId { get; set; }
        public List<string> ReportName { get; set; }
        public List<string> IsScheduled { get; set; }
        public List<string> OpcoId { get; set; }
        public List<string> ReportVertical { get; set; }
        public List<string> ExportFilePath { get; set; }
        public List<string> ExportFileFormat { get; set; }
        public List<int> ScheduledDate { get; set; }
        public List<string> ScheduledDayinWeek { get; set; }
    }
}
