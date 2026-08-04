using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Models
{
    [Table("Reportscheduler")]
    public class ReportScheduler : AuditableEntity
    {
        [Key]
        public decimal ReportSchedulerId { get; set; }
        public string ReportName { get; set; }
        public bool? IsScheduled { get; set; }
        public string OpcoId { get; set; }
        public string ReportVertical { get; set; }
        public string ExportFilePath { get; set; }
        public string ExportFileFormat { get; set; }
        public int? ScheduledDate { get; set; }
        public string ScheduledDayinWeek { get; set; }

    }
}
