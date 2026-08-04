using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Supportedservices
    {
        public Supportedservices()
        {
            Designaspectssupportedsvr = new HashSet<Designaspectssupportedsvr>();
            Subnetworksupportedsvr = new HashSet<Subnetworksupportedsvr>();
        }

        public int Id { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Description { get; set; }
        public bool? Default { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Designaspectssupportedsvr> Designaspectssupportedsvr { get; set; }
        public virtual ICollection<Subnetworksupportedsvr> Subnetworksupportedsvr { get; set; }
    }
}
