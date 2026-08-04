using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.LookUp
{
   public class TipologicaGridDtoCombinationRule : TipologicaGridDtoRule
    {
        [OrderGrid(Order = 6)]
        public int ProjectStatusCombinationRule { get; set; }
    }
}
