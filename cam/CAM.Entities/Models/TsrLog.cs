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
    [Table("Tsrlogs")]
    public class TsrLog : AuditableEntity
    {
        [Key]
        public decimal TsrLogId { get; set; }
        public string TypeOfOperation { get; set; }
        public string FileName { get; set; }
        public long? TotalRecord { get; set; }
        public long? ProcessedRecord { get; set; }  
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
        public string Domain { get; set; }
        public string BatchIdentifier { get; set; }

    }
}
