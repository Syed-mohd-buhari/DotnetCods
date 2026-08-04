using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Resourcetypes
    {
        public Resourcetypes()
        {
            Resourcekeymaster = new HashSet<Resourcekeymaster>();
        }

        public long Resourcetypesid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Resourcekeymaster> Resourcekeymaster { get; set; }
    }
}
