using CAM.Infrastucture.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.VolteKPI
{
    public class VolteKPIColumn
    {
        public string Value { get; set; }
        public string BackgroundColor { get; set; }
        public long VolteKPIId { get; set; }
        public short OpCoId { get; set; }
        public VolteKPIType VolteKPIType { get; set; }
    }
}
