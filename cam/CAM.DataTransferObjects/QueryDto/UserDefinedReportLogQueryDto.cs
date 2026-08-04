using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class UserDefinedReportLogQueryDto : QueryObject
    {
        public List<long> UserDefinedReportsLogId { get; set; }        
        public List<string> ReportName { get; set; }
        public List<string> ReportFormat { get; set; }
        public List<string> ReportDownloadedPath { get; set; }
        public List<string> ReportStatus { get; set; }        
        public List<string> LastModificateUser { get; set; }
        public DateFilter LastModifiedValue { get; set; }
    }
}
