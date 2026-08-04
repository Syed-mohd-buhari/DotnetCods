using System.Collections.Generic;
using CAM.DataTransferObjects.Entita.SoftwareBuildCompatibility;
using System.ComponentModel.DataAnnotations;
using CAM.DataTransferObjects.FunctionalityDto;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class ComponentSoftwareBuildDtoCreate : ComponentSoftwareBuildDto
    {
        public long ComponentSoftwareBuildId { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
        public List<KeyValuePairDto> ComponentManufacturerResource { get; set; }
        [Required(ErrorMessage = "Component Manufacturer is required")]
        public long ComponentManufacturerId { get; set; }
       
        public string VulnerabilityStatus { get; set; }       
        public List<KeyValuePairDto> OperatingSystemResource { get; set; }

        public short? OperatingSystemId { get; set; }
        public List<KeyValuePairDto> CriticalAssetTypeResource { get; set; }
        public int? CriticalAssetTypeId { get; set; }

        // public long? ExistComponentSoftwareBuildId { get; set; }
        public List<KeyValuePairDto> DesignContacts { get; set; }

        public IEnumerable<int> DesignContactIds { get; set; }
         
    }
   
}