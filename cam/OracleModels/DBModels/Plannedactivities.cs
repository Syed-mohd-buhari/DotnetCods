using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Plannedactivities
    {
        public Plannedactivities()
        {
            Budgetprojecttrackers = new HashSet<Budgetprojecttrackers>();
            Clusterupgradestatus = new HashSet<Clusterupgradestatus>();
            Daassetmigration = new HashSet<Daassetmigration>();
            Damigrationstatus = new HashSet<Damigrationstatus>();
            Daplannedactivitydcf = new HashSet<Daplannedactivitydcf>();
            Deliverytrackings = new HashSet<Deliverytrackings>();
            Projectsplan = new HashSet<Projectsplan>();
        }

        public long Plannedactivityid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Plannedimplementationyear { get; set; }
        public short Activitystatusid { get; set; }
        public short Planningactivitystatusid { get; set; }
        public string Plannedactivity { get; set; }
        public string Activitydetails { get; set; }
        public string Deliveryprojectname { get; set; }
        public string Localapproval { get; set; }
        public short? Deliverystatusid { get; set; }
        public short? Responsibilityphaseid { get; set; }
        public DateTime? Plannedcompletion { get; set; }
        public string Sparefieldsjson { get; set; }
        public long? Designcomponentid { get; set; }
        public long? Lcmengineeringid { get; set; }
        public short? Relatestoid { get; set; }
        public decimal? Budgetvalue { get; set; }
        public short? Plannedactivityresourceid { get; set; }
        public short? Budgetavailabilityid { get; set; }
        public short? Engineeringriskid { get; set; }
        public short? Operationalriskid { get; set; }
        public short? Opcoid { get; set; }
        public long? Linkedtoplannedactivityid { get; set; }
        public long? Networkelementasplannedid { get; set; }
        public bool Isnewservicearchitecture { get; set; }
        public bool Isreplacementexistingsolution { get; set; }
        public short? Benefitid { get; set; }
        public short? Driverid { get; set; }
        public short? Planningriskid { get; set; }
        public string Currency { get; set; }
        public string Notes { get; set; }
        public string Overallriskevaluation { get; set; }
        public string Deliveryprojectid { get; set; }
        public string Planningrisk { get; set; }
        public string Projectstatus { get; set; }
        public string Riskengineeringnotes { get; set; }
        public string Riskoperationalnotes { get; set; }
        public string Budgettrackingid { get; set; }
        public long? Designaspectid { get; set; }
        public long? Designcomponentfamilyid { get; set; }
        public DateTime? Startdate { get; set; }
        public bool? Foraddasset { get; set; }
        public bool? Foreditasset { get; set; }
        public bool? Archived { get; set; }
        public long? Originallcmengineeringid { get; set; }
        public bool Deliveryplanavailable { get; set; }
        public string Projectowner { get; set; }
        public bool? Ispareleasedetailunknown { get; set; }
        public long Buildbagid { get; set; }
        public string Plannedactivityteam { get; set; }
        public string Priority { get; set; }
        public short? Plannedactivitycategoryid { get; set; }
        public short? Lcmcategories { get; set; }
        public long? Programid { get; set; }
        public string Projectdescription { get; set; }
        public DateTime? Prebaselinedate { get; set; }
        public bool? Isserviceplan { get; set; }
        public int? Serviceplanid { get; set; }

        public virtual Activitystatuses Activitystatus { get; set; }
        public virtual Benefits Benefit { get; set; }
        public virtual Budgetavailability Budgetavailability { get; set; }
        public virtual Buildbags Buildbag { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Deliverystatuses Deliverystatus { get; set; }
        public virtual Designaspects Designaspect { get; set; }
        public virtual Designcomponents Designcomponent { get; set; }
        public virtual Designcomponentfamilies Designcomponentfamily { get; set; }
        public virtual Drivers Driver { get; set; }
        public virtual Risk Engineeringrisk { get; set; }
        public virtual Lcmengineering Lcmengineering { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Networkelementsasplanned Networkelementasplanned { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Risk Operationalrisk { get; set; }
        public virtual Lcmengineering Originallcmengineering { get; set; }
        public virtual Plannedactivitycategory Plannedactivitycategory { get; set; }
        public virtual Plannedactivityresources Plannedactivityresource { get; set; }
        public virtual Planningactivitystatuses Planningactivitystatus { get; set; }
        public virtual Planningrisks PlanningriskNavigation { get; set; }
        public virtual Program ProgramNavigation { get; set; }
        public virtual Responsibilityphases Responsibilityphase { get; set; }
        public virtual Serviceplan Serviceplan { get; set; }
        public virtual ICollection<Budgetprojecttrackers> Budgetprojecttrackers { get; set; }
        public virtual ICollection<Clusterupgradestatus> Clusterupgradestatus { get; set; }
        public virtual ICollection<Daassetmigration> Daassetmigration { get; set; }
        public virtual ICollection<Damigrationstatus> Damigrationstatus { get; set; }
        public virtual ICollection<Daplannedactivitydcf> Daplannedactivitydcf { get; set; }
        public virtual ICollection<Deliverytrackings> Deliverytrackings { get; set; }
        public virtual ICollection<Projectsplan> Projectsplan { get; set; }
    }
}
