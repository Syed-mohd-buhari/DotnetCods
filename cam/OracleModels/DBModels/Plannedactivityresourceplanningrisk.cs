using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Plannedactivityresourceplanningrisk
    {
        public long Plnactresourceplanningriskid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Plannedactivityresourceid { get; set; }
        public short Planningriskid { get; set; }
        public bool Forlcm { get; set; }
        public bool? Fordesignaspect { get; set; }
        public bool? Foraddasset { get; set; }
        public bool? Foreditasset { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Plannedactivityresources Plannedactivityresource { get; set; }
        public virtual Planningrisks Planningrisk { get; set; }
    }
}
