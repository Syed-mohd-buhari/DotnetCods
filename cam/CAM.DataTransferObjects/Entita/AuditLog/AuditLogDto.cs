using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Identity;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.AuditLog
{
    public class AuditLogDto : GridDtoBase
    {
        

        [Default]
        [OrderGrid(Order = 1)]
        public string EntityName { get; set; }
        [Default]
        [OrderGrid(Order = 2)]
        public string EntityField { get; set; }
        [Default]
        [OrderGrid(Order = 3)]
        public string EntityState { get; set; }
        [OrderGrid(Order = 4)]
        [IgnoreGrid]
        public long AuditLogId { get; set; }
        [Default]
        [OrderGrid(Order = 5)]
        public string OldValue { get; set; }
        [Default]
        [OrderGrid(Order = 6)]
        public string NewValue { get; set; }
        [Default]
        [OrderGrid(Order = 7)]
        public long EntityId { get; set; }

        [IgnoreGrid]
        [OrderGrid(Order = 8)]        
        [DateRangeGrid]
        [DisplayName("Last Modified")]
        public string LastModifiedValue { get; set; }

    }

}
