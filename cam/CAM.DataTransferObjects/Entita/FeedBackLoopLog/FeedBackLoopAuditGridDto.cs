using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.FeedBackLoopLog
{
    public class FeedBackLoopAuditGridDto
    {
        [OrderGrid(Order = 1)]
        [Default]
        public string OpCo { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        public string Oem { get; set; }
        [OrderGrid(Order = 3)]
        [Default]
        public string NodeType { get; set; }
        [OrderGrid(Order = 4)]
        [IgnoreGrid]
        public decimal FeedBackLoopAuditId { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        public long? FileCount { get; set; }
        [OrderGrid(Order = 6)]
        [Default]
        // [DateRangeGrid]
        public string ProcessStartTime { get; set; }
        [OrderGrid(Order = 7)]
        [Default]
        //[DateRangeGrid]
        public string ProcessEndTime { get; set; }
        [OrderGrid(Order = 8)]
        [Default]
        [IgnoreGrid]
        public int CreationUser { get; set; }
        [OrderGrid(Order = 9)]
        [Default]
        [DateRangeGrid]
        public DateTime CreationDate { get; set; }
        [OrderGrid(Order = 10)]
        [IgnoreGrid]
        public int ModificationUser { get; set; }
        [OrderGrid(Order = 11)]
        [Default]
        [DateRangeGrid]
        public DateTime ModificationDate { get; set; }
        public TimeSpan? ProcessingTime { get; set; }
        public List<string> ProcessedFile { get; set; }


    }


}