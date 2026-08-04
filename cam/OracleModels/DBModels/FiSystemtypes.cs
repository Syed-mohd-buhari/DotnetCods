using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class FiSystemtypes
    {
        public long FiSystemtypesid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long Sessionid { get; set; }
        public long? Designcomponentid { get; set; }
        public long Systemtypesid { get; set; }
        public bool Todelete { get; set; }
        public string Description { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
