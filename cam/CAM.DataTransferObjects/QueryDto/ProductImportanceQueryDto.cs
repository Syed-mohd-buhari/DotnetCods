using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ProductImportanceQueryDto : QueryObject
    {
        public List<string> ProductImportanceDescription { get; set; }
    }
}