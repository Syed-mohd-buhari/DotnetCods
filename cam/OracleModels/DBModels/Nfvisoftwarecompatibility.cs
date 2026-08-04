using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Nfvisoftwarecompatibility
    {
        public long Nfvisoftwarecompatibilityid { get; set; }
        public long Plaftformid { get; set; }
        public decimal Productid { get; set; }
        public string Minimumsupportedversion { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Vendorid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Majorsoftwarebuilds Plaftform { get; set; }
        public virtual Productname Product { get; set; }
        public virtual Originalequipmentmanufacturers Vendor { get; set; }
    }
}
