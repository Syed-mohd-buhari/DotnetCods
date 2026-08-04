using CAM.DataTransferObjects.FunctionalityDto;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.NfviSoftwareCompatibility
{
    public class NfviSoftwareCompatibilityDto
    {
        public long NfviSoftwareCompatibilityId { get; set; }

        public short VendorId { get; set; }
        public long PlaftFormId { get; set; }
        public long ProductId { get; set; }
        public string MinimumSupportedVersion { get; set; }
         
    }

    public class NfviSoftwareCompatibilityCreateEditPageDto : NfviSoftwareCompatibilityDto
    {
       
        public Dictionary<long, string> vmwareMswPlatform { get; set; }
        public List<KeyValuePairDto> ProductName { get; set; }

        public List<KeyValuePairDto> VendorResource { get; set; }

    }
    public class NfviSoftwareCompatibilityDtoDtoCreateUpdate
    {
       // public  List<NfviSoftwareCompatibilityDto> NfviSoftwareBuildCompatibilityList { get; set; }

        public NfviSoftwareCompatibilityDto NfviSoftwareBuildCompatibility
        { get; set; }

    }
    public class NfviSoftwareCompatibilityDtoDelete
    {
        public long NfviSoftwareCompatibilityId { get; set; } 
        
    }
}