using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Systemtypessubdomainspoc
    {
        public long Systemtypessubdomainspocid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long Systemtypeid { get; set; }
        public short Subdomainspocid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Subdomainspocs Subdomainspoc { get; set; }
        public virtual Systemtypes Systemtype { get; set; }
    }
}
