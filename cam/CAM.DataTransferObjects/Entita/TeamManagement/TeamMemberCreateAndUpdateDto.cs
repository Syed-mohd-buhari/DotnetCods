using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.TeamManagement
{
    public class TeamMemberCreateAndUpdateDto : TeamMemberGridDto
    {
        public Dictionary<int,string> UserResource {  get; set; }
        public int TeamId { get; set; }
    }
}
