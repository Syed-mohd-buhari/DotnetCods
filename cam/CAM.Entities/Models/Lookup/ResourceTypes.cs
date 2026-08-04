using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Models.Lookup
{
    [Table("Resourcetypes")]
    public class ResourceTypes : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Resourcetypesid")]
        public long ResourceTypesId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        [InverseProperty(nameof(Models.ResourceKeyMaster.ResourceTypes))]
        public virtual ICollection<ResourceKeyMaster> ResourceKeyMaster { get; set; }
    }
}
