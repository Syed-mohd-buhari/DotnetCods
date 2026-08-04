using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ProblemCategoryQueryDto : QueryObject
    {
        public List<long> ProblemCategoryId { get; set; }
        public List<string> ProblemCategoryDescription { get; set; }
    }
}
