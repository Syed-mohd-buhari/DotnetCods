using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Teammembers
    {
        public int Teammemberid { get; set; }
        public int? Teamid { get; set; }
        public int? Userid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Teams Team { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
