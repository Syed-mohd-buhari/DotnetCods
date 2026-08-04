using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Assetcategories
    {
        public Assetcategories()
        {
            Assettypes = new HashSet<Assettypes>();
            Systemtypes = new HashSet<Systemtypes>();
        }

        public int Assetcategoryid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Assetcategory { get; set; }
        public int? Assetclassid { get; set; }
        public bool Takefromassettypetable { get; set; }

        public virtual Assetclasses Assetclass { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Assettypes> Assettypes { get; set; }
        public virtual ICollection<Systemtypes> Systemtypes { get; set; }
    }
}
