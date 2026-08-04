using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Swconfigsubfunctionareas
    {
        public int Swconfigsubfunctionareaid { get; set; }
        public int? Swconfigsubfunctionid { get; set; }
        public string Subfunctionareaname { get; set; }
        public string Subfunctionareadescription { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Swconfigsubfunction Swconfigsubfunction { get; set; }
    }
}
