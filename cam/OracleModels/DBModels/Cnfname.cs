using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Cnfname
    {
        public Cnfname()
        {
            Cnfcluster = new HashSet<Cnfcluster>();
            Cnfclusterinfo = new HashSet<Cnfclusterinfo>();
        }

        public long Cnfnameid { get; set; }
        public string Cnfdescription { get; set; }
        public decimal? Productid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Productname Product { get; set; }
        public virtual ICollection<Cnfcluster> Cnfcluster { get; set; }
        public virtual ICollection<Cnfclusterinfo> Cnfclusterinfo { get; set; }
    }
}
