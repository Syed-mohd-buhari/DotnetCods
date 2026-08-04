using CAM.Entities.Models.AssetHardwareConfig;
using CAM.Entities.Models.Base; 
using CAM.Entities.Models.Lookup;
using System.Collections.Generic;

namespace CAM.Entities.Models
{
    public partial class MajorHardwareBuildAsIs : AuditableEntity
    {
        public MajorHardwareBuildAsIs()
        {
            AssetHardwareAncillary = new HashSet<AssetHardwareAncillary>();
        }
        public long MajorHardwareBuildAsIsId { get; set; }
      
        public short OriginalEquipmentManufacturerId { get; set; }       
        
        public short BuildConstructionId { get; set; }
        public string HardwareType { get; set; }
 

        public string HardwareSolution { get; set; }
        public short PlatformId { get; set; }

        public short? HardwareSolutionReourceId { get; set; }
        public BuildConstruction BuildConstruction { get; set; }
        public virtual HardwareSolutionResource HardwareSolutionResource { get; set; }

        public virtual OriginalEquipmentManufacturer OriginalEquipmentManufacturer { get; set; }
        public virtual Platform Platform { get; set; }

        public virtual ICollection<AssetHardwareAncillary> AssetHardwareAncillary { get; set; }
    }
}
