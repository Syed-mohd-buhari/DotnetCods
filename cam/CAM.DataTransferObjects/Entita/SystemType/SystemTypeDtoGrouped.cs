using System;
using System.Collections.Generic;
using System.Text;
using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.SystemType
{
   public class SystemTypeDtoGrouped : SystemTypeDtoGrid
    {
        [IgnoreGrid]
        public string KeyGrouped { get; set; }

    }
}
