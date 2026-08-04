using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Function
    {
        public Function()
        {
            Functionarea = new HashSet<Functionarea>();
            Swconfigfunctionareas = new HashSet<Swconfigfunctionareas>();
        }

        public decimal Functionid { get; set; }
        public decimal? Softwareconfigurationid { get; set; }
        public string Functionname { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Softwareconfiguration Softwareconfiguration { get; set; }
        public virtual ICollection<Functionarea> Functionarea { get; set; }
        public virtual ICollection<Swconfigfunctionareas> Swconfigfunctionareas { get; set; }
    }
}
