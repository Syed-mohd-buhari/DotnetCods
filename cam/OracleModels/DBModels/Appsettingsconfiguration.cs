using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Appsettingsconfiguration
    {
        public long Appconfigurationsettingid { get; set; }
        public long? Appsettingid { get; set; }
        public string Settingsvalue { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Appsettings Appsetting { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
