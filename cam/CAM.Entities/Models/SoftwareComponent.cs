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
    [Table("SoftwareComponent")]
    public class SoftwareComponent : AuditableEntity
    {
        public SoftwareComponent()
        {
            Component = new HashSet<Components>();
        }

        [Key]
        public decimal SoftwarecomponentId { get; set; }
        public decimal? NetworkelementId { get; set; }
        public string Elementname { get; set; }
        public string Opco { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public string Mainsoftwareversion { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public string Oem { get; set; }
        public long? ComponentId { get; set; }
        public string Componentname { get; set; }
        public DateTime? Productiondate { get; set; }
        public string Productionnumber { get; set; }
        public string Productionrevision { get; set; }
        public string Componentcreationuser { get; set; }
        public DateTime Componentcreationdate { get; set; }
        public string Componentmodificationuser { get; set; }
        public DateTime Componentmodificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public virtual ICollection<Components> Component { get; set; }
        public Networkelement Networkelement { get; set; }


    }
}
