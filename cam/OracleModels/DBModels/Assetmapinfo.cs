using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Assetmapinfo
    {
        public long Assetmapinfoid { get; set; }
        public string Omcassetname { get; set; }
        public string Temsassetname { get; set; }
        public string Enmassetname { get; set; }
        public string Site { get; set; }
        public string Datasourcename { get; set; }
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
