using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Nfvistatuses
    {
        public Nfvistatuses()
        {
            NfvitransitionsStatus12kswitch = new HashSet<Nfvitransitions>();
            NfvitransitionsStatuslabmc = new HashSet<Nfvitransitions>();
            NfvitransitionsStatuslabsc = new HashSet<Nfvitransitions>();
            NfvitransitionsStatuslivemc = new HashSet<Nfvitransitions>();
            NfvitransitionsStatuslivesc = new HashSet<Nfvitransitions>();
        }

        public short Nfvistatusid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Nfvistatus { get; set; }
        public string Color { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Nfvitransitions> NfvitransitionsStatus12kswitch { get; set; }
        public virtual ICollection<Nfvitransitions> NfvitransitionsStatuslabmc { get; set; }
        public virtual ICollection<Nfvitransitions> NfvitransitionsStatuslabsc { get; set; }
        public virtual ICollection<Nfvitransitions> NfvitransitionsStatuslivemc { get; set; }
        public virtual ICollection<Nfvitransitions> NfvitransitionsStatuslivesc { get; set; }
    }
}
