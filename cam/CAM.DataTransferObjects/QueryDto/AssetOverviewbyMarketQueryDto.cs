using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class AssetOverviewbyMarketQueryDto : QueryObject
    {
        public List<short> SubDomainResponseCeFunctionId { get; set; }
        public List<string> SubDomainResponseCeFunction { get; set; }
        public List<short> OemVendorId { get; set; }
        public List<string> OemVendor { get; set; }
        public List<string> ProductNameNeInstances { get; set; }
        public List<short> NetworkElementsAsPlannedId { get; set; }
        public List<string> NetworkElementsPlannedName { get; set; }
        public List<short> OpcoId { get; set; }
        public List<string> OpCoDescrption { get; set; }
        public List<short> VerticalId { get; set; }
        public List<string> VerticalDescrption { get; set; }
        public List<int> NetworkElementCount { get; set; }
        public List<long> DcfId { get; set; }
        public List<long> HwBuild { get; set; }
        public List<long> SupportService { get; set; }
        public List<short> EnvironmentId { get; set; }
        public List<short> SystemTypetId { get; set; }

    }   
}
