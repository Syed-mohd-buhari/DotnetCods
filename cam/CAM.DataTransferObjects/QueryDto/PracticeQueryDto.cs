using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
   public class PracticeQueryDto : QueryObject
    {
        public List<int> PracticeId { get; set; }
        public List<string> PracticeDescription { get; set; }
        public List<string> PracticeEmail { get; set; }



    }
}
