using System.Collections.Generic;
using CAM.Contracts;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Enum;

namespace CAM.DataTransferObjects.QueryDto.Base
{
    public abstract class QueryObject : IQueryObject
    {
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public DateFilter LastModified { get; set; }
        public int PrincipalId { get; set; }
        public bool? Deleted { get; set; }
        public bool? Orphan { get; set; }
        public List<string> LastModifiedBy { get; set; }
        public DateFilter LastModifiedValue { get; set; }
    }
}