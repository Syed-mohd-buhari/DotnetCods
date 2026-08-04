using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Subfunction
    {
        public Subfunction()
        {
            Subfunctionarea = new HashSet<Subfunctionarea>();
            Subfunctionarea2 = new HashSet<Subfunctionarea2>();
        }

        public decimal Subfunctionid { get; set; }
        public decimal? Functionareaid { get; set; }
        public string Subfunctionname { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Functionarea Functionarea { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Subfunctionarea> Subfunctionarea { get; set; }
        public virtual ICollection<Subfunctionarea2> Subfunctionarea2 { get; set; }
    }
}
