using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Lcmdbexportupdatehistory
    {
        public short Id { get; set; }
        public DateTime Lasthwupdatedate { get; set; }
        public DateTime Lastswupdatedate { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
