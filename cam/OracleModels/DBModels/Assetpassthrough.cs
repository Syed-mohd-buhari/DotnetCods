using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Assetpassthrough
    {
        public long Passthroughid { get; set; }
        public long? Vodafoneuniqueidentifier { get; set; }
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
        public string Maintenancesupportsuppliersecond { get; set; }
        public string Dependantsystemsoftware { get; set; }
        public string Resiliencemodel { get; set; }
        public string Geographicsiteresilience { get; set; }
        public string Localsiteresilience { get; set; }
        public string Nameofproductsdependantonasset { get; set; }
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
        public string Critical { get; set; }
        public string Partnumber { get; set; }
        public string Prodorlab { get; set; }
        public string Localmarketownership { get; set; }
        public string Assurancecall { get; set; }
        public string Servicelevel { get; set; }
        public string Lastpentestrefno { get; set; }
        public string Pidata { get; set; }
        public string Encryptedpidata { get; set; }
        public string Maintenancehardwareendofsupportdate { get; set; }
        public string Dateassetmovedtolivestatus { get; set; }
        public string Dateassetdecommissioned { get; set; }
        public string Lastpentestdate { get; set; }
        public string Firmwareversion { get; set; }
        public string Boardormoduletypecomponentsubtype { get; set; }
        public string Boardormoduletypecomponentversionnumber { get; set; }
        public string Serialnumber { get; set; }
        public string Hardwaretypeofhardwareasset { get; set; }
        public string Hwendofsale { get; set; }
        public string Softwareproducttype { get; set; }
        public string Applicationhostedonsoftware { get; set; }
        public string Uuidorserialnumberofsoftware { get; set; }
        public string Swendofsale { get; set; }
        public string VendorendofmaintenanceDate { get; set; }
        public string Application { get; set; }
        public string Physicalserverhostname { get; set; }
        public string Physicalserveripaddress { get; set; }
        public string Physicalserverserialnumber { get; set; }
        public string PhysicalserverhwModel { get; set; }
        public string Physicalservervendor { get; set; }
        public string Virtualserverhostedon { get; set; }
        public string Virtualservermanufacturer { get; set; }
        public string Virtualservertypeofdevice { get; set; }
        public string Virtualmachinetype { get; set; }
        public string Virtualserverserialnumber { get; set; }
        public string Virtualservertype { get; set; }
        public string Osstartdate { get; set; }
        public string Osinstallationdate { get; set; }
        public string Osstatus { get; set; }
        public string Softwarename { get; set; }
        public string Version { get; set; }
        public string Release { get; set; }
        public string Language { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Hwopscontractstatus { get; set; }
        public string Swopscontractstatus { get; set; }
        public string Plannedhwmodel { get; set; }
        public string Locationordatacentre { get; set; }
        public string Physicalserverosname { get; set; }
        public string Physicalosversion { get; set; }
        public string Physicalserverosstartdate { get; set; }
        public string Physicalserverosinstallationdate { get; set; }
        public string Physicalserverosstatus { get; set; }
        public string Cloud { get; set; }
        public int? Nontemsvertical { get; set; }
        public string Resourcekey { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Verticalresponsibles NontemsverticalNavigation { get; set; }
        public virtual Swpassthroughlcm ResourcekeyNavigation { get; set; }
    }
}
