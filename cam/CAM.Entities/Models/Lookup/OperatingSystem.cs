using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using OracleModels.DBModels;

namespace CAM.Entities.Models.Lookup
{
    [Table("OperatingSystems")]
    public class OperatingSystem : AuditableEntity
    {
        public OperatingSystem()
        {
            MajorSoftwareBuilds = new HashSet<MajorSoftwareBuild>();
            ComponentSoftwareBuilds = new HashSet<ComponentSoftwareBuild>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short OperatingSystemId { get; set; }
        public string OperatingSystemName { get; set; }
        public string OperatingSystemVersion { get; set; }


        [InverseProperty(nameof(MajorSoftwareBuild.OperatingSystem))]
        public virtual ICollection<MajorSoftwareBuild> MajorSoftwareBuilds { get; set; }


        [InverseProperty(nameof(ComponentSoftwareBuild.OperatingSystem))]
        public virtual ICollection<ComponentSoftwareBuild> ComponentSoftwareBuilds { get; set; }
    }
}
