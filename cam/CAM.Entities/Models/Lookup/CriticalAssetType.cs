using CAM.Entities.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.Lookup
{
    public class CriticalAssetType : AuditableEntity
    {

        public CriticalAssetType()
        {
            MajorSoftwareBuilds = new HashSet<MajorSoftwareBuild>();
            ComponentSoftwareBuilds = new HashSet<ComponentSoftwareBuild>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }

        [InverseProperty(nameof(MajorSoftwareBuild.CriticalAssetType))]
        public virtual ICollection<MajorSoftwareBuild> MajorSoftwareBuilds { get; set; }

        [InverseProperty(nameof(ComponentSoftwareBuild.CriticalAssetType))]
        public virtual ICollection<ComponentSoftwareBuild> ComponentSoftwareBuilds { get; set; }

       
    }

}
