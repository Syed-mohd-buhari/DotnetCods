using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Aspnetuserroles
    {
        public int Userid { get; set; }
        public int Roleid { get; set; }
        public int? Creationuser { get; set; }
        public DateTime? Creationdate { get; set; }
        public int? Modificationuser { get; set; }
        public DateTime? Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public decimal Aspnetuserroleid { get; set; }

        public virtual Aspnetroles Role { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
