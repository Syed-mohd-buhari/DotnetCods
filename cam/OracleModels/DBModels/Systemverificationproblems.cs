using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Systemverificationproblems
    {
        public long Systemverificationproblemid { get; set; }
        public string Problemid { get; set; }
        public short? Opcoid { get; set; }
        public long? Systemtypeid { get; set; }
        public short? Environmentid { get; set; }
        public string Subnetwork { get; set; }
        public DateTime? Datefound { get; set; }
        public long? Problemcategoryid { get; set; }
        public string Problemdescription { get; set; }
        public string Vendorcsr { get; set; }
        public string Maintenancereference { get; set; }
        public int? Severity { get; set; }
        public string Status { get; set; }
        public string Mitigation { get; set; }
        public string Solutiondescription { get; set; }
        public string Patchreference { get; set; }
        public string Productupgradereference { get; set; }
        public string Supplemental { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Testreport { get; set; }
        public string Standardnir { get; set; }
        public string Ericssonsecreport { get; set; }
        public string Swandstentries { get; set; }
        public string Pentestingreport { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Environments Environment { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Problemcategory Problemcategory { get; set; }
        public virtual Severity SeverityNavigation { get; set; }
        public virtual Systemtypes Systemtype { get; set; }
    }
}
