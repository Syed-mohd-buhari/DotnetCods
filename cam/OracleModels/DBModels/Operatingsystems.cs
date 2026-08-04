using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Operatingsystems
    {
        public Operatingsystems()
        {
            Componentsoftwarebuilds = new HashSet<Componentsoftwarebuilds>();
            Majorsoftwarebuilds = new HashSet<Majorsoftwarebuilds>();
        }

        public short Operatingsystemid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Operatingsystemname { get; set; }
        public string Operatingsystemversion { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Componentsoftwarebuilds> Componentsoftwarebuilds { get; set; }
        public virtual ICollection<Majorsoftwarebuilds> Majorsoftwarebuilds { get; set; }
    }
}
