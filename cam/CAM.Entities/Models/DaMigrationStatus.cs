using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;

namespace CAM.Entities.Models
{
    public class DaMigrationStatus : AuditableEntity
    {
        public long DaMigrationStatuId { get; set; }
        public long PlannedActivityId { get; set; }
        public short OpcoId { get; set; }
        public short LocationId { get; set; }
        public short StatusId { get; set; }
        public string OpcoName { get; set; }
        public string LocationName { get; set; }
        public string StatusName { get; set; }

        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual Location Location { get; set; }
      
        public virtual OpCo Opco { get; set; }
        public virtual PlannedActivity PlannedActivity { get; set; }
    }
}
