using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class AuditLogQueryDto : QueryObject
    {
        public List<long> AuditLogId { get; set; }
        public List<string> EntityName { get; set; }
        public List<string> EntityField { get; set; }
        public List<string> OldValue { get; set; }
        public List<string> NewValue { get; set; }
        public DateFilter LastModifiedValue { get; set; }
        public List<long> EntityId { get; set; }
        public List<string> EntityState { get; set; }
    }
}
