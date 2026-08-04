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
    [Table("Identities")]
    public class NodeIdentity:AuditableEntity
    {
        [Key]
        public decimal Identityid { get; set; }
        public decimal? NetworkelementId { get; set; }
        public string Elementname { get; set; }
        public string oem { get; set; }
        public string Opco { get; set; }
        public string Apnodeaipaddress { get; set; }
        public string Apnodebipaddress { get; set; }
       // public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public string Ipaddress { get; set; }
       // public int? Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public Networkelement Networkelement  { get; set; }

       
    }
}
