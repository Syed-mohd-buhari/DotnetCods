using System.Collections.Generic;

namespace CAM.DataTransferObjects.LookUp.VNFName
{
    public class VnfNameCreateDto : VnfNameDtoGrid
    {
        public decimal ProductId { get; set; }

        public Dictionary<decimal,string> ProductResource {  get; set; }
    }
}
