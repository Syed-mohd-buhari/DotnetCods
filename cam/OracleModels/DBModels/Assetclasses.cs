using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Assetclasses
    {
        public Assetclasses()
        {
            Assetcategories = new HashSet<Assetcategories>();
            Systemtypes = new HashSet<Systemtypes>();
        }

        public int Assetclassid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Assetclass { get; set; }
        public int Assetcategoryid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Assetcategories> Assetcategories { get; set; }
        public virtual ICollection<Systemtypes> Systemtypes { get; set; }
    }
}
