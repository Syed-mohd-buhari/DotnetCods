using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Infrastucture.Enums;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class VolteKPIQueryDto : QueryObject
    {
        public List<long> VolteKPIId { get; set; }
        public List<short> OpCo { get; set; }
        public List<VolteKPIType> VolteKPITypes { get; set; }
        public short? Month { get; set; }
        public short? Year { get; set; }
    }
}