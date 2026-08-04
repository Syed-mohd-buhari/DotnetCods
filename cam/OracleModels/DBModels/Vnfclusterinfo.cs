using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Vnfclusterinfo
    {
        public Vnfclusterinfo()
        {
            Vnfinfo = new HashSet<Vnfinfo>();
        }

        public long Vnfclusterinfoid { get; set; }
        public short Opcoid { get; set; }
        public short Locationid { get; set; }
        public long Clusternameid { get; set; }
        public short? Noofblades { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Filename { get; set; }
        public int? Revision { get; set; }
        public long Hardwaretypeid { get; set; }

        public virtual Clustername Clustername { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Vnfhardware Hardwaretype { get; set; }
        public virtual Locations Location { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual ICollection<Vnfinfo> Vnfinfo { get; set; }
    }
}
