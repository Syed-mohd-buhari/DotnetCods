using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Gridcustomcolumn
    {
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int Userid { get; set; }
        public string Jsongridcustomizationdata { get; set; }
        public string Classname { get; set; }
        public decimal Gridcustomcolumnid { get; set; }
        public int? Preferencedate { get; set; }
        public int? Messagingdate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
