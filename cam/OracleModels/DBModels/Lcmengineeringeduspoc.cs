using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Lcmengineeringeduspoc
    {
        public long Lcmengineeringeduspocid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long Lcmengineeringid { get; set; }
        public int? Eduspocid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers Eduspoc { get; set; }
        public virtual Lcmengineering Lcmengineering { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
