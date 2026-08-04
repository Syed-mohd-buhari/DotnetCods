using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;

namespace CAM.Entities.Models.Lookup
{
    public class XmlParseRun :AuditableEntity
    {
        public XmlParseRun()
        {
            NodeParseHistory = new HashSet<NodeParseHistory>();
        }

        public decimal Xmlparserunid { get; set; }
        public string Filename { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public DateTime? FileProcessStartTime { get; set; }
        public DateTime? FileprocessEndTime { get; set; }
        public virtual ICollection<NodeParseHistory> NodeParseHistory { get; set; }
    }
}
