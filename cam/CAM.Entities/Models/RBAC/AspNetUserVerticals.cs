using CAM.Entities.Models.Base;
using CAM.Identity;

namespace CAM.Entities.Models.RBAC
{
    public class AspNetUserVerticals : AuditableEntity
    {
        public int Aspnetuserverticalid { get; set; }
        public int? Userid { get; set; }
        public long? Organisationid { get; set; }
        public bool? Isvertical { get; set; }
        public bool? Isverticalresponcible { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual OrganisationModel Organisation { get; set; }

    }
}
