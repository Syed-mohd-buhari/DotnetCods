using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class NetworkElementsAsIsQueryDto : QueryObject
    {
        public List<long> NetworkElementAsIsId { get; set; }
        public List<short> OpCo { get; set; }
        public List<short> Oem { get; set; }
        public List<string> NetworkFunction { get; set; }
        public List<string> NodeType { get; set; }
        public List<string> ElementDeploymentName { get; set; }
        public List<short> Location { get; set; }
        public List<long> SystemTypeId { get; set; }
        public List<string> SoftwareReleaseInformation { get; set; }
        public List<string> SoftwareProductNumber { get; set; }
        public DateFilter SoftwareProductionDateValue { get; set; }
        public DateFilter SoftwareInstallDateValue { get; set; }
        public List<string> HardwareSolution { get; set; }
        public List<string> Platform { get; set; }
        public List<string> HardwareType { get; set; }
        public List<string> OtherHardwareInfo { get; set; }
        public List<string> HardwareAcquisition { get; set; }
        public List<bool> ManualOverride { get; set; }
        public List<long> HardwareSystemId { get; set; }
        public DateFilter DataAcquisitionDateValue { get; set; }
        public List<string> DataAcquisitionMethod { get; set; }
        public List<string> ElementManager { get; set; }
        public List<string> ElementManagerExportFileFormat { get; set; }
        public List<string> SpareFieldsJson { get; set; }
        public DateFilter LastModifiedValue { get; set; }
        public DateFilter HardwareInstallDate { get; set; }
        public List<string> VerticalName { get; set; }  
    }
}