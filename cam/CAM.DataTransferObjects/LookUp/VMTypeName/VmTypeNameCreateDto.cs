using System.Collections.Generic;

namespace CAM.DataTransferObjects.LookUp.VMTypeName
{
    public class VmTypeNameCreateDto : VmTypeNameDtoGrid
    {
        public long VnfNameId { get; set; }

        public Dictionary<long,string> VnfNamResource { get; set; }
    }
}
