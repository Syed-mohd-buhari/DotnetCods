using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System.Collections.Generic;


namespace CAM.Entities.Models.CBom
{
    public partial class CnfClusterInfo : AuditableEntity
    {
        public CnfClusterInfo()
        {
            CnfPodInfo = new HashSet<CnfPodInfo>();
        }

        public long CnfClusterInfoId { get; set; }
        public long CnfNameId { get; set; }
        public long CnfClusterId { get; set; }
        public short OpcoId { get; set; }
        public short SiteId { get; set; }
        public long CnfHardwareId { get; set; }        
        public bool? NodePoolBreakup { get; set; }
        public string SpecialRequirements { get; set; }
        public string Hyperthreading { get; set; }
        public string OverProvisioning { get; set; }
        public string? WorkerNodeConfiguration { get; set; }
        public string? HardwareDescription { get; set; }
        public decimal? CpuKubelet { get; set; }
        public int? MemKubelet { get; set; }
        public decimal? CpuSystem { get; set; }
        public int? MemSystem { get; set; }
        public int VerticalResponsibleId { get; set; }
        public string? Comments { get; set; }
        public string? Notes { get; set; }
        public string FileName { get; set; }
        public string Revision { get; set; }
        public string AggregateImageClusterSize { get; set; }
        public string OpcoIdName { get; set; }
        public string SiteName { get; set; }
        public string LocationName { get; set; }
       
        public string CnfClusterIdName { get; set; }
        public string NodePoolName { get; set; }
        public string CnfNameDescription { get; set; }
        public string K8ClusterAliasName { get; set; }
        public string VerticalResponsibleName { get; set; }

        public string? HardwareTypeDescription { get; set; }

        public long? CnfClusterNodePoolId { get; set; }
        public virtual CnfCluster CnfCluster { get; set; }
        public virtual CnfHardware CnfHardware { get; set; }
        public virtual CnfName CnfName { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Locations Site { get; set; }
        public virtual VerticalResponsible Verticalresponsible { get; set; }
        public virtual ICollection<CnfPodInfo> CnfPodInfo { get; set; }

        public virtual CnfCluster CnfClusterNodePool { get; set; }
    }
}
