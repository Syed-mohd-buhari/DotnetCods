using CAM.Entities.Models.Base;
using CAM.Identity;

namespace CAM.Entities.Models.CBom
{
    public partial class CnfCapacity : AuditableEntity
    {
        public long CnfCapacityId { get; set; }
        public long CnfPodInfoId { get; set; }
        public short FinancialYear { get; set; }
        public int FinancialVersion { get; set; }
        public long? VcpuLimitForPodType { get; set; }
        public long? PcpuRequestForPodType { get; set; }
        public decimal? MemRequestForPodType { get; set; }
        public decimal? MemLimitForPodType { get; set; }
        public int? NumberOfPodsPerPodType { get; set; }
        public long? VcpuRequestForPodType { get; set; }
        
        public string NonPresistentStorageForProdType { get; set; }
        public bool? IsPresistentVolumesRequired { get; set; }
        public string PersistentVolumNeaccessMode { get; set; }
        public string PersistentStorageForPodType { get; set; }
        public string StorageIopsForPodType { get; set; }
        public string StoragerWorkloadDistribution { get; set; }
        public string NorthSouthBandWidthForPodType { get; set; }
        public string EastWestBandWidthForPodType { get; set; }
        public string SpecialRequirementPerPodType { get; set; }
        public string ListOfCapacitySpecialRequirement { get; set; }

        public int? NoOfCnfInstancesPerSite { get; set; }
         
        public virtual CnfPodInfo CnfPodInfo { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
    }
}
