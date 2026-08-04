using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Temsfntreport
    {
        public long Temsfntreportid { get; set; }
        public string Hostname { get; set; }
        public string Serialnumberofhardwareasset { get; set; }
        public string Locationofhardwareasset { get; set; }
        public string Hardwaretypeofhardwareasset { get; set; }
        public string Vendor { get; set; }
        public string Ipaddressofhardwareasset { get; set; }
        public string Market { get; set; }
        public string Hwendoflife { get; set; }
        public string Hwendofsupport { get; set; }
        public string Hwendofsale { get; set; }
        public string Hardwaremodules { get; set; }
        public string Softwareproducttype { get; set; }
        public string Softwareproductversion { get; set; }
        public string Softwareisvirtualized { get; set; }
        public string OperatingSystemofvirtualmachine { get; set; }
        public string Applicationhostedonsoftware { get; set; }
        public string Uuidserialnumberofsoftware { get; set; }
        public string Softwarevendor { get; set; }
        public string Locationofsoftware { get; set; }
        public string Servicetype { get; set; }
        public string Swendoflife { get; set; }
        public string Swendofsupport { get; set; }
        public string Swendofsale { get; set; }
        public string Verticalengineeringteam { get; set; }
        public string Verticalsubdomain { get; set; }
        public string Platform { get; set; }
        public string Riskcluster { get; set; }
        public string Operationscontactpoint { get; set; }
        public string Assetcategory { get; set; }
        public string Assetclass { get; set; }
        public string Assettype { get; set; }
        public string Assetdescription { get; set; }
        public string Productimportance { get; set; }
        public string Operationsmaintenancecontract { get; set; }
        public string Vendorendofmaintenancedate { get; set; }
        public string Identifiedaction { get; set; }
        public string Descriptionofplannedaction { get; set; }
        public string Model { get; set; }
        public string Businessservicename { get; set; }
        public string Opmaintenancecontractenddate { get; set; }
        public string Incidentclass { get; set; }
        public string Occurrenceprobability { get; set; }
        public string Meverticalresposible { get; set; }
        public string Assetstatus { get; set; }
        public string Typeofnetworkelement { get; set; }
        public string Localmarket { get; set; }
        public string Application { get; set; }
        public string Cloud { get; set; }
        public string Physicalserverhostname { get; set; }
        public string Physicalserveripaddress { get; set; }
        public string Physicalserverserialnumber { get; set; }
        public string Physicalserverhwmodel { get; set; }
        public string Physicalservervendor { get; set; }
        public string Virtualserverhostedon { get; set; }
        public string Virtualservermanufacturer { get; set; }
        public string Virtualservertypeofdevice { get; set; }
        public string Virtualmachinetype { get; set; }
        public string Virtualserveripaddress { get; set; }
        public string Virtualserverserialnumber { get; set; }
        public string Virtualservertype { get; set; }
        public string Osname { get; set; }
        public string Osversion { get; set; }
        public string Osstartdate { get; set; }
        public string Osinstallationdate { get; set; }
        public string Osstatus { get; set; }
        public string Softwarename { get; set; }
        public string Version { get; set; }
        public string Release { get; set; }
        public string Manufacturer { get; set; }
        public string Language { get; set; }
        public string Hwopscontractstatus { get; set; }
        public string Hwopscontractenddate { get; set; }
        public string Swopscontractstatus { get; set; }
        public string Swopscontractenddate { get; set; }
        public string Hwoperationscontactpoint { get; set; }
        public string Swoperationscontactpoint { get; set; }
        public string Physicalserverosname { get; set; }
        public string Physicalserverosversion { get; set; }
        public string Physicalserverosstartdate { get; set; }
        public string Physicalserverosinstallationdate { get; set; }
        public string Physicalserverosstatus { get; set; }
        public string Plannedhwmodel { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
