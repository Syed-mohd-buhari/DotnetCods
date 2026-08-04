using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Aspnetuserverticals
    {
        public int Aspnetuserverticalid { get; set; }
        public int? Userid { get; set; }
        public long? Organisationid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public bool? Isvertical { get; set; }
        public bool? Isverticalresponcible { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Organisation Organisation { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
