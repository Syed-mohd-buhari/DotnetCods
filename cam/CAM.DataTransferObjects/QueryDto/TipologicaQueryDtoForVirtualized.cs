using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
   public class TipologicaQueryDtoForVirtualized : TipologicaQueryDto 
    {
        public List<bool> ForVirtualized { get; set; }

    }
}
