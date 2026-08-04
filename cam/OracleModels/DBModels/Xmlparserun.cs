using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Xmlparserun
    {
        public Xmlparserun()
        {
            Nodeparsehistory = new HashSet<Nodeparsehistory>();
        }

        public decimal Xmlparserunid { get; set; }
        public string Filename { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public DateTime? Fileprocessstarttime { get; set; }
        public DateTime? Fileprocessendtime { get; set; }
        public decimal? Feedbackloopauditid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Feedbackloopaudits Feedbackloopaudit { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Nodeparsehistory> Nodeparsehistory { get; set; }
    }
}
