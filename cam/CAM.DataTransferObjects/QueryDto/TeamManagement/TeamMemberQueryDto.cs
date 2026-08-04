using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;
namespace CAM.DataTransferObjects.QueryDto.TeamManagement
{
    public class TeamMemberQueryDto : QueryObject
    {
        public List<int> TeamMemberId {  get; set; }
        public List<int> UserId { get; set; }
        public List<int> Creationuser { get; set; }
        public DateFilter Creationdate { get; set; }
        public List<int> Modificationuser { get; set; }
        public DateFilter Modificationdate { get; set; }
    }
}
