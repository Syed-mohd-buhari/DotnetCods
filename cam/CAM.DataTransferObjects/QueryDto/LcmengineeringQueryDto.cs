using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class LcmEngineeringQueryDto : QueryObject
    {

        public List<long> LcmEngineeringId { get; set; }
        public List<long> DesignComponentFamilyId { get; set; }
        public List<int> VodafoneName { get; set; }
        public List<long> DesignComponent { get; set; }
        public List<short> LcmDeploymentStatus { get; set; }
        public List<short> OpCo { get; set; }
        public List<string> OperationalContact { get; set; }
        public List<long> DesignComponentId { get; set; }
        public List<string> SoftwareSupportProvider { get; set; }
        public DateFilter SoftwareEndOfWarrantyDate { get; set; }
        public List<string> SoftwareSupportType { get; set; }
        public List<int> NumberOfNodes { get; set; }
        public List<int> NumberOfNodesInLab { get; set; }
        public List<string> SubDomainSpoc { get; set; }
        public List<string> ProductImportanceId { get; set; }
        public List<string> Eduspoc { get; set; }

        public DateFilter SoftwareEndOfSupportContract { get; set; }
        public DateFilter HardwareEndOfSupportContract { get; set; }
        public List<bool> Warranty { get; set; }

        public List<bool> Archived { get; set; }
        public bool OnSoftwareOrHardware { get; set; }
        public bool RenewalInProgress { get; set; }
        public List<long> PlannedActivity { get; set; }
        public new List<string> LastModifiedBy { get; set; }
        public List<string> HardwareSupportProvider { get; set; }
        public List<string> HardwareSupportType { get; set; }

        public new DateFilter LastModifiedValue { get; set; }

        public List<long> OriginalLcm { get; set; }
        public List<string> ResourceKey { get; set; }
        public List<string> PreviousResourceKey { get; set; }
        public List<string> IsExtendedSupportOfferedByVendor { get; set; }
        public List<string> HwIsExtendedSupportOfferedByVendor { get; set; }
        public List<bool> IsReleaseDetailUnKnown { get; set; }
        public List<string> IsLcmAncillaryData { get; set; }

        public List<int> VerticalId { get; set; }
        public List<string> VerticalName { get; set; }

        public List<int> VerticalResponsible { get; set; }

        public List<long> CommentOnProjectStatus { get; set; }

        public List<long> ReasonForNoPlan { get; set; }
        public List<long> BuildBagDescription { get; set; }

    }
}