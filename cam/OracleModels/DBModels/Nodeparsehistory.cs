using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Nodeparsehistory
    {
        public decimal Nodeparsehistoryid { get; set; }
        public decimal? Xmlparserunid { get; set; }
        public string Opco { get; set; }
        public string Oem { get; set; }
        public string Elementname { get; set; }
        public string Parsetype { get; set; }
        public short? Updatedrows { get; set; }
        public string Status { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public DateTime? Nodeprocessstarttime { get; set; }
        public DateTime? Nodeprocessendtime { get; set; }
        public string Nodetype { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Xmlparserun Xmlparserun { get; set; }
    }
}
