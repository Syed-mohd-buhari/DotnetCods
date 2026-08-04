using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Subnetworksupportedsvr
    {
        public int Id { get; set; }
        public long Subnetworkid { get; set; }
        public int Serviceid { get; set; }
        public int? Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int? Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Supportedservices Service { get; set; }
        public virtual Subnetworkboundaries Subnetwork { get; set; }
    }
}
