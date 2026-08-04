using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.GenericReportDto
{
    public class GenericQueryDto : QueryObject
    {
        #region LcmQuery
        public List<long> Lcmengineeringid { get; set; }
        public List<long> Designcomponentid { get; set; }
        public List<short> Opcoid { get; set; }
        public List<string> Opco { get; set; }
        public DateFilter Softwareendofwarrantydate { get; set; }
        public List<int> Numberofnodes { get; set; }
        public List<short> Productimportanceid { get; set; }
        public List<bool> Warranty { get; set; }
        public List<int> Numberofnodesinlab { get; set; }
        public List<string> Hardwaresheetindex { get; set; }
        public List<string> Softwaresheetindex { get; set; }
        public List<bool> Onhardware { get; set; }
        public List<bool> Onsoftware { get; set; }
        public List<short> Fullorpartialsupportid { get; set; }
        public List<string> FullOrPartialSupportIdDesc { get; set; }
        public DateFilter Hardwareendofsupportcontract { get; set; }
        public List<short> Hardwaresupportedid { get; set; }
        public List<string> HardwareSupportedIdDesc { get; set; }
        public List<bool> Renewalinprogress { get; set; }
        public DateFilter Softwareendofsupportcontract { get; set; }
        public List<short> Softwaresupportedid { get; set; }
        public List<string> SoftwareSupportedIdDesc { get; set; }
        public List<bool> Sparesprovisioned { get; set; }
        public DateFilter Vendorendmntdatehw { get; set; }
        public DateFilter Vendorendmntedatesw { get; set; }
        public List<bool> Elementcount { get; set; }
        public List<string> Hardwaresupportprovider { get; set; }
        public List<string> Hardwaresupporttype { get; set; }
        public List<string> Lcmstatusenghardware { get; set; }
        public List<string> Lcmstatusengsoftware { get; set; }
        public List<string> Lcmstatushardware { get; set; }
        public List<string> Lcmstatusopshardware { get; set; }
        public List<string> Lcmstatusopssoftware { get; set; }
        public List<string> Lcmstatussoftware { get; set; }
        public List<string> Outputtolcmhardware { get; set; }
        public List<string> Outputtolcmsoftware { get; set; }
        public List<string> Softwaresupportprovider { get; set; }
        public List<string> Softwaresupporttype { get; set; }
        public List<short> Fullorpartialsupporthwid { get; set; }
        public List<string> FullOrPartialSupportHwIdDesc { get; set; }
        public List<string> Lcmspreadsheethwid { get; set; }
        public List<string> Lcmspreadsheetswid { get; set; }
        public List<bool> Archived { get; set; }
        public List<string> Lcmdeploymentstatus { get; set; }
        public List<string> OperationalContract { get; set; }
        public List<string> EngKpi2 { get; set; }
        public List<string> IpAddress { get; set; }
        public List<string> ExpLcmStatusAtEndOfFY24 { get; set; }
        public List<string> ManagedByGdc { get; set; }

        public List<string> Resourcekey { get; set; }
        public List<string> Previousresourcekey { get; set; }
        public List<string> Isextendedsupportofferedbyvendor { get; set; }
        public List<string> Hwisextendedsupportofferedbyvendor { get; set; }
        public List<string> OperationsContactPoint { get; set; }
        public DateFilter VendorEndOfVulnerabilitySecuritySupportDateValue { get; set; }
        public List<string> AssetServiceFunctionality { get; set; }
        public List<string> EngineeringContactPoint { get; set; }
        #endregion

        #region Assests
        public List<string> NetworkElementAsPlannedId { get; set; }
        public List<bool> AutomatedFeedback { get; set; }
        public List<bool> PlannedAction { get; set; }
        public List<string> Environment { get; set; }
        public List<short> DeploymentStatusId { get; set; }
        public List<string> DeploymentTypeIdDesc { get; set; }
        public List<short> DeploymentTypeId { get; set; }
        public List<string> LocationName { get; set; }
        public List<short> NfviBundleidId { get; set; }
        public List<short> OrgEqpManufacturerId { get; set; }
        public List<string> OrgEqpManufacturerIdDesc { get; set; }
        public List<string> CapacityPlanReference { get; set; }
        public List<string> ElementName { get; set; }
        public List<string> AdditionalInformation1 { get; set; }
        public List<string> NetworkConstruct { get; set; }
        public List<string> AdditionalInformation2 { get; set; }
        public List<string> HwResourceKey { get; set; }
        public List<string> PreviousHwResourceKey { get; set; }
        public List<string> SwResourceKey { get; set; }
        public List<string> PreviousSwResourceKey { get; set; }
        public List<string> MeStatus { get; set; }
        public List<string> MeVirtualFlg { get; set; }
        public List<string> MeDeploymentType { get; set; }
        public List<string> MeType { get; set; }
        public List<string> MeSerialNumber { get; set; }
        public List<string> MeExternalConnectionFlg { get; set; }
        public List<string> HardwareModules { get; set; }
        public List<string> HardwareManfacturer { get; set; }
        public List<string> LastCheckedTime { get; set; }
        #endregion

        #region PlannedActivities
        public List<long> Plannedactivityid { get; set; }
        public List<short> Plannedimplementationyear { get; set; }
        public List<short> Activitystatusid { get; set; }
        public List<string> ActivityStatusIdDesc { get; set; }
        public List<short> Planningactivitystatusid { get; set; }
        public List<string> PlanningActivityStatusIdDesc { get; set; }
        public List<string> Plannedactivity { get; set; }
        public List<string> Activitydetails { get; set; }
        public List<string> Deliveryprojectname { get; set; }
        public List<string> Localapproval { get; set; }
        public List<short> Deliverystatusid { get; set; }
        public List<string> DeliveryStatusIdDesc { get; set; }
        public List<short> Responsibilityphaseid { get; set; }
        public List<string> ResponsibilityPhaseIdDesc { get; set; }
        public DateFilter Plannedcompletion { get; set; }
        public List<string> PlannedSparefieldsjson { get; set; }
        public List<short> Relatestoid { get; set; }
        public List<decimal> Budgetvalue { get; set; }
        public List<string> Plannedactivityresource { get; set; } 
        public List<short> Budgetavailabilityid { get; set; }
        public List<string> BudgetAvailabilityIdDesc { get; set; }
        public List<short> Engineeringriskid { get; set; }
        public List<string> EngineeringRiskIdDesc { get; set; }
        public List<short> Operationalriskid { get; set; }
        public List<string> OperationalRiskIdDesc { get; set; }
        public List<long> Linkedtoplannedactivityid { get; set; }
        public List<long> Networkelementasplannedid { get; set; }
        public List<bool> Isnewservicearchitecture { get; set; }
        public List<bool> Isreplacementexistingsolution { get; set; }
        public List<short> Benefitid { get; set; }
        public List<string> BenefitIdDesc { get; set; }
        public List<short> Driverid { get; set; }
        public List<string> DriverIdDesc { get; set; }
        public List<short> Planningriskid { get; set; }
        public List<string> PlanningRiskIdDesc { get; set; }
        public List<string> Currency { get; set; }
        public List<string> Notes { get; set; }
        public List<string> Overallriskevaluation { get; set; }
        public List<string> Deliveryprojectid { get; set; }
        public List<string> Planningrisk { get; set; }
        public List<string> Projectstatus { get; set; }
        public List<string> Riskengineeringnotes { get; set; }
        public List<string> Riskoperationalnotes { get; set; }
        public List<string> Budgettrackingid { get; set; }
        public List<string> RagStatus { get; set; }

        //public List<long> Designaspectid { get; set; }
        public List<long> Designcomponentfamilyid { get; set; }
        public DateFilter Startdate { get; set; }
        public List<bool> Foraddasset { get; set; }
        public List<bool> Foreditasset { get; set; }
        public List<long> Originallcmengineeringid { get; set; }
        public List<bool> Deliveryplanavailable { get; set; }
        public List<string> Program { get; set; }
        public List<string> Projectowner { get; set; }
        public List<string> BudgetEstimated { get; set; }
        public List<string> BundleBudget { get; set; }
        public List<string> BundleId { get; set; }
        public List<string> PlannedSoftwareVersion { get; set; }

        public List<long> PlannedDc { get; set; }
        public List<long> PlannedDcIndex { get; set; }
        public List<string> IsPlannedActivity { get; set; }
        public List<string> Implemented { get; set; }
        public List<string> IsAnicallaryDataExists { get; set; }
        public List<short> DeliveryStatus { get; set; }
        public List<short> PlannedActivityStatus { get; set; }
        #endregion

        #region System
        public List<long> Systemtypeid { get; set; }
        public List<string> Systemtypenamevodafone { get; set; }
        public List<string> Systemtypename3gpp { get; set; }
        public List<string> Systemtypenameoem { get; set; }
        public List<long> Majorsoftwarebuildsid { get; set; }
        public DateFilter Constraintscaling { get; set; }
        public DateFilter Endofmaintenance { get; set; }
        public List<string> Verticalresponsible { get; set; }
        public List<int> VerticalNameId { get; set; }
        public List<string> Subdomainresponsible { get; set; }
         public List<string> AssetVirtualized { get; set; } 
       public List<string> BuildConstruction { get; set; }

        public List<string> SystemTypeDesignContact { get; set; }
        public List<int> Assetcategoryid { get; set; }
        public List<string> AssetCategoryIdDesc { get; set; }
        public List<string> SystemSparefieldsjson { get; set; }
        public List<int> Assetclassid { get; set; }
        public List<string> AssetClassIdDesc { get; set; }
        public List<int> Assettypeid { get; set; }
        public List<string> AssetTypeIdDesc { get; set; }
        public List<string> AssetClass { get; set; }
        public List<string> Constraintlcm { get; set; }
        public List<string> Vodafonename { get; set; }
        public List<string> SoftWareDesignContactEmail { get; set; }
        public List<string> SoftWareVertical { get; set; }
        public List<string> SoftWareSubdomain { get; set; }
        public List<string> HardWareDesignContactEmail { get; set; }
        public List<string> HardWareVertical { get; set; }
        public List<string> HardWareSubdomain { get; set; }
        #endregion

        #region AssetCategory
        public List<string> Assetcategory { get; set; }
        public List<bool> Takefromassettypetable { get; set; }
        #endregion

        #region SubnetWork
        public List<long> Id { get; set; }
        public List<string> SubDescription { get; set; }
        public List<bool> Default { get; set; }
        public List<string> Alias { get; set; }
        public List<int> Order { get; set; }
        public List<string> Swapplicationname { get; set; }
        public List<bool> Gdprrelevant { get; set; }
        public List<bool> Internetfacing { get; set; }
        public List<string> Lcmpolicy { get; set; }
        public List<string> Criticality { get; set; }
        public List<bool> Securityelement { get; set; }
        public List<int> Gdprclassification { get; set; }
        public List<bool> PciSox { get; set; }
        public List<bool> C3C4 { get; set; }
        public List<bool> Missioncritical { get; set; }
        public List<decimal> Productnameid { get; set; }
        public List<string> ProductNameIdDesc { get; set; }
        public List<int> Vodafonenameid { get; set; }
        public List<string> VodafoneNameIdDesc { get; set; }
        #endregion

        #region ProductImpor
        public List<string> Productimportance { get; set; }
        #endregion

        #region Majorsoftware
        public List<string> Orgeqpmanufacturer { get; set; }
        public List<string> Softwareversion { get; set; }
        public DateFilter Lasttimebuynew { get; set; }
        public DateFilter Lasttimebuyupgrades { get; set; }
        public DateFilter Lasttimebuyexpansions { get; set; }
        public DateFilter MajEndofmaintenance { get; set; }
        public DateFilter Endofsupport { get; set; }
        public DateFilter Generaavailabledate { get; set; }
        public List<string> Deliverymethod { get; set; }
        public List<string> MajSparefieldsjson { get; set; }
        public List<short> Operatingsystemid { get; set; }
        public List<string> OperatingSystemIdDesc { get; set; }

        public List<string> Vulnerabilitystatus { get; set; }
        public List<short> Eomstatus { get; set; }
        public List<string> Criticalassettype { get; set; }
        public List<string> MajorDescription { get; set; }
        public List<string> ProductName { get; set; }
        public List<string> TCPBundleVersion { get; set; }

        public List<string> TCIBundleVersion { get; set; }
        public List<string> SoftwareDesignContact { get; set; }

        #endregion

        #region MajorHardWare
        public List<long> MajorHardwareId { get; set; }
        public List<string> HardwareSolution { get; set; }
        public List<string> OtherHardwareInfo { get; set; }
        public DateFilter HardwareLastTimeBuyNew { get; set; }
        public DateFilter HardwareLastTimeBuyUpgrades { get; set; }
        public DateFilter HardwareLastTimeBuyExpansions { get; set; }
        public DateFilter HardwareEndofmaintenance { get; set; }
        public DateFilter HardwareEndofsupport { get; set; }
        public List<bool> ProprietaryHardware { get; set; }
        public List<short> PlatformId { get; set; }
        public List<string> PlatformIdDesc { get; set; }
        public List<short> Buildconstructionid { get; set; }
        public List<string> BuildconstructionidDesc { get; set; }
        public List<string> HardwareType { get; set; }
        public List<string> OperatingSystem { get; set; }
        public List<string> TypeOfProcessor { get; set; }
        public List<string> HardwareVulnerabilityStatus { get; set; }
        public List<short> HardwareEomStatus { get; set; }
        public DateFilter GeneralAvailabledate { get; set; }
        public List<string> HardwareModel { get; set; }
     //   public List<string> AssetVirtualized { get; set; }
        public List<string> HardwareDescription { get; set; }
        public List<string> HardwareDesignContact { get; set; }
        #endregion

        #region Risk
        public List<short> Riskid { get; set; }
        public List<int> Severity { get; set; }
        public List<string> RiskDescription { get; set; }
        #endregion

        #region //LcmAncillaryData
        public List<string> Lcmancillarydataid { get; set; }
        public List<string> Productcode { get; set; }
        public List<bool> Handedovertooperation { get; set; }
        public List<string> Contractrenewalplan { get; set; }
        public List<string> Reasonfornoplan { get; set; }
        public List<string> Commentonprojectstatus { get; set; }
        public List<string> Scopeofsimplification { get; set; }
        public List<string> Datasource { get; set; }
        public List<string> Incidentclass { get; set; }
        public List<string> Occurenceprobability { get; set; }
        public List<string> Securityriskpotential { get; set; }
        public List<string> Securityriskeffective { get; set; }
        public List<string> Securitymitigation { get; set; }
        public List<string> Assetoutofscope { get; set; }
        public List<string> Includedinsecurityscanning { get; set; }
        public List<string> Raid { get; set; }
        public List<string> Requestid { get; set; }
        public DateFilter Lastscandate { get; set; }
        public DateFilter Lastupgradedate { get; set; }
        public List<string> Eomcontrol { get; set; }
        public List<string> Engupdatetracker { get; set; }
        public List<string> Opsupdatetracker { get; set; }
        public List<string> Kpistatusservice { get; set; }
        public List<string> Custom { get; set; }
        public List<string> Custom1 { get; set; }
        public List<string> Custom2 { get; set; }
        public List<string> Idnew { get; set; }
        public List<string> Exnetworks { get; set; }
        public List<string> Productimportancehistory2 { get; set; }
        public List<string> Cloudversion { get; set; }
        public List<string> Certifiedswrealesefornfvibundle { get; set; }
        public List<string> Lcmstatus { get; set; }
        public List<string> Originalhwlcmid { get; set; }
        public List<string> Originalswlcmid { get; set; }

        public List<string> NewopsRiskEvaluation { get; set; }

        public List<string> Securityriskoverall { get; set; }

        public List<string> Labswrelease { get; set; }
        #region // 719 Regulatory Fiels changes
        public List<bool> IsPecn { get; set; }
        public List<bool> IsPecs { get; set; }
        public List<bool> IsScf { get; set; }
        public List<bool> IsNof { get; set; }
        #endregion
        public List<string> ExposedEdgeFlag { get; set; }
        public List<bool> ExternalFacingFlag { get; set; }
        public List<string> InfrastructureLocation { get; set; }
        public List<string> Vulnerabilityrating { get; set; }
        public List<string> Cyberriskrequestid { get; set; }
        #endregion

        #region //DesignAspect
        public List<long> DesignAspectId { get; set; }
        public List<string> SubNetworkBoundary { get; set; }
        public List<string> DesignAspectDescription { get; set; }
        public List<string> SupportedServices { get; set; }
        public List<int> AuthenicationTypeId { get; set; }
        public List<string> AuthenicationTypeName { get; set; }
        public List<int> SecurityManagerId { get; set; }
        public List<string> SecurityManagerName { get; set; }
        public List<int> LicenseModelId { get; set; }
        public List<string> LicenseModelName { get; set; }
        public List<int> ThirdPartyAccessId { get; set; }
        public List<string> ThirdPartyAccessName { get; set; }
        public List<int> SiteResilienceId { get; set; }
        public List<string> SiteResilienceName { get; set; }
        public List<int> SWDeliveryLifeCycleId { get; set; }
        public List<string> SwDeliveryLifeCycleName { get; set; }
        public List<int> BusinessContinuityMethodId { get; set; }
        public List<string> BusinessContinuityMethodName { get; set; }
        public List<int> InstanceResilienceId { get; set; }
        public List<string> InstanceResilienceName { get; set; }
        public List<bool> CriticalNationalInfrastructureName { get; set; }
        public List<short> SecurityTireZoneId { get; set; }
        public List<string> SecurityTireZoneName { get; set; }
        public List<bool> DaArchived { get; set; }
        public List<string> NominalCapacityLimit { get; set; }
        public List<string> MaxAllowedLoading { get; set; }
        public List<string> DesignedCapacityLimit { get; set; }
        public List<int> CriticalityRating { get; set; }
        public List<bool> CountrySpecificCriticality { get; set; }
        public List<string> VerticalName { get; set; }
        public List<long> VerticalId { get; set; }
        public List<string> UsedNetworkFunctions { get; set; }
        #endregion

        #region //DesignComponent
        public List<long> SubNetworkBoundaryId { get; set; }
        public List<string> DesignComponentName { get; set; }
        #endregion

        #region //DesignComponentFamilyName
        public List<bool> SystemIsShared { get; set; }
        public List<short> SharingTypeId { get; set; }
        public List<short> MajorHardwareOemId { get; set; }
        public List<string> MajorHardwareOemIdDesc { get; set; }
        public List<short> MajorSoftwareOemId { get; set; }
        public List<string> MajorSoftwareOemIdDesc { get; set; }
        public List<string> Description { get; set; }
        public List<string> SystemTypeIdentityName { get; set; }
        public List<bool> Implementation { get; set; }
        public List<string> DesignComponentFamilyName { get; set; }
        #endregion

        #region //SystemVerificationproblems
        public List<long> SystemVerificationProblemId { get; set; }
        public List<string> ProblemId { get; set; }
        public DateFilter DateFound { get; set; }
        public List<long> ProblemCategoryId { get; set; }
        public List<string> ProblemDescription { get; set; }
        public List<string> MaintenanceReference { get; set; }
        public List<string> StatusUrl { get; set; }
        public List<string> Mitigation { get; set; }
        public List<string> SolutionDescription { get; set; }
        public List<string> PatchReference { get; set; }
        public List<string> ProductUpgradeReference { get; set; }
        public List<string> SuppleMental { get; set; }
        public List<string> VendorCsr { get; set; }
        public List<string> SubNetwork { get; set; }
        public List<string> TestReport { get; set; }
        public List<string> StandardNir { get; set; }
        public List<string> EricssonSecReport { get; set; }
        public List<string> SwAndStEntries { get; set; }
        public List<string> PenTestingReport { get; set; }
        #endregion

    }
}
