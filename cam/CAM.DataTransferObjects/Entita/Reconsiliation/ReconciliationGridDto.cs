using CAM.DataAttributes.Grid;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;

namespace CAM.DataTransferObjects.Entita.Reconsiliation
{
    public class ReconciliationGridDto
    {
        [IgnoreGrid]
        public decimal ReconciliationId { get; set; }
        [Default]
        [OrderGrid(Order = 1)]
        public string OpCo { get; set; }
        [Default]
        [OrderGrid(Order = 2)]
        public string ElementName { get; set; }
        [Default]
        [OrderGrid(Order = 3)]
        public string DeploymentStatus { get; set; }
        [Default]
        [OrderGrid(Order = 4)]
        public string CurrentSwVersion { get; set; }
        [Default]
        [OrderGrid(Order = 5)]
        public string NewSwVersion { get; set; }
        [Default]
        [OrderGrid(Order = 6)]
        public string Status { get; set; }
        [IgnoreGrid]
        public long? AssetId { get; set; }
        [IgnoreGrid]
        public int Creationuser { get; set; }
        [IgnoreGrid]
        public DateTime Creationdate { get; set; }
        [IgnoreGrid]
        public int Modificationuser { get; set; }
        [IgnoreGrid]
        public DateTime Modificationdate { get; set; }
        [IgnoreGrid]
        public bool? Deleted { get; set; }
        [IgnoreGrid]
        public DateTime? Deletiondate { get; set; }
        [IgnoreGrid]
        public long? LcmId { get;set; }
        [IgnoreGrid]
        public long PlannedActivityId { get; set; }

    }
}
