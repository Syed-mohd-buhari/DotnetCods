using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Vnfvminstances
    {
        public Vnfvminstances()
        {
            Vnfvmcapacity = new HashSet<Vnfvmcapacity>();
        }

        public long Vnfvminstanceid { get; set; }
        public long Vnfinfoid { get; set; }
        public short Opcoid { get; set; }
        public short Locationid { get; set; }
        public long Noofvnfinstances { get; set; }
        public long Noofvmspertype { get; set; }
        public bool? Numa { get; set; }
        public string Socket { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Locations Location { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Vnfinfo Vnfinfo { get; set; }
        public virtual ICollection<Vnfvmcapacity> Vnfvmcapacity { get; set; }
    }
}
