using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Auditlogs
    {
        public decimal Auditlogid { get; set; }
        public string Entityname { get; set; }
        public string Entityfield { get; set; }
        public string Oldvalue { get; set; }
        public string Newvalue { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long? Entityid { get; set; }
        public string Entitystate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
