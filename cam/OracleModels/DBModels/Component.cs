using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Component
    {
        public decimal Componentid { get; set; }
        public decimal? Softwarecomponentid { get; set; }
        public string Componentname { get; set; }
        public DateTime? Productiondate { get; set; }
        public string Productionnumber { get; set; }
        public string Productionrevision { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Softwarecomponent Softwarecomponent { get; set; }
    }
}
