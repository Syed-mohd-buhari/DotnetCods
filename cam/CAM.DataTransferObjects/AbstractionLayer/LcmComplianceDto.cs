using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.AbstractionLayer
{
    public class LcmComplianceDto
    {
        public List<string> OpcoDetails { get; set; }
        public List<string> VerticalDetails { get; set; }
        public string GreenCompatibilityPercentage { get; set; }
        public string AmberCompatibilityPercentage { get; set; }
        public string RedCompatibilityPercentage { get; set; }
        public string TotalPercentage { get; set; }
        public string Opco { get; set; }
        public string Eomdate { get; set; }
        public string Oem {  get; set; }
        public string Product { get; set; }

    }
}
