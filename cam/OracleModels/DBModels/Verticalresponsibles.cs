using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Verticalresponsibles
    {
        public Verticalresponsibles()
        {
            Assetpassthrough = new HashSet<Assetpassthrough>();
            Cnfclusterinfo = new HashSet<Cnfclusterinfo>();
            Hwpassthroughlcm = new HashSet<Hwpassthroughlcm>();
            Infraclusterasplanned = new HashSet<Infraclusterasplanned>();
            Organisation = new HashSet<Organisation>();
            Swpassthroughlcm = new HashSet<Swpassthroughlcm>();
            Tsrpassthrough = new HashSet<Tsrpassthrough>();
        }

        public int Verticalresponsibleid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Verticalresponsible { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Assetpassthrough> Assetpassthrough { get; set; }
        public virtual ICollection<Cnfclusterinfo> Cnfclusterinfo { get; set; }
        public virtual ICollection<Hwpassthroughlcm> Hwpassthroughlcm { get; set; }
        public virtual ICollection<Infraclusterasplanned> Infraclusterasplanned { get; set; }
        public virtual ICollection<Organisation> Organisation { get; set; }
        public virtual ICollection<Swpassthroughlcm> Swpassthroughlcm { get; set; }
        public virtual ICollection<Tsrpassthrough> Tsrpassthrough { get; set; }
    }
}
