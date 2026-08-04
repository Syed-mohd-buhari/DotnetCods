using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Systemtypes
    {
        public Systemtypes()
        {
            Designcomponents = new HashSet<Designcomponents>();
            Networkelementsasis = new HashSet<Networkelementsasis>();
            Systemtypesmajorhardwarebuilds = new HashSet<Systemtypesmajorhardwarebuilds>();
            Systemtypessubdomainspoc = new HashSet<Systemtypessubdomainspoc>();
            Systemverificationproblems = new HashSet<Systemverificationproblems>();
        }

        public long Systemtypeid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Systemtypenamevodafone { get; set; }
        public string Systemtypename3gpp { get; set; }
        public string Systemtypenameoem { get; set; }
        public long? Majorsoftwarebuildsid { get; set; }
        public DateTime? Constraintscaling { get; set; }
        public DateTime? Endofmaintenance { get; set; }
        public int? Assetcategoryid { get; set; }
        public string Sparefieldsjson { get; set; }
        public short? Productimportanceid { get; set; }
        public int? Assetclassid { get; set; }
        public int? Assettypeid { get; set; }
        public string Constraintlcm { get; set; }
        public int? Vodafonename { get; set; }

        public virtual Assetcategories Assetcategory { get; set; }
        public virtual Assetclasses Assetclass { get; set; }
        public virtual Assettypes Assettype { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Majorsoftwarebuilds Majorsoftwarebuilds { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Productimportances Productimportance { get; set; }
        public virtual Vodafonenames VodafonenameNavigation { get; set; }
        public virtual ICollection<Designcomponents> Designcomponents { get; set; }
        public virtual ICollection<Networkelementsasis> Networkelementsasis { get; set; }
        public virtual ICollection<Systemtypesmajorhardwarebuilds> Systemtypesmajorhardwarebuilds { get; set; }
        public virtual ICollection<Systemtypessubdomainspoc> Systemtypessubdomainspoc { get; set; }
        public virtual ICollection<Systemverificationproblems> Systemverificationproblems { get; set; }
    }
}
