using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Criticalassettypes
    {
        public Criticalassettypes()
        {
            Componentsoftwarebuilds = new HashSet<Componentsoftwarebuilds>();
            Majorsoftwarebuilds = new HashSet<Majorsoftwarebuilds>();
            Subnetworkboundaries = new HashSet<Subnetworkboundaries>();
        }

        public int Id { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Description { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Componentsoftwarebuilds> Componentsoftwarebuilds { get; set; }
        public virtual ICollection<Majorsoftwarebuilds> Majorsoftwarebuilds { get; set; }
        public virtual ICollection<Subnetworkboundaries> Subnetworkboundaries { get; set; }
    }
}
