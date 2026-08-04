using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class SystemTypeQueryDto : QueryObject
    {
        public List<int> SystemTypeId { get; set; }
        public List<int> MajorHardwareBuildId { get; set; }
        public List<int> MajorSoftwareBuildId { get; set; }
        public List<int> vodafoneName { get; set; }
        public List<int> VodafoneNameIds { get; set; }
        public List<string> SystemTypeName3Gpp { get; set; }
        public List<string> SystemTypeNameOem { get; set; }
        public List<long> MajorSoftwareBuild { get; set; }
        public List<string> MajorHardwareBuild { get; set; }
        public List<int> SystemSolution { get; set; }
        public DateFilter ConstraintScaling { get; set; }
        public List<string> ConstraintLcm { get; set; }
        public DateFilter EndOfMaintenance { get; set; }
        public DateFilter LastModifiedValue { get; set; }
        public List<int> ProductImportance { get; set; }
        public List<int> VerticalResponsible { get; set; }
        public List<int> UserListBasedOnVerticalId { get; set; }
        //public List<int> SubDomainResponsible { get; set; }
        //public List<string> SubDomainSpoc { get; set; }
        public List<int> AssetCategory { get; set; }
        public List<int> AssetClass { get; set; }
        public List<int> AssetType { get; set; }
        public List<string> SpareFieldsJson { get; set; }
        public List<short> SoftwareOem { get; set; }
        public List<short> HardwareOem { get; set; }
        public List<string> LastModifiedBy { get; set; }
        public List<string> SoftWareDesignContactEmail { get; set; }
        public List<string> SoftWareVertical { get; set; }
        public List<string> SoftWareSubdomain { get; set; }
        public List<string> HardWareDesignContactEmail { get; set; }
        public List<string> HardWareVertical { get; set; }
        public List<string> HardWareSubdomain { get; set; }


    }
}