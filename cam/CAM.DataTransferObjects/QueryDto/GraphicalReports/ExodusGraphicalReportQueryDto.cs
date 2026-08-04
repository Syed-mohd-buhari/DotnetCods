using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.GraphicalReports
{
    public class ExodusGraphicalReportQueryDto
    {
        public List<string> PlannedActivityType { get; set; }
        public List<short> PlannedActivityResourceRuleId { get; set; }
        public List<long> OpcoId { get; set; }
        public List<string> OpCoDescrption { get; set; }
        public List<string> Vendor { get; set; }
        public List<long> VendorId { get; set; }
        public List<string> InitialStack { get; set; }
        public List<long> PlatformId { get; set; }
        public List<string> TotalNodesInCurrentStack { get; set; }
        public List<string> TargetStack { get; set; }
        public List<long> TargetPlatformId { get; set; }
        public List<string> Status { get; set; }
        public List<string> ProductName { get; set; }
        public List<long> ProductId { get; set; }
        public List<string> CurrentDcf { get; set; }
        public List<long> CurrentDcfId { get; set; }
        public List<string> VerticalName { get; set; }
        public List<int> VerticalId { get; set; }
        public List<short> EnvironmentId { get; set; }
    }
}
