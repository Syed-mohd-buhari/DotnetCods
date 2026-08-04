using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Networkelementsasis
    {
        public long Networkelementasisid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Opcoid { get; set; }
        public long Systemtypeid { get; set; }
        public short Orgeqpmanufacturerid { get; set; }
        public long Networkelementasplannedid { get; set; }
        public short Locationid { get; set; }
        public DateTime? Softwareproductiondate { get; set; }
        public DateTime? Softwareinstalldate { get; set; }
        public DateTime? Dataacquisitiondate { get; set; }
        public bool Manualoverride { get; set; }
        public string Softwareproductnumber { get; set; }
        public string Elementmanager { get; set; }
        public string Hardwareacquisition { get; set; }
        public string Elementmanagerexportfileformat { get; set; }
        public string Patchdetails { get; set; }
        public string Elementdeploymentname { get; set; }
        public string Nodetype { get; set; }
        public string Dataacquisitionmethod { get; set; }
        public DateTime? Hardwareinstalldate { get; set; }
        public string Platformtype { get; set; }
        public string Hardwaretype { get; set; }
        public string Softwarereleaseinformation { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Locations Location { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Networkelementsasplanned Networkelementasplanned { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Originalequipmentmanufacturers Orgeqpmanufacturer { get; set; }
        public virtual Systemtypes Systemtype { get; set; }
    }
}
