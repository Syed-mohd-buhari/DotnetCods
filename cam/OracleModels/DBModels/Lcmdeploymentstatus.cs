using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Lcmdeploymentstatus
    {
        public Lcmdeploymentstatus()
        {
            Lcmengineering = new HashSet<Lcmengineering>();
            Settingupdateplannedactivitylcmdeploymentstatus = new HashSet<Settingupdateplannedactivitylcmdeploymentstatus>();
        }

        public short Id { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Description { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Lcmengineering> Lcmengineering { get; set; }
        public virtual ICollection<Settingupdateplannedactivitylcmdeploymentstatus> Settingupdateplannedactivitylcmdeploymentstatus { get; set; }
    }
}
