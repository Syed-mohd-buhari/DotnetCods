using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Audittablesandcolumns
    {
        public decimal Audittablesandcolumnid { get; set; }
        public string Entityname { get; set; }
        public string Entityfield { get; set; }
        public bool? Islogenabled { get; set; }
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
