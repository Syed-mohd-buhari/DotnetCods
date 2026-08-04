using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Subnetworkboundarycustomerwheel
    {
        public long Id { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long? Subnetworkboundaryid { get; set; }
        public int? Customerwheelid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Customerwheels Customerwheel { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Subnetworkboundaries Subnetworkboundary { get; set; }
    }
}
