using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Riskclustervodafonenames
    {
        public int Riskclustervodafonenameid { get; set; }
        public int? Riskclusterid { get; set; }
        public int? Vodafonenameid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Riskclusters Riskcluster { get; set; }
        public virtual Vodafonenames Vodafonename { get; set; }
    }
}
