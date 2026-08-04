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
    [Table("AuditLogs")]
    public class AuditLogs : AuditableEntity
    {
        [Key]
        public decimal AuditLogId { get; set; }
        public string EntityName { get; set; }
        public string EntityField { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public long? EntityId { get; set; }
        public string EntityState { get; set; }

    }
}
