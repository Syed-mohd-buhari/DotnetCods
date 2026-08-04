using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace CAM.DataTransferObjects
{
    public class RelatedRecordsResultDto
    {
        public string EntityName { get; set; }
        public string RecordName { get; set; }
        public List<ResultMessageDto> DataRelatedList { get; set; }
    }

}