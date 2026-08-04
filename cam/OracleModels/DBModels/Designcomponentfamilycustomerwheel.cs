using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Designcomponentfamilycustomerwheel
    {
        public long Id { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public long? Designcomponentfamilyid { get; set; }
        public int? Customerwheelid { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Customerwheels Customerwheel { get; set; }
        public virtual Designcomponentfamilies Designcomponentfamily { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
