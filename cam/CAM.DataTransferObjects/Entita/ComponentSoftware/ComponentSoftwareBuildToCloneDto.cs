using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Enum;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class ComponentSoftwareBuildToCloneDto
    {
        public long ComponentSoftwareBuildId { get; set; }
        public string ComponentManufacturers { get; set; }

        public long ComponentManufacturerId { get; set; }
        public string ProductName { get; set; }
        public string SoftwareVersion { get; set; }
        public decimal? ProductNameId { get; set; }
       
        public IEnumerable<int> DesignContactIds { get; set; }

        public List<KeyValuePairDto> DesignContacts { get; set; }
     
    }
}
