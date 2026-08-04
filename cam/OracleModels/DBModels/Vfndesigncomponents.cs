using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Vfndesigncomponents
    {
        public Vfndesigncomponents()
        {
            Vnftransitions = new HashSet<Vnftransitions>();
        }

        public short Vnfdesigncomponentid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Designcomponent { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Vnftransitions> Vnftransitions { get; set; }
    }
}
