using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Assethardwareancillary
    {
        public Assethardwareancillary()
        {
            Assetcapacityinfo = new HashSet<Assetcapacityinfo>();
        }

        public long Assethardwareancillaryid { get; set; }
        public long Networkelementasplannedid { get; set; }
        public long? Datacenterid { get; set; }
        public long? Clusternameid { get; set; }
        public long? Assetclustertypeid { get; set; }
        public long? Assetclusterid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long? Majorhardwarebuildasisid { get; set; }

        public virtual Assetcluster Assetcluster { get; set; }
        public virtual Assetclustertype Assetclustertype { get; set; }
        public virtual Clustername Clustername { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Datacenter Datacenter { get; set; }
        public virtual Majorhardwarebuildasis Majorhardwarebuildasis { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Networkelementsasplanned Networkelementasplanned { get; set; }
        public virtual ICollection<Assetcapacityinfo> Assetcapacityinfo { get; set; }
    }
}
