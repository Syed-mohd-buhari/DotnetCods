using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Networkelement
    {
        public Networkelement()
        {
            Hardwareconfiguration = new HashSet<Hardwareconfiguration>();
            Identities = new HashSet<Identities>();
            Softwarecomponent = new HashSet<Softwarecomponent>();
            Softwareconfiguration = new HashSet<Softwareconfiguration>();
        }

        public decimal Networkelementid { get; set; }
        public string Opco { get; set; }
        public string Oem { get; set; }
        public string Elementname { get; set; }
        public DateTime? Dataacquisitiondate { get; set; }
        public string Nodetype { get; set; }
        public string Platformtype { get; set; }
        public string Sitelocation { get; set; }
        public DateTime? Softwareinstalldate { get; set; }
        public DateTime? Softwareinstalldateap { get; set; }
        public DateTime? Softwareinstalldatecp { get; set; }
        public DateTime? Softwareproductdate { get; set; }
        public DateTime? Softwareproductdateap { get; set; }
        public DateTime? Softwareproductdatecp { get; set; }
        public string Softwareproductnumber { get; set; }
        public string Softwareproductnumberap { get; set; }
        public string Softwareproductnumbercp { get; set; }
        public string Softwarereleaseinformation { get; set; }
        public string Softwarereleaseinformationap { get; set; }
        public string Softwarereleaseinformationcp { get; set; }
        public string Spare1ossorenm { get; set; }
        public string Spare2xmlversion { get; set; }
        public DateTime? Xmllastparsefiledate { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Nodetypename { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Hardwareconfiguration> Hardwareconfiguration { get; set; }
        public virtual ICollection<Identities> Identities { get; set; }
        public virtual ICollection<Softwarecomponent> Softwarecomponent { get; set; }
        public virtual ICollection<Softwareconfiguration> Softwareconfiguration { get; set; }
    }
}
