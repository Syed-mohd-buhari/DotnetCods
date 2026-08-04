using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.QueryDto
{
  public  class TipologicaQueryDtoCombinationRule : TipologicaQueryDtoRule
    {
        public List<int> ProjectStatusCombinationRule { get; set; }
    }
}
