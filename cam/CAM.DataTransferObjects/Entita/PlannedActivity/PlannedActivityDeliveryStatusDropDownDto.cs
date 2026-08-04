using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.PlannedActivity
{
    public class PlannedActivityDeliveryStatusDropDownDto
    {
        
        public short  Key { get; set; }       
        public string Value { get; set; }
        public bool NeedPlannedAsset { get; set; }

    } 
}
