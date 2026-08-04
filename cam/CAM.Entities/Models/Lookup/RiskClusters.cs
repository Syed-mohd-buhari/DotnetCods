using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;

namespace CAM.Entities.Models.Lookup
{
    public partial class RiskClusters : AuditableEntity
    {
        public RiskClusters()
        {
            RiskClusterVodafoneNames = new HashSet<RiskClusterVodafoneNames>();
        }

        public int Riskclusterid { get; set; }
        public string Description { get; set; }
        public string Risklevel { get; set; }        
       
        public virtual ICollection<RiskClusterVodafoneNames> RiskClusterVodafoneNames { get; set; }
    }
}
