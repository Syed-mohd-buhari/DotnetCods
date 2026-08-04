using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Tsrpassthrough
    {
        public long Tsrpassthroughid { get; set; }
        public string Assetid { get; set; }
        public string Vodafoneuniqueidentifier { get; set; }
        public string Assetname { get; set; }
        public string Assetdescriptionorpurpose { get; set; }
        public string Assettype { get; set; }
        public string Businessowner { get; set; }
        public string Supportowner { get; set; }
        public string Supportteam { get; set; }
        public string Supportteamsplaceintheorganisation { get; set; }
        public string Assetfunction { get; set; }
        public string Deploymentorlifecyclestatus { get; set; }
        public string Relatedriskidsfromriskregisters { get; set; }
        public string Regulatoryscope { get; set; }
        public string Countrywhereassetislocated { get; set; }
        public string Geolocation { get; set; }
        public string Infrastructure { get; set; }
        public string Upstreamdependencies { get; set; }
        public string Downstreamdependencies { get; set; }
        public string Changestotheassetsincedeployment { get; set; }
        public string Cloudhostedasset { get; set; }
        public string Cloudtype { get; set; }
        public string Cloudvendor { get; set; }
        public string Equipmentname { get; set; }
        public string Hostlocationwithinphysicallocation { get; set; }
        public string Softwarevendorname { get; set; }
        public string Firmwareversionpatchlevel { get; set; }
        public string Maintenancesupportsupplier { get; set; }
        public string Dependanthardware { get; set; }
        public string Instancetype { get; set; }
        public string Operatingsystemname { get; set; }
        public string Operatingsystemswvversion { get; set; }
        public string Operatingsystemswversionpatchlevel { get; set; }
        public string Systemnamedns { get; set; }
        public string Systemnamemanagementipaddress { get; set; }
        public string Systemnamenetbios { get; set; }
        public string Systemnamehostname { get; set; }
        public string Hardwarevendorname { get; set; }
        public string Maintenancesupportsuppliersecond { get; set; }
        public string Dependantsystemsoftware { get; set; }
        public string Resiliencemodel { get; set; }
        public string Geographicsiteresilience { get; set; }
        public string Localsiteresilience { get; set; }
        public string Nameofproductsdependantonasset { get; set; }
        public string Technicalservicenames { get; set; }
        public string Customer { get; set; }
        public string Privilegedaccesslogging { get; set; }
        public string Boardormodulenamecomponentname { get; set; }
        public string Exposededge { get; set; }
        public string Externallyfacingsystem { get; set; }
        public string Managementplane { get; set; }
        public string Networkoversightfunction { get; set; }
        public string Pecn { get; set; }
        public string Pecs { get; set; }
        public string Securitycriticalfunction { get; set; }
        public string Productimportance { get; set; }
        public string Critical { get; set; }
        public string Criticalitytype { get; set; }
        public string Serialnumber { get; set; }
        public string Partnumber { get; set; }
        public string Descriptionofplannedaction { get; set; }
        public string Identifiedaction { get; set; }
        public string Prodorlab { get; set; }
        public string Localmarketownership { get; set; }
        public string Budgetestimated { get; set; }
        public string Bundlebudget { get; set; }
        public string Assurancecall { get; set; }
        public string Commentonprojectstatus { get; set; }
        public string Projectstatus { get; set; }
        public string Servicelevel { get; set; }
        public string Lastpentestrefno { get; set; }
        public string Pidata { get; set; }
        public string Encryptedpidata { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Vendorhardwareendofsupportdate { get; set; }
        public string Maintenancehardwareendofsupportdate { get; set; }
        public string Dateassetmovedtolivestatus { get; set; }
        public string Dateassetdecommissioned { get; set; }
        public string Vendorsoftwareendofsupportdate { get; set; }
        public string Maintenancesoftwareendofsupportdate { get; set; }
        public string Lastupgradedate { get; set; }
        public string Lastpentestdate { get; set; }
        public string Projectenddate { get; set; }
        public int? Recordclassifier { get; set; }
        public int? Nontemsvertical { get; set; }
        public string Firmwareversion { get; set; }
        public string Boardormoduletypecomponentsubtype { get; set; }
        public string Boardormoduletypecomponentversionnumber { get; set; }
        public string Model { get; set; }
        public string Productname { get; set; }
        public string Softwareversion { get; set; }
        public string Subdomainresponsible { get; set; }
        public bool? Issortingallowed { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Verticalresponsibles NontemsverticalNavigation { get; set; }
    }
}
