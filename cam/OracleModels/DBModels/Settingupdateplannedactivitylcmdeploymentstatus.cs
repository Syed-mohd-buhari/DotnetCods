using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Settingupdateplannedactivitylcmdeploymentstatus
    {
        public short Id { get; set; }
        public short Settingupdateplannedactivityid { get; set; }
        public short Lcmdeploymentstatusid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Lcmdeploymentstatus Lcmdeploymentstatus { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Settingsupdateplannedactivity Settingupdateplannedactivity { get; set; }
    }
}
