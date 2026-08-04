using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CAM.Entities.Models.Lookup
{
    [Table("Locations")]
    public partial class Location : AuditableEntity
    {
        public Location()
        {
            Locationdeploymenttypes = new List<LocationDeploymentTypes>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short LocationId { get; set; }

        [Column("Location")]
       
        public string LocationDescription { get; set; }


        public short? OpcoId { get; set; }
        public bool DefaultValue { get; set; }       
        public virtual OpCo OpCo { get; set; }

        public string ShortDescription { get; set; }


        [InverseProperty(nameof(NetworkElementAsPlanned.Location))]
        public virtual ICollection<NetworkElementAsPlanned> Networkelementsasplanned { get; set; }

        [InverseProperty(nameof(NetworkElementAsIs.Location))]
        public virtual ICollection<NetworkElementAsIs> Networkelementsasis { get; set; }

        public virtual ICollection<LocationDeploymentTypes> Locationdeploymenttypes { get; set; }
    }
}
