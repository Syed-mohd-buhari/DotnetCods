using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Hwpassthroughlcm
    {
        public long Hwpassthroughlcmid { get; set; }
        public int? Nontemsvertical { get; set; }
        public string Reportid { get; set; }
        public string Localmarket { get; set; }
        public string Verticalengineeringteam { get; set; }
        public string Verticalsubdomain { get; set; }
        public string Engineeringcontactpoint { get; set; }
        public string Assetcategory { get; set; }
        public string Assetclass { get; set; }
        public string Assettype { get; set; }
        public string Assetdescription { get; set; }
        public string Assetvirtualized { get; set; }
        public string Productimportance { get; set; }
        public string Hardwaremodel { get; set; }
        public string Productcode { get; set; }
        public string Numberofnodes { get; set; }
        public string Handedovertooperation { get; set; }
        public string Contractrenewalplan { get; set; }
        public string Vendorendofvulnerabilitysecuritysupportdate { get; set; }
        public string Lcmstatus { get; set; }
        public string Identifiedaction { get; set; }
        public string Descriptionofplannedaction { get; set; }
        public string Projectstatus { get; set; }
        public string Reasonfornoplan { get; set; }
        public string Commentonprojectstatus { get; set; }
        public string Projectenddate { get; set; }
        public string Ragstatus { get; set; }
        public string Trackingnumberprojectname { get; set; }
        public string Program { get; set; }
        public string Wbscode { get; set; }
        public string Bptid { get; set; }
        public string Ppmid { get; set; }
        public string Scopeofsimplification { get; set; }
        public string Datasource { get; set; }
        public string Projectowner { get; set; }
        public string Budgetestimated { get; set; }
        public string Notes { get; set; }
        public string Bundlebudget { get; set; }
        public string Bundleid { get; set; }
        public string Assetservicefunctionality { get; set; }
        public string Platform { get; set; }
        public string Engriskevaluation { get; set; }
        public string Engriskevaluationnotes { get; set; }
        public string Opsriskevaluation { get; set; }
        public string Opsriskevaluationnotes { get; set; }
        public string Incidentclass { get; set; }
        public string Occurrenceprobability { get; set; }
        public string Newopsriskevaluation { get; set; }
        public string Overallriskevaluation { get; set; }
        public string Riskcluster { get; set; }
        public string Securityriskpotential { get; set; }
        public string Vulnerabilityscore { get; set; }
        public string Comments { get; set; }
        public string Qid { get; set; }
        public string Requestid { get; set; }
        public string Vulnerabilityrating { get; set; }
        public string Securityriskeffective { get; set; }
        public string Securitymitigation { get; set; }
        public string Securityriskoverall { get; set; }
        public string Includedinsecurityscanning { get; set; }
        public string Raid { get; set; }
        public string Lcmcumulativeriskid { get; set; }
        public string Lcmcumulativerisklevel { get; set; }
        public string Cyberriskrequestid { get; set; }
        public string Criticality { get; set; }
        public string Assetstatus { get; set; }
        public string Gdprrelevant { get; set; }
        public string Lastscandate { get; set; }
        public string Lastupgradedate { get; set; }
        public string Assetoutofscopeforreportingpurposes { get; set; }
        public string Mainorganization { get; set; }
        public string Isextendedsupportofferedbyvendor { get; set; }
        public string Eomcontrol { get; set; }
        public string Engupdatetracker { get; set; }
        public string Opsupdatetracker { get; set; }
        public string Typeofnetworkelement { get; set; }
        public string Engkpi2 { get; set; }
        public string Ipaddress { get; set; }
        public string Serialnumber { get; set; }
        public string Hostname { get; set; }
        public string Exnetworks { get; set; }
        public string Originallcmid { get; set; }
        public string Hwoperationscontactpoint { get; set; }
        public string Hwvendor { get; set; }
        public string Hwoperationsmaintenancecontract { get; set; }
        public string Hwvendorendofmaintenancedate { get; set; }
        public string Plannedhwmodel { get; set; }
        public string Hwopsmaintenancecontractenddate { get; set; }
        public string Lcmstatusenghardware { get; set; }
        public string Lcmstatusopshardware { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Hwresourcekey { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Verticalresponsibles NontemsverticalNavigation { get; set; }
    }
}
