using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Networkelementclusterasplanned
    {
        public Networkelementclusterasplanned()
        {
            Clusterupgradestatus = new HashSet<Clusterupgradestatus>();
        }

        public long Networkelementclusterasplannedid { get; set; }
        public long? Infraclusterasplannedid { get; set; }
        public decimal? Applicationid { get; set; }
        public string Appclustername { get; set; }
        public short? Deploymentstatusid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Productname Application { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Deploymentstatuses Deploymentstatus { get; set; }
        public virtual Infraclusterasplanned Infraclusterasplanned { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Clusterupgradestatus> Clusterupgradestatus { get; set; }
    }
}
