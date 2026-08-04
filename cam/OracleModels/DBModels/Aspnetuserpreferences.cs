using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Aspnetuserpreferences
    {
        public int Aspnetuserpreferenceid { get; set; }
        public int? Userid { get; set; }
        public int? Aspnetmoduleid { get; set; }
        public short? Permission { get; set; }
        public int? Order { get; set; }
        public bool? Isuserpreference { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetmodules Aspnetmodule { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
