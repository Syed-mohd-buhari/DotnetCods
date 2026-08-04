using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Organisation
    {
        public Organisation()
        {
            Aspnetuserverticals = new HashSet<Aspnetuserverticals>();
        }

        public long Organisationid { get; set; }
        public int Mainorganisationid { get; set; }
        public int Practiceid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int? Verticalid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Mainorganisation Mainorganisation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Practice Practice { get; set; }
        public virtual Verticalresponsibles Vertical { get; set; }
        public virtual ICollection<Aspnetuserverticals> Aspnetuserverticals { get; set; }
    }
}
