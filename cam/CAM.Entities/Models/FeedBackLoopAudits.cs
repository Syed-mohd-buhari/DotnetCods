using System;
using System.Collections.Generic;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models
{
    public class FeedBackLoopAudits
    {
        public FeedBackLoopAudits()
        {
            XmlParseRun = new HashSet<XmlParseRun>();
        }

        public decimal FeedBackLoopAuditId { get; set; }
        public string OpCo { get; set; }
        public string NodeType { get; set; }
        public long? FileCount { get; set; }
        public DateTime? ProcessStartTime { get; set; }
        public DateTime? ProcessEndTime { get; set; }
        public int CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public int ModificationUser { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string Oem { get; set; }

        public virtual ICollection<XmlParseRun> XmlParseRun { get; set; }
    }
}
