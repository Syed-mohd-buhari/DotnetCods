using CAM.Infrastucture.Enums;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.VolteKPI
{
    public class VolteKPIReportDto
    {
        public short Year { get; set; }
        public List<VolteKPIReportRow> Provisioned { get; set; }
        public List<VolteKPIReportRow> Registered { get; set; }
        public IDictionary<short, string> OpCoResource { get; set; }
    }
    public class VolteKPIReportRow
    {
        public VolteKPIType Type { get; set; }
        public string OpCo { get; set; }
        public List<VolteKPIReportColumn> MonthValues { get; set; }
    }

    public class VolteKPIReportColumn
    {
        public VolteKPIReportColumn() { }
        public VolteKPIReportColumn(VolteKPIReportColumn copy)
        {
            Quarter = copy.Quarter;
            Month = copy.Month;
            MonthYearLabel = copy.MonthYearLabel;
            Value = copy.Value;
            BackgroundColor = copy.BackgroundColor;
        }
        public string Quarter { get; set; }
        public short Month { get; set; }
        public string MonthYearLabel { get; set; }
        public string Value { get; set; }
        public string BackgroundColor { get; set; }
    }
}
