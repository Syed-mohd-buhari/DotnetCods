using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Aspnetroles
    {
        public Aspnetroles()
        {
            Aspnetroleclaims = new HashSet<Aspnetroleclaims>();
            Aspnetuserrolepermissions = new HashSet<Aspnetuserrolepermissions>();
            Aspnetuserroles = new HashSet<Aspnetuserroles>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Normalizedname { get; set; }
        public string Concurrencystamp { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Description { get; set; }
        public string Abstractiontaborder { get; set; }

        public int? Portalroleid { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Aspnetroleclaims> Aspnetroleclaims { get; set; }
        public virtual ICollection<Aspnetuserrolepermissions> Aspnetuserrolepermissions { get; set; }
        public virtual ICollection<Aspnetuserroles> Aspnetuserroles { get; set; }
    }
}
