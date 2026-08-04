using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Models
{
    [Table("NetworkElement")]
    public class NetworkElement : AuditableEntity
    {
        public  NetworkElement()
        {
            SoftwareConfiguration =new HashSet<SoftwareConfiguration>();
        }
        public decimal Networkelementid { get; set; }
        public string Opco { get; set; }
        public string Oem { get; set; }
        public string Elementname { get; set; }
        public DateTime? Dataacquisitiondate { get; set; }
        public string Nodetype { get; set; }
        public string Platformtype { get; set; }
        public string Sitelocation { get; set; }
        public DateTime? Softwareinstalldate { get; set; }
        public DateTime? Softwareinstalldateap { get; set; }
        public DateTime? Softwareinstalldatecp { get; set; }
        public DateTime? Softwareproductdate { get; set; }
        public DateTime? Softwareproductdateap { get; set; }
        public DateTime? Softwareproductdatecp { get; set; }
        public string Softwareproductnumber { get; set; }
        public string Softwareproductnumberap { get; set; }
        public string Softwareproductnumbercp { get; set; }
        public string Softwarereleaseinformation { get; set; }
        public string Softwarereleaseinformationap { get; set; }
        public string Softwarereleaseinformationcp { get; set; }
        public string Spare1ossorenm { get; set; }
        public string Spare2xmlversion { get; set; }
        public DateTime? Xmllastparsefiledate { get; set; }
       // public string Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
       // public string Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string NodeTypeName { get; set; }

        public virtual ICollection<SoftwareConfiguration> SoftwareConfiguration { get; set; }

    }
}
