using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class MajorHardwareBuildQueryDto : QueryObject
    {
        public List<long> MajorHardwareBuildId { get; set; }
        public string Name { get; set; }
        public List<short> OriginalEquipmentManufacturer { get; set; }
        public List<string> HardwareSolution { get; set; }
        public List<short> Platform { get; set; }
        public List<string> HardwareType { get; set; }
        public List<string> OtherHardwareInfo { get; set; }
        public DateFilter LastTimeBuyNewValue { get; set; }
        public DateFilter LastTimeBuyUpgradesValue { get; set; }
        public DateFilter LastTimeBuyExpansionsValue { get; set; }
        public DateFilter EndOfMaintenanceValue { get; set; }
        public DateFilter EndOfsupportValue { get; set; }
        public List<string> VulnerabilityStatus { get; set; }
        public List<string> SpareFieldsJson { get; set; }
        public List<bool> ProprietaryHardware { get; set; }
        public IEnumerable<int> PrincipalIdList { get; set; }

        public List<string> BuildConstruction { get; set; }
        public List<string> LastModifiedBy { get; set; }

        public List<string> Description { get; set; }

        public List<string> DesignContact { get; set; }
        public DateFilter GeneraAvailableDateValue { get; set; }

        public DateFilter LastModifiedValue { get; set; }
    }
}