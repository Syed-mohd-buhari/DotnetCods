using CAM.DataTransferObjects.FunctionalityDto;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class BuildBagEditPageDto
    {
        [StringLength(500)]
        public string BuildBagDescription { get; set; }
        public long BuildBagId { get; set; }
        public string BagVersion { get; set; }
        public List<KeyValuePairDto> DcfResource { get; set; }
        public List<KeyValuePairDto> OpcoResource { get; set; }

        public virtual Opcos Opco { get; set; }
        public virtual Designcomponentfamilies DesignComponentFamily { get; set; }
        public short OpCoId { get; set; }
        public long DesignComponentFamilyId { get;set; }

        public ComponentBuildBagEditPageDto componentBuildBagEditPageDto { get; set; }
    }

}