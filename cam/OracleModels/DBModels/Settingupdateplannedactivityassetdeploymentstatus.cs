using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Settingupdateplannedactivityassetdeploymentstatus
    {
        public short Id { get; set; }
        public short Settingupdateplannedactivityid { get; set; }
        public short Assetdeploymentstatusid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Deploymentstatuses Assetdeploymentstatus { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Settingsupdateplannedactivity Settingupdateplannedactivity { get; set; }
    }
}
