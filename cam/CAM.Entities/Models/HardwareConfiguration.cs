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
    [Table("HardwareConfiguration")]
    public class HardwareConfiguration :AuditableEntity
    {
        [Key]
        public decimal Hardwareconfigurationid { get; set; }
        public decimal? Networkelementid { get; set; }
        public string Opco { get; set; }
        public string Oem { get; set; }
        public string Elementname { get; set; }
        public string Hardwaretype { get; set; }
        public string Productname { get; set; }
        public string Serialnumber { get; set; }
        public string Unitlocation { get; set; }
        public string Vendor { get; set; }
        //public string Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
       // public string Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Productnumber { get; set; }
        public string Revision { get; set; }

        public virtual Networkelement Networkelement { get; set; }


    }
}
