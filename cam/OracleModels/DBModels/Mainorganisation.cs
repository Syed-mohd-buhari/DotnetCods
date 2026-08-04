using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Mainorganisation
    {
        public Mainorganisation()
        {
            Organisation = new HashSet<Organisation>();
        }

        public int Mainorganisationid { get; set; }
        public string Mainorganisationdescription { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Organisation> Organisation { get; set; }
    }
}
