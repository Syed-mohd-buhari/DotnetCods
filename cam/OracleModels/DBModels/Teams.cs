using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Teams
    {
        public Teams()
        {
            Teammembers = new HashSet<Teammembers>();
        }

        public int Teamid { get; set; }
        public string Teamname { get; set; }
        public string Teamdescription { get; set; }
        public bool? Isactive { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Teammembers> Teammembers { get; set; }
    }
}
