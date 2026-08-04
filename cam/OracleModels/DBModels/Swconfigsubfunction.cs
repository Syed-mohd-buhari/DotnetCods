using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Swconfigsubfunction
    {
        public Swconfigsubfunction()
        {
            Swconfigsubfunctionareas = new HashSet<Swconfigsubfunctionareas>();
        }

        public int Swconfigsubfunctionid { get; set; }
        public decimal? Swconfigfunctionareaid { get; set; }
        public string Subfunctionname { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Swconfigfunctionareas Swconfigfunctionarea { get; set; }
        public virtual ICollection<Swconfigsubfunctionareas> Swconfigsubfunctionareas { get; set; }
    }
}
