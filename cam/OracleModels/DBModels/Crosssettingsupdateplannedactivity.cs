using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Crosssettingsupdateplannedactivity
    {
        public short Id { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Settingsupdateplnactinid { get; set; }
        public short Settingsupdateplnactoutid { get; set; }
        public int Crosssettingsrule { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Settingsupdateplannedactivity Settingsupdateplnactin { get; set; }
        public virtual Settingsupdateplannedactivity Settingsupdateplnactout { get; set; }
    }
}
