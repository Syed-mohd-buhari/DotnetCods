using CAM.Entities.Models.Base;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Models
{
    public partial class ComponentSoftwareBuildBag : AuditableEntity
    {
        public long ComponentSoftwareBuildBagId { get; set; }
        public long BuildBagId { get; set; }
        public long ComponentSoftwareBuildId { get; set; } 
        public virtual BuildBag BuildBags { get; set; }
        public virtual ComponentSoftwareBuild ComponentSoftwareBuilds { get; set; }
        public virtual ApplicationUser CreationUserNavigation { get; set; }
        public virtual ApplicationUser ModificationUserNavigation { get; set; }
    }
}
