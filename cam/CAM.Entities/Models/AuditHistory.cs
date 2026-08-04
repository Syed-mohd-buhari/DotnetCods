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
    [Table("Audithistory")]
    public class AuditHistory : AuditableEntity
    {
        [Key]
        public decimal Audithistoryid { get; set; }
        public decimal? Primarykey { get; set; }
        public string Opco { get; set; }
        public string Oem { get; set; }
        public string Elementname { get; set; }
        public string Tablename { get; set; }
        public string Columnname { get; set; }
        public string Oldvalue { get; set; }
        public string Newvalue { get; set; }
        public string Status { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }    
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

    }
}
