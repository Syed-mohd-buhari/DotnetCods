using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Entities.Models.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.LookUp.MajorSwBuildsDesignContact
{
    public class MajorSwBuildsDesignContactDto : GridDtoBase
    {
        [OrderGrid(Order =1)]        
        public long MajorSwBuidlsDesignContactId { get; set; }
        [Default]
        [OrderGrid(Order = 2)]
        public int DesignContactId { get; set; }
        [OrderGrid(Order = 3)]
        public string DesignContact { get; set; }
        [Default]
        [OrderGrid(Order = 4)]
        public long MajorSoftwareBuildId { get; set; }
        [Default]
        [OrderGrid(Order = 5)]
        public string MajorSoftwareBuild { get; set;}
    }
}
