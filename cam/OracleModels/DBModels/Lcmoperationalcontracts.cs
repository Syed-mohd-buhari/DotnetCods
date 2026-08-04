using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Lcmoperationalcontracts
    {
        public short Id { get; set; }
        public long Lcmid { get; set; }
        public short Operationalcontractid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Lcmengineering Lcm { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Operationalcontracts Operationalcontract { get; set; }
    }
}
