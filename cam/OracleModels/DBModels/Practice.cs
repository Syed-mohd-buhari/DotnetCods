using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Practice
    {
        public Practice()
        {
            Organisation = new HashSet<Organisation>();
        }

        public int Practiceid { get; set; }
        public string Practicedescription { get; set; }
        public int? Practiceemailid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Aspnetusers Practiceemail { get; set; }
        public virtual ICollection<Organisation> Organisation { get; set; }
    }
}
