
using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.AbstractionLayer
{
    public class PlanndActivitySoftwareUpgradDetailsDtoGrid
    {
        public string Product { get; set; }
        public string Oem {  get; set; }
        public List<PaPlannedActionDto> PlannedAction { get; set; }
    }

    public class PaPlannedActionDto
    {
        public long PaId { get; set; }
        public string ActionDetail { get; set; }
        public string CurrentDcName { get; set; }
        public string PlannedDcName { get; set; }
        public DateTime? Plannedcompletion { get; set; }
        public long? Lcmengineeringid { get; set; }

    }
    public class SignPostAndArchivePaPlannedActionDtoGrid
    {
        public long PaId { get; set; }
        public string ActionDetail { get; set; }
        public string CurrentDcName { get; set; }
        public string PlannedDcName { get; set; }
        public DateTime? Plannedcompletion { get; set; }
        public long? Lcmengineeringid { get; set; }    
        public string Product { get; set; }
        public string Oem { get; set; }
        public string PaSwVersion { get; set; }

    }

    public class ProductComplainceDto
    {
        public Decimal ProductId { get; set; }
        public string ProductName { get; set; }
        public string EOMValue { get; set; }
        public DateTime? EndOfMaintenance { get; set; }
        public DateTime? EndOfSupport { get; set; }
        public string CompliantProductCount { get; set; }
        public string NonCompliantProductCount { get; set; }
        public string TotalProductCount { get; set; }
        public string CompatibilityColorCode { get; set; }

    }

    public class ProductComplianceSummaryDto
    {
        public List<ProductComplainceDto> Products { get; set; }

        public string OverallGreenPercentage { get; set; }
        public string OverallRedPercentage { get; set; }
        public long  OverallProductCount { get;set; }
        public long  OverallComplaintProductCount { get;set; }
        public long  OverallNonComplaintProductCount { get;set; }
    }


}
