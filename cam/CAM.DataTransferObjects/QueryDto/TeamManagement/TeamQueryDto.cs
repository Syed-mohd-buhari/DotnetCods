using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.TeamManagement
{
    public class TeamQueryDto : QueryObject
    {
        public List<int> TeamId { get; set; }
        public List<string> TeamName {  get; set; }
        public List<string> TeamDescription { get; set; }
        public List<bool> Active { get; set; }
        public List<int> UserId { get; set; }
        public List<int> Creationuser { get; set; }
        public DateFilter Creationdate { get; set; }
        public List<int> Modificationuser { get; set; }
        public DateFilter Modificationdate { get; set; }

    }
}
