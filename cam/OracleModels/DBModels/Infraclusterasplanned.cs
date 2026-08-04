using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Infraclusterasplanned
    {
        public Infraclusterasplanned()
        {
            Clusterupgradestatus = new HashSet<Clusterupgradestatus>();
            Networkelementclusterasplanned = new HashSet<Networkelementclusterasplanned>();
        }

        public long Infraclusterasplannedid { get; set; }
        public short? Opcoid { get; set; }
        public short? Locationid { get; set; }
        public string Site { get; set; }
        public short? Platformid { get; set; }
        public long? Clustertypeid { get; set; }
        public string Clustername { get; set; }
        public long? Hardwaretype { get; set; }
        public short? Deploymentstatusid { get; set; }
        public int? Verticalresponsibleid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Majorsoftwarebuilds Clustertype { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Deploymentstatuses Deploymentstatus { get; set; }
        public virtual Majorhardwarebuilds HardwaretypeNavigation { get; set; }
        public virtual Locations Location { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Platforms Platform { get; set; }
        public virtual Verticalresponsibles Verticalresponsible { get; set; }
        public virtual ICollection<Clusterupgradestatus> Clusterupgradestatus { get; set; }
        public virtual ICollection<Networkelementclusterasplanned> Networkelementclusterasplanned { get; set; }
    }
}
