using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Componentmanufacturers
    {
        public Componentmanufacturers()
        {
            Componentsoftwarebuilds = new HashSet<Componentsoftwarebuilds>();
        }

        public long Componentmanufacturerid { get; set; }
        public string Componentmanufacturer { get; set; }
        public string Componentname { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Componentsoftwarebuilds> Componentsoftwarebuilds { get; set; }
    }
}
