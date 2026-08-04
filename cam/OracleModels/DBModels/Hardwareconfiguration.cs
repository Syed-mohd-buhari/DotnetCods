using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Hardwareconfiguration
    {
        public decimal Hardwareconfigurationid { get; set; }
        public decimal? Networkelementid { get; set; }
        public string Opco { get; set; }
        public string Oem { get; set; }
        public string Elementname { get; set; }
        public string Hardwaretype { get; set; }
        public string Productname { get; set; }
        public string Productnumber { get; set; }
        public string Revision { get; set; }
        public string Serialnumber { get; set; }
        public string Unitlocation { get; set; }
        public string Vendor { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Networkelement Networkelement { get; set; }
    }
}
