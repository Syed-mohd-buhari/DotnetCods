using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Nfvitransitions
    {
        public long Nfvitransitionid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Opcoid { get; set; }
        public short? Statuslabmcid { get; set; }
        public short? Statuslabscid { get; set; }
        public short? Statuslivemcid { get; set; }
        public short? Statuslivescid { get; set; }
        public string Spare1json { get; set; }
        public short? Status12kswitchid { get; set; }
        public string Nextstep { get; set; }
        public string Nfvisitedesignation { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Nfvistatuses Status12kswitch { get; set; }
        public virtual Nfvistatuses Statuslabmc { get; set; }
        public virtual Nfvistatuses Statuslabsc { get; set; }
        public virtual Nfvistatuses Statuslivemc { get; set; }
        public virtual Nfvistatuses Statuslivesc { get; set; }
    }
}
