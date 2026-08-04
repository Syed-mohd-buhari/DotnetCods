using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.QueryDto
{
   public class TipologicaQueryDtoProjectStatusCombinationRule : TipologicaQueryDto
    {
        public List<int> ProjectStatusCombinationRule { get; set; }
        public List<bool> Default { get; set; }
    }
}
