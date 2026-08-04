using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Reconciliations
    {
        public decimal Reconciliationid { get; set; }
        public string Opco { get; set; }
        public string Elementname { get; set; }
        public string Deploymentstatus { get; set; }
        public string Currentswversion { get; set; }
        public string Newswversion { get; set; }
        public string Status { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long? Assetid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
