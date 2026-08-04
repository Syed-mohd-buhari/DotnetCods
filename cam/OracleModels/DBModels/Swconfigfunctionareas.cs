using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Swconfigfunctionareas
    {
        public Swconfigfunctionareas()
        {
            Swconfigsubfunction = new HashSet<Swconfigsubfunction>();
        }

        public decimal Swconfigfunctionareaid { get; set; }
        public decimal? Functionid { get; set; }
        public string Functionareaname { get; set; }
        public string Functionareadescription { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Function Function { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Swconfigsubfunction> Swconfigsubfunction { get; set; }
    }
}
