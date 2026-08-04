using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Majorswbuildsdesigncontacts
    {
        public long Majorswbuidlsdesigncontactid { get; set; }
        public int Designcontactid { get; set; }
        public long Majorsoftwarebuildsid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers Designcontact { get; set; }
        public virtual Majorsoftwarebuilds Majorsoftwarebuilds { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
