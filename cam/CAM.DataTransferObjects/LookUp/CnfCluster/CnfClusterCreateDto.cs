using System.Collections.Generic;

namespace CAM.DataTransferObjects.LookUp.CnfCluster
{
    public class CnfClusterCreateDto : CnfClusterDtoGrid
    {
        public long CnfNameId { get; set; }

        public Dictionary<long,string> CnfNameResource {  get; set; }
    }
}
