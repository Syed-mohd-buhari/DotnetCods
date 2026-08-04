using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Plannedactivityresourcedriver
    {
        public long Planactivityresdriverid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Plannedactivityresourceid { get; set; }
        public short Driverid { get; set; }
        public bool Forlcm { get; set; }
        public bool? Fordesignaspect { get; set; }
        public bool? Foraddasset { get; set; }
        public bool? Foreditasset { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Drivers Driver { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Plannedactivityresources Plannedactivityresource { get; set; }
    }
}
