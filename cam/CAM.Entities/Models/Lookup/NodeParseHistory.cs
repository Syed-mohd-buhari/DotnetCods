using CAM.Entities.Models.Base;
using CAM.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.Lookup
{
    public partial class NodeParseHistory :AuditableEntity
    {
        public decimal NodeParseHistoryId { get; set; }
        public decimal? XmlParseRunId { get; set; }
        public string OpCo { get; set; }
        public string Oem { get; set; }
        public string ElementName { get; set; }
        public string ParseType { get; set; }
        public short? UpdatedRows { get; set; }
        public string Status { get; set; }
        public int CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public int ModificationUser { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public DateTime? NodeProcessStartTime { get; set; }
        public DateTime? NodeProcessEndTime { get; set; }
        public string NodeType { get; set; }
        public long? NodeCount { get; set; }
        public virtual XmlParseRun XmlParseRun { get; set; }
    }
}
