using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Classes
    {
        public Classes()
        {
            Identitiesasis = new HashSet<Identitiesasis>();
            Types = new HashSet<Types>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int Categoryid { get; set; }
        public int Creationuser { get; set; }
        public DateTime? Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime? Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Identitiesasis> Identitiesasis { get; set; }
        public virtual ICollection<Types> Types { get; set; }
    }
}
