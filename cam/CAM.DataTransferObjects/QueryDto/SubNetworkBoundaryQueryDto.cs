using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
   public class SubNetworkBoundaryQueryDto : QueryObject
    {
        public List<long> SubNetworkBoundaryId { get; set; }
        public List<string> SubNetworkBoundaryDescription { get; set; }
        public List<int> VodafoneName { get; set; }

        public List<string> Alias { get; set; }

        public List<short> AllSupportedServices { get; set; }
        public List<int> GdprRelevant { get; set; }
        public List<bool> InternetFacing { get; set; }
        public List<short> LcmPolicy { get; set; }
        public List<string> Criticality { get; set; }
        public List<int> GdrpClassificationValue { get; set; }
        public List<bool> Pcisox { get; set; }

        public List<bool> C3C4 { get; set; }

        public List<bool> MissionCritical { get; set; }

        public List<bool> SecurityElement { get; set; }

        public List<int> CriticalAssetType { get; set; }
        public List<short> SystemFunction { get; set; }
        public List<short> CustomerWheel { get; set; }

        public DateFilter LastModifiedValue { get; set; }
    }
}
