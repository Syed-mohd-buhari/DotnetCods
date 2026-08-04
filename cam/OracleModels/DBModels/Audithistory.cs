using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Audithistory
    {
        public decimal Audithistoryid { get; set; }
        public string Tablename { get; set; }
        public string Columnname { get; set; }
        public decimal? Primarykey { get; set; }
        public string Opco { get; set; }
        public string Oem { get; set; }
        public string Elementname { get; set; }
        public string Oldvalue { get; set; }
        public string Newvalue { get; set; }
        public string Status { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
