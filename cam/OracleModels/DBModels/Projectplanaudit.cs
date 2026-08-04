using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Projectplanaudit
    {
        public long Projectplanauditid { get; set; }
        public long? Projectsplanid { get; set; }
        public string Oldvalue { get; set; }
        public string Newvalue { get; set; }
        public short? Processtype { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Projectsplan Projectsplan { get; set; }
    }
}
