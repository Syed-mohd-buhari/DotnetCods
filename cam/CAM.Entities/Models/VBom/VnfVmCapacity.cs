using CAM.Entities.Models.Base;
using CAM.Identity;

namespace CAM.Entities.Models.VBom
{
    public partial class VnfVmCapacity : AuditableEntity
    {
        public long VnfVmCapacityId { get; set; }
        public long VnfInfoId { get; set; }
        public short FinancialYear { get; set; }
        public long VnfCpuPerVm { get; set; }
        public bool? RxTxCpuCount { get; set; }
        public long RamPerVm { get; set; }
        public long DataDisk { get; set; }
        public long? OsDisk { get; set; }
        public string? IopsRunning { get; set; }
        public string? IopsLoading { get; set; }
        public string VmWorkLoadDistribution { get; set; }
        public string NorthDouthBoundBandWidth { get; set; }
        public string EastWestBoundBandWidth { get; set; }
        public string OtherRequirements { get; set; }
        public bool? BackupRequired { get; set; }
        public bool? ProbIngRequired { get; set; }

        public long? NoOfVnfInstances { get; set; }
        public long? NoOfVmsPerType { get; set; }
        public int FinancialVersion { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual VnfInfo VnfInfo { get; set; }
    }
}
