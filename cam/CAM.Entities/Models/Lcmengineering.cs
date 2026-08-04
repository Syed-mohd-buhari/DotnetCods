using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models
{
    [Table("LCMEngineering")]
    public partial class LcmEngineering : AuditableEntity
    {
        public LcmEngineering()
        {
            PlannedActivities = new HashSet<PlannedActivity>();
            CheckboxResourceLcmEngineeringSoftwares = new HashSet<ReasonCheckboxResourceLcmEngineeringSoftware>();
            CheckboxResourceLcmEngineeringHardwares = new HashSet<ReasonCheckboxResourceLcmEngineeringHardware>();
            LcmEngineeringEduSpoc = new HashSet<LcmEngineeringEduSpoc>();
            LcmEngineeringSubDomainSpoc = new HashSet<LcmEngineeringSubDomainSpoc>();
            LCMOperationContracts = new HashSet<LcmOperationalContracts>();
            LcmAncillaryData = new HashSet<LcmAncillaryData>();
        }
        public long LcmengineeringId { get; set; }

        public short? OpCoId { get; set; }
        public string? VodafoneName { get; set; }
        public long DesignComponentId { get; set; }
        public short? LCMDeploymentStatusId { get; set; }
        public long? DesignComponentFamilyId { get; set; }
        public int NumberOfNodes { get; set; }
        public int NumberOfNodesInLab { get; set; }
        public short? ProductImportanceId { get; set; }
        public bool OnSoftware { get; set; }
        public bool OnHardware { get; set; }
        public bool ElementCount { get; set; }

        //Software
        public bool Warranty { get; set; }
        [Column(TypeName = "date")] public DateTime? SoftwareEndOfWarrantyDate { get; set; }
        public short? SoftwareSupportedId { get; set; }
        public DateTime? SoftwareEndOfSupportContract { get; set; }
        public short? FullorPartialSupportId { get; set; }

        public short? FullorPartialSupportHWId { get; set; }
        public string SoftwareSupportProvider { get; set; }
        public string LcmStatusHardware { get; set; }
        public string LcmStatusSoftware { get; set; }


        public string OutputToLCMHardware { get; set; }
        public string OutputToLCMSoftware { get; set; }

        public string LCMStatusOPSHardware { get; set; }
        public string LCMStatusOPSSoftware { get; set; }

        public string LcmStatusEngSoftware { get; set; }
        public string LcmStatusEngHardware { get; set; }

        public virtual ICollection<ReasonCheckboxResourceLcmEngineeringSoftware> CheckboxResourceLcmEngineeringSoftwares { get; set; }
        public string SoftwareSupportType { get; set; }

        //hardware
        public short? HardwareSupportedId { get; set; }
        public bool SparesProvisioned { get; set; }
        public DateTime? HardwareEndOfSupportContract { get; set; }
        public bool RenewalInProgress { get; set; }
        public string HardwareSupportType { get; set; }
        public string HardwareSupportProvider { get; set; }

        public int Order { get; set; }

        public bool? Archived { get; set; }

        public string ResourceKey { get; set; }
        public string PreviousResourceKey { get; set; }
        public bool? IsExtendedSupportOfferedByVendor { get; set; }
        public bool? HwIsExtendedSupportOfferedByVendor { get; set; }
        public bool? Isreleasedetailunknown { get; set; }
        public int? VerticalResponsibleId { get; set; }

        public virtual ICollection<ReasonCheckboxResourceLcmEngineeringHardware> CheckboxResourceLcmEngineeringHardwares { get; set; }

        // contacts
        public virtual ICollection<LcmEngineeringEduSpoc> LcmEngineeringEduSpoc { get; set; }

        public virtual ICollection<LcmAncillaryData> LcmAncillaryData { get; set; }
        public virtual ICollection<LcmEngineeringSubDomainSpoc> LcmEngineeringSubDomainSpoc { get; set; }

        public virtual ICollection<LcmOperationalContracts> LCMOperationContracts { get; set; }

        [StringLength(1000)]
        public string HardwareSheetIndex { get; set; }
        [StringLength(1000)]
        public string SoftwareSheetIndex { get; set; }

        public string LCMDeploymentStatusValue { get; set; }

        [ForeignKey(nameof(DesignComponentId))]
        [InverseProperty("Lcmengineerings")]
        public virtual DesignComponent DesignComponent { get; set; }

        public virtual DesignComponentFamily Designcomponentfamily { get; set; }

        [ForeignKey(nameof(LCMDeploymentStatusId))]
        [InverseProperty("Lcmengineerings")]
        public virtual LCMDeploymentStatus LCMDeploymentStatus { get; set; }

        [ForeignKey(nameof(OpCoId))]
        [InverseProperty("Lcmengineerings")]
        public virtual OpCo OpCo { get; set; }
        [InverseProperty(nameof(PlannedActivity.LcmEngineering))]
        public virtual ICollection<PlannedActivity> PlannedActivities { get; set; }
        [ForeignKey(nameof(ProductImportanceId))]
        [InverseProperty(nameof(ProductImportance.LcmEngineerings))]
        public virtual ProductImportance ProductImportanceRel { get; set; }
        [ForeignKey(nameof(SoftwareSupportedId))]
        [InverseProperty(nameof(SupportedResource.LcmEngineeringsSoftware))]
        public virtual SupportedResource SoftwareSupported { get; set; }
        [ForeignKey(nameof(HardwareSupportedId))]
        [InverseProperty(nameof(SupportedResource.LcmEngineeringsHardware))]
        public virtual SupportedResource HardwareSupported { get; set; }
        [ForeignKey(nameof(FullorPartialSupportId))]
        [InverseProperty(nameof(FullOrPartialResource.LcmEngineering))]
        public virtual FullOrPartialResource FullOrPartialResourceRel { get; set; }

        public DateTime? VendorEndOfMaintenanceDateSoftware { get; set; }
        public DateTime? VendorEndOfMaintenanceDateHardware { get; set; }

        public VerticalResponsible VerticalResponsible { get; set; }
        public long BuildBagId { get; set; }

        public virtual BuildBag BuildBag { get; set; }
    }
}