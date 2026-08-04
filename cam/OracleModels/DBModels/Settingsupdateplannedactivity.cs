using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Settingsupdateplannedactivity
    {
        public Settingsupdateplannedactivity()
        {
            CrosssettingsupdateplannedactivitySettingsupdateplnactin = new HashSet<Crosssettingsupdateplannedactivity>();
            CrosssettingsupdateplannedactivitySettingsupdateplnactout = new HashSet<Crosssettingsupdateplannedactivity>();
            Projectsplan = new HashSet<Projectsplan>();
            Settingupdateplannedactivityassetdeploymentstatus = new HashSet<Settingupdateplannedactivityassetdeploymentstatus>();
            Settingupdateplannedactivitylcmdeploymentstatus = new HashSet<Settingupdateplannedactivitylcmdeploymentstatus>();
        }

        public short Settingsupdateplnactid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Planningactivitystatusid { get; set; }
        public short Budgetavailabilityid { get; set; }
        public short Deliverystatusid { get; set; }
        public int Order { get; set; }
        public int Maxorder { get; set; }
        public DateTime? Lastmodified { get; set; }
        public int Rule { get; set; }
        public int Ruleelementcount { get; set; }
        public string Lastmodifiedby { get; set; }
        public string Localapproval { get; set; }
        public string Settingsupdateplnactdes { get; set; }
        public short? Plannedactivityresourceid { get; set; }
        public short? Successorplannedactivityresourceid { get; set; }
        public bool? Ruleforsuccessorplannedactivitycreation { get; set; }
        public short? Plannedactivitytypefor { get; set; }
        public bool? Specifydc { get; set; }
        public bool? Needplannedasset { get; set; }
        public bool? Isrollback { get; set; }
        public int? Milestonestatus { get; set; }
        public int? Milestonestatusduration { get; set; }
        public bool? Ismilestone { get; set; }

        public virtual Budgetavailability Budgetavailability { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Deliverystatuses Deliverystatus { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Plannedactivityresources Plannedactivityresource { get; set; }
        public virtual Planningactivitystatuses Planningactivitystatus { get; set; }
        public virtual Plannedactivityresources Successorplannedactivityresource { get; set; }
        public virtual ICollection<Crosssettingsupdateplannedactivity> CrosssettingsupdateplannedactivitySettingsupdateplnactin { get; set; }
        public virtual ICollection<Crosssettingsupdateplannedactivity> CrosssettingsupdateplannedactivitySettingsupdateplnactout { get; set; }
        public virtual ICollection<Projectsplan> Projectsplan { get; set; }
        public virtual ICollection<Settingupdateplannedactivityassetdeploymentstatus> Settingupdateplannedactivityassetdeploymentstatus { get; set; }
        public virtual ICollection<Settingupdateplannedactivitylcmdeploymentstatus> Settingupdateplannedactivitylcmdeploymentstatus { get; set; }
    }
}
