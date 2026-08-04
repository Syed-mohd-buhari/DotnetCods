using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Environments
    {
        public Environments()
        {
            Daassetmigration = new HashSet<Daassetmigration>();
            Networkelementsasplanned = new HashSet<Networkelementsasplanned>();
            Systemverificationproblems = new HashSet<Systemverificationproblems>();
        }

        public short Environmentid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Environment { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Daassetmigration> Daassetmigration { get; set; }
        public virtual ICollection<Networkelementsasplanned> Networkelementsasplanned { get; set; }
        public virtual ICollection<Systemverificationproblems> Systemverificationproblems { get; set; }
    }
}
