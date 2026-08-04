using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Componentsoftwarebuildsdesigncontacts
    {
        public long Componentsoftwarebuildsdesigncontactid { get; set; }
        public int Designcontactid { get; set; }
        public long Componentsoftwarebuildid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Componentsoftwarebuilds Componentsoftwarebuild { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers Designcontact { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
