using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Networkelementsasplanned
    {
        public Networkelementsasplanned()
        {
            Assethardwareancillary = new HashSet<Assethardwareancillary>();
            Daassetmigration = new HashSet<Daassetmigration>();
            Identitiesasis = new HashSet<Identitiesasis>();
            Networkelementasplannededuspoc = new HashSet<Networkelementasplannededuspoc>();
            Networkelementasplannedsubdomainspoc = new HashSet<Networkelementasplannedsubdomainspoc>();
            Networkelementsasis = new HashSet<Networkelementsasis>();
            Plannedactivities = new HashSet<Plannedactivities>();
        }

        public long Networkelementasplannedid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Opcoid { get; set; }
        public long Designcomponentid { get; set; }
        public bool Automatedfeedback { get; set; }
        public bool Plannedaction { get; set; }
        public short Environmentid { get; set; }
        public short Deploymentstatusid { get; set; }
        public short? Deploymenttypeid { get; set; }
        public short? Locationid { get; set; }
        public short? Nfvibundleidid { get; set; }
        public short? Orgeqpmanufacturerid { get; set; }
        public string Capacityplanreference { get; set; }
        public string Elementname { get; set; }
        public string Additionalinformation1 { get; set; }
        public string Networkconstruct { get; set; }
        public string Additionalinformation2 { get; set; }
        public string Hwresourcekey { get; set; }
        public string Previoushwresourcekey { get; set; }
        public string Swresourcekey { get; set; }
        public string Previousswresourcekey { get; set; }
        public long? Lcmengineeringid { get; set; }
        public long? Designcomponentfamilyid { get; set; }
        public bool? Isfinalasset { get; set; }
        public long Buildbagid { get; set; }
        public DateTime? Assetlivestatusdate { get; set; }
        public DateTime? Assetdecommissioneddate { get; set; }
        public bool? Isassured { get; set; }
        public string Elementdomianname { get; set; }
        public DateTime? Assetrfodate { get; set; }
        public DateTime? Assetrfsdate { get; set; }
        public DateTime? Bomsubmitteddate { get; set; }
        public DateTime? Hwporaiseddate { get; set; }
        public DateTime? Hwpoarriveddate { get; set; }
        public DateTime? Rfadate { get; set; }

        public virtual Buildbags Buildbag { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Deploymentstatuses Deploymentstatus { get; set; }
        public virtual Deploymenttypes Deploymenttype { get; set; }
        public virtual Designcomponents Designcomponent { get; set; }
        public virtual Designcomponentfamilies Designcomponentfamily { get; set; }
        public virtual Environments Environment { get; set; }
        public virtual Lcmengineering Lcmengineering { get; set; }
        public virtual Locations Location { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Nfvibundleids Nfvibundleid { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual ICollection<Assethardwareancillary> Assethardwareancillary { get; set; }
        public virtual ICollection<Daassetmigration> Daassetmigration { get; set; }
        public virtual ICollection<Identitiesasis> Identitiesasis { get; set; }
        public virtual ICollection<Networkelementasplannededuspoc> Networkelementasplannededuspoc { get; set; }
        public virtual ICollection<Networkelementasplannedsubdomainspoc> Networkelementasplannedsubdomainspoc { get; set; }
        public virtual ICollection<Networkelementsasis> Networkelementsasis { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
    }
}
