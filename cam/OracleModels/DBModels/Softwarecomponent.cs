using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Softwarecomponent
    {
        public Softwarecomponent()
        {
            Component = new HashSet<Component>();
        }

        public decimal Softwarecomponentid { get; set; }
        public decimal? Networkelementid { get; set; }
        public string Opco { get; set; }
        public string Oem { get; set; }
        public string Elementname { get; set; }
        public string Mainsoftwareversion { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Networkelement Networkelement { get; set; }
        public virtual ICollection<Component> Component { get; set; }
    }
}
