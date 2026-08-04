using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Componentsoftwarebuildbags
    {
        public long Componentsoftwarebuildbagid { get; set; }
        public long Buildbagid { get; set; }
        public long Componentsoftwarebuildid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Buildbags Buildbag { get; set; }
        public virtual Componentsoftwarebuilds Componentsoftwarebuild { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
