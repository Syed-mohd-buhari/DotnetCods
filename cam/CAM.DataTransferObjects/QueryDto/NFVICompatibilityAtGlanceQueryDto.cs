using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;
using System;
using CAM.DataTransferObjects.FunctionalityDto;

namespace CAM.DataTransferObjects.QueryDto
{
    public class NFVICompatibilityAtGlanceQueryDto : QueryObject
    {
       
        public List<short> Market { get; set; }
        public List<int> Application { get; set; }

        public List<long> Domain { get; set; }

        public List<long> DesignComponent { get; set; }

        public List<long> CurrentVNF { get; set; }

        public List<long> MinimumVNF { get; set; }

        public List<long> PlannedVNF { get; set; }

        public DateFilter PlannedUpgrade { get; set; }
       
        public List<string> Status { get; set; }

        public List<long> DeleiveryStatus { get; set; }

        public List<long> EduSpoc { get; set; }

        public List<long> SubDomainSpoc { get; set; }
        public List<int> OemId { get; set; }
        public List<int> VodafoneNameId { get; set; }

        public List<long> ComplaintFilters { get; set; }

        public List<long> DecommissioningFilters { get; set; }

        public List<long> SWUpgradePlanOkFilters { get; set; }

        public List<long> SWUpgradePlanNotOkFilters { get; set; }

        public List<long> NoMinVnfProvidedForPlannedDcFilters { get; set; }

        public List<long> SWUpgradeNoPlanFilters { get; set; }

        public List<long> NoMinVnfProvidedFilters { get; set; }

        public List<long> PlannedCompletionDataProvidedFilters { get; set; }

        public List<long> PlannedCompletionDataNotProvidedFilters { get; set; }



    }
}
