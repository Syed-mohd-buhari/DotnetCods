using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class MajorSoftwareBuildQueryDto : QueryObject
    {
        public List<long> MajorSoftwareBuildId { get; set; }
        public List<short> OriginalEquipmentManufacturer { get; set; }
        //public List<string> SoftwareApplicationType { get; set; }
        public List<string> SoftwareVersion { get; set; }
        public List<decimal> ProductName { get; set; }
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

        //Ticket 767 - Bundle - CRUD operation for MajorsoftwareBuildBundle Table and implement Configuration File   
        public List<string> TCPBundleVersion { get; set; }

        public List<string> TCIBundleVersion { get; set; }

        public List<string> DesignContact { get; set; }
        public List<string> IsPlatform { get; set; }
       
        public string GlobalSearchKeyword { get; set; }
    }

    public class ProductComplainceQueryDto : MajorSoftwareBuildQueryDto
    {
        public List<int> DesignContactIds { get; set; }
    }

    public class ProductBasedSwQueryDto : QueryObject
    {     
      
        public long ProductId { get; set; }
        public long CurrentVersionSwId { get; set; }
    }
}