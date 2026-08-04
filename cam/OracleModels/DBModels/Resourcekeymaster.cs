using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Resourcekeymaster
    {
        public long Resourcekeymasterid { get; set; }
        public long? Resourcetypesid { get; set; }
        public string Resourcekey { get; set; }
        public short? Opcoid { get; set; }
        public long? Dcfid { get; set; }
        public bool? Keystatus { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Elementname { get; set; }
        public int Lifecycleid { get; set; }

        public long Buildbagid { get; set; }
        public long? Componentid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Designcomponentfamilies Dcf { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
         
        public virtual Resourcetypes Resourcetypes { get; set; }
    }
}
