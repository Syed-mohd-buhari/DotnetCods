using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Infrastucture.Enums;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.VolteKPI
{
    public class VolteKPIDashboardDto
    {
        public List<VolteKPIDtoGrid> Items { get; set; }
        public IDictionary<short, string> OpCoResource { get; set; }
        public IDictionary<int, string> VolteKPITypeResource { get; set; }
    }
    public class VolteKPIDtoGrid : GridDtoBase
    {
        public VolteKPIType Type { get; set; }
        public string Program { get; set; }
        public List<VolteKPIColumn> OpCoList { get; set; }
        public List<string> OpCoColumns { get; set; }
    }
}
