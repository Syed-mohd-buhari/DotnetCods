using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.LookUp.SystemNames
{
    public class SystemNamesDtoGrid : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [Default]
        public long SystemNameId { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Domain Name")]
        [Default]
        public string SystemNameDescription { get; set; }

    }
}
