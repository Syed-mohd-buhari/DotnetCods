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
    [Table("Component")]
    public class Components : AuditableEntity
    {
        [Key]
        public decimal Componentid { get; set; }
        public decimal? softwarecomponentid { get; set; }
        public string OpCO { get; set; }
        public string Oem { get; set; }
        public string Elementname { get; set; }
        public decimal? NetworkElementId { get; set; }
        public string Mainsoftwareversion { get; set; }
        public string Componentname { get; set; }
        public DateTime? Productiondate { get; set; }
        public string Productionnumber { get; set; }
        public string ProductionRevision { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public int ComponentCreationuser { get; set; }
        public DateTime ComponentCreationdate { get; set; }
        public int? ComponentModificationuser { get; set; }
        public DateTime ComponentModificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual SoftwareComponent SoftwareComponent { get; set; }

    }
}
