using System.Collections.Generic;
using CAM.DataTransferObjects.Entita.SoftwareBuildCompatibility;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita.MajorSoftwareBuild
{
    public class MajorSoftwareBuildDtoCreate : MajorSoftwareBuildDto
    {
        public long MajorSoftwareBuildId { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
        public IDictionary<short, string>? OriginalEquipmentManufacturerResource { get; set; }
        [Required(ErrorMessage = "Equipment Manufacturer is required")]
        public short OriginalEquipmentManufacturerId { get; set; }
     
        public string VulnerabilityStatus { get; set; }
        public IEnumerable<long> ThirdPartySoftwareComponentsId { get; set; }
        public IDictionary<short, string>? OperatingSystemResource { get; set; }

        public short? OperatingSystemId { get; set; }
        public IDictionary<int, string>? CriticalAssetTypeResource { get; set; }
        public int ? CriticalAssetTypeId { get; set; }

        public List<ProductNameDropdownList>  ProductNamesResource { get; set; }
        public decimal? ProductNameId { get; set; }

        public IEnumerable<int> NetworkFunctionsIds { get; set; }

        public IDictionary<int, string>? NetworkFunctionsResource { get; set; }

        //Ticket 653 -Rename Grid display name from Assests to Assets  and  the file name from Dynamic Report to Generic Report in Generic Report 
        public  long? ExistSystemTypeId { get; set; }

        //Ticket 767 - Bundle - CRUD operation for MajorsoftwareBuildBundle Table and implement Configuration File        
        public List<long> TCPSoftwareCompatibilityIdList { get; set; }
        public List<long> TCISoftwareCompatibilityIdList { get; set; }

        public IDictionary<long, string> TCPBundleVersion { get; set; }

        public IDictionary<long, string> TCIBundleVersion { get; set; }

        public IDictionary<int, string> DesignContacts { get; set; }

        public IEnumerable<int> DesignContactIds { get; set; }
        public bool? IsPlatform { get; set; }



    }
    public class ProductNameDropdownList
    {

        public short Key { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }


    }
}