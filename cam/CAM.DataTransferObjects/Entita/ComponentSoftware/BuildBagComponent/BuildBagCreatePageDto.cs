using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.Entita.ComponentSoftware.MappedComponentBuildBag;
using CAM.DataTransferObjects.FunctionalityDto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class BuildBagCreatePageDto
    {
        [StringLength(500)]
        public string BuildBagDescription { get; set; }

        public List<KeyValuePairDto> DcfResource { get; set; }
        public List<KeyValuePairDto> OpcoResource { get; set; }

        public List<ViewComponentSoftwareBuild> ComponentSofwareBuild { get; set; }
        public List<long> ComponentSoftwareId { get; set; }

        public string BagVersion { get; set; }

        [IgnoreGrid]
        public long LcmEngineeringId { get; set; }

        public long DesignComponentFamilyId { get; set; }
        public short OpCoId { get; set; }
    }
    

}