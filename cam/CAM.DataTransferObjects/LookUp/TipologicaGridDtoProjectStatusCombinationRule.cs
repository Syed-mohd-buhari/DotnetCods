using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.LookUp
{
   public class TipologicaGridDtoProjectStatusCombinationRule : TipologicaGridDto
    {
        [OrderGrid(Order = 5)]
        public int ProjectStatusCombinationRule { get; set; }

        [OrderGrid(Order = 6)]
        public bool Default { get; set; }
    }
}
