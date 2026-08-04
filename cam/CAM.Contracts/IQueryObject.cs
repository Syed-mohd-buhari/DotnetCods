using System.Collections.Generic;

namespace CAM.Contracts
{
    public interface IQueryObject 
    {
        string SortBy { get; set; }
        bool IsSortAscending { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
        public bool? Deleted { get; set; }
        public bool? Orphan { get; set; }
        public List<string> LastModifiedBy { get; set; }
    }
}
