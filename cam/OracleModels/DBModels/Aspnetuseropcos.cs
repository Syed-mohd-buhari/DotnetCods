using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Aspnetuseropcos
    {
        public int Aspnetuseropcoid { get; set; }
        public int? Userid { get; set; }
        public short? Opcoid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public bool? Isrestrictedopco { get; set; }
        public bool? Isinusedopcos { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
