using CAM.Infrastucture.Enums;

namespace CAM.Infrastucture.QueryResult
{
    public class GenericReportInfrastructureDto
    {
        public int? Order { get; set; }
        public bool Archive { get; set; }
        public string ColorHeader { get; set; }
        public string TableName { get; set; }
        public string PropertyName { get; set; }
        public string UpdatedPropertyName { get; set; }
        public bool Show { get; set; }
        public GridFilterType Type { get; set; }
        public string Tab { get; set; }
        public bool Ignore { get; set; }
    }

}