using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.GraphicalReports
{
    public class ExodusGraphicalReportDto
    {
        public string PlannedActivityType { get; set; }
        public long OpcoId { get; set; }
        public string OpCoDescrption { get; set; }
        public string Vendor { get; set; }
        public string ProductName { get; set; }
        public long ProductId { get; set; }
        public long SourceNodeCount { get; set; }
        public string InitialStack { get; set; }
        public string TargetStack { get; set; }
        public long PlatformId { get; set; }
        public string BuildConstruction { get; set; }
        public string Status { get; set; }
        public string CurrentDcf { get; set; }
        public long CurrentDcfId { get; set; }

        #region Comapatability Details
        [IgnoreGrid]
        public string compatibilityColorCode { get; set; }
        [IgnoreGrid]
        public long greenCompatibleCount { get; set; }
        [IgnoreGrid]
        public long redCompatibleCount { get; set; }
        [IgnoreGrid]
        public long amberCompatibleCount { get; set; }
        public string greenCompatibilityPercentage { get; set; }
        public string redCompatibilityPercentage { get; set; }
        public string amberCompatibilityPercentage { get; set; }


        public string TotalPercentage { get; set; }
        [IgnoreGrid]
        public short? greenNodeCount { get; set; }
        [IgnoreGrid]
        public short? redNodeCount { get; set; }
        [IgnoreGrid]
        public short? amberNodeCount { get; set; }

        public string greenNodePercentage { get; set; }
        public string redNodePercentage { get; set; }
        public string amberNodePercentage { get; set; }
        public string TotalNodePercentage { get; set; }

        #endregion

        [IgnoreGrid]
        public Dictionary<string,string> VerticalId { get; set; }
        public string VerticalResponsibleName { get; set; }


        public List<FilterValueDto> VerticalFilterDto { get; set; }


    }
}
