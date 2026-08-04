using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Designcomponentfamilysystemfunction
    {
        public long Designcompfamilysysfuncid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long Designcomponentfamilyid { get; set; }
        public short Systemfunctionid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Designcomponentfamilies Designcomponentfamily { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Systemfunctions Systemfunction { get; set; }
    }
}
