using CAM.DataTransferObjects.FunctionalityDto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class ComponentBuilBagClonedDto  
    {
        [StringLength(2000)]
        public string ComponentBagDescription { get; set; }         
        public List<long> ComponentSoftwareId { get; set; }
        public long BuildBagDetailsId { get; set; } 

    }
  
}