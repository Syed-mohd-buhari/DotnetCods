using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Designaspectsnetworkfunctions
    {
        public int Id { get; set; }
        public long Designaspectid { get; set; }
        public int Networkfunctionid { get; set; }
        public int? Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int? Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Designaspects Designaspect { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Networkfunctions Networkfunction { get; set; }
    }
}
