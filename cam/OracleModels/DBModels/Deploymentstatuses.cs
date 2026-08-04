using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Deploymentstatuses
    {
        public Deploymentstatuses()
        {
            Daassetmigration = new HashSet<Daassetmigration>();
            Infraclusterasplanned = new HashSet<Infraclusterasplanned>();
            Networkelementclusterasplanned = new HashSet<Networkelementclusterasplanned>();
            Networkelementsasplanned = new HashSet<Networkelementsasplanned>();
            Settingupdateplannedactivityassetdeploymentstatus = new HashSet<Settingupdateplannedactivityassetdeploymentstatus>();
        }

        public short Deploymentstatusid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int? Rule { get; set; }
        public bool Checkplannedactivity { get; set; }
        public bool Readonlyplannedactivity { get; set; }
        public bool Defaultvalue { get; set; }
        public string Deploymentstatus { get; set; }
        public string Plannedactivityresourceallowed { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Daassetmigration> Daassetmigration { get; set; }
        public virtual ICollection<Infraclusterasplanned> Infraclusterasplanned { get; set; }
        public virtual ICollection<Networkelementclusterasplanned> Networkelementclusterasplanned { get; set; }
        public virtual ICollection<Networkelementsasplanned> Networkelementsasplanned { get; set; }
        public virtual ICollection<Settingupdateplannedactivityassetdeploymentstatus> Settingupdateplannedactivityassetdeploymentstatus { get; set; }
    }
}
