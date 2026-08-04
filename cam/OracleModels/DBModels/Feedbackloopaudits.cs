using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Feedbackloopaudits
    {
        public Feedbackloopaudits()
        {
            Xmlparserun = new HashSet<Xmlparserun>();
        }

        public decimal Feedbackloopauditid { get; set; }
        public string Opco { get; set; }
        public string Nodetype { get; set; }
        public long? Filecount { get; set; }
        public DateTime? Processstarttime { get; set; }
        public DateTime? Processendtime { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Oem { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Xmlparserun> Xmlparserun { get; set; }
    }
}
