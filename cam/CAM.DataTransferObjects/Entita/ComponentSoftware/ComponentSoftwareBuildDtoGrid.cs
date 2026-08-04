using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class ComponentSoftwareBuildDtoGrid : ComponentSoftwareBuildDto
    {
        [OrderGrid(Order = 1)]
        [DisplayName("Component Manufacturer")]
        [Default]
        public string ComponentManufacturers { get; set; }


        [OrderGrid(Order = 5)]
        [DisplayName("Component Software Build Index")]

        public long ComponentSoftwareBuildId { get; set; }
      

        [OrderGrid(Order = 6)]
        [DisplayName("EOM")]
        [Default]
        [DateRangeGrid]
        public string EndOfMaintenanceValue { get; set; }

        [OrderGrid(Order = 7)]
        [DateRangeGrid]
        [DisplayName("GA Date")]
        [Default]
        public string GeneraAvailableDateValue { get; set; }

        [OrderGrid(Order = 10)]
        [DateRangeGrid]
        [DisplayName("EOS")]
        [Default]
        public string EndOfsupportValue { get; set; }

        [OrderGrid(Order = 11)]
        [DisplayName("Vulnerability Status")]
        public string VulnerabilityStatus { get; set; }

        [OrderGrid(Order = 12)]
        [DisplayName("OS")]
        public string OperatingSystem { get; set; }

        [OrderGrid(Order = 13)]
        [DisplayName("Software Family")]
        [Default]
        public string CriticalAssetType { get; set; } 

       
        [DisplayName("Description")]
        [OrderGrid(Order = 15)]
       
        public string Description { get; set; } 
       

        [OrderGrid(Order = 19)]
        [DateRangeGrid]
        [Default]
        [DisplayName("Last Modified")]
        public string LastModifiedValue { get; set; }

        [IgnoreGrid]
        [DisplayName("Third Party SW Components")]
        public IDictionary<long, string> ThirdPartySoftwareComponents { get; set; }


    }
}