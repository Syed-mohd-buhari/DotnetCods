using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.LcmEngineering
{
    public class NFVICompatibilityAtGlanceDtoGrid
    {
        [DisplayName("Market")]
        [Default]
        public string Market { get; set; }

        [DisplayName("Application(Product)")]
        [Default]
        public string Application { get; set; }

        [DisplayName("Domain(Vertical)")]
        [Default]
        public string Domain { get; set; }

        [DisplayName("Design Component")]
        [Default]
        public string DesignComponent { get; set; }

        [DisplayName("Current VNF")]
        [Default]
        public string CurrentVNF { get; set; }

        [DisplayName("Minimum VNF")]
        [Default]
        public double MinimumVNF { get; set; }

        [DisplayName("Planned VNF")]
        [Default]
        public string PlannedVNF { get; set; }

        [DisplayName("Planned Upgrade")]
        [DateRangeGrid]
        [Default]
        public DateTime? PlannedUpgrade { get; set; } = null;

        [DisplayName("Status")]
        [Default]
        public string Status { get; set; }

        [DisplayName("Deleivery Status")]
        [Default]
        public string DeleiveryStatus { get; set; }

        [DisplayName("EDU SPOC")]
        [Default]
        public string EduSpoc { get; set; }

        [DisplayName("SUB Domain SPOC")]
        [Default]
        public string SubDomainSpoc { get; set; }

        [IgnoreGrid]
        public int VodafoneNameId { get;set; }
        [IgnoreGrid]
        public int OpcoId { get; set; }
        [IgnoreGrid]
        public decimal ProductNameId { get; set; }
        [IgnoreGrid]
        public int OemId { get; set; }
        [IgnoreGrid]
        public string OemName { get; set; }
        [IgnoreGrid]
        public int VerticalId { get; set; }

        [IgnoreGrid]
        public string VodafoneName { get; set; }
        [IgnoreGrid]
        public List<FilterValueDto> VerticalDropDown { get; set; }
        [IgnoreGrid]
        public FilterValueDto OpcoDropDown { get; set; }
        [IgnoreGrid]
        public FilterValueDto OemDropDown { get; set; }
        [IgnoreGrid]
        public FilterValueDto VfDropDown { get; set; }
        [IgnoreGrid]
        public FilterValueDto PlannedCompletionDropDown { get; set; }
        [IgnoreGrid]
        public long ComplaintFilters { get; set; }
        [IgnoreGrid]
        public long DecommissioningFilters { get; set; }
        [IgnoreGrid]
        public long SWUpgradePlanOkFilters { get; set; }
        [IgnoreGrid]
        public long SWUpgradePlanNotOkFilters { get; set; }
        [IgnoreGrid]
        public long NoMinVnfProvidedForPlannedDcFilters { get; set; }
        [IgnoreGrid]
        public long SWUpgradeNoPlanFilters { get; set; }
        [IgnoreGrid]
        public long NoMinVnfProvidedFilters { get; set; }
        [IgnoreGrid]
        public long PlannedCompletionDataProvidedFilters { get; set; }
        [IgnoreGrid]
        public long PlannedCompletionDataNotProvidedFilters { get; set; }

    }

    public class NFVICompatibilityAtGlanceDto
    {
        public long Errorid { get;set; }
        public decimal ProductNameId { get; set; }
        public int OemId { get; set; }
        public string LcmDeploymentStatus { get; set; }
        public double CurrentSoftwareVersion { get; set; }
        public virtual ICollection<Plannedactivities> PlannedactivitiesLcmengineering { get; set; }
        public decimal PlannedProductNameId { get; set; }
        public int PlannedOemId { get; set; }
        public double PlannedSoftwareVersion { get; set; }
        public string Opco { get; set; }
        public string ProductName { get; set; }
        public string VerticalName { get; set; }
        public string DesignComponent { get; set; }
        public DateTime PlannedCompletionDate { get; set; }

        public string DeliveryStatus { get; set; }

        public string EduSpoc { get; set; }
        public string SubDomainSpoc { get; set; }


    }

}