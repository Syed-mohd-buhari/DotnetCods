using CAM.Infrastucture.Enums;

namespace CAM.Infrastucture.QueryResult
{
    public class RenderDetail
    {
        public string PropertyName { get; set; }
        public bool Show { get; set; }
        public bool Archive { get; set; }
        public int Order { get; set; }
        public GridFilterType Type { get; set; }
        public string Tab { get; set; }
        public string ColorHeader { get; set; }
        public bool Ignore { get; set; }

    }

}