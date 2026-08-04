using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Dcflifecycle
    {
        public long Dcflifecycleid { get; set; }
        public long Dcfid { get; set; }
        public string Resourcekey { get; set; }
        public string Previousresourcekey { get; set; }
        public short? Opcoid { get; set; }
        public long? Dcid { get; set; }
        public short? EventId { get; set; }
        public string EventName { get; set; }
        public string Notes { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long? Nodeindex { get; set; }
        public string Currentdetails { get; set; }
        
        public string Opcodescription { get; set; }
        public string Dcfdescription { get; set; }
        public int? Categorytype { get; set; }
        public string Planneddetails { get; set; }
        public string? Bagname { get; set; }

        public string? Dcdescription { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
