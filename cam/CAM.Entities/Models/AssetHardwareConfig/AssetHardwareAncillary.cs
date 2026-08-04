using CAM.Entities.Models.Base;
using CAM.Entities.Models.VBom;
using CAM.Identity;
using OracleModels.DBModels;
using System.Collections.Generic;

namespace CAM.Entities.Models.AssetHardwareConfig
{
    public partial class AssetHardwareAncillary : AuditableEntity
    {
        public AssetHardwareAncillary()
        {
            AssetCapacityInfo = new HashSet<AssetCapacityInfo>();
        }
        public long AssetHardwareAncillaryId { get; set; }
       
        public long NetworkElementAsPlannedId { get; set; }
        public long? MajorHardwareBuildAsIsId { get; set; }
        public long? DataCenterId { get; set; }
        public long? ClusterNameId { get; set; }
        public long? AssetClusterTypeId { get; set; }
        public long ? AssetClusterId { get; set; }

        public string? ElementName { get; set; } = string.Empty;

        public string? MajorHardwareName { get; set; } = string.Empty;

        public string? DataCeterName { get; set; } = string.Empty;

        public string? ClusterNameDesc { get; set; } = string.Empty;

        public string? AssetClusterTypeDesc { get; set; } = string.Empty;
        public string? AssetClusterDesc { get; set; } = string.Empty;


        public virtual AssetCluster AssetCluster { get; set; }
        public virtual AssetClusterType AssetClusterType { get; set; }
        public virtual ClusterName ClusterName { get; set; }
        
        public virtual DataCenter DataCenter { get; set; }
        public virtual MajorHardwareBuildAsIs MajorHardwareBuildAsIs { get; set; }
         
        public virtual NetworkElementAsPlanned NetworkElementsAsPlanned { get; set; }

        public virtual ICollection<AssetCapacityInfo> AssetCapacityInfo { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
    }
}
