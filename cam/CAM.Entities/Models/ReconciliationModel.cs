using CAM.Entities.Models.Base;
using System;

namespace CAM.Entities.Models
{
    public class ReconciliationModel
    {
        public decimal ReconciliationId { get; set; }
        public string OpCo { get; set; }
        public string ElementName { get; set; }
        public string DeploymentStatus { get; set; }
        public string CurrentSwVersion { get; set; }
        public string NewSwVersion { get; set; }
        public string Status { get; set; }
        public long? AssetId { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
    }
}
