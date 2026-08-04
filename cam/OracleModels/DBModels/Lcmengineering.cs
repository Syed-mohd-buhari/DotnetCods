using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Lcmengineering
    {
        public Lcmengineering()
        {
            Lcmancillarydata = new HashSet<Lcmancillarydata>();
            Lcmengineeringeduspoc = new HashSet<Lcmengineeringeduspoc>();
            Lcmengineeringsubdomainspoc = new HashSet<Lcmengineeringsubdomainspoc>();
            Lcmoperationalcontracts = new HashSet<Lcmoperationalcontracts>();
            Networkelementsasplanned = new HashSet<Networkelementsasplanned>();
            PlannedactivitiesLcmengineering = new HashSet<Plannedactivities>();
            PlannedactivitiesOriginallcmengineering = new HashSet<Plannedactivities>();
            Reasoncheckboxresourcelcmengineeringhardware = new HashSet<Reasoncheckboxresourcelcmengineeringhardware>();
            Reasoncheckboxresourcelcmengineeringsoftware = new HashSet<Reasoncheckboxresourcelcmengineeringsoftware>();
        }

        public long Lcmengineeringid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long Designcomponentid { get; set; }
        public short? Opcoid { get; set; }
        public DateTime? Softwareendofwarrantydate { get; set; }
        public int Numberofnodes { get; set; }
        public short? Productimportanceid { get; set; }
        public bool Warranty { get; set; }
        public int Numberofnodesinlab { get; set; }
        public string Hardwaresheetindex { get; set; }
        public string Softwaresheetindex { get; set; }
        public bool Onhardware { get; set; }
        public bool Onsoftware { get; set; }
        public short? Fullorpartialsupportid { get; set; }
        public DateTime? Hardwareendofsupportcontract { get; set; }
        public short? Hardwaresupportedid { get; set; }
        public bool Renewalinprogress { get; set; }
        public DateTime? Softwareendofsupportcontract { get; set; }
        public short? Softwaresupportedid { get; set; }
        public bool Sparesprovisioned { get; set; }
        public DateTime? Vendorendmntdatehw { get; set; }
        public DateTime? Vendorendmntedatesw { get; set; }
        public bool Elementcount { get; set; }
        public string Hardwaresupportprovider { get; set; }
        public string Hardwaresupporttype { get; set; }
        public string Lcmstatusenghardware { get; set; }
        public string Lcmstatusengsoftware { get; set; }
        public string Lcmstatushardware { get; set; }
        public string Lcmstatusopshardware { get; set; }
        public string Lcmstatusopssoftware { get; set; }
        public string Lcmstatussoftware { get; set; }
        public string Outputtolcmhardware { get; set; }
        public string Outputtolcmsoftware { get; set; }
        public string Softwaresupportprovider { get; set; }
        public string Softwaresupporttype { get; set; }
        public short? Fullorpartialsupporthwid { get; set; }
        public string Lcmspreadsheethwid { get; set; }
        public string Lcmspreadsheetswid { get; set; }
        public bool? Archived { get; set; }
        public short? Lcmdeploymentstatusid { get; set; }
        public string Resourcekey { get; set; }
        public string Previousresourcekey { get; set; }
        public bool? Isextendedsupportofferedbyvendor { get; set; }
        public bool? Hwisextendedsupportofferedbyvendor { get; set; }
        public bool? Isreleasedetailunknown { get; set; }
        public long? Designcomponentfamilyid { get; set; }
        public long Buildbagid { get; set; }

        public virtual Buildbags Buildbag { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Designcomponents Designcomponent { get; set; }
        public virtual Designcomponentfamilies Designcomponentfamily { get; set; }
        public virtual Fullorpartialresource Fullorpartialsupport { get; set; }
        public virtual Fullorpartialresource Fullorpartialsupporthw { get; set; }
        public virtual Supportedresource Hardwaresupported { get; set; }
        public virtual Lcmdeploymentstatus Lcmdeploymentstatus { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Productimportances Productimportance { get; set; }
        public virtual Supportedresource Softwaresupported { get; set; }
        public virtual ICollection<Lcmancillarydata> Lcmancillarydata { get; set; }
        public virtual ICollection<Lcmengineeringeduspoc> Lcmengineeringeduspoc { get; set; }
        public virtual ICollection<Lcmengineeringsubdomainspoc> Lcmengineeringsubdomainspoc { get; set; }
        public virtual ICollection<Lcmoperationalcontracts> Lcmoperationalcontracts { get; set; }
        public virtual ICollection<Networkelementsasplanned> Networkelementsasplanned { get; set; }
        public virtual ICollection<Plannedactivities> PlannedactivitiesLcmengineering { get; set; }
        public virtual ICollection<Plannedactivities> PlannedactivitiesOriginallcmengineering { get; set; }
        public virtual ICollection<Reasoncheckboxresourcelcmengineeringhardware> Reasoncheckboxresourcelcmengineeringhardware { get; set; }
        public virtual ICollection<Reasoncheckboxresourcelcmengineeringsoftware> Reasoncheckboxresourcelcmengineeringsoftware { get; set; }
    }
}
