using System.Collections.Generic;

namespace CAM.Entities.Models.Team
{
    public partial class Teams
    {
        public Teams()
        {
            TeamMembers = new HashSet<TeamMembers>();
        }

        public int Teamid { get; set; }
        public string Teamname { get; set; }
        public string Teamdescription { get; set; }
        public bool? Isactive { get; set; }
        public virtual ICollection<TeamMembers> TeamMembers { get; set; }
    }
}
