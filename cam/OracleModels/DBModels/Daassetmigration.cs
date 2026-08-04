using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Daassetmigration
    {
        public long Daassetmigrationid { get; set; }
        public long Plannedactivityid { get; set; }
        public long? Networkelementasplannedid { get; set; }
        public string Newelementname { get; set; }
        public long? Targetdesigncomponenetid { get; set; }
        public short? Environmentid { get; set; }
        public short? Deploymentstatusid { get; set; }
        public short Opcoid { get; set; }
        public short? Locationid { get; set; }
        public DateTime? Rfodate { get; set; }
        public DateTime? Rfsdate { get; set; }
        public DateTime? Migrationcompletiondate { get; set; }
        public string Trafficnodepercentage { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public DateTime? Bomsubmitteddate { get; set; }
        public DateTime? Hwporaiseddate { get; set; }
        public DateTime? Hwpoarriveddate { get; set; }
        public DateTime? Rfadate { get; set; }
        public short? Platformid { get; set; }
        public decimal? Productnameid { get; set; }
        public bool? Isdecommissioned { get; set; }
        public DateTime? Vecdate { get; set; }
        public DateTime? Startofappintegration { get; set; }
        public DateTime? Migrationstart { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Deploymentstatuses Deploymentstatus { get; set; }
        public virtual Environments Environment { get; set; }
        public virtual Locations Location { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Networkelementsasplanned Networkelementasplanned { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Plannedactivities Plannedactivity { get; set; }
        public virtual Platforms Platform { get; set; }
        public virtual Productname Productname { get; set; }
        public virtual Designcomponents Targetdesigncomponenet { get; set; }
    }
}
