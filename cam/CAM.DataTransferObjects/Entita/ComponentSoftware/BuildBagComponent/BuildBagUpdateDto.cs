using CAM.Entities.Models.Lookup;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class BuildBagUpdateDto
    {
        public long BuildBagId { get; set; }

        [StringLength(500)]
        public string BuildBagDescription { get; set; }
        public List<long> ComponentSoftwareId { get; set; }

        public string BagVersion { get; set; }
        public short OpCoId { get; set; }
        public long DesignComponentFamilyId { get; set; }
    }
}