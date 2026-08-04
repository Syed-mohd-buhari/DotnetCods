using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.FunctionalityDto
{
    public class SystemTypeFilterDto
    {
        public List<long> SystemTypeId { get; set; }
        public List<string> SystemTypeNameVodafone { get; set; }
        public List<string> SystemTypeName3Gpp { get; set; }
        public List<string> SystemTypeNameOem { get; set; }
        public List<long> MajorSoftwareBuildsId { get; set; }
        public List<int> SystemSolutionId { get; set; }
        public List<string> ConstraintScaling { get; set; }
        public List<string> ConstraintLcm { get; set; }
        public List<DateTime> EndOfMaintenance { get; set; }
        public List<int> ProductImportance { get; set; }
        public List<int> VerticalResponsibleId { get; set; }
        public List<int> SubDomainResponsibleId { get; set; }
        public List<string> SubDomainSpoc { get; set; }
        public List<int> AssetCategoryId { get; set; }
        public List<int> AssetClassId { get; set; }
        public List<int> AssetTypeId { get; set; }
        public List<string> SpareFieldsJson { get; set; }
        public List<int> AssetCategory { get; set; }
        public List<int> AssetClassIdNavigation { get; set; }

        public List<int> AssetTypeIdNavigation { get; set; }
        public List<int> MajorSoftwareBuilds { get; set; }
        public List<int> SubDomainResponsible { get; set; }
        
        public List<int> VerticalResponsible { get; set; }

    }
}
