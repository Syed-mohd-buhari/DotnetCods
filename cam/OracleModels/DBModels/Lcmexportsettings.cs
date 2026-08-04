using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Lcmexportsettings
    {
        public int Id { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Description { get; set; }
        public string Lcmhistoricalinfosw { get; set; }
        public bool? Ishistorical { get; set; }
        public int? Reportlevel { get; set; }
        public string Lcmhistoricalinfohw { get; set; }
        public bool? Isdefault { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
