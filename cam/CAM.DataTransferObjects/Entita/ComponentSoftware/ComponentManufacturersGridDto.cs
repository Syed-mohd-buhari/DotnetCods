using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class ComponentManufacturersGridDto : GridDtoBase
    {
        [DisplayName("Component Manufacturer Id")]
        [Default]
        public long ComponentManufacturerId { get; set; }

        [DisplayName("Component Manufacturer")]
        [Default]
        public string ComponentManufacturer { get; set; }

        [DisplayName("Component Name")]
        [Default]
        public string ComponentName { get; set; }


    }
}