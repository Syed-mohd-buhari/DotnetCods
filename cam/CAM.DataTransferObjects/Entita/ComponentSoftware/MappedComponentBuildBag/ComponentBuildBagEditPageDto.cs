using CAM.DataTransferObjects.Entita.ComponentSoftware.MappedComponentBuildBag;
using CAM.DataTransferObjects.FunctionalityDto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class ComponentBuildBagEditPageDto  : ComponentBuilBagCreatePageDto
    {
        [StringLength(2000)]
        public string ComponentBagDescription { get; set; }
        public long BuildBagId { get; set; }

        public List<ViewComponentSoftwareBuild> ExistingComponentSwBuild { get; set; }
    }
 
}