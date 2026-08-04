using CAM.Entities.Models.Base;
using System;

namespace CAM.Entities.Models.Lookup
{
    public partial class RiskClusterVodafoneNames :AuditableEntity
    {
        public int Riskclustervodafonenameid { get; set; }
        public int? Riskclusterid { get; set; }
        public int? Vodafonenameid { get; set; }            
        public virtual RiskClusters Riskcluster { get; set; }
        public virtual VodafoneNames VodafoneName { get; set; }
    }
}
