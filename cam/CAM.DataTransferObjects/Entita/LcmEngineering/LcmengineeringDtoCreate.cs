using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.LookUp.ReasonCheckbox;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.LcmEngineering
{
    public interface ILcmEngineeringDto
    {
        public string HardwareSupportType { get; set; }
        List<PlannedActivityDtoUpdate> PlannedActivityDto { get; set; }
        List<int?> SubDomainSpocIds { get; set; }
        List<int?> EduSpocIds { get; set; }
        public List<short> OperationContractsIds { get; set; }

        IDictionary<int, string>? SubDomainSpocResource { get; set; }
        public IDictionary<int, string>? OperationalContractResource { get; set; }

        IDictionary<long, string> DesignComponentResource { get; set; }
        long DesignComponentId { get; set; }
        IDictionary<short, string>? OpCoResource { get; set; }
        short OpCoId { get; set; }
        IDictionary<short, string>? ProductImportanceResource { get; set; }
        short? ProductImportanceId { get; set; }
        IDictionary<short, TipologicaGridDtoRule> SupportedResource { get; set; }
        short? HardwareSupportedId { get; set; }
        short? SoftwareSupportedId { get; set; }
        IDictionary<short, string> FullorPartialSupportResource { get; set; }

        IDictionary<short, string> FullorPartialSupportHWResource { get; set; }
        short? FullorPartialSupportId { get; set; }

        short? FullorPartialSupportHWId { get; set; }

        IEnumerable<int> CheckboxResourceLcmEngineeringSoftwares { get; set; }
        IEnumerable<int> CheckboxResourceLcmEngineeringHardwares { get; set; }
        IDictionary<int, ReasonCheckboxDto>? CheckboxResourceResource { get; set; }
        //string OperationalContact { get; set; }
        //long LcmengineeringId { get; set; }
        string HardwareSheetIndex { get; set; }
        string SoftwareSheetIndex { get; set; }
        bool Warranty { get; set; }
        bool OnSoftware { get; set; }
        bool OnHardware { get; set; }
        int? NumberOfNodes { get; set; }
        int? NumberOfNodesInLab { get; set; }
        DateTime? SoftwareEndOfWarrantyDate { get; set; }
        //DateTime? DateOfLastReviewOrUpdate { get; set; }
        DateTime? SoftwareEndOfSupportContract { get; set; }
        DateTime? HardwareEndOfSupportContract { get; set; }
        string SoftwareSupportProvider { get; set; }
        string SoftwareSupportType { get; set; }
        string HardwareSupportProvider { get; set; }
        bool RenewalInProgress { get; set; }
        bool? Deleted { get; set; }
        bool? Orphan { get; set; }
        DateTime? LastModified { get; set; }
        public bool SparesProvisioned { get; set; }


        IEnumerable<NetworkElementAssociated> NetworkElementAssociateds { get; set; }


    }

    public class LcmEngineeringDtoCreate : LcmEngineeringDto, ILcmEngineeringDto
    {
        public bool? Archived { get; set; } = false;
        public new string HardwareSupportType { get; set; }
        public long DesignComponentFamilyid { get; set; }
        public bool IsReleaseDetailUnKnown { get; set; }
        public bool SparesProvisioned { get; set; }
        public IEnumerable<NetworkElementAssociated> NetworkElementAssociateds { get; set; }

        public IDictionary<long, string> DesignComponentFamilyResource { get; set; }

        public List<PlannedActivityDtoUpdate> PlannedActivityDto { get; set; }

        public List<int?> SubDomainSpocIds { get; set; }

        public List<int?> EduSpocIds { get; set; }

        public List<short> OperationContractsIds { get; set; }

        public IDictionary<int, string>? SubDomainSpocResource { get; set; }
        public IDictionary<int, string>? EduSpocResource { get; set; }

        public IDictionary<int, string>? OperationalContractResource { get; set; }

        public IDictionary<long, string> DesignComponentResource { get; set; }

        ///Ticket 1810 :  Design Components Not Getting Listed in PROD
        public IDictionary<long, string> TransientDesignComponentResource { get; set; }
        public long DesignComponentId { get; set; }
        public IDictionary<short, string> LCMDeploymentStatusResource { get; set; }
        public short? LCMDeploymentStatusId { get; set; }

        public IDictionary<short, string>? OpCoResource { get; set; }

        public short OpCoId { get; set; }

        public IDictionary<short, string>? ProductImportanceResource { get; set; }

        public short? ProductImportanceId { get; set; }

        public IDictionary<short, TipologicaGridDtoRule> SupportedResource { get; set; }

        public short? HardwareSupportedId { get; set; }
        public short? SoftwareSupportedId { get; set; }

        public IDictionary<short, string> FullorPartialSupportResource { get; set; }

        public IDictionary<short, string> FullorPartialSupportHWResource { get; set; }

        public short? FullorPartialSupportId { get; set; }
        public short? FullorPartialSupportHWId { get; set; }

        public IEnumerable<int> CheckboxResourceLcmEngineeringSoftwares { get; set; }
        public IEnumerable<int> CheckboxResourceLcmEngineeringHardwares { get; set; }

        public IDictionary<int, ReasonCheckboxDto>? CheckboxResourceResource { get; set; }
        public IDictionary<int, string> VerticalResponsibles { get; set; }

        #region SystemOfsystem
        public List<ViewBagandComponenetDto>  BuildBagResources { get; set; }
        public long BuildBagId { get; set; }
        #endregion

    }
}