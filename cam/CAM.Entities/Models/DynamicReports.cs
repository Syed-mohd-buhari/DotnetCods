using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Models
{
    [Table("dynamicreports")]
    public class DynamicReports : AuditableEntity
    {
        [Key]
        public decimal DynamicReportsId { get; set; }
        public int UserId { get; set; }
        public string ReportName { get; set; }
        public string JsongridCustomizationData { get; set; }
        public bool? Published { get; set; }
        public bool? Isscheduled { get; set; }
        public string OpcoId { get; set; }
        public virtual ApplicationUser Users { get; set; }
        public string ExportFilePath { get; set; }
        public string ExportFileFormat { get; set; }
        public int? ScheduledDate { get; set; }
        public short? ExportType { get; set; }
        public string? ScheduledDayInWeek { get; set; }
        public short? ScheduledType { get; set; }
        public bool? IsTestNodeRequired { get; set; }
    }
}
