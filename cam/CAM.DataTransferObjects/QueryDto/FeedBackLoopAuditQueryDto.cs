using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class FeedBackLoopAuditQueryDto : QueryObject
    {
        public List<decimal> FeedBackLoopAuditId { get; set; }
        public List<string> OpCo { get; set; }
        public List<string> Oem { get; set; }
        public List<string> CreationUser { get; set; }
        public DateFilter CreationDate { get; set; }
        public List<string> ModificationUser { get; set; }
        public DateFilter ModificationDate { get; set; }
        public List<bool> Deleted { get; set; }
        public DateFilter DeletionDate { get; set; }
        public List<DateTime> ProcessStartTime { get; set; }
        public List<DateTime> ProcessEndTime { get; set; }
        public List<string> NodeType { get; set; }
        public List<long> FileCount { get; set; }

        public List<string> ProcessingTime { get;set; }
        public List<string> ProcessedFile { get; set; }
    }
}