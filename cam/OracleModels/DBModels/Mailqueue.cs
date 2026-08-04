using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Mailqueue
    {
        public long Mailid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public byte State { get; set; }
        public int Approvaluserid { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Errormessage { get; set; }
        public string To { get; set; }

        public virtual Aspnetusers Approvaluser { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
