using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita.MajorHardwareBuild
{
    public class MajorHardwareBuildDtoCreate : MajorHardwareBuildDto
    {
        [StringLength(2000)]
        public string Description { get; set; }

        public IDictionary<short, string>? OriginalEquipmentManufacturerResource { get; set; } //Solo i dictionary delle select (virtual nel modello NON ICollection)
        [Required(ErrorMessage = "Equipment Manufacturer is required")]
        public short OriginalEquipmentManufacturerId { get; set; } //Id del selezionato
       
        //public IDictionary<short, string>? VulnerabilityStatusResource { get; set; }
        public IDictionary<short, string> HardwareSolutionReource { get; set; }
        
        public short? HardwareSolutionReourceId { get; set; }

        public IDictionary<short, string> BuildConstructionResource { get; set; }
        public short? BuildConstructionId { get; set; }

        //public IDictionary<short, string> HardwareTypeResource { get; set; }


        public IDictionary<short, string> PlatformResource { get; set; }

        public IDictionary<int, string> DesignContacts { get; set; }

        public short? PlatformId { get; set; }

        
        public string HardwareType { get; set; }

        public IEnumerable<int> DesignContactIds { get; set; }



    }
}