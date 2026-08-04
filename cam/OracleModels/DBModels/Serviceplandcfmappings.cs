using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Serviceplandcfmappings
    {
        public int Serviceplandcfmappingid { get; set; }
        public int? Serviceplanid { get; set; }
        public long? Dcfid { get; set; }
        public short? Status { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Designcomponentfamilies Dcf { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Serviceplan Serviceplan { get; set; }
    }
}
