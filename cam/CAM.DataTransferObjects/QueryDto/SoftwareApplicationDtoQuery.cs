using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ProductNameDtoQuery : QueryObject
    {
        public List<decimal> Id { get; set; }
        public List<string> Description { get; set; }
        public List<string> VodafoneName { get; set; }
        public List<string> IsPlatformSoftware { get; set; }
    }
}
