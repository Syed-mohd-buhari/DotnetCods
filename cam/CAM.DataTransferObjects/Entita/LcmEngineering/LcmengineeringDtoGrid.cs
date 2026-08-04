using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.LcmEngineering
{
    public class LcmEngineeringDtoGrid : LcmEngineeringDto
    {
        [OrderGrid(Order = 1)]
        [DisplayName("OpCo")]
        [Default]
        public string OpCo { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Design Component")]
        [Default]
        public string DesignComponent { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("Bag Name")]
        [Default]
        public string BuildBagDescription { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("Lcm Engineering Index")]
        public long LcmEngineeringId { get; set; }

        [IgnoreGrid]
        public string HardwareSupportedId { get; set; }

        [IgnoreGrid]
        public string SoftwareSupportedId { get; set; }

        [IgnoreGrid]
        public string FullorPartialSupportId { get; set; }

        [IgnoreGrid]
        public string FullorPartialSupportHWId { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("DC Index")]
        [Default]
        public long DesignComponentId { get; set; }

        [OrderGrid(Order = 7)]
        [DisplayName("DCF Index")]
        public long DesignComponentFamilyId { get; set; }

        [OrderGrid(Order = 11)]
        [DisplayName("Planned Action")]
        [Default]
        public IDictionary<short, string> PlannedActivity { get; set; }

        [OrderGrid(Order = 12)]
        [DisplayName("Comment On Project Status")]
        [Default]
        public string CommentOnProjectStatus { get; set; }

        [OrderGrid(Order = 13)]
        [DisplayName("Reason For NoPlan")]
        [Default]
        public string ReasonForNoPlan { get; set; }

        [OrderGrid(Order = 21)]
        [DisplayName("Product Importance")]
        [Default]
        public string ProductImportanceId { get; set; }


        [OrderGrid(Order = 22)]
        [DisplayName("Edu-Spoc")]
        [ColorGrid(Color = "red")]
        [HeaderColor(BackgroundColor = 0xbfbfbf, FontColor = 0xe60000 )]
        [Default]
        public string Eduspoc { get; set; }
 
        [OrderGrid(Order = 23)]
        [DisplayName("Archived")]
        public bool Archived { get; set; }


        [OrderGrid(Order = 24)]
        [DisplayName("Sub-Domain Spoc")]
        [ColorGrid(Color = "red")]
        [HeaderColor(BackgroundColor = 0xbfbfbf, FontColor = 0xe60000)]
        [Default]
        public string SubDomainSpoc { get; set; }

        [OrderGrid(Order = 25)]
        [DisplayName("Original LCM Hyperlink")]
        [Default]
        public IDictionary<long, string> OriginalLcm { get; set; }


        [OrderGrid(Order = 26)]
        [DisplayName("Operational Contact")]
        [ColorGrid(Color = "green")]
        [HeaderColor(BackgroundColor = 0xbfbfbf, FontColor = 0x00c300 )]
        [Default]
        public string OperationalContact { get; set; }

        [OrderGrid(Order = 27)]
        [DisplayName("Lcm Deployment Status")]
        [Default]
        public string LcmDeploymentStatus { get; set; }

        [OrderGrid(Order = 29)]
        [DisplayName("Vodafone Name")]
        [Default]
        public string VodafoneName { get; set; }

        [Default]
        [OrderGrid(Order = 34)]
        [DisplayName("Ancillary Data Exists?")]
        public bool IsLcmAncillaryData { get; set; }

        [OrderGrid(Order = 35)]
        [DisplayName("Vertical Name")]
        [ColorGrid(Color = "red")]
        [HeaderColor(BackgroundColor = 0xbfbfbf, FontColor = 0xe60000)]
        [Default]
        public string VerticalName { get; set; }

        [OrderGrid(Order = 36)]
        [DisplayName("Vertical Id")]
        [IgnoreGrid]
        public int VerticalId { get; set; }

        [OrderGrid(Order = 37)]
        [DisplayName("Vertical Responsible")]
        [IgnoreGrid]
        public string VerticalResponsible { get; set; }

        [OrderGrid(Order = 38)]
        [DisplayName("Vertical Responsible Id")]
        [IgnoreGrid]
        public int VerticalResponsibleId { get; set; }
        
        [IgnoreGrid]
        public short OpCoId { get; set; }

    }

    public class LcmEngineeringDtoGridImpact : LcmEngineeringDtoGrid
    {
        private string DesignComponentNameImpact { get; set; }
    }
    public class LcmAtGlanceGridDto
    {

        [IgnoreGrid]
        public long? LcmengineeringId { get; set; } 
        [IgnoreGrid]
        public long OpcoId { get; set; }
        [OrderGrid(Order = 1)]
        public string OpCoDescrption { get; set; }
        [IgnoreGrid]
        public string compatibilityColorCode { get; set; }
        [IgnoreGrid]
        public string compatibilityColorCode2026 { get; set; }
        [IgnoreGrid]
        public string compatibilityColorCode2027 { get; set; }
        [IgnoreGrid]
        public string compatibilityColorCode2028 { get; set; }
        [IgnoreGrid]
        public long greenCompatibleCount { get; set; }
        [IgnoreGrid]
        public long amberCompatibleCount { get; set; }
        [IgnoreGrid]
        public long redCompatibleCount { get; set; }

        [OrderGrid(Order = 2)]
        public string greenCompatibilityPercentage { get; set; }
        [OrderGrid(Order = 3)]
        public string amberCompatibilityPercentage { get; set; }
        [OrderGrid(Order = 4)]
        public string redCompatibilityPercentage { get; set; }


        public string TotalPercentage { get; set; }
        public string TotalPercentage2026 { get; set; }
        public string TotalPercentage2027 { get; set; }
        public string TotalPercentage2028 { get; set; }

        [IgnoreGrid]
        public short? greenNodeCount { get; set; }
        [IgnoreGrid]
        public short? amberNodeCount { get; set; }
        [IgnoreGrid]
        public short? redNodeCount { get; set; }
        [IgnoreGrid]
        public long NodesCount { get; set; }

        [OrderGrid(Order = 5)]
        public string greenNodePercentage { get; set; }
        [OrderGrid(Order = 6)]
        public string amberNodePercentage { get; set; }
        [OrderGrid(Order = 7)]
        public string redNodePercentage { get; set; }
        [OrderGrid(Order = 8)]
        public string TotalNodePercentage { get; set; }



        [OrderGrid(Order = 9)]
        public string ProductName { get; set; }

        public long ProductId { get; set; }

        [OrderGrid(Order = 10)]
        public string SupportedServicesDescription { get; set; }

        public long SupportedServicesId { get; set; }
        public long OrigninalSupportedServicesId { get; set; }


        [IgnoreGrid]
        public long VerticalId { get; set; }
        [OrderGrid(Order = 11)]
        public string VerticalResponsibleName { get; set; }


        [OrderGrid(Order = 12)]
        public string CurrentDc { get; set; }

        public long CurrentDcId { get; set; }
        public List<FilterValueDto> VerticalFilterDto { get; set; }

        public FilterValueDto ProductImportantFilterDto { get; set; }

        public List<ColorCompatability> ColorCompatability { get; set; }
    }

    public class ColorCompatability
    {
        public string year { get; set; }
        public string greenNodePercentage { get; set; }
        public string redNodePercentage { get; set; }

    }

}