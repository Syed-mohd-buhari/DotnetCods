using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Models
{
    public class UserDefinedReportsLogs : AuditableEntity
    {

        public int UserDefinedReportsLogId { get; set; }
        public string ReportName { get; set; }
        public string ReportFormat { get; set; }
        public string ReportDownloadedPath { get; set; }
        public string ReportStatus { get; set; }
       
        public bool? Deleted { get; set; }
        public DateTime? DeletionDate { get; set; }

    }
}
