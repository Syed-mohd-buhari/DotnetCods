using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Datacenter
    {
        public Datacenter()
        {
            Assethardwareancillary = new HashSet<Assethardwareancillary>();
        }

        public long Datacenterid { get; set; }
        public string Description { get; set; }
        public short Opcoid { get; set; }
        public bool? Environmentzone { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual ICollection<Assethardwareancillary> Assethardwareancillary { get; set; }
    }
}
