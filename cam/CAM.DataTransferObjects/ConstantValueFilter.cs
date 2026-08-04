using CAM.DataTransferObjects.FunctionalityDto;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CAM.DataTransferObjects
{
    public static class ConstantValueFilter
    {
        //public static  string unKnownSoftwareVersion = "unknown";
        //public static string unKnownSoftwareVersionValue = "unknown";
        public static string subnetworkAllSupportedServie = "supportedallservices,allsupportedservices";
        public static string duplicateAssetMigrated = "Target DC/LCM already have below assets with same Name, these assets will not be migrated to avoid duplication";

        #region PlannedActivity
        public static string toBeDecommissioned = "<b>" + "TO BE DECOMMISSIONED - " + "</b>";
        #endregion



        #region  MajorHarware 
        public static string multipleApplicationsForHardwareSolution = "Multiple Applications";
        public static string variousHardwareType = "Various";
        public static string virtualizedPlatform = "virtualized";
        //public static  string unKnownHardwareType = "unknown";
        public static string unKnownValue = "Unknown";

        #endregion

        #region SystmeType
        public static string notAnnouncedEOM = "not announced";
        public static string notSpecifiedEOM = "not specified";
        #endregion

        #region DCF

        public static string proprietaryHw = "PROPRIETARY HW";
        public static string commercialOfTheShelfCotsHw = "COMMERCIAL OF THE SHELF (COTS) HW";
        #endregion

        #region LCM
        //public static string isLowerDeletedTrueValue = "yes";
        //public static string isLowerDeletedFalseValue = "no";

        //public static string yes = "YES";
        //public static string no = "NO";

        public static string CamalYes = "Yes";
        public static string CamalNo = "No";

        //public static string isCamelCaseDeletedTrueValue = "Yes";
        //public static string isCamelCaseDeletedFlaseValue = "No";

        public static bool isTrue = true;

        public static string completedActivityStatus = "COMPLETE";
        //public static string lcmDeployStatusInservice = "in-service";
        //public static string lcmDeployStatusIRemoved = "removed";
        public static string paPlanningActivityStatus = "noplannedactivity";
        public static string paActivityStatus = "noplannedactivity";
        #endregion
        #region Setting PlannedActivity 
        public static string paResourceLCM = " is used for planned activity of Lcm Engineering";
        public static string paResourceAddAsset = " is used for planned activity of add asset";
        public static string paResoureEditAsset = " is used for planned activity of edit asset";
        public static string paResoureDCF = " is used for planned activity of DCF";
        public static string paArchivePlannedActivityAndParentyEntry = "Archive Planned Activity And ParentEntry";
        public static string paArchivePlannedActivityOnly = "Archive Planned Activity Only";
        public static string paNoRule = "No Rule";

        public static string assetMigrated = "Asset Migrated";
        public static string assetRemovedThroughModernized = "Asset Removed Through Modernized";
        public static string assetDecommissioned = "Asset Decommissioned";
        public static string initialNetworkDeployment = "initialnetworkdeployment";
        public static string engineering = "Engineering";
        public static string production = "production";
        public static string lcmEngineering = "Lcm Engineering";
        public static string asset = "Asset";
        public static string Operations = "Operations";

        public static string refactorDcAsset = "Asset Removed Through DA Platform Migration Refactor"; //"Asset Refactor from DA";
        #endregion

        #region DCF Manager
        public static string startOfLifeCycle = "Start Of Life Cycle";
        public static string resourceKeySuffixZero = "_00";
        public static string lcmDeletedManually = "LCM Deleted Manually";
        public static string lcmCreated = "LCM Created";
        public static string specifiedDesignComponent = "Specified Design Component";
        public static string resourceKeyUpdated = "Resource Key Updated";
        public static string[] virtualisedHWTypeArray = { "BLUEPRINT NFVI", "OTHER NFVI", "BLUEPRINT NFCI", "OTHER NFCI", "VIRTUALISED" };
        public static string identitydeletedmanually = "Identity Deleted Manually";
        #endregion

        #region  Network
        public static string plannedActivity = "Planned Activity";
        public static string linkedPlannedActivity = "Linked Planned Activity";
        public static string networkElementAsIs = "Network Elements As Is";

        #endregion

        #region
        public static string YES = "YES";
        public static string YesVirtualized = "Yes, already Virtualized";
        public static string YesNewVirtualization = "Yes, new Virtualization";
        public static string NO = "NO";
        public static string False = "false";
        public static string InPlanning = "inplanning";
        public static string RolloutComplete = "rolloutcomplete";
        public static string No = "No";
        public static string Yes = "Yes";
        public static string InMobilisation = "inmobilisation";
        public static string Planned = "planned";
        public static string InDeliveryEngineering = "indeliveryengineering";
        public static string InDeliveryOperations = "indeliveryoperations";
        public static string Confirmed = "confirmed";
        public static string Rejected = "rejected";
        public static string Completed = "completed";
        public static string RFSAchieved = "rfsachieved";
        public static string FSIAchieved = "fsiachieved";
        public static string Removed = "removed";
        public static string InService = "in-service";
        public static string Solution = "Solution";
        public static string OEMSW = "OEM SW";
        public static string OEMHW = "OEM HW";
        public static string MAJORRELEASE = "MAJOR RELEASE";
        public static string VIRTUALIZESYSTEM = "VIRTUALIZE SYSTEM";
        public static string Unknown = "unknown";
        public static string All = "all";

        public static string PlannedActivity = "Planned Activity";
        public static string LinkedPlannedActivity = "Linked Planned Activity";
        public static string DesignAspect = "Design Aspect";


        public static string CamalTrue = "True";
        public static string CamalFalse = "False";
        public static string NetworkElementsAsIs = "networkelementsasis";
        public static string NetworkElementsAsPlannned = "networkelementsasplannned";
        public static string NetworkElement = "networkelement";
        public static string ApplicationOrPlatform = "application/platform";
        public static string Identities = "identities";
        public static string HardwareConfiguration = "hardwareconfiguration";
        public static string CreatePlannedActivity = "Create Planned Activity";
        public static string NoActionRequired = "No Action Required";
        public static string PerformReconciliation = "Perform Reconciliation";
        public static string EditPlannedActivity = "Update Planned Activity Status";
        public static string Softwarereleaseinformation = "softwarereleaseinformation";


        public static string SuccessfullyRejected = "Successfully Rejected";
        public static string FollowingIdsarenotrejected = "Following Ids are not rejected";
        public static string FollowingIdsarenotApproved = "Following Ids are not approved";
        public static string FollowingIdsarenotOverride = "Following Ids are not Override";
        public static string SoftwareProductDate = "softwareproductdate";
        public static string SoftwareProductionDate = "softwareproductiondate";
        public static string Spare1ossorenm = "spare1ossorenm";

        public static List<string> tsrOpcos = new List<string>() { "uk", "group", "zzz" };
        public static string InfrastructureLocation = "infrastructureLocation";
        public static string ExternalFacingFlag = "externalFacingFlag";
        public static string RegulatoryFields = "regulatoryFields";

        public static string PECN = "PECN";
        public static string PECS = "PECS";
        public static string SCF = "SCF";
        public static string NOF = "NOF";

        public static string[] excludeDeployementStatus = { "removed" };
        public static string[] includeDeployementStatus = { "in-service", "in commissioning", "decommissioning", "planned" };

        public static string PasswordHash = "AQAAAAEAACcQAAAAEByQ9on2IEJlaXfSviZLOIthoyRE9Xq7l8Qd1pB9vSKZaxJp6rZ65hmKzEzSHRY8UA==";
        public static string SecurityStamp = "867b421c-e23a-4c0d-9be8-b26da8f65feb";
        public static string KPIAdministrator = "KPI Administrator";
        public static string OpCo = "opco";
        public static string KPI1 = "Spring II - KPI1 Capacity Current/EoY Target [M provisioned subs]";
        public static string KPI2 = "Spring II - KPI2 Current Utilization Provisoned/Registered [M subs]";
        public static string KPI3 = "Spring II - KPI3 VoLTE Penetration Current/EoY Target [Erlangs]";
        public static string KPI43G = "SPRINT II - 3G ShutDown - Actual Monthly / Final Target [%]";
        public static string White = "white";
        public static string Green = "green";
        public static string Red = "red";
        public static string Orange = "orange";
        public static string Gray = "gray";
        public static string Amber = "amber";
        public static string Yellow = "yellow";
        public static string Apricot = "apricot";
        public static string Q4 = "Q4";
        public static string Q1 = "Q1";
        public static string Q2 = "Q2";
        public static string Q3 = "Q3";

        public static string AMBER = "AMBER";
        public static string RED = "RED";
        public static string GREEN = "GREEN";
        #endregion

        #region //Import
        public static string NetworkElementAsIsId = "NetworkElementAsIsId";
        public static string Opco = "OpCo";
        public static string BulkImportData = "Bulk Import Data";
        public static string EngineeringInput = "Engineering Input";
        public static string ElementDeploymentName = "ElementDeploymentName";
        public static string NodeType = "NodeType";
        public static string HardwareInstallDate = "HardwareInstallDate";
        public static string ManualOverride = "ManualOverride";
        public static string Platform = "Platform";
        public static string HardwareType = "HardwareType";
        public static string PatchDetails = "PatchDetails";
        public static string ElementManagerExportFileFormat = "ElementManagerExportFileFormat";
        public static string SoftwareReleaseInformation = "SoftwareReleaseInformation";
        public static string SoftwareProductNumber = "SoftwareProductNumber";
        public static string SoftwareProductionDateValue = "SoftwareProductionDateValue";
        public static string SoftwareInstallDateValue = "SoftwareInstallDateValue";
        public static string HardwareAcquisition = "HardwareAcquisition";
        public static string DataAcquisitionDateValue = "DataAcquisitionDateValue";
        public static string DataAcquisitionMethod = "DataAcquisitionMethod";
        public static string ElementManager = "ElementManager";
        public static string Location = "Location";
        public static string Value = "Value";
        public static string Manual = "Manual";
        #endregion

        #region Export
        #region Software
        public static string Software = "software";
        public static string Nse = "NSE";
        public static string Management = "Management";
        public static string IPAddress = "ipaddress";
        public static string Virtualized = "Virtualized";
        public static string OnHardware = "On Hardware";
        public static string OnSoftware = "On Software";
        public static string Na = "N/A";
        public static string Hardware = "hardware";
        public static string NotAnnounced = "Not Announced";
        public static string DateFormat = "dd/MM/yyyy";
        public static string BluprintNfvi = "blueprintnfvi";
        public static string BluprintNfci = "blueprintnfci";
        public static string OtherNfvi = "othernfvi";
        public static string OtherNfci = "othernfci";
        public static string Production = "production";
        public static string[] ProductionAndTest = { "production" , "test" };
        public static string Lab = "lab";
        public static string no = "No";
        public static string yes = "Yes";
        #endregion


        #endregion



        #region // VIA Report
        public static string osIsProvidedByHardwareSolution = "OS is provided by Hardware Solution";
        public static string PropritaryHW = "Propritary HW";
        public static string Operational = "Operational";
        public static string Unspecified = "Unspecified";
        public static string NA = "N/A";
        public static string NotSpecified = "Not Specified";
        public static string Modificationdate = "Modificationdate";
        public static string Platformsupportingmultipleapplications = "Platform supporting multiple applications";
        public static string Platformsupporting = "Platform supporting ";
        public static string Hardwaresolutionsupporting = "Hardware solution supporting, ";
        #endregion

        #region LcmAncillary

        public static string _engUpdateTracker = "To be started";
        public static string completed = "Completed";
        public static string InProgress = "InProgress";
        public static string historicalback_up = "Historical back-up (remediation action completed)";
        public static string assetPlannedTobeInserted = "Asset planned to be inserted in the network";
        public static string inScope = "In scope";
        #endregion



        #region // HWReport
        public static string Cloudversion = "Cloud version";
        public static string productImportance = "productImportance";
        public static string HardwareUpgrade = "hardwareupgrade";
        public static string High = "High";
        public static string Moderate = "Moderate";
        public static string Low = "Low";
        public static string Extreme = "Extreme";
        public static string Container = "Container";
        public static string OnTrack = "ontrack";
        public static string Delayed = "Delayed";
        public static string Virtual = "Virtual";
        #endregion

        #region GenericReporting
        public static string moduleType = "moduletype";
        public static string componentVersionNumber = "componentversionnumber";
        public static string lastCheckedTime = "lastcheckedtime";
        public static string meSerialNumber = "meserialnumber";

        public static string AssetMigrated = "assetmigrated";
        public static string assetRemovedManually = "assetremovedmanually";
        public static string assetRemoved = "assetremoved";
        public static string assetDecommissionedManually = "assetdecommissionedmanually";
        public static string StartOfLifeCycle = "startoflifecycle";
        #endregion
        public static int eosCalulatedDayCount = 3;
        #region LCM@Glance
        public static Dictionary<int, string> filteredPlannedActivityRule = new Dictionary<int, string>()
{
    { 1, "SwArchitectureUpgrade_SwMajorRelease"},
    { 9, "Replace_Solution_Change_Equipment_Manufacturer"},
    { 14, "Modernize_Solution_Successor_Network"}
};

        public static string majorReleaseLiteral = "MAJOR RELEASE:";

        #endregion

        public static string blankTextValue = "---";
        public static int blankZeroValue = 0;
        public static int NodeSelectionAskToUser = 1;
        public static int NodeSelectionTransferAllNodes = 0;

        public static string finacialYearPrefix = "FY";

        #region SystemOfSystem
        public static string oneTrack = "One Track";
        public static string deliveryMethodCiCd = "CI/CD";
        public static string oemEricsson = "ericsson";
        public static string oemTraditional = "Traditional";

        public static string dummyBagName = "Empty";
        public static string dummyVersion = "1.0";
        #endregion

        public static string DCF = "Designcomponentfamily";
        public static string SupportService = "Supportedservice";
        public static string Bagname = "Bagname";
        public static string Componentname = "Componentname";
        public static string Componentresourcekey = "Componentresourcekey";
        public static string Components = "Components";



        #region SOS - Component DCF LifeCyle Event Details

        public static List<KeyValuePairDto> componentDcfEventDeatil = new List<KeyValuePairDto>
        {
            new KeyValuePairDto(1,"Component Added"),
            new KeyValuePairDto(2,"Component Upgraded"),
            new KeyValuePairDto(3,"Component Removed"),
            new KeyValuePairDto(4,"Component Deassociated"),
        };


        #endregion

        public static List<string> assetOverViewExport = new List<string>()
        {
            "Verticaldescrption","Oemvendor","Productnameneinstances","Opcodescrption","Networkelementcount","HWBuildCons","SupportService","DCFDescription","Environment","SystemTypeDescription"
        };
        public static string CsCoreEnablers = "c&s_core enablers";
        public static string CsIms = "c&s_ims";

        public static List<string> VoicCoreValue = new List<string>()
        {
            "c&s_coreenablers","c&s_ims"
        };
        public static string ZOpco = "zzz";

        #region TSR Report
        public static string DataRefreshTypeofoperation = "Data Refresh";
        public static string ImportTypeofoperation = "Import";
        public static string TsrLogInProgressStatus = "In Progress";
        public static string TsrLogCompletedStatus = "Completed";
        public static string TsrLogFailedStatus = "Completed";
        public static string AssetNameIsEmpty = "Asset Name is empty";
        public static string NonTemsRecordsUnableToImport = "These many NON-TEMS records are unable to Import to the Database ";
        public static string TemsFnt = "TEMS-FNT";

        #endregion

        #region
        public static string mswVmware = "vmware";

        #endregion

        #region //LcmDeployment status

        public static List<string> LcmDeploymentStatus = new List<string>()
        {
            "in-service","in-decommissioning"
        };

        public static string inDecommission = "in-decommissioning";

        public static List<string> assetDeploymentStatus = new List<string>()
        {
            "in-service","in commissioning", "traffic free"
        };

        public static string inService = "in-service";
        public static string inCommisioning = "in commissioning";
        public static string trafficFree = "traffic free";


        #endregion

        #region //PlannedActivity status
        public static List<string> PlannedActivityStatus = new List<string>()
        {
            "softwareupgrade","replacesolution","migrateanddecommission"
        };
        #endregion

        #region // NFVI CompatibilityStatus

        public static string Complaint = " Compliant";
        public static string SWUpgradePlanOk = "SW Upgrade Required, Plan is OK";
        public static string SWUpgradeNoPlan = "SW Upgrade Required & No Plan";
        public static string NoMinVnfProvided = "No Min VNF Provided";
        public static string Decommissioning = "Decommissioning";
        public static string SWUpgradePlanNotOk = "SW Upgrade Required & Plan is not OK";
        public static string NoMinVnfProvidedForPlannedDc = "No Min VNF Provided for the planned Design component";
        #endregion

        #region //Assured
        public static string Assured = "Assured";
        public static string NotAssured = "Not Assured";
        #endregion

        #region BPT
        public static string bptTemplateFileName = "TEMS_BPT_Export_Draft_v1.0.xlsx";
        public static string bptTemplateLegacyFileName = "TEMS_BPT_Export_Draft_v1.0.xls";
        public static string bptTemplateClientFileName = "_capex_nse_template_op.xls";

        public static string bptExcelProcessName = "bpt";
        public static string bptImportUniqueId = "currenttrackingnumber";
        #endregion
        public static string PlannedActivityTeam = "CES_EDU";

        public static string[] assetPaArchivePaType = { "addnode", "decommissionnode" };
        public static string[] assetDeploymentStatusForArchivePa = { inService, "decommissioning", "removed" };

        #region // VBOM
        public static string vbomExcelProcessName = "vbom";
        public static string vnfName = "vnfname";
        public static string vmTypeName = "vmtypename";
        public static string intra = "affinityrules-'intravmtype'";
        public static string inter = "affinityrules-'intervmtype'";
        public static string workload = "vmworkloadtype";

        #endregion

        #region // CBOM
        public static string cbomExcelProcessName = "cbom";
        public static string cnfNmae = "cnfname";
        public static string cluster = "k8cluster";
        public static string nodePool = "nodepool";
        public static string podType = "podtype";
        public static string descriptionOfEachPodRole = "descriptionofeachpodrole";
        public static string siteName = "site";
        public static string functionStandardName = "functionstandardname";
        public static string verticalDomianOwner = "verticaldomianowner";
        public static string priority = "priority";
        public static string cnfcluster = "k8clustername";
        public static string cnfnodePool = "nodepoolname";



        #endregion


        public static string CnfRequestType = "Committed";

        #region Exodus
        public static Dictionary<int, string> daMigrationStatusCode = new Dictionary<int, string>()
{
    { 1, "Planned"},
    { 2, "InProgress"},
    { 3, "Completed"}
};

        public static string[] assetDeployementStatusForPlatformMigration = { "in-service", "in commissioning", "planned" };
        public static string notStrategic = "not strategic";
        public static string TypeAcceptance = "type acceptance";
        public static string AssetDeploymentStatusError = "LCM Status is Planned - check asset status";
        public static string StatusNotAssigned = "Status: Not Assigned";
        public static string LcmIn_Commissioning = "in-commissioning";
        public static string VEC_NFVI = "vec-nfvi";
        public static string VEC_NFCI = "vec-nfci";
        public static string VNF = "VNF";
        public static string CNF = "CNF";
        public static string Other = "Other";
        public static string IaaS_B = "IaaS-B";
        public static string VEC_B = "VEC-B";
        public static string RemoveStatusForDecommissionedNode = "Remove";
        #endregion

        #region Passthrough
        public static string passThroughExcelProcessName = "PassThrough";
        public static string passThroughImportUniqueName = "me_name";
        public static string passThroughImportUniqueKey = "ME_SOURCE_ASSET_ID";
        public static string passThroughImportResourceKey = "ResourceKey";
        public static string passThroughLcmSoftware = "PassThroughLcmSoftware";
        public static string passThroughLcmSWAndHWImportUniqueName = "ID";
        public static string passThroughLcmHardware = "PassThroughLcmHardware";

        public static string passThroughLcmOpco = "LocalMarket";
        public static string passThroughLcmAssetClass = "AssetClass";
        public static string passThroughLcmSwVendor = "SWVendor";
        public static string passThroughLcmSwVersion = "SWRelease";
        public static string passThroughLcmHwModel = "HWModel";
        public static string passThroughLcmAssetDescription = "AssetDescription";


        public static string Infrastructure = "Infrastructure";
        public static string CNIS = "CNIS";
        public static string CCDCNIS = "CCD-CNIS";

        #endregion

        #region TEMS FNT
        public static string TemsFntReport = "temsfnt";
        public static string HostName = "me_name";
        public static string SerialNumberOfHardwareAsset = "me_source_asset_id";

        public static List<string> editTemsFntReport = new List<string>
                                                    {
                                                        {"HW_END_OF_SALE"},
                                                        {"Application hosted on Software"},
                                                        {"UUID_SERIALNUMBER_OF_SOFTWARE"},
                                                        {"SW_END_OF_SALE"},
                                                        {"Physical Server Hostname"},
                                                        {"Physical Server IP Address"},
                                                        {"Physical Server Serial Number"},
                                                        {"Physical Server HW Model"},
                                                        {"Physical Server Vendor"},
                                                        {"Virtual Server hosted on"},
                                                        {"Virtual Server Manufacturer"},
                                                        {"Virtual Server Type of device"},
                                                        {"Virtual Machine Type"},
                                                        {"Virtual Server IP Address"},
                                                        {"Virtual Server Serial Number"},
                                                        {"Virtual Server type"},
                                                        {"OS_START_DATE"},
                                                        {"OS_INSTALLATION_DATE"},
                                                        {"OS_STATUS"},
                                                        {"Software Name"},
                                                        {"Version"},
                                                        {"Release"},
                                                        {"LANGUAGE"},
                                                        {"Physical Server OS name"},
                                                        {"Physical Server OS Version"},
                                                        {"Physical Server OS Start Date"},
                                                        {"Physical Server OS installation date"},
                                                        {"Physical Server OS status"},


                                                    };
        #endregion

        public static string Hostname = "Hostname";
        #region Build construction Rule
        public static List<KeyValuePairDto> buildConstructionRule = new List<KeyValuePairDto>
        {
            new KeyValuePairDto(0,"No Rule"),
            new KeyValuePairDto(1,"As Proprietary HW"),
            new KeyValuePairDto(2,"As COTS or Other"),
            new KeyValuePairDto(3,"As NFVI"),
            new KeyValuePairDto(4,"As NFCI"),
        };

        #endregion

        public static Dictionary<string, string> exodusPlatformStackName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"VMWare CaaS","IaaS-B"},
            {"VMWare NFVI","IaaS-B"},
        };

        #region // Abstraction Layer

        public static string UserPreferenceName = "UserPreferenceDto";        
        public static int WhatsGoingOnDate = 6;  // Prefrence date
        public static int MessagingDate = 3;

        public static int NewPortal_WhatsGoingOnDate = 1;  // Prefrence date
        public static int NewPortal_MessagingDate = 1;
        public static string RoleWisePreferenceName = "RoleWisePreferenceName";
        public static string ProdImportanceStrategic = "strategic";
        #endregion

        //ClusterLevel LCM PA
        public static string[] assetDeploymentStatusForClusterLevelPa = { InService, "planned" ,inCommisioning ,trafficFree,Removed};
        public static string   buildConst_proprietaryHW = "proprietary hw";
        #region // Service PA
        public static string ServicePlanned = "Planned";
        public static List<string> filtersToHide = new List<string>
                {
                    "activitydetails",
                    "originaldesigncomponent",
                    "planneddesigncomponent",
                    "currentbuildbagdescription",
                    "designcomponentfamilyindex",
                    "originaldesigncomponentindex",
                    "planneddesigncomponentindex",
                    "planneddesigncomponentname",
                    "foraddasset",
                    "foreditasset",
                    "program",
                    "projectowner",
                    "forlcmlink",
                    "fordesignaspectlink",
                    "buildbagid",
                    "verticalname",
                    "deliveryprojectname",
                };

        public static List<string> fieldsTODisplay = new List<string>
        {
            "servicemaster",
            "programname"
        };

        public static string deliveryprojectname = "deliveryprojectname";
        public static string Program = "programname";


        #endregion

        #region New - Service level Pa
        public static string OriginalDesignComponent = "OriginalDesignComponent";
        public static string OriginalDesignComponentIndex = "OriginalDesignComponentIndex";
        public static string PlannedDesignComponent = "PlannedDesignComponent";
        public static string PlannedDesignComponentIndex = "PlannedDesignComponentIndex";
        public static string DesignComponentFamilyName = "DesignComponentFamilyName";
        public static string DesignComponentFamilyIndex = "DesignComponentFamilyIndex";
        public static string PlannedDesignComponentName = "PlannedDesignComponentName";

        public static string PlannedActivityDesignComponentId = "PlannedActivityDesignComponentId";
        #endregion


        #region Exodus Graphical report
        //public static string[] InfraBuildConstructions = { "CEE-CNIS", "CCD-CNIS", "RH OCP Virt", "NRH OSO", "NRHO", "RH OCP" };
        public static string[] InfraBuildConstructions = { "CEE-CNIS", "CCD-CNIS", "RHOCPVIRT", "NRHOSO", "NRHO", "RHOCP" };
        public static string[] verticalStacks = { "Vertical Stack-NFVI", "Vertical Stack-NFCI", "VEC-NFCI" };
        #endregion
    }
}
