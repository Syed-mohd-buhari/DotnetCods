using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Sites
    {
        public int Siteid { get; set; }
        public short Locationid { get; set; }
        public string Sitecode { get; set; }
        public string Sitecategory { get; set; }
        public string Region { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Locations Location { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
