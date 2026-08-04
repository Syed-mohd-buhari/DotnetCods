using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Aspnetmodules
    {
        public Aspnetmodules()
        {
            Aspnetuserpreferences = new HashSet<Aspnetuserpreferences>();
            Aspnetuserrolepermissions = new HashSet<Aspnetuserrolepermissions>();
        }

        public int Aspnetmoduleid { get; set; }
        public string Module { get; set; }
        public string Modulepath { get; set; }
        public string Category { get; set; }
        public bool? Isdefault { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Menu { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Aspnetuserpreferences> Aspnetuserpreferences { get; set; }
        public virtual ICollection<Aspnetuserrolepermissions> Aspnetuserrolepermissions { get; set; }
    }
}
