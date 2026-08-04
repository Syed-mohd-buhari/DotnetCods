using System.Collections.Generic;

namespace CAM.Infrastucture
{
    public class RelatedResource
    {
        public string Value { get; set; }
        public string Id { get; set; }
    }
    public class RelatedResourceListValue
    {
        public Dictionary<string,string> Value { get; set; }
        public string Id { get; set; }
    }
}


