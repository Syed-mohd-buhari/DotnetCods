using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.Entita.AuditHistory
{
    public class AuditHistoryDto
    {
        [OrderGrid(Order =8)]
        [Default]
        public string OldValue { get; set; }
        [OrderGrid(Order = 9)]
        [Default]
        public string NewValue { get; set; }
        [OrderGrid(Order =10)]
        public string Status { get; set; }
        [OrderGrid(Order =11)]
        public string CreationUser { get; set; }
        [OrderGrid(Order =12)]
        [DateRangeGrid]
        public DateTime CreationDate { get; set; } 
        [OrderGrid(Order =13)]
        public string ModificationUser { get; set; }
        [OrderGrid(Order =14)]
        [DateRangeGrid]
        public DateTime ModificationDate { get; set; }
    }
    public class AuditHistoryDtoGrid : AuditHistoryDto
    {

        [OrderGrid(Order = 1)]
        [Default]
        public string OpCo { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        public string Oem { get; set; }
        [OrderGrid(Order = 3)]
        [Default]
        public string ElementName { get; set; }
        [OrderGrid(Order = 4)]
        public long AuditHistoryId { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        [IgnoreGrid]
        public long PrimaryKey { get; set; }
        [OrderGrid(Order = 6)]
        [Default]
        [IgnoreGrid]
        public string TableName { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        public string ColumnName { get; set; }
       

    }
}
