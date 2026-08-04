using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;
using CAM.Contracts.RepositoryContracts.CBOM;
using CAM.Contracts.RepositoryContracts.ClusterLevelPA;
using CAM.Contracts.RepositoryContracts.ComponentSoftware;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Contracts.RepositoryContracts.ForeignIndex;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Contracts.RepositoryContracts.Mail;
using CAM.Contracts.RepositoryContracts.OMC;
using CAM.Contracts.RepositoryContracts.RBAC;
using CAM.Contracts.RepositoryContracts.Settings;
using CAM.Contracts.RepositoryContracts.VBom;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace CAM.Contracts.RepositoryContracts.Base
{
    public interface IRepositoryWrapperNew
    {
        IProjectPlanAuditRepository ProjectPlanAuditRepository { get; }
        IDaPlannedActivityDcfRepository DaPlannedActivityDcfRepository { get; }
        IProjectPlanRepository ProjectPlanRepository { get; }
        IProgramRepository ProgramRepository { get; }
        ISystemNamesRepository SystemNamesRepository { get; }   
        ISiteRepository SiteRepository { get; }
        IAppSettingsConfigurationRepository AppConfigurationSettingsRepository { get; }
        IAppSettingsRepository AppConfigurationRepository { get; }
        ITsrLogRepository TsrLogRepository { get; }
        INonTemsNweAsPlannedPassThroughRepository NonTemsNweAsPlannedPassThroughRepository { get; }
        ITsrPassThroughRepository TsrPassThroughRepository { get; }
        IMajorHwBuidlsDesignContactsRepository MajorHwBuidlsDesignContactsRepository { get; }
        IMajorSwBuidlsDesignContactsRepository MajorSwBuidlsDesignContactsRepository { get; }
        IPracticeRepository PracticeRepository { get; }
        IMainOrganisationRepository MainOrganisationRepository { get; }
        IOrganisationRepository OrganisationRepository { get; }
        ISystemVerificationProblemRepository SystemVerificationProblemRepository { get; }
        IProblemCategoryRepository ProblemCategoryRepository { get; }
        ISeverityRepository SeverityRepository { get; }
        ISWConfigFunctionAreaRepository SWConfigFunctionAreaRepository { get; }
        //SW Config Redesign changes.
        ISWConfigSubFunctionRepository SWConfigSubFunctionRepository { get; }
        ISWConfigSubFunctionAreasRepository SWConfigSubFunctionAreasRepository { get; }
        IFeedBackLoopAuditRepository FeedBackLoopAuditRepository { get; }
        INodeParseHistoryRepository NodeParseHistoryRepository { get; }
        IRiskClusterRepository RiskClusterRepository { get; }
        IReconciliationRepository ReconciliationRepository { get; }
        IRiskClusterVodafoneNamesRepository RiskClusterVodafoneNamesRepository { get; }
        INetworkElementRepository NetworkElement { get; }
        IIdentityRepository Identity { get; }
        ISoftwareComponentRepository SoftwareComponent { get; }
        IComponentRepository Component { get; }
        ISoftwareConfigurationRepository SoftwareConfiguration { get; }
        IHardwareConfigurationRepository HardwareConfiguration { get; }
        IResourceKeyMasterRepository ResourceKeyMasterRepository { get; }
        IResourceTypeRepository ResourceTypeRepository { get; }
        IDesignComponentFamilyLifeCycleRepository DesignComponentFamilyLifeCycleRepository { get; }
        IGenericReportRepository GenericReportRepository { get; }
        IAuditHistoryRepository AuditHistory { get; }
        IActivityStatusRepository ActivityStatus { get; }
        IBudgetAvailabilityRepository BudgetAvailability { get; }
        IAssetCategoryRepository AssetCategory { get; }
        IAssetClassRepository AssetClass { get; }
        IAssetTypeRepository AssetType { get; }
        IBuildConstructionRepository BuildConstruction { get; }
        IRepositoryLcmDBExportUpdateHistory LcmDBExportUpdateHistory { get; }
        ISupportedServiceRepository SupportedServiceRepository { get; }
        ILocationDeploymentTypeRepository LocationDeploymentTypeRepository { get; }
        IPlannedActivityTypesRepository PlannedActivityTypesRepository { get; }
        ISubnetworkSupportedServiceRepository SubnetworkSupportedServiceRepository { get; }
        ISubnetworkCustomerWheelsRepository SubnetworkCustomerWheelsRepository { get; }

        ISubnetworkSystemFunctionsRepository SubnetworkSystemFunctionsRepository { get; }

        INetworkFunctionRepository NetworkFunctionRepository { get; }
        ICustomerWheelRepository CustomerWheelRepository { get; }
        ICriticalAssetTypeRepository CriticalAssetTypeRepository { get; }
        INFVIStatusRepository NFVIStatus { get; }
        IDeliveryStatusRepository DeliveryStatus { get; }
        IDesignComponentRepository DesignComponent { get; }

        IDesignComponentFamilyRepository DesignComponentFamily { get; }
        ILcmEngineeringRepository Lcmengineering { get; }
        IVNFDesignComponentRepository VNFDesignComponent { get; }
        IMajorHardwareBuildRepository MajorHardwareBuild { get; }
        IBundleUpgradeInitiativeRepository BundleUpgradeInitiative { get; }
        IVNFTransitionRepository VNFTransition { get; }
        IEquipmentStatusRepository EquipmentStatus { get; }

        IMajorSoftwareBuildRepository MajorSoftwareBuild { get; }
        IOpCoRepository OpCo { get; }
        IHardwareSolutionResourceRepository HardwareSolutionResource { get; }
        IOriginalEquipmentManufacturerRepository OriginalEquipmentManufacturer { get; }
        IPlannedActivityRepository PlannedActivity { get; }
        IPlanningActivityStatusRepository PlanningActivityStatus { get; }
        IProductImportanceRepository ProductImportance { get; }
        IResponsibilityPhaseRepository ResponsibilityPhase { get; }
        ISubDomainResponsibleRepository SubDomainResponsible { get; }

        ISystemTypeRepository SystemType { get; }
        ISystemTypesMajorHardwareBuildRepository SystemTypesMajorHardwareBuild { get; }
        IVerticalResponsibleRepository VerticalResponsible { get; }
        IVulnerabilityStatusRepository VulnerabilityStatus { get; }
        IOperatingSystemRepository OperatingSystem { get; }
        IPlannedActivityResourceRepository PlannedActivityResourceRepository { get; }
        IGridCustomColumnRepository GridCustomColumnRepository { get; }
        INFVITransitionRepository NFVITransitionRepository { get; }
        INFVIBundleIDRepository NFVIBundleID { get; }
        IEndOfSupportContractRepository EndOfSupportContract { get; }
        IPlatformRepository Platform { get; }
        IHardwareTypeRepository HardwareType { get; }
        ISystemFunctionRepository SystemFunction { get; }
        ISubDomainSpocRepository SubDomainSpoc { get; }
        ISystemTypesSubDomainSpocRepository SystemTypesSubDomainSpoc { get; }

        IAuthenicationTypeRepository AuthenicationTypeRepository { get; }
        IBusinessContinuityMethodRepository BusinessContinuityMethodRepository { get; }
        ISiteResilienceRepository SiteResilienceRepository { get; }
        IInstanceResilienceRepository InstanceResilienceRepository { get; }
        ISecurityManagerRepository SecurityManagerRepository { get; }
        IThirdPartyAccessTypeRepository ThirdPartyAccessTypeRepository { get; }
        ISWDeliveryLifeCycleRepository SWDeliveryLifeCycleRepository { get; }
        ILicenseModelRepository LicenseModelRepository { get; }

        IPlannedActivityResourceBenefitRepository PlannedActivityResourceBenefit { get; }

        ILCMOperationalContractsRepository LCMOperationalContracts { get; }

        IPlannedActivityResourceDriverRepository PlannedActivityResourceDriver { get; }
        IPlannedActivityResourcePlanningRiskRepository PlannedActivityResourcePlanningRisk { get; }

        IDesignComponentFamilySystemFunctionRepository DesignComponentFamilySystemFunction { get; }

        IDesignComponentFamilyCustomerWheelRepository DesignComponentFamilyCustomerWheel { get; }

        IMajorSoftwareBuildNetworkFunctionRepository MajorSoftwareBuildNetworkFunction { get; }


        ILcmEngineeringSubDomainSpocRepository LcmEngineeringSubDomainSpoc { get; }

        ILcmEngineeringEduSpocRepository LcmEngineeringEduSpoc { get; }
 
        //Round 9
        ILcmAncillaryDataRepository LcmAncillaryDataRepository { get; }

        IRiskRepository Risk { get; }
        IReasonCheckboxResourceRepository ReasonCheckboxResource { get; }
        ISupportedResourceRepository SupportedResource { get; }
        IFullOrPartialResourceRepository FullOrPartialResource { get; }
        #region VBOM
        IClusterNameRepository ClusterNameRepository { get; }
        IVmTypeNameRepository VmTypeNameRepository { get; }
        IVnfInfoRepository VnfInfoRepository { get; }
        IVnfNameRepository VnfNameRepository { get; }
        IVnfVmCapacityRepository VnfVmCapacityRepository { get; }
      
        IVnfClusterInfoRepository VnfClusterInfoRepository { get; }
        IIntraVmTypeRepository IntraVmTypeRepository { get; }
        IInterVmTypeRepository InterVmTypeRepository { get; }
        IVmWorkLoadTypeRepository VnfWorkLoadTypeRepository { get; }
        #endregion
        #region AssetHardwareConfig
        IDataCenterRepository DataCenterRepository { get; }
        IAssetClusterRepository AssetClusterRepository { get; }
        IAssetClusterTypeRepository AssetClusterTypeRepository { get; }
        IAssetHardwareAncillaryRepository AssetHardwareAncillaryRepository { get; }
        IAssetCapacityInfoRepository AssetCapacityInfoRepository { get; }

        #endregion
        #region CBOM
        ICnfCapacityRepository CnfCapacityRepository { get; }
        ICnfClusterInfoRepository CnfClusterInfoRepository { get; }
        ICnfClusterRepository CnfClusterRepository { get; }
        ICnfPodInfoRepository CnfPodInfoRepository { get; }
        ICnfNameRepository CnfNameRepository { get; }
        ICnfHardwareRepository CnfHardwareRepository { get; }
        IPodTypeInfoRepository PodTypeInfoRepository { get; }
        ICnfPriorityRepository CnfPriorityRepository { get; }
        IFunctionStandardNameRepository FunctionStandardNameRepository { get; }
        #endregion

        #region Exodus 
        IDaMigrationStatusRepository daMigrationStatusRepository { get; }
        IDaAssetMigrationRepository DaAssetMigrationRepository { get; }
        IVnfHardwareRepository VnfHardwareRepository { get; }
        IMajorHardwareBuildAsIsRepository MajorHardwareBuildAsIs { get; }
        #endregion

        #region TEMS FNT
        ITemsFntReportRepository TemsFntReportRepository { get; }
        #endregion

        #region ClusterLevel PA
        INetworkElementClusterAsPlannedRepository NetworkElementClusterAsPlannedRepository { get; }
        IClusterUpGradeStatusRepository ClusterUpGradeStatusRepository { get; }
        IInfraClusterAsPlannedRepository InfraClusterAsPlannedRepository { get; }
        #endregion

        #region RBAC
        IAspNetModulesRepository AspNetModulesRepository { get; }
        IAspNetUserRolePermissionsRepository AspNetUserRolePermissionsRepository { get; }

        #endregion

        #region // Team Management
        ITeamRepository TeamRepository { get; }
        ITeamMemberRepository TeamMemberRepository { get; }

        #endregion
        ISettingUpdatePlannedActivityLcmDeploymentStatusRepository SettingUpdatePlannedActivityLcmDeploymentStatusRepository { get; }

        ISettingUpdatePlannedActivityAssetDeploymentStatusRepository SettingUpdatePlannedActivityAssetDeploymentStatusRepository { get; }

        IReasonCheckboxResourceLcmEngineeringHardwareRepository CheckboxResourceLcmEngineeringHardware { get; }
        IReasonCheckboxResourceLcmEngineeringSoftwareRepository CheckboxResourceLcmEngineeringSoftware { get; }

        ISettingsUpdatePlannedActivityRepository SettingsUpdatePlannedActivity { get; }
        ICrossSettingsUpdatePlannedActivityRepository CrossSettingsUpdatePlannedActivity { get; }
        INetworkElementAsPlannedRepository NetworkElementAsPlanned { get; }
        INetworkElementAsPlannedEduSpocRepository NetworkElementAsPlannedEduSpoc { get; }
        INetworkElementAsPlannedSubDomainSpocRepository NetworkElementAsPlannedSubDomainSpoc { get; }

        IEnvironmentRepository Environment { get; }
        IDeploymentStatusRepository DeploymentStatus { get; }
        IDeploymentTypeRepository DeploymentType { get; }

        ILocationRepository Location { get; }
        INetworkConstructRepository NetworkConstruct { get; }

        IActivityDetailsRepository ActivityDetails { get; }
        IBenefitRepository Benefit { get; }

        IOperationalContractsRepository OperationalContract { get; }

        IDriverRepository Driver { get; }

        IProductNameRepository ProductNameRepository { get; }
        IVodafoneNameRepository VodafoneNameRepository { get; }
        IUsersLoggingLevelRepository UsersLoggingLevelRepository { get; }
        IPlanningRiskRepository PlanningRisk { get; }

        INetworkElementAsIsRepository NetworkElementAsIs { get; }
        IVolteKPIRepository VolteKPI { get; }
        IVolteKPIWorklogRepository VolteKPIWorklog { get; }
        IMailQueueRepository MailQueue { get; }
        ISubNetworkBoundaryRepository SubNetworkBoundaries { get; }

        ISecurityTireZoneRepository SecurityTireZone { get; }
        ISharingTypeRepository SharingType { get; }

        #region Foreign Index
        IFI_SessionsRepository FI_Sessions { get; }
        IFI_DesignComponentsRepository FI_DesignComponents { get; }
        IFI_SystemTypesRepository FI_SystemTypes { get; }
        #endregion
        IUserRepository UserRepository { get; }

        IDesignAspectRepository DesignAspectRepository { get; }

        IDesignAspectNetworkFunctionRepository DesignAspectNetworkFunctionRepository { get; }
        IDesignAspectSupportedServiceRepository DesignAspectSupportedServiceRepository { get; }
        ILcmDeploymentStatusRepository LcmDeploymentStatusRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }


        IClaimsRepository ClaimsRepository { get; }


        IRoleRepository RoleRepository { get; }

        ITokenRepository TokenRepository { get; }

        IDeliveryTrackingRepository DeliveryTrackingRepository { get; }
        IGlossaryItemsRepository GlossaryItemsRepository { get; }
        IUserDefinedReportsLogsRepository UserDefinedReportsLogsRepository { get; }
        #region SystemofSystem

        IComponentSoftwareBuildRepository ComponentSoftwareBuildRepository { get; }
        IComponentSoftwareBuildsDesignContactRepository ComponentSoftwareBuildsDesignContactRepository { get; }
         
        IComponentSoftwareBuildBagRepository ComponentSoftwareBuildBagRepository { get; }
        IBuildBagRepository BuildBagRepository { get; }
        IComponentManufacturersRepository ComponentManufacturersRepository { get; }

        #endregion
        IExcelTemplateConfigurationRepository ExcelTemplateConfigurationRepository { get; }

        #region // BGT
        IBudgetProjectTrackersRepository BudgetProjectTrackersRepository { get; }
        IPlannedActivityCategoryRepository PlannedActivityCategoryRepository { get; }
        #endregion
        IPassThroughRepository PassThroughRepository { get; }
        ISwPassThroughRepositoryLcm SwPassThroughLcmRepository { get; }
        IHwPassThroughRepositoryLcm HwPassThroughLcmRepository { get; }

        INonTemsPassThroughConfigRepository NonTemsPassThroughConfigRepository { get; }

        #region OMC
        IAssetMapInfoRepository AssetMapInfoRepository { get; }
        IAssetAsIsSdiInfoRepository AssetAsIsSdiInfoRepository { get; }
        IAssetAsIsHwAncillaryDataRepository AssetAsIsHwAncillaryDataRepository { get; }
        IAssetAsIsSdiSwitchInfoRepository AssetAsIsSdiSwitchInfoRepository { get; }
        #endregion
        IReportSchedulerRepository ReportSchedulerRepository { get; }   

        IServiceMasterRepository ServiceMasterRepository { get; }
        IServicePlanRepository ServicePlanRepository { get; }
        IServicePlanDcfMappingRepository ServicePlanDcfMappingRepository { get; }

        IAspNetUserPreferenceRepository aspNetUserPreferenceRepository { get; }
        IAspNetUserOpcosRepository AspNetUserOpcosRepository { get; }
        IAspNetUserVerticalsRepository AspNetUserVerticalsRepository { get; }
        IExodusMilestoneAndActivityRepository ExodusMilestoneAndActivityRepository { get; }

        void Save();
        Task SaveAsync();
        IDbContextTransaction BeginTransaction();
        Task ClearTracker();

        Task<IDbContextTransaction> BeginTransactionAsync();
        ISoftwareBuildCompatibilityRepository SoftwareBuildCompatibility { get; }
        Task<dynamic> GetLinkedReferenceDetails(string entityName, long PKeyId);

    }
}