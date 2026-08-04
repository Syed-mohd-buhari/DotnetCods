using System.Collections.Generic;

namespace CAM.DataTransferObjects.LookUp.CnfName
{
    public class CnfNameCreateDto : CnfNameDtoGrid
    {
        public decimal ProductId { get; set; }

        public Dictionary<decimal,string> ProductResource {  get; set; }
    }
}
