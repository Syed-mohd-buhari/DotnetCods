using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class TsrLogQueryDto : QueryObject
    {
        public List<decimal> TsrLogId { get; set; }
        public List<string> TypeOfOperation { get; set; }
        public List<string> FileName { get; set; }
        public List<long> TotalRecord { get; set; }
        public List<long> ProcessedRecord { get; set; }
        public DateFilter StartTime { get; set; }
        public DateFilter EndTime { get; set; }
        public List<string> Status { get; set; }
        public List<string> Domain { get; set; }
        public List<string> BatchIdentifier { get; set; }
    }
}
