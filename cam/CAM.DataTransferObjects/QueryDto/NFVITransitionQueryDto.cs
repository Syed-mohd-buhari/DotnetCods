using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class NFVITransitionQueryDto : QueryObject
    {
        public List<long> NfviTransitionId { get; set; }
        public List<short> OpCo { get; set; }
        public List<short> StatusLabMC { get; set; }
        public List<short> StatusLabSC { get; set; }
        public List<short> StatusLiveMC { get; set; }
        public List<short> StatusLiveSC { get; set; }
        public List<string> NfviSiteDesignation { get; set; }
        public List<string> NextStep { get; set; }
        public List<short> Status12KSwitch { get; set; }

        public List<string> Spare1Json { get; set; }
        public List<string> LastModifiedBy { get; set; }

        public DateFilter LastModifiedValue { get; set; }
    }
}
