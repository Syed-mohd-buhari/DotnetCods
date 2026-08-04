

using System.Collections.Generic;

namespace CAM.DataTransferObjects.LookUp.Type
{
    public class TypeDtoUpdate: TypeDtoCreate       
    {
        public IDictionary<int, string> ClassResource { get; set; }
        public int CategoryId { get; set; }
    }
}
