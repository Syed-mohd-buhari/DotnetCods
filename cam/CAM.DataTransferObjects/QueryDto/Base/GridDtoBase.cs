using System;
using System.ComponentModel;
using CAM.Contracts;
using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.QueryDto.Base
{
    public abstract class GridDtoBase : IGridDtoBase
    {
        [IgnoreGrid]
        public bool? Deleted { get; set; }
        [IgnoreGrid]
        public bool? Orphan { get; set; }
        [DateRangeGrid]
        [DisplayName("Last Modified Date")]
        [Default]
        public virtual DateTime? LastModified { get; set; }
        [MailTo]
        [Default]
        [DisplayName("Last Modified By")]
        public virtual string LastModifiedBy { get; set; }
    }
}