using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.GenericReportDto
{
    public class GenericReportQueryDto : QueryObject
    {
        public List<long> DynamicReportsId { get; set; }
        public List<int> UserId { get; set; }
       // public List<string> JsonGridCustomizationData { get; set; }
        public List<string> Published { get; set; }
        public List<string> ReportName { get; set; }
        public List<bool> IsScheduled { get; set; }
        public List<string> CreationUser { get; set; }
        public DateFilter CreationDate { get; set; }
        public List<string> ModificationUser { get; set; }
        public DateFilter ModificationDate { get; set; }
        
        #region //Ticket 852 - Insert or update the details for the automation report process from the UI
        public List<string> ExportFilePath { get; set; }
        public List<string> ExportFileFormat { get; set; }
        public List<int> ScheduledDate { get; set; }
        public List<short> ExportType { get; set; }
        public List<string> ScheduledDayInWeek { get; set; }
        public List<short> ScheduledType { get; set; }
        #endregion

        public List<string> IsTestNodeRequired { get; set; }
    }
}
