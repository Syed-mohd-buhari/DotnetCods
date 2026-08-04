using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Userdefinedreportslogs
    {
        public int Userdefinedreportslogid { get; set; }
        public string Reportname { get; set; }
        public string Reportformat { get; set; }
        public string Reportdownloadedpath { get; set; }
        public string Reportstatus { get; set; }
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
