using CAM.DataTransferObjects.FunctionalityDto;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware.MappedComponentBuildBag
{
    public class ComponentBuilBagCreatePageDto
    {

        public List<ViewComponentSoftwareBuild> UpgradeComponentSoftwareDetails { get; set; }
        public List<KeyValuePairDto> BuildBagDetails { get; set; }

        public List<KeyValuePairDto> ComponentManufacturersResource { get; set; }

    }
    public class ViewComponentSoftwareBuild
    {
        public List<KeyValuePairDto> RelavantComponetSwBuild { get; set; }
        public long ComponentManufacturerId { get; set; }

        public string Version { get; set; }
        public string DisplayDescription { get; set; }

        public long ComponentSoftwareBuildId { get; set; }
    }
}