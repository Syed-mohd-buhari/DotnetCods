using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.PlannedActivityTypes
{
    public class PlannedActivityTypesDropDownDto
    {
        [Default]
        [OrderGrid(Order = 2)]
        public string  PlannedActivityTypeDescription { get; set; }

        [Default]
        [OrderGrid(Order = 1)]
        public int PlannedActivityTypesId { get; set; }

         
        [IgnoreGrid]
        public int PagewiseId { get; set; }

    } 
}
