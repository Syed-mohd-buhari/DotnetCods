using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.ComponentSoftware
{
    public class ComponentSoftwareBuildQueryDto : QueryObject
    {
        public List<long> ComponentSoftwareBuildId { get; set; }
        public List<long> ComponentManufacturers { get; set; }
        
        public List<string> SoftwareVersion { get; set; }
        public List<int> CriticalAssetType { get; set; }
        public DateFilter LastTimeBuyNew { get; set; }
        public DateFilter LastTimeBuyUpgrades { get; set; }
        public DateFilter LastTimeBuyExpansions { get; set; }
        public DateFilter EndOfMaintenanceValue { get; set; }
        public DateFilter EndOfsupportValue { get; set; }
        public DateFilter GeneraAvailableDateValue { get; set; }
        public List<string> DeliveryMethod { get; set; }
        public List<string> VulnerabilityStatus { get; set; }
        public List<short> OperatingSystem { get; set; }
        public List<string> SpareFieldsJson { get; set; }
        public List<string> LastModifiedBy { get; set; }

        public DateFilter LastModifiedValue { get; set; }

        public List<short> NetworkFunction { get; set; }

        public List<string> Description { get; set; } 

        public List<string> DesignContact { get; set; }
    }
}