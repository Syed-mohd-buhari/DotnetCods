using System;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp
{
    public class TipologicaGridDtoForVirtualized : TipologicaGridDto
    {
        [OrderGrid(Order = 6)]
        public bool ForVirtualized { get; set; }
    }
}
