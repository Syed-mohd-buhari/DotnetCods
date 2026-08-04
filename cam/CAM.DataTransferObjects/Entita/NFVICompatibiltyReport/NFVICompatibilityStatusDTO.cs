using CAM.DataTransferObjects.FunctionalityDto;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.NFVICompatibiltyReport
{
    public class NFVICompatibilityStatusDTO
    {
        public int OpCoId { get; set; }
        public string Opco { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int VerticalId {get;set;}
        public string VerticalName { get; set; }
        public int VodafoneNameId { get; set; }
        public string VodafoneName { get; set; }
        public int PlannedCompletionDataProvided { get; set; }
        public int PlannedCompletionDataNotProvided { get; set; }
        public int Complaint { get;set; }
        public int SWUpgradePlanOk { get; set; }
        public int SWUpgradeNoPlan { get; set; }
        public int NoMinVnfProvided { get; set; }
        public int Decommissioning { get; set; }
        public int SWUpgradePlanNotOk { get; set; }
        public int NoMinVnfProvidedForPlannedDc { get; set; }
        public int OverAllStatusCount { get; set; }
        public int ProductNameId { get; set; }
        public decimal PlatformVersionId { get; set; }
        public List<FilterValueDto> VerticalIdDto { get; set; }
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
