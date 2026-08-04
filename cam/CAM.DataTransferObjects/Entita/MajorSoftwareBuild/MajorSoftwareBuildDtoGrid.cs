using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.MajorSoftwareBuild
{
    public class MajorSoftwareBuildDtoGrid : MajorSoftwareBuildDto
    {
        [OrderGrid(Order = 5)]
        [DisplayName("Software Index")]
       
        public long MajorSoftwareBuildId { get; set; }
        [OrderGrid(Order = 1)]
        [DisplayName("Equipment Manufacturer")]
        [Default]
        public string OriginalEquipmentManufacturer { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("EOM")]
        [Default]
        [DateRangeGrid]
        public string EndOfMaintenanceValue { get; set; }
        [OrderGrid(Order = 11)]
        [DisplayName("Vulnerability Status")]
        public string VulnerabilityStatus { get; set; }
        [IgnoreGrid]
        [DisplayName("Third Party SW Components")]
        public IDictionary<long, string> ThirdPartySoftwareComponents { get; set; }
        [OrderGrid(Order = 12)]
        [DisplayName("OS")]
        public string OperatingSystem { get; set; }

        [OrderGrid(Order = 13)]
        [DisplayName("Software Family")]
        [Default]
        public string CriticalAssetType { get; set; }

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

        [DisplayName("Functional Entity")]
        [OrderGrid(Order = 14)]
        //[Default]
        public string NetworkFunction { get; set; }

        [DisplayName("Description")]
        [OrderGrid(Order = 16)]
        //[Default]
        public string Description { get; set; }



        //Ticket 767 - Bundle - CRUD operation for MajorsoftwareBuildBundle Table and implement Configuration File
        [OrderGrid(Order = 17)]
        public string TciBundleVersion { get; set; }

        [OrderGrid(Order = 18)]
         
        public string TcpBundleVersion { get; set; }

        [OrderGrid(Order = 20)]
        [DateRangeGrid]
        [Default]
        [DisplayName("Last Modified")]
        public string LastModifiedValue { get; set; }
        [OrderGrid(Order = 15)]
        [DisplayName("Is Platform Software")]
        public bool IsPlatform { get; set; }
        [IgnoreGrid]
        [DisplayName("Original Equipment Manufacturer Id")]
        public long OriginalEquipmentManufacturerId { get; set; }
        [IgnoreGrid]
        [DisplayName("Product Name Id")]
        public long ProductNameId { get; set; }

    }
    public class ProductBasedSoftwareBuildDtoGrid 
    {
        [IgnoreGrid]        
        public long MajorSoftwareBuildId { get; set; }
        [OrderGrid(Order = 1)]
        public string   SoftwareVersion { get; set; }
        [OrderGrid(Order = 2)]
        public string LifeCycleStatus { get; set; }
        [OrderGrid(Order = 3)]
        public string NetworkStatus { get; set; }

       
        [DateRangeGrid]
        [OrderGrid(Order = 4)]
        [DisplayName("EOS")]        
        public string EndOfsupport { get; set; }

        [DateRangeGrid]
        [OrderGrid(Order = 5)]
        [DisplayName("EOM")]
        public string EndOfMaintenance { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("DesignContact")]
        public string DesignContactEmail { get; set; }

        [IgnoreGrid]
        public string SystemTypeId { get; set; }
        [IgnoreGrid]
        public string DesignComponentId { get; set; }
        [IgnoreGrid]
        public string LcmEngineeringId { get; set; }
        [IgnoreGrid]
        public string LcmOpcos { get; set; }
        [IgnoreGrid]
        public string UserVerticals { get; set; }
        [IgnoreGrid]
        public string LinkedHardware { get; set; }
        [IgnoreGrid]
        public string LcmOpcoMapping { get; set; }
    }
}