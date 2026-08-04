using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
   public class MainOrganisationQueryDto : QueryObject
    {
        public List<int> MainorganisationId { get; set; }
        public List<string> MainorganisationDescription { get; set; }
       

    }
}
