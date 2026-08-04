using CAM.Entities.Models.Base;
using CAM.Identity;
namespace CAM.Entities.Models.Team
{
    public partial class TeamMembers : AuditableEntity
    {
        public int Teammemberid { get; set; }
        public int? Teamid { get; set; }
        public int? Userid { get; set; }
        public virtual Teams Team { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
