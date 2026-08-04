using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class AuditHistoryQueryDto : QueryObject
    {
        public List<long> Audithistoryid { get; set; }
        public List<string> Opco { get; set; }
        public List<string> Oem  { get; set; }
        public List<string> ElementName { get; set; }
        public List<long> Primarykey { get; set; }
        public List<string> Tablename { get; set; }
        public List<string> Columnname { get; set; }
        public List<string> Oldvalue { get; set; }
        public List<string> Newvalue { get; set; }
        public List<string> Status { get; set; }
        public List<string> Creationuser { get; set; }
        public DateFilter Creationdate { get; set; }
        public List<string> Modificationuser { get; set; }
        public DateFilter Modificationdate { get; set; }
    }
}
