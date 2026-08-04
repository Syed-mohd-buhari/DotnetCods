using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
   public class TipologicaQueryDto : QueryObject
    {
        public List<short> Id { get; set; }
        public List<string> Description { get; set; }

    }

    public class DriverQueryDto : TipologicaQueryDto
    {
        public List<string> BptDriverDetails { get; set; }
    }
}
