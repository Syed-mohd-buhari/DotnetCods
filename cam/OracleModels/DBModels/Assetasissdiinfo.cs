using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Assetasissdiinfo
    {
        public long Assetasissdiinfoid { get; set; }
        public string Datasourcename { get; set; }
        public string Datasourcetype { get; set; }
        public string Swversion { get; set; }
        public string Firmwareversion { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Tsrmodel { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
