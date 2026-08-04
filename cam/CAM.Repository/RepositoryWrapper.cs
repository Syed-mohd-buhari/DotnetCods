using CAM.Contracts;
using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;
using CAM.Contracts.RepositoryContracts.Base;
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
using CAM.Infrastucture.QueryResult;
using CAM.Repository.AssetHardwareConfig;
using CAM.Repository.CBom;
using CAM.Repository.CluserLevelPA;
using CAM.Repository.ComponentSoftware;
using CAM.Repository.Cross;
using CAM.Repository.Entity;
using CAM.Repository.ForeignIndex;
using CAM.Repository.LookUp;
using CAM.Repository.Mail;
using CAM.Repository.OMC;
using CAM.Repository.RBAC;
using CAM.Repository.Settings;
using CAM.Repository.VBom;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CAM.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        //private readonly RepositoryContext _repoContext;
        private readonly ModelContext _modelContext;
        private IMajorSoftwareBuildRepository _MajorSoftwareBuild;
        private IBuildConstructionRepository _BuildConstruction;
        private IRepositoryLcmDBExportUpdateHistory _LcmDBExportUpdateHistory;

        private IBundleUpgradeInitiativeRepository _BundleUpgradeInitiative;
        private IEquipmentStatusRepository _EquipmentStatus;
        private INFVIStatusRepository _NFVIStatus;
        private IMajorHardwareBuildRepository _MajorHardwareBuild;
        private IVulnerabilityStatusRepository _VulnerabilityStatus;
        private IAssetCategoryRepository _AssetCategory;
        private ISubDomainResponsibleRepository _SubDomainResponsible;
        private IVerticalResponsibleRepository _VerticalResponsible;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private IActivityStatusRepository _ActivityStatus;
        private IVNFDesignComponentRepository _VNFDesignComponent;
        private ISystemTypesMajorHardwareBuildRepository _SystemTypesMajorHardwareBuild;
        private IAssetClassRepository _AssetClass;
        private IAssetTypeRepository _AssetType;
        private IDeliveryStatusRepository _DeliveryStatus;
        private IDesignComponentRepository _DesignComponent;
        private IDesignComponentFamilyRepository _DesignComponentFamily;
        private ILcmEngineeringRepository _Lcmengineering;
        private IVNFTransitionRepository _VNFTransition;
        private ISystemTypeRepository _SystemType;
        private IProductImportanceRepository _ProductImportance;
        private IGlossaryItemsRepository _GlossaryItems;

        private IOpCoRepository _OpCo;
        private IHardwareSolutionResourceRepository _HardwareSolutionResource;
        private IOperatingSystemRepository _OperatingSystem;
        private IPlannedActivityRepository _PlannedActivityRepository;
        private IPlanningActivityStatusRepository _PlanningActivityStatus;
        private IResponsibilityPhaseRepository _ResponsibilityPhase;
        private IPlannedActivityResourceRepository _PlannedActivityResourceRepository;
        private IGridCustomColumnRepository _GridCustomColumnRepository;
        private INFVITransitionRepository _NFVITransitionRepository;
        private INFVIBundleIDRepository _NFVIBundleID;
        private IEndOfSupportContractRepository _EndOfSupportContract;
        private IPlatformRepository _Platform;
        private IHardwareTypeRepository _HardwareType;
        private ISystemFunctionRepository _SystemFunction;
        private IDesignComponentFamilySystemFunctionRepository _DesignComponentFamilySystemFunction;

        private IUserDefinedReportsLogsRepository _UserDefinedReportsLogsRepository;

        public IGlossaryItemsRepository GlossaryItemsRepository
        {
            get
            {
                if (_GlossaryItems == null)
                {
                    _GlossaryItems = new GlossaryItemsRepository(_modelContext);
                }
                return _GlossaryItems;
            }
        }

        public IDesignComponentFamilySystemFunctionRepository DesignComponentFamilySystemFunction
        {
            get
            {
                if (_DesignComponentFamilySystemFunction == null)
                {
                    _DesignComponentFamilySystemFunction = new DesignComponentFamilySystemFunctionRepository(_modelContext);
                }
                return _DesignComponentFamilySystemFunction;
            }
        }

        private IDesignComponentFamilyCustomerWheelRepository _DesignComponentFamilyCustomerWheel;
        public IDesignComponentFamilyCustomerWheelRepository DesignComponentFamilyCustomerWheel
        {
            get
            {
                if (_DesignComponentFamilyCustomerWheel == null)
                {
                    _DesignComponentFamilyCustomerWheel = new DesignComponentFamilyCustomerWheelRepository(_modelContext);
                }
                return _DesignComponentFamilyCustomerWheel;
            }
        }

        private IProductNameRepository _ProductNameRepository;
        public IProductNameRepository ProductNameRepository
        {
            get
            {
                if (_ProductNameRepository == null)
                {
                    _ProductNameRepository = new ProductNameRepository(_modelContext);
                }
                return _ProductNameRepository;
            }
        }

        private IVodafoneNameRepository _VodafoneNameRepository;
        public IVodafoneNameRepository VodafoneNameRepository
        {
            get
            {
                if (_VodafoneNameRepository == null)
                {
                    _VodafoneNameRepository = new VodafoneNameRepository(_modelContext);
                }
                return _VodafoneNameRepository;
            }
        }

        private IUsersLoggingLevelRepository _UsersLoggingLevelRepository;
        public IUsersLoggingLevelRepository UsersLoggingLevelRepository
        {
            get
            {
                if (_UsersLoggingLevelRepository == null)
                {
                    _UsersLoggingLevelRepository = new UsersLoggingLevelRepository(_modelContext);
                }
                return _UsersLoggingLevelRepository;
            }
        }
        public IPlannedActivityTypesRepository _PlannedActivityTypesRepository;
        public IPlannedActivityTypesRepository PlannedActivityTypesRepository
        {
            get
            {
                if (_PlannedActivityTypesRepository == null)
                {
                    _PlannedActivityTypesRepository = new PlannedActivityTypesRepositary(_modelContext);
                }
                return _PlannedActivityTypesRepository;
            }
        }
        public INfviSoftwareCompatibilityRepository _nfviSoftwareCompatibilityRepository;
        public INfviSoftwareCompatibilityRepository NfviSoftwareCompatibilityRepository
        {
            get
            {
                if (_nfviSoftwareCompatibilityRepository == null)
                {
                    _nfviSoftwareCompatibilityRepository = new NfviSoftwareCompatibilityRepository(_modelContext);
                }
                return _nfviSoftwareCompatibilityRepository;
            }
        }
        private ISettingUpdatePlannedActivityLcmDeploymentStatusRepository _SettingUpdatePlannedActivityLcmDeploymentStatusRepository;
        public ISettingUpdatePlannedActivityLcmDeploymentStatusRepository SettingUpdatePlannedActivityLcmDeploymentStatusRepository
        {
            get
            {
                if (_SettingUpdatePlannedActivityLcmDeploymentStatusRepository == null)
                {
                    _SettingUpdatePlannedActivityLcmDeploymentStatusRepository = new SettingUpdatePlannedActivityLcmDeploymentStatusRepository(_modelContext);
                }
                return _SettingUpdatePlannedActivityLcmDeploymentStatusRepository;
            }
        }

        private ISoftwareBuildCompatibilityRepository _softwareBuildCompatibility;
        public ISoftwareBuildCompatibilityRepository SoftwareBuildCompatibility
        {
            get
            {
                if (_softwareBuildCompatibility == null)
                {
                    _softwareBuildCompatibility = new SoftwareBuildCompatibilityRepository(_modelContext);
                }
                return _softwareBuildCompatibility;
            }
        }


        private ISettingUpdatePlannedActivityAssetDeploymentStatusRepository _SettingUpdatePlannedActivityAssetDeploymentStatusRepository;
        public ISettingUpdatePlannedActivityAssetDeploymentStatusRepository SettingUpdatePlannedActivityAssetDeploymentStatusRepository
        {
            get
            {
                if (_SettingUpdatePlannedActivityAssetDeploymentStatusRepository == null)
                {
                    _SettingUpdatePlannedActivityAssetDeploymentStatusRepository = new SettingUpdatePlannedActivityAssetDeploymentStatusRepository(_modelContext);
                }
                return _SettingUpdatePlannedActivityAssetDeploymentStatusRepository;
            }
        }

        private ILcmDeploymentStatusRepository _LcmDeploymentStatusRepository;
        public ILcmDeploymentStatusRepository LcmDeploymentStatusRepository
        {
            get
            {
                if (_LcmDeploymentStatusRepository == null)
                {
                    _LcmDeploymentStatusRepository = new LcmDeploymentStatusRepository(_modelContext);
                }
                return _LcmDeploymentStatusRepository;
            }
        }


        private IDriverRepository _Driver;
        public IDriverRepository Driver
        {
            get
            {
                if (_Driver == null)
                {
                    _Driver = new DriverRepository(_modelContext);
                }
                return _Driver;
            }
        }


        private IMajorSoftwareBuildNetworkFunctionRepository _MajorSoftwareBuildNetworkFunction;

        public IMajorSoftwareBuildNetworkFunctionRepository MajorSoftwareBuildNetworkFunction
        {
            get
            {
                if (_MajorSoftwareBuildNetworkFunction == null)
                {
                    _MajorSoftwareBuildNetworkFunction = new MajorSoftwareBuildNetworkFunctionRepository(_modelContext);
                }
                return _MajorSoftwareBuildNetworkFunction;
            }
        }

        private ISystemTypesSubDomainSpocRepository _SystemTypesSubDomainSpoc;
        private IPlannedActivityResourceBenefitRepository _PlannedActivityResourceBenefit;
        private IPlannedActivityResourceDriverRepository _PlannedActivityResourceDriver;
        private IPlannedActivityResourcePlanningRiskRepository _PlannedActivityResourcePlanningRisk;
        private ILcmEngineeringEduSpocRepository _LcmEngineeringEduSpoc;
        private ILcmEngineeringSubDomainSpocRepository _LcmEngineeringSubDomainSpoc;

        private ISettingsUpdatePlannedActivityRepository _SettingsUpdatePlannedActivity;
        private ICrossSettingsUpdatePlannedActivityRepository _CrossSettingsUpdatePlannedActivity;

        private IVolteKPIRepository _VolteKPI;
        public IVolteKPIRepository VolteKPI
        {
            get
            {
                if (_VolteKPI == null)
                {
                    _VolteKPI = new VolteKPIRepository(_modelContext);
                }
                return _VolteKPI;
            }
        }

        private IVolteKPIWorklogRepository _VolteKPIWorklog;
        public IVolteKPIWorklogRepository VolteKPIWorklog
        {
            get
            {
                if (_VolteKPIWorklog == null)
                {
                    _VolteKPIWorklog = new VolteKPIWorklogRepository(_modelContext);
                }
                return _VolteKPIWorklog;
            }
        }


        private INetworkElementAsIsRepository _NetworkElementAsIs;
        public INetworkElementAsIsRepository NetworkElementAsIs
        {
            get
            {
                if (_NetworkElementAsIs == null)
                {
                    _NetworkElementAsIs = new NetworkElementAsIsRepository(_modelContext);
                }
                return _NetworkElementAsIs;
            }
        }


        private IPlanningRiskRepository _PlanningRisk;
        public IPlanningRiskRepository PlanningRisk
        {
            get
            {
                if (_PlanningRisk == null)
                {
                    _PlanningRisk = new PlanningRiskRepository(_modelContext);
                }
                return _PlanningRisk;
            }
        }
        private IBenefitRepository _Benefit;
        public IBenefitRepository Benefit
        {
            get
            {
                if (_Benefit == null)
                {
                    _Benefit = new BenefitRepository(_modelContext);
                }
                return _Benefit;
            }
        }

        private IActivityDetailsRepository _ActivityDetails;
        public IActivityDetailsRepository ActivityDetails
        {
            get
            {
                if (_ActivityDetails == null)
                {
                    _ActivityDetails = new ActivityDetailsRepository(_modelContext);
                }
                return _ActivityDetails;
            }
        }


        private IProductNameRepository _ProductName;
        public IProductNameRepository ProductName
        {
            get
            {
                if (_ProductName == null)
                {
                    _ProductName = new ProductNameRepository(_modelContext);
                }
                return _ProductName;
            }
        }

        private ILocationRepository _Location;
        public ILocationRepository Location
        {
            get
            {
                if (_Location == null)
                {
                    _Location = new LocationRepository(_modelContext);
                }
                return _Location;
            }
        }




        private INetworkConstructRepository _NetworkConstruct;
        public INetworkConstructRepository NetworkConstruct
        {
            get
            {
                if (_NetworkConstruct == null)
                {
                    _NetworkConstruct = new NetworkConstructRepository(_modelContext);
                }
                return _NetworkConstruct;
            }
        }

        private IEnvironmentRepository _Environment;
        public IEnvironmentRepository Environment
        {
            get
            {
                if (_Environment == null)
                {
                    _Environment = new EnvironmentRepository(_modelContext);
                }
                return _Environment;
            }
        }

        private IDeploymentStatusRepository _DeploymentStatus;
        public IDeploymentStatusRepository DeploymentStatus
        {
            get
            {
                if (_DeploymentStatus == null)
                {
                    _DeploymentStatus = new DeploymentStatusRepository(_modelContext);
                }
                return _DeploymentStatus;
            }
        }

        private IDeploymentTypeRepository _DeploymentType;
        public IDeploymentTypeRepository DeploymentType
        {
            get
            {
                if (_DeploymentType == null)
                {
                    _DeploymentType = new DeploymentTypeRepository(_modelContext);
                }
                return _DeploymentType;
            }
        }




        private IBudgetAvailabilityRepository _BudgetAvailability;
        public IBudgetAvailabilityRepository BudgetAvailability
        {
            get
            {
                if (_BudgetAvailability == null)
                {
                    _BudgetAvailability = new BudgetAvailabilityRepository(_modelContext);
                }
                return _BudgetAvailability;
            }
        }




        public ICrossSettingsUpdatePlannedActivityRepository CrossSettingsUpdatePlannedActivity
        {
            get
            {
                if (_CrossSettingsUpdatePlannedActivity == null)
                {
                    _CrossSettingsUpdatePlannedActivity = new CrossSettingsUpdatePlannedActivityRepository(_modelContext);
                }
                return _CrossSettingsUpdatePlannedActivity;
            }
        }


        private INetworkElementAsPlannedRepository _NetworkElementAsPlanned;
        private INetworkElementAsPlannedEduSpocRepository _NetworkElementAsPlannedEduSpoc;
        private INetworkElementAsPlannedSubDomainSpocRepository _NetworkElementAsPlannedSubDomainSpoc;

        public INetworkElementAsPlannedRepository NetworkElementAsPlanned
        {
            get
            {
                if (_NetworkElementAsPlanned == null)
                {
                    _NetworkElementAsPlanned = new NetworkElementAsPlannedRepository(_modelContext);
                }
                return _NetworkElementAsPlanned;
            }
        }

        public INetworkElementAsPlannedEduSpocRepository NetworkElementAsPlannedEduSpoc
        {
            get
            {
                if (_NetworkElementAsPlannedEduSpoc == null)
                {
                    _NetworkElementAsPlannedEduSpoc = new NetworkElementAsPlannedEduSpocRepository(_modelContext);
                }
                return _NetworkElementAsPlannedEduSpoc;
            }
        }
        public INetworkElementAsPlannedSubDomainSpocRepository NetworkElementAsPlannedSubDomainSpoc
        {
            get
            {
                if (_NetworkElementAsPlannedSubDomainSpoc == null)
                {
                    _NetworkElementAsPlannedSubDomainSpoc = new NetworkElementAsPlannedSubDomainSpocRepository(_modelContext);
                }
                return _NetworkElementAsPlannedSubDomainSpoc;
            }
        }


        public ISettingsUpdatePlannedActivityRepository SettingsUpdatePlannedActivity
        {
            get
            {
                if (_SettingsUpdatePlannedActivity == null)
                {
                    _SettingsUpdatePlannedActivity = new SettingsUpdatePlannedActivityRepository(_modelContext);
                }
                return _SettingsUpdatePlannedActivity;
            }
        }

        private IReasonCheckboxResourceLcmEngineeringHardwareRepository _CheckboxResourceLcmEngineeringHardware;
        public IReasonCheckboxResourceLcmEngineeringHardwareRepository CheckboxResourceLcmEngineeringHardware
        {
            get
            {
                if (_CheckboxResourceLcmEngineeringHardware == null)
                {
                    _CheckboxResourceLcmEngineeringHardware = new ReasonCheckboxResourceLcmEngineeringHardwareRepository(_modelContext);
                }
                return _CheckboxResourceLcmEngineeringHardware;
            }
        }
        private IReasonCheckboxResourceLcmEngineeringSoftwareRepository _CheckboxResourceLcmEngineeringSoftware;
        public IReasonCheckboxResourceLcmEngineeringSoftwareRepository CheckboxResourceLcmEngineeringSoftware
        {
            get
            {
                if (_CheckboxResourceLcmEngineeringSoftware == null)
                {
                    _CheckboxResourceLcmEngineeringSoftware = new ReasonCheckboxResourceLcmEngineeringSoftwareRepository(_modelContext);
                }
                return _CheckboxResourceLcmEngineeringSoftware;
            }
        }
        private IFullOrPartialResourceRepository _FullOrPartialResource;
        public IFullOrPartialResourceRepository FullOrPartialResource
        {
            get
            {
                if (_FullOrPartialResource == null)
                {
                    _FullOrPartialResource = new FullOrPartialResourceRepository(_modelContext);
                }
                return _FullOrPartialResource;
            }
        }

        private ISupportedResourceRepository _SupportedResource;
        public ISupportedResourceRepository SupportedResource
        {
            get
            {
                if (_SupportedResource == null)
                {
                    _SupportedResource = new SupportedResourceRepository(_modelContext);
                }
                return _SupportedResource;
            }
        }

        private IReasonCheckboxResourceRepository _ReasonCheckboxResource;
        public IReasonCheckboxResourceRepository ReasonCheckboxResource
        {
            get
            {
                if (_ReasonCheckboxResource == null)
                {
                    _ReasonCheckboxResource = new ReasonCheckboxResourceRepository(_modelContext);
                }
                return _ReasonCheckboxResource;
            }
        }

        private IRiskRepository _OperationalRisk;
        public IRiskRepository Risk
        {
            get
            {
                if (_OperationalRisk == null)
                {
                    _OperationalRisk = new OperationalRiskRepository(_modelContext);
                }
                return _OperationalRisk;
            }
        }
        public ILcmEngineeringEduSpocRepository LcmEngineeringEduSpoc
        {
            get
            {
                if (_LcmEngineeringEduSpoc == null)
                {
                    _LcmEngineeringEduSpoc = new LcmEngineeringEduSpocRepository(_modelContext);
                }
                return _LcmEngineeringEduSpoc;
            }
        }
        public ILcmEngineeringSubDomainSpocRepository LcmEngineeringSubDomainSpoc
        {
            get
            {
                if (_LcmEngineeringSubDomainSpoc == null)
                {
                    _LcmEngineeringSubDomainSpoc = new LcmEngineeringSubDomainSpocRepository(_modelContext);
                }
                return _LcmEngineeringSubDomainSpoc;
            }
        }
        private ISubDomainSpocRepository _SubDomainSpoc;

        public ISubDomainSpocRepository SubDomainSpoc
        {
            get
            {
                if (_SubDomainSpoc == null)
                {
                    _SubDomainSpoc = new SubDomainSpocRepository(_modelContext);
                }
                return _SubDomainSpoc;
            }
        }

        private ILCMOperationalContractsRepository _LCMOperationalContracts;

        public ILCMOperationalContractsRepository LCMOperationalContracts
        {
            get
            {
                if (_LCMOperationalContracts == null)
                {
                    _LCMOperationalContracts = new LCMOperationalContractsRepository(_modelContext);
                }
                return _LCMOperationalContracts;
            }
        }

        private IOperationalContractsRepository _OperationalContracts;

        public IOperationalContractsRepository OperationalContract
        {
            get
            {
                if (_OperationalContracts == null)
                {
                    _OperationalContracts = new OperationalContractsRepository(_modelContext);
                }
                return _OperationalContracts;
            }
        }

        public ISystemTypesSubDomainSpocRepository SystemTypesSubDomainSpoc
        {
            get
            {
                if (_SystemTypesSubDomainSpoc == null)
                {
                    _SystemTypesSubDomainSpoc = new SystemTypesSubDomainSpocRepository(_modelContext);
                }
                return _SystemTypesSubDomainSpoc;
            }
        }


        public IPlannedActivityResourceBenefitRepository PlannedActivityResourceBenefit
        {
            get
            {
                if (_PlannedActivityResourceBenefit == null)
                {
                    _PlannedActivityResourceBenefit = new PlannedActivityResourceBenefitRepository(_modelContext);
                }
                return _PlannedActivityResourceBenefit;
            }
        }



        public IPlannedActivityResourceDriverRepository PlannedActivityResourceDriver
        {
            get
            {
                if (_PlannedActivityResourceDriver == null)
                {
                    _PlannedActivityResourceDriver = new PlannedActivityResourceDriverRepository(_modelContext);
                }
                return _PlannedActivityResourceDriver;
            }
        }
        public IPlannedActivityResourcePlanningRiskRepository PlannedActivityResourcePlanningRisk
        {
            get
            {
                if (_PlannedActivityResourcePlanningRisk == null)
                {
                    _PlannedActivityResourcePlanningRisk = new PlannedActivityResourcePlanningRiskRepository(_modelContext);
                }
                return _PlannedActivityResourcePlanningRisk;
            }
        }



        public IGridCustomColumnRepository GridCustomColumnRepository
        {
            get
            {
                if (_GridCustomColumnRepository == null)
                {
                    _GridCustomColumnRepository = new GridCustomColumnRepository(_modelContext);
                }
                return _GridCustomColumnRepository;
            }
        }



        public INFVITransitionRepository NFVITransitionRepository
        {
            get
            {
                if (_NFVITransitionRepository == null)
                {
                    _NFVITransitionRepository = new NFVITransitionRepository(_modelContext);
                }
                return _NFVITransitionRepository;
            }
        }

        public IPlannedActivityRepository PlannedActivity
        {
            get
            {
                if (_PlannedActivityRepository == null)
                {
                    _PlannedActivityRepository = new PlannedActivityRepository(_modelContext);
                }
                return _PlannedActivityRepository;
            }
        }

        public IOpCoRepository OpCo
        {
            get
            {
                if (_OpCo == null)
                {
                    _OpCo = new OpCoRepository(_modelContext);
                }
                return _OpCo;
            }
        }
        public IHardwareSolutionResourceRepository HardwareSolutionResource
        {
            get
            {
                if (_HardwareSolutionResource == null)
                {
                    _HardwareSolutionResource = new HardwareSolutionResourceRepository(_modelContext);
                }
                return _HardwareSolutionResource;
            }
        }
        public IProductImportanceRepository ProductImportance
        {
            get
            {
                if (_ProductImportance == null)
                {
                    _ProductImportance = new ProductImportanceRepository(_modelContext);
                }
                return _ProductImportance;
            }
        }

        public IActivityStatusRepository ActivityStatus
        {
            get
            {
                if (_ActivityStatus == null)
                {
                    _ActivityStatus = new ActivityStatusRepository(_modelContext);
                }
                return _ActivityStatus;
            }
        }
        public ISystemTypesMajorHardwareBuildRepository SystemTypesMajorHardwareBuild
        {
            get
            {
                if (_SystemTypesMajorHardwareBuild == null)
                {
                    _SystemTypesMajorHardwareBuild = new SystemTypesMajorHardwareBuildRepository(_modelContext);
                }
                return _SystemTypesMajorHardwareBuild;
            }
        }
        public ISystemTypeRepository SystemType
        {
            get
            {
                if (_SystemType == null)
                {
                    _SystemType = new SystemTypeRepository(_modelContext);
                }
                return _SystemType;
            }
        }
        public IAssetClassRepository AssetClass
        {
            get
            {
                if (_AssetClass == null)
                {
                    _AssetClass = new AssetClassRepository(_modelContext);
                }
                return _AssetClass;
            }
        }
        public IDeliveryStatusRepository DeliveryStatus
        {
            get
            {
                if (_DeliveryStatus == null)
                {
                    _DeliveryStatus = new DeliveryStatusRepository(_modelContext);
                }
                return _DeliveryStatus;
            }
        }
        public ILcmEngineeringRepository Lcmengineering
        {
            get
            {
                if (_Lcmengineering == null)
                {
                    _Lcmengineering = new LcmengineeringRepository(_modelContext);
                }
                return _Lcmengineering;
            }
        }

        public IVNFTransitionRepository VNFTransition
        {
            get
            {
                if (_VNFTransition == null)
                {
                    _VNFTransition = new VNFTransitionRepository(_modelContext);
                }
                return _VNFTransition;
            }
        }

        public IDesignComponentRepository DesignComponent
        {
            get
            {
                if (_DesignComponent == null)
                {
                    _DesignComponent = new DesignComponentRepository(_modelContext);
                }
                return _DesignComponent;
            }
        }


        public IDesignComponentFamilyRepository DesignComponentFamily
        {
            get
            {
                if (_DesignComponentFamily == null)
                {
                    _DesignComponentFamily = new DesignComponentFamilyRepository(_modelContext);
                }
                return _DesignComponentFamily;
            }
        }
        public IAssetTypeRepository AssetType
        {
            get
            {
                if (_AssetType == null)
                {
                    _AssetType = new AssetTypeRepository(_modelContext);
                }
                return _AssetType;
            }
        }

        public IAssetCategoryRepository AssetCategory
        {
            get
            {
                if (_AssetCategory == null)
                {
                    _AssetCategory = new AssetCategoryRepository(_modelContext);
                }
                return _AssetCategory;
            }
        }

        public IMajorSoftwareBuildRepository MajorSoftwareBuild
        {
            get
            {
                if (_MajorSoftwareBuild == null)
                {
                    _MajorSoftwareBuild = new MajorSoftwareBuildRepository(_modelContext);
                }
                return _MajorSoftwareBuild;
            }
        }

        public IMajorHardwareBuildRepository MajorHardwareBuild
        {
            get
            {
                if (_MajorHardwareBuild == null)
                {
                    _MajorHardwareBuild = new MajorHardwareBuildRepository(_modelContext);
                }
                return _MajorHardwareBuild;
            }
        }

        #region Exdous
        private IMajorHardwareBuildAsIsRepository _MajorHardwareBuildAsIs;
        public IMajorHardwareBuildAsIsRepository MajorHardwareBuildAsIs
        {
            get
            {
                if (_MajorHardwareBuildAsIs == null)
                {
                    _MajorHardwareBuildAsIs = new MajorHardwareBuildAsIsRepository(_modelContext);
                }
                return _MajorHardwareBuildAsIs;
            }
        }


        #endregion
        public IBundleUpgradeInitiativeRepository BundleUpgradeInitiative
        {
            get
            {
                if (_BundleUpgradeInitiative == null)
                {
                    _BundleUpgradeInitiative = new BundleUpgradeInitiativeRepository(_modelContext);
                }
                return _BundleUpgradeInitiative;
            }
        }

        public IVerticalResponsibleRepository VerticalResponsible
        {
            get
            {
                if (_VerticalResponsible == null)
                {
                    _VerticalResponsible = new VerticalResponsibleRepository(_modelContext);
                }
                return _VerticalResponsible;
            }
        }

        private IOriginalEquipmentManufacturerRepository _OriginalEquipmentManufacture;



        public IOriginalEquipmentManufacturerRepository OriginalEquipmentManufacturer
        {
            get
            {
                if (_OriginalEquipmentManufacture == null)
                {
                    _OriginalEquipmentManufacture = new OriginalEquipmentManufacturerRepository(_modelContext);
                }
                return _OriginalEquipmentManufacture;
            }
        }
        public IVulnerabilityStatusRepository VulnerabilityStatus
        {
            get
            {
                if (_VulnerabilityStatus == null)
                {
                    _VulnerabilityStatus = new VulnerabilityStatusRepository(_modelContext);
                }
                return _VulnerabilityStatus;
            }
        }
        public ISubDomainResponsibleRepository SubDomainResponsible
        {
            get
            {
                if (_SubDomainResponsible == null)
                {
                    _SubDomainResponsible = new SubDomainResponsibleRepository(_modelContext);
                }
                return _SubDomainResponsible;
            }
        }
        public IOperatingSystemRepository OperatingSystem
        {
            get
            {
                if (_OperatingSystem == null)
                {
                    _OperatingSystem = new OperatingSystemRepository(_modelContext);
                }
                return _OperatingSystem;
            }
        }

        public IPlannedActivityResourceRepository PlannedActivityResourceRepository
        {
            get
            {
                if (_PlannedActivityResourceRepository == null)
                {
                    _PlannedActivityResourceRepository = new PlannedActivityResourceRepository(_modelContext);
                }

                return _PlannedActivityResourceRepository;
            }
        }
        public IPlanningActivityStatusRepository PlanningActivityStatus
        {
            get
            {
                if (_PlanningActivityStatus == null)
                {
                    _PlanningActivityStatus = new PlanningActivityStatusRepository(_modelContext);
                }
                return _PlanningActivityStatus;
            }
        }
        public IResponsibilityPhaseRepository ResponsibilityPhase
        {
            get
            {
                if (_ResponsibilityPhase == null)
                {
                    _ResponsibilityPhase = new ResponsibilityPhaseRepository(_modelContext);
                }
                return _ResponsibilityPhase;
            }
        }

        public IVNFDesignComponentRepository VNFDesignComponent
        {
            get
            {
                if (_VNFDesignComponent == null)
                {
                    _VNFDesignComponent = new VNFDesignComponentRepository(_modelContext);
                }
                return _VNFDesignComponent;
            }
        }

        public IEquipmentStatusRepository EquipmentStatus
        {
            get
            {
                if (_EquipmentStatus == null)
                {
                    _EquipmentStatus = new EquipmentStatusRepository(_modelContext);
                }
                return _EquipmentStatus;
            }
        }

        public IPlatformRepository Platform
        {
            get
            {
                if (_Platform == null)
                {
                    _Platform = new PlatformRepository(_modelContext);
                }
                return _Platform;
            }
        }
        public IHardwareTypeRepository HardwareType
        {
            get
            {
                if (_HardwareType == null)
                {
                    _HardwareType = new HardwareTypeRepository(_modelContext);
                }
                return _HardwareType;
            }
        }

        public ISystemFunctionRepository SystemFunction
        {
            get
            {
                if (_SystemFunction == null)
                {
                    _SystemFunction = new SystemFunctionRepository(_modelContext);
                }
                return _SystemFunction;
            }
        }
        public INFVIStatusRepository NFVIStatus
        {
            get
            {
                if (_NFVIStatus == null)
                {
                    _NFVIStatus = new NFVIStatusRepository(_modelContext);
                }
                return _NFVIStatus;
            }
        }

        public INFVIBundleIDRepository NFVIBundleID
        {
            get
            {
                if (_NFVIBundleID == null)
                {
                    _NFVIBundleID = new NFVIBundleIDRepository(_modelContext);
                }
                return _NFVIBundleID;
            }
        }

        public IBuildConstructionRepository BuildConstruction
        {
            get
            {
                if (_BuildConstruction == null)
                {
                    _BuildConstruction = new BuildConstructionRepository(_modelContext);
                }
                return _BuildConstruction;
            }
        }


        public IRepositoryLcmDBExportUpdateHistory LcmDBExportUpdateHistory
        {
            get
            {
                if (_LcmDBExportUpdateHistory == null)
                {
                    _LcmDBExportUpdateHistory = new RepositoryLcmDBExportUpdateHistory(_modelContext);
                }
                return _LcmDBExportUpdateHistory;
            }
        }

        public IEndOfSupportContractRepository EndOfSupportContract
        {
            get
            {
                if (_EndOfSupportContract == null)
                {
                    _EndOfSupportContract = new EndOfSupportContractRepository(_modelContext);
                }
                return _EndOfSupportContract;
            }
        }

        private IMailQueueRepository _MailQueue;
        public IMailQueueRepository MailQueue
        {
            get
            {
                if (_MailQueue == null)
                {
                    _MailQueue = new MailQueueRepository(_modelContext);
                }
                return _MailQueue;
            }
        }
        private ISubNetworkBoundaryRepository _SubNetworkBoundaries;
        public ISubNetworkBoundaryRepository SubNetworkBoundaries
        {
            get
            {
                if (_SubNetworkBoundaries == null)
                {
                    _SubNetworkBoundaries = new SubNetworkBoundaryRepository(_modelContext);
                }
                return _SubNetworkBoundaries;
            }
        }


        private ISecurityTireZoneRepository _SecurityTireZone;
        public ISecurityTireZoneRepository SecurityTireZone
        {
            get
            {
                if (_SecurityTireZone == null)
                {
                    _SecurityTireZone = new SecurityTireZoneRepository(_modelContext);
                }
                return _SecurityTireZone;
            }
        }

        private ISharingTypeRepository _SharingType;
        public ISharingTypeRepository SharingType
        {
            get
            {
                if (_SharingType == null)
                {
                    _SharingType = new SharingTypeRepository(_modelContext);
                }
                return _SharingType;
            }
        }


        #region Foreign Index
        private IFI_SessionsRepository _FI_SessionsRepository;
        public IFI_SessionsRepository FI_Sessions
        {
            get
            {
                if (_FI_SessionsRepository == null)
                {
                    _FI_SessionsRepository = new FI_SessionRepository(_modelContext);
                }
                return _FI_SessionsRepository;
            }
        }
        private IFI_DesignComponentsRepository _FI_DesignComponentsRepository;
        public IFI_DesignComponentsRepository FI_DesignComponents
        {
            get
            {
                if (_FI_DesignComponentsRepository == null)
                {
                    _FI_DesignComponentsRepository = new FI_DesignComponentRepository(_modelContext);
                }
                return _FI_DesignComponentsRepository;
            }
        }
        private IFI_SystemTypesRepository _FI_SystemTypesRepository;
        public IFI_SystemTypesRepository FI_SystemTypes
        {
            get
            {
                if (_FI_SystemTypesRepository == null)
                {
                    _FI_SystemTypesRepository = new FI_SystemTypesRepository(_modelContext);
                }
                return _FI_SystemTypesRepository;
            }
        }
        #endregion

        private IRoleRepository _RoleRepository;
        public IRoleRepository RoleRepository
        {
            get
            {
                if (_RoleRepository == null)
                {
                    _RoleRepository = new RoleRepository(_modelContext);
                }
                return _RoleRepository;
            }
        }

        private IUserRepository _UserRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (_UserRepository == null)
                {
                    _UserRepository = new UserRepository(_modelContext);
                }
                return _UserRepository;
            }
        }

        private IClaimsRepository _ClaimsRepository;
        public IClaimsRepository ClaimsRepository
        {
            get
            {
                if (_ClaimsRepository == null)
                {
                    _ClaimsRepository = new ClaimsRepository(_modelContext);
                }
                return _ClaimsRepository;
            }
        }

        private IUserRoleRepository _UserRoleRepository;
        public IUserRoleRepository UserRoleRepository
        {
            get
            {
                if (_UserRoleRepository == null)
                {
                    _UserRoleRepository = new UserRoleRepository(_modelContext);
                }
                return _UserRoleRepository;
            }
        }


        private ITokenRepository _TokenRepository;
        public ITokenRepository TokenRepository
        {
            get
            {
                if (_TokenRepository == null)
                {
                    _TokenRepository = new TokenRepository(_modelContext);
                }
                return _TokenRepository;
            }
        }

        private ISupportedServiceRepository _SupportedServiceRepository;
        public ISupportedServiceRepository SupportedServiceRepository
        {
            get
            {
                if (_SupportedServiceRepository == null)
                {
                    _SupportedServiceRepository = new SupportedServiceRepository(_modelContext);
                }
                return _SupportedServiceRepository;
            }
        }

        private ISubnetworkSupportedServiceRepository _SubnetworkSupportedServiceRepository;
        public ISubnetworkSupportedServiceRepository SubnetworkSupportedServiceRepository
        {
            get
            {
                if (_SubnetworkSupportedServiceRepository == null)
                {
                    _SubnetworkSupportedServiceRepository = new SubnetworkSupportedServiceRepository(_modelContext);
                }
                return _SubnetworkSupportedServiceRepository;
            }
        }

        private ISubnetworkCustomerWheelsRepository _SubnetworkCustomerWheelsRepository;
        public ISubnetworkCustomerWheelsRepository SubnetworkCustomerWheelsRepository
        {
            get
            {
                if (_SubnetworkCustomerWheelsRepository == null)
                {
                    _SubnetworkCustomerWheelsRepository = new SubnetworkCustomerWheelsRepository(_modelContext);
                }
                return _SubnetworkCustomerWheelsRepository;
            }
        }

        private ISubnetworkSystemFunctionsRepository _SubnetworkSystemFunctionsRepository;
        public ISubnetworkSystemFunctionsRepository SubnetworkSystemFunctionsRepository
        {
            get
            {
                if (_SubnetworkSystemFunctionsRepository == null)
                {
                    _SubnetworkSystemFunctionsRepository = new SubnetworkSystemFunctionsRepository(_modelContext);
                }
                return _SubnetworkSystemFunctionsRepository;
            }
        }

        private INetworkFunctionRepository _NetworkFunctionRepository;
        public INetworkFunctionRepository NetworkFunctionRepository
        {
            get
            {
                if (_NetworkFunctionRepository == null)
                {
                    _NetworkFunctionRepository = new NetworkFunctionRepository(_modelContext);
                }
                return _NetworkFunctionRepository;
            }
        }

        private ICustomerWheelRepository _CustomerWheelRepository;
        public ICustomerWheelRepository CustomerWheelRepository
        {
            get
            {
                if (_CustomerWheelRepository == null)
                {
                    _CustomerWheelRepository = new CustomerWheelRepository(_modelContext);
                }
                return _CustomerWheelRepository;
            }
        }


        private ICriticalAssetTypeRepository _CriticalAssetTypeRepository;
        public ICriticalAssetTypeRepository CriticalAssetTypeRepository
        {
            get
            {
                if (_CriticalAssetTypeRepository == null)
                {
                    _CriticalAssetTypeRepository = new CriticalAssetTypeRepository(_modelContext);
                }
                return _CriticalAssetTypeRepository;
            }
        }

        private IAuthenicationTypeRepository _AuthenicationTypeRepository;

        public IAuthenicationTypeRepository AuthenicationTypeRepository
        {

            get
            {
                if (_AuthenicationTypeRepository == null)
                {
                    _AuthenicationTypeRepository = new AuthenicationTypeRepository(_modelContext);
                }
                return _AuthenicationTypeRepository;
            }
        }

        private IBusinessContinuityMethodRepository _BusinessContinuityMethodRepository;

        public IBusinessContinuityMethodRepository BusinessContinuityMethodRepository
        {

            get
            {
                if (_BusinessContinuityMethodRepository == null)
                {
                    _BusinessContinuityMethodRepository = new BusinessContinuityMethodRepository(_modelContext);
                }
                return _BusinessContinuityMethodRepository;
            }
        }


        private ISiteResilienceRepository _SiteResilienceRepository;

        public ISiteResilienceRepository SiteResilienceRepository
        {

            get
            {
                if (_SiteResilienceRepository == null)
                {
                    _SiteResilienceRepository = new SiteResilienceRepository(_modelContext);
                }
                return _SiteResilienceRepository;
            }
        }


        private IInstanceResilienceRepository _InstanceResilienceRepository;

        public IInstanceResilienceRepository InstanceResilienceRepository
        {

            get
            {
                if (_InstanceResilienceRepository == null)
                {
                    _InstanceResilienceRepository = new InstanceResilienceRepository(_modelContext);
                }
                return _InstanceResilienceRepository;
            }
        }


        private ISecurityManagerRepository _SecurityManagerRepository;

        public ISecurityManagerRepository SecurityManagerRepository
        {

            get
            {
                if (_SecurityManagerRepository == null)
                {
                    _SecurityManagerRepository = new SecurityManagerRepository(_modelContext);
                }
                return _SecurityManagerRepository;
            }
        }


        private IThirdPartyAccessTypeRepository _ThirdPartyAccessTypeRepository;

        public IThirdPartyAccessTypeRepository ThirdPartyAccessTypeRepository
        {

            get
            {
                if (_ThirdPartyAccessTypeRepository == null)
                {
                    _ThirdPartyAccessTypeRepository = new ThirdPartyAccessTypeRepository(_modelContext);
                }
                return _ThirdPartyAccessTypeRepository;
            }
        }


        private ISWDeliveryLifeCycleRepository _SWDeliveryLifeCycleRepository;

        public ISWDeliveryLifeCycleRepository SWDeliveryLifeCycleRepository
        {

            get
            {
                if (_SWDeliveryLifeCycleRepository == null)
                {
                    _SWDeliveryLifeCycleRepository = new SWDeliveryLifeCycleRepository(_modelContext);
                }
                return _SWDeliveryLifeCycleRepository;
            }
        }

        private ILicenseModelRepository _LicenseModelRepository;

        public ILicenseModelRepository LicenseModelRepository
        {

            get
            {
                if (_LicenseModelRepository == null)
                {
                    _LicenseModelRepository = new LicenseModelRepository(_modelContext);
                }
                return _LicenseModelRepository;
            }
        }

        private IDesignAspectRepository _DesignAspectRepository;
        public IDesignAspectRepository DesignAspectRepository
        {
            get
            {
                if (_DesignAspectRepository == null)
                {
                    _DesignAspectRepository = new DesignAspectRepository(_modelContext);
                }
                return _DesignAspectRepository;
            }
        }

        private IDesignAspectSupportedServiceRepository _DesignAspectSupportedServiceRepository;
        public IDesignAspectSupportedServiceRepository DesignAspectSupportedServiceRepository
        {
            get
            {
                if (_DesignAspectSupportedServiceRepository == null)
                {
                    _DesignAspectSupportedServiceRepository = new DesignAspectSupportedServiceRepository(_modelContext);
                }
                return _DesignAspectSupportedServiceRepository;
            }
        }
        private IDesignAspectNetworkFunctionRepository _DesignAspectNetworkFunctionRepository;
        public IDesignAspectNetworkFunctionRepository DesignAspectNetworkFunctionRepository
        {
            get
            {
                if (_DesignAspectNetworkFunctionRepository == null)
                {
                    _DesignAspectNetworkFunctionRepository = new DesignAspectNetworkFunctionRepository(_modelContext);
                }
                return _DesignAspectNetworkFunctionRepository;
            }
        }

        private ILocationDeploymentTypeRepository _locationDeploymentTypeRepository;
        public ILocationDeploymentTypeRepository LocationDeploymentTypeRepository
        {
            get
            {
                if (_locationDeploymentTypeRepository == null)
                {
                    _locationDeploymentTypeRepository = new LocationDeploymentTypeRepository(_modelContext);
                }
                return _locationDeploymentTypeRepository;
            }
        }

        public INetworkElementRepository _NetworkElement;
        public INetworkElementRepository NetworkElement
        {
            get
            {
                if (_NetworkElement == null)
                {
                    _NetworkElement = new NetworkElementRepository(_modelContext);
                }
                return _NetworkElement;
            }
        }

        private IIdentityRepository _Identity;
        public IIdentityRepository Identity
        {
            get
            {
                if (_Identity == null)
                {
                    _Identity = new IdentityRepository(_modelContext);
                }
                return _Identity;
            }
        }

        public IHardwareConfigurationRepository _HardwareConfiguration;

        public IHardwareConfigurationRepository HardwareConfiguration
        {
            get
            {
                if (_HardwareConfiguration == null)
                {
                    _HardwareConfiguration = new HardwareConfigurationRepository(_modelContext);
                }
                return _HardwareConfiguration;
            }
        }
        private ISoftwareComponentRepository _softwareComponent;
        public ISoftwareComponentRepository SoftwareComponent
        {
            get
            {
                if (_softwareComponent == null)
                {
                    _softwareComponent = new SoftwareComponentRepository(_modelContext);
                }
                return _softwareComponent;
            }
        }

        private ISoftwareConfigurationRepository _SoftwareConfiguration;
        public ISoftwareConfigurationRepository SoftwareConfiguration
        {
            get
            {
                if (_SoftwareConfiguration == null)
                {
                    _SoftwareConfiguration = new SoftwareConfigurationRepository(_modelContext);
                }
                return _SoftwareConfiguration;
            }
        }
        public IComponentRepository _Component;
        public IComponentRepository Component
        {
            get
            {
                if (_Component == null)
                {
                    _Component = new ComponentsRepository(_modelContext);
                }
                return _Component;
            }
        }

        private IFunctionRepository _functioinRepository;
        public IFunctionRepository Function
        {
            get
            {
                if (_functioinRepository == null)
                {
                    _functioinRepository = new FunctionRepository(_modelContext);
                }
                return _functioinRepository;
            }
        }
        private IFunctionAreaRepository _functionArea;
        public IFunctionAreaRepository FunctionArea
        {
            get
            {
                if (_functionArea == null)
                {
                    _functionArea = new FunctionAreaRepository(_modelContext);
                }
                return _functionArea;
            }
        }
        private ISubFunctionRepository _subFunctionRepository;
        public ISubFunctionRepository SubFunction
        {
            get
            {
                if (_subFunctionRepository == null)
                {
                    _subFunctionRepository = new SubFunctionRepository(_modelContext);
                }
                return _subFunctionRepository;
            }
        }
        private ISubFunctionAreaRepository _subFunctionAreaRepository;
        public ISubFunctionAreaRepository SubFunctionArea
        {
            get
            {
                if (_subFunctionAreaRepository == null)
                {
                    _subFunctionAreaRepository = new SubFunctionAreaRepository(_modelContext);
                }
                return _subFunctionAreaRepository;
            }
        }
        private IAuditHistoryRepository _auditHistory;
        public IAuditHistoryRepository AuditHistory
        {
            get
            {
                if (_auditHistory == null)
                {
                    _auditHistory = new AuditHistoryRepository(_modelContext);
                }
                return _auditHistory;
            }
        }

        private IGenericReportRepository _genericReport;
        public IGenericReportRepository GenericReportRepository
        {
            get
            {
                if (_genericReport == null)
                {
                    _genericReport = new GenericReportRepository(_modelContext);
                }
                return _genericReport;
            }
        }

        private IDeliveryTrackingRepository _deliveryTrackingRepository;
        public IDeliveryTrackingRepository DeliveryTrackingRepository
        {
            get
            {
                if (_deliveryTrackingRepository == null)
                {
                    _deliveryTrackingRepository = new DeliveryTrackingRepository(_modelContext);
                }
                return _deliveryTrackingRepository;
            }
        }

        // IDriverRepository IRepositoryWrapper.Driver => throw new NotImplementedException();


        private ICategoryRepository _CategoryRepository;
        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_CategoryRepository == null)
                {
                    _CategoryRepository = new CategoryRepository(_modelContext);
                }
                return _CategoryRepository;
            }
        }


        private IClassRepository _ClassRepository;
        public IClassRepository ClassRepository
        {
            get
            {
                if (_ClassRepository == null)
                {
                    _ClassRepository = new ClassRepository(_modelContext);
                }
                return _ClassRepository;
            }
        }
        private ITypeRepository _TypeRepository;
        public ITypeRepository TypeRepository
        {
            get
            {
                if (_TypeRepository == null)
                {
                    _TypeRepository = new TypeRepository(_modelContext);
                }
                return _TypeRepository;
            }
        }
        private IIdentityAsIsRepository _IdentityAsIsRepository;
        public IIdentityAsIsRepository IdentityAsIsRepository
        {
            get
            {
                if (_IdentityAsIsRepository == null)
                {
                    _IdentityAsIsRepository = new IdentityAsIsRepository(_modelContext);
                }
                return _IdentityAsIsRepository;
            }
        }

        public IResourceKeyMasterRepository _ResourceKeyMaster;
        public IResourceKeyMasterRepository ResourceKeyMaster
        {
            get
            {
                if (_ResourceKeyMaster == null)
                {
                    _ResourceKeyMaster = new ResourceKeyMasterRepository(_modelContext);
                }
                return _ResourceKeyMaster;
            }
        }

        public IResourceTypeRepository _ResourceType;
        public IResourceTypeRepository ResourceType
        {
            get
            {
                if (_ResourceType == null)
                {
                    _ResourceType = new ResourceTypeRepository(_modelContext);
                }
                return _ResourceType;
            }

        }

        private IDesignComponentFamilyLifeCycleRepository _DesignComponentFamilyLifeCycleRepository;
        public IDesignComponentFamilyLifeCycleRepository DesignComponentFamilyLifeCycleRepository
        {
            get
            {
                if (_DesignComponentFamilyLifeCycleRepository == null)
                {
                    _DesignComponentFamilyLifeCycleRepository = new DesignComponentFamilyLifeCycleRepository(_modelContext);
                }
                return _DesignComponentFamilyLifeCycleRepository;
            }
        }

        private ILcmExportSettingRepository _LcmExportSettingRepository;
        public ILcmExportSettingRepository LcmExportSettingRepository
        {
            get
            {
                if (_LcmExportSettingRepository == null)
                {
                    _LcmExportSettingRepository = new LcmExportSettingRepository(_modelContext);
                }

                return _LcmExportSettingRepository;
            }
        }

        private ILcmAncillaryDataRepository _LcmAncillaryDataRepositor;
        public ILcmAncillaryDataRepository LcmAncillaryData
        {
            get
            {
                if (_LcmAncillaryDataRepositor == null)
                {
                    _LcmAncillaryDataRepositor = new LcmAncillaryDataRepository(_modelContext);
                }
                return _LcmAncillaryDataRepositor;
            }
        }

        private IRiskClusterRepository _RiskClusterRepository;
        public IRiskClusterRepository RiskClusterRepository
        {
            get
            {
                if(_RiskClusterRepository == null)
                {
                    _RiskClusterRepository = new RiskClusterRepository(_modelContext);
                }
                return _RiskClusterRepository;
            }
            
        }

        private IRiskClusterVodafoneNamesRepository _RiskClusterVodafoneNamesRepository;
        public IRiskClusterVodafoneNamesRepository RiskClusterVodafoneNamesRepository
        {
            get
            {
                if(_RiskClusterVodafoneNamesRepository == null)
                {
                    _RiskClusterVodafoneNamesRepository = new RiskClusterVodafoneNameRepository(_modelContext);
                }
                return _RiskClusterVodafoneNamesRepository;
            }
        }

        private IReconciliationRepository _reconciliationRepository;
        public IReconciliationRepository ReconciliationRepository
        {
            get
            {
                if(_reconciliationRepository == null)
                {
                    _reconciliationRepository = new ReconciliationRepository(_modelContext);
                }
                return _reconciliationRepository;
            }
        }

        public IAuditTablesAndColumnsRepository _auditTablesAndColumnsRepository;
        public IAuditTablesAndColumnsRepository AuditTablesAndColumnsRepository
        {
            get
            {
                if (_auditTablesAndColumnsRepository == null)
                {
                    _auditTablesAndColumnsRepository = new AuditTablesAndColumnsRepository(_modelContext);
                }
                return _auditTablesAndColumnsRepository;
            }
        }

        private IAuditLogRepository _auditLogRepository;
        public IAuditLogRepository AuditLogRepository
        {
            get
            {
                if (_auditLogRepository == null)
                {
                    _auditLogRepository = new AuditLogRepositary(_modelContext);
                }
                return _auditLogRepository;
            }
        }

        private INodeParseHistoryRepository _nodeParseHistoryRepository;
        public INodeParseHistoryRepository NodeParseHistoryRepository
        {
            get
            {
                if (_nodeParseHistoryRepository == null)
                {
                    _nodeParseHistoryRepository = new NodeParseHistoryRepository(_modelContext);
                }
                return _nodeParseHistoryRepository;
            }
        }

        public IFeedBackLoopAuditRepository _feedBackLoopAuditRepository;
        public IFeedBackLoopAuditRepository FeedBackLoopAuditRepository
        {
            get
            {
                if (_feedBackLoopAuditRepository == null)
                {
                    _feedBackLoopAuditRepository = new FeedBackLoopAuditRepository(_modelContext);
                }
                return _feedBackLoopAuditRepository;
            }
        }

        private ISWConfigFunctionAreaRepository _swConfigFunctionAreaRepository;
        public ISWConfigFunctionAreaRepository SWConfigFunctionAreaRepository
        {
            get
            {
                if (_swConfigFunctionAreaRepository == null)
                {
                    _swConfigFunctionAreaRepository = new SWConfigFunctionAreaRepository(_modelContext);
                }
                return _swConfigFunctionAreaRepository;
            }
        }

        private ISystemVerificationProblemRepository _systemVerificationProblemRepository;
        public ISystemVerificationProblemRepository SystemVerificationProblemRepository 
        {
            get
            {
                if (_systemVerificationProblemRepository == null)
                {
                    _systemVerificationProblemRepository = new SystemVerificationProblemRepository(_modelContext);
                }
                return _systemVerificationProblemRepository;
            }
        }

        private IProblemCategoryRepository _problemCategoryRepository;
        public IProblemCategoryRepository ProblemCategoryRepository 
        {
            get
            {
                if (_problemCategoryRepository == null)
                {
                    _problemCategoryRepository = new ProblemCategoryRepository(_modelContext);
                }
                return _problemCategoryRepository;
            }
        }

        private ISeverityRepository _severityRepository;
        public ISeverityRepository SeverityRepository 
        {
            get
            {
                if (_severityRepository == null)
                {
                    _severityRepository = new SeverityRepository(_modelContext);
                }
                return _severityRepository;
            }
        }

        private ISWConfigSubFunctionRepository _sWConfigSubFunctionRepository;
        public ISWConfigSubFunctionRepository SWConfigSubFunctionRepository
        {
            get
            {
                if (_sWConfigSubFunctionRepository == null)
                {
                    _sWConfigSubFunctionRepository = new SWConfigSubFunctionRepository(_modelContext);
                }
                return _sWConfigSubFunctionRepository;
            }
        }

        private ISWConfigSubFunctionAreasRepository _sWConfigSubFunctionAreasRepository;
        public ISWConfigSubFunctionAreasRepository SWConfigSubFunctionAreasRepository
        {
            get
            {
                if (_sWConfigSubFunctionAreasRepository == null)
                {
                    _sWConfigSubFunctionAreasRepository = new SWConfigSubFunctionAreasRepository(_modelContext);
                }
                return _sWConfigSubFunctionAreasRepository;
            }
        }

        private IUserDefinedReportsLogsRepository _userDefinedReportsLogsRepository;
        public IUserDefinedReportsLogsRepository  UserDefinedReportsLogsRepository
        {
            get
            {
                if (_userDefinedReportsLogsRepository == null)
                {
                    _userDefinedReportsLogsRepository = new UserDefinedReportsLogsRepository(_modelContext);
                }
                return _userDefinedReportsLogsRepository;
            }
        }
        private IOrganisationRepository _organisationRepository;

        public IOrganisationRepository OrganisationRepository
        {
            get
            {
                if(_organisationRepository == null)
                {
                    _organisationRepository = new OrganisationRepository(_modelContext);
                }
                return _organisationRepository;
            }
        }

        public IPracticeRepository _practiceRepository;
        public IPracticeRepository PracticeRepository
        {
            get
            {
                if (_practiceRepository == null)
                {
                    _practiceRepository = new PracticeRepository(_modelContext);
                }
                return _practiceRepository;
            }
        }

        public IMainOrganisationRepository _mainOrganisationRepository;
        public IMainOrganisationRepository MainOrganisationRepository
        {
            get
            {
                if (_mainOrganisationRepository == null)
                {
                    _mainOrganisationRepository = new MainOrganisationRepository(_modelContext);
                }
                return _mainOrganisationRepository;
            }
        }

        private IMajorSwBuidlsDesignContactsRepository _majorSwBuidlsDesignContactsRepository;
        public IMajorSwBuidlsDesignContactsRepository MajorSwBuidlsDesignContactsRepository
        {
            get
            {
                if(_majorSwBuidlsDesignContactsRepository == null)
                {
                    _majorSwBuidlsDesignContactsRepository = new MajorSwBuidlsDesignContactsRepository(_modelContext);
                }
                return _majorSwBuidlsDesignContactsRepository;
            }
        }

        private IMajorHwBuidlsDesignContactsRepository _majorHwBuidlsDesignContactsRepository;
        public IMajorHwBuidlsDesignContactsRepository MajorHwBuidlsDesignContactsRepository
        {
            get
            {
                if (_majorHwBuidlsDesignContactsRepository == null)
                {
                    _majorHwBuidlsDesignContactsRepository = new MajorHwBuidlsDesignContactsRepository(_modelContext);
                }
                return _majorHwBuidlsDesignContactsRepository;
            }
        }

        #region  / /Systemo Of System

        private IComponentSoftwareBuildBagRepository _componentSoftwareBuildBagRepository;
        public IComponentSoftwareBuildBagRepository ComponentSoftwareBuildBagRepository
        {
            get
            {
                if (_componentSoftwareBuildBagRepository == null)
                {
                    _componentSoftwareBuildBagRepository = new ComponentSoftwareBuildBagRepository(_modelContext);
                }
                return _componentSoftwareBuildBagRepository;
            }
        }
 
        private IComponentSoftwareBuildsDesignContactRepository _componentSoftwareBuildsDesignContactRepository;
        public IComponentSoftwareBuildsDesignContactRepository ComponentSoftwareBuildsDesignContactRepository
        {
            get
            {
                if (_componentSoftwareBuildsDesignContactRepository == null)
                {
                    _componentSoftwareBuildsDesignContactRepository = new ComponentSoftwareBuildsDesignContactRepository(_modelContext);
                }
                return _componentSoftwareBuildsDesignContactRepository;
            }
        }

        private IBuildBagRepository _BuildBagRepository;
        public IBuildBagRepository BuildBagRepository
        {
            get
            {
                if (_BuildBagRepository == null)
                {
                    _BuildBagRepository = new BuildBagRepository(_modelContext);
                }
                return _BuildBagRepository;
            }
        }

        private IComponentSoftwareBuildRepository _componentSoftwareBuildRepository;
        public IComponentSoftwareBuildRepository ComponentSoftwareBuildRepository
        {
            get
            {
                if (_componentSoftwareBuildRepository == null)
                {
                    _componentSoftwareBuildRepository = new ComponentSoftwareBuildRepository(_modelContext);
                }
                return _componentSoftwareBuildRepository;
            }
        }

        #endregion

        #region ClusterLevel PA
        private INetworkElementClusterAsPlannedRepository _networkElementClusterAsPlannedRepository;
        public INetworkElementClusterAsPlannedRepository NetworkElementClusterAsPlannedRepository
        {
            get
            {
                if (_networkElementClusterAsPlannedRepository == null)
                {
                    _networkElementClusterAsPlannedRepository = new NetworkElementClusterAsPlannedRepository(_modelContext);
                }
                return _networkElementClusterAsPlannedRepository;
            }
        }

        private IClusterUpGradeStatusRepository _clusterUpGradeStatusRepository;
        public IClusterUpGradeStatusRepository ClusterUpGradeStatusRepository
        {
            get
            {
                if (_clusterUpGradeStatusRepository == null)
                {
                    _clusterUpGradeStatusRepository = new ClusterUpGradeStatusRepository(_modelContext);
                }
                return _clusterUpGradeStatusRepository;
            }
        }


        private IInfraClusterAsPlannedRepository _infraClusterAsPlannedRepository;
        public IInfraClusterAsPlannedRepository InfraClusterAsPlannedRepository
        {
            get
            {
                if (_infraClusterAsPlannedRepository == null)
                {
                    _infraClusterAsPlannedRepository = new InfraClusterAsPlannedRepository(_modelContext);
                }
                return _infraClusterAsPlannedRepository;
            }
        }


        #endregion

        #region RBAC
        private IAspNetModulesRepository _AspNetModulesRepository;
        public IAspNetModulesRepository AspNetModulesRepository
        {
            get
            {
                if (_AspNetModulesRepository == null)
                {
                    _AspNetModulesRepository = new AspNetModulesRepository(_modelContext);
                }
                return _AspNetModulesRepository;
            }
        }
        private IAspNetUserRolePermissionsRepository _AspNetUserRolePermissionsRepository;
        public IAspNetUserRolePermissionsRepository AspNetUserRolePermissionsRepository
        {
            get
            {
                if (_AspNetUserRolePermissionsRepository == null)
                {
                    _AspNetUserRolePermissionsRepository = new AspNetUserRolePermissionsRepository(_modelContext);
                }
                return _AspNetUserRolePermissionsRepository;
            }
        }

        #endregion
        public ITsrPassThroughRepository _tsrPassThroughRepository;
        public ITsrPassThroughRepository TsrPassThroughRepository
        {
            get
            {
                if (_tsrPassThroughRepository == null)
                {
                    _tsrPassThroughRepository = new TSRReportPassThroughRepository(_modelContext);
                }
                return _tsrPassThroughRepository;
            }
        }

        
        public INonTemsNweAsPlannedPassThroughRepository _nonTemsNweAsPlannedPassThroughRepository;
        public INonTemsNweAsPlannedPassThroughRepository NonTemsNweAsPlannedPassThroughRepository
        {
            get
            {
                if (_nonTemsNweAsPlannedPassThroughRepository == null)
                {
                    _nonTemsNweAsPlannedPassThroughRepository = new NonTemsNweAsPlannedPassThroughRepository(_modelContext);
                }
                return _nonTemsNweAsPlannedPassThroughRepository;
            }
        }

        public ITsrLogRepository _tsrLogRepository;
        public ITsrLogRepository TsrLogRepository
        {
            get
            {
                if (_tsrLogRepository == null)
                {
                    _tsrLogRepository = new TsrLogRepository(_modelContext);
                }
                return _tsrLogRepository;
            }
        }

        public IAppSettingsConfigurationRepository _appConfigurationSettingsRepository;
        public IAppSettingsConfigurationRepository AppConfigurationSettingsRepository
        {
            get
            {
                if (_appConfigurationSettingsRepository == null)
                {
                    _appConfigurationSettingsRepository = new AppSettingsConfigurationRepository(_modelContext);
                }
                return _appConfigurationSettingsRepository;
            }
        }
        public IAppSettingsRepository _appConfigurationRepository;
        public IAppSettingsRepository AppConfigurationRepository
        {
            get
            {
                if (_appConfigurationRepository == null)
                {
                    _appConfigurationRepository = new AppSettingsRepository(_modelContext);
                }
                return _appConfigurationRepository;
            }
        }
        #region Exodus 
        private IDaMigrationStatusRepository _daMigrationStatusRepository;
        public IDaMigrationStatusRepository DaMigrationStatusRepository
        {
            get
            {
                if (_daMigrationStatusRepository == null)
                {
                    _daMigrationStatusRepository = new DaMigrationStatusRepository(_modelContext);
                }
                return _daMigrationStatusRepository;
            }
        }

        private IDaAssetMigrationRepository _daAssetMigrationRepository;
        public IDaAssetMigrationRepository  DaAssetMigrationRepository
        {
            get
            {
                if (_daAssetMigrationRepository == null)
                {
                    _daAssetMigrationRepository = new DaAssetMigrationRepository(_modelContext);
                }
                return _daAssetMigrationRepository;
            }
        }

        private IVnfHardwareRepository  _vnfHardwareRepository;
        public IVnfHardwareRepository VnfHardwareRepository
        {
            get
            {
                if (_vnfHardwareRepository == null)
                {
                    _vnfHardwareRepository = new VnfHardwareRepository(_modelContext);
                }
                return _vnfHardwareRepository;
            }
        }
        #endregion
        #region CBOM 
        public ICnfCapacityRepository _CnfCapacityRepository;
        public ICnfCapacityRepository CnfCapacityRepository
        {
            get
            {
                if (_CnfCapacityRepository == null)
                {
                    _CnfCapacityRepository = new CnfCapacityRepository(_modelContext);
                }
                return _CnfCapacityRepository;
            }
        }

        public ICnfClusterRepository _CnfClusterRepository;
        public ICnfClusterRepository CnfClusterRepository
        {
            get
            {
                if (_CnfClusterRepository == null)
                {
                    _CnfClusterRepository = new CnfClusterRepository(_modelContext);
                }
                return _CnfClusterRepository;
            }
        }

        public ICnfClusterInfoRepository _CnfClusterInfoRepository;
        public ICnfClusterInfoRepository CnfClusterInfoRepository
        {
            get
            {
                if (_CnfClusterInfoRepository == null)
                {
                    _CnfClusterInfoRepository = new CnfClusterInfoRepository(_modelContext);
                }
                return _CnfClusterInfoRepository;
            }
        }


        public ICnfPodInfoRepository _CnfPodInfoRepository;
        public ICnfPodInfoRepository CnfPodInfoRepository
        {
            get
            {
                if (_CnfPodInfoRepository == null)
                {
                    _CnfPodInfoRepository = new CnfPodInfoRepository(_modelContext);
                }
                return _CnfPodInfoRepository;
            }
        }

        public ICnfNameRepository _CnfNameRepository;
        public ICnfNameRepository CnfNameRepository
        {
            get
            {
                if (_CnfNameRepository == null)
                {
                    _CnfNameRepository = new CnfNameRepository(_modelContext);
                }
                return _CnfNameRepository;
            }
        }
        private ICnfHardwareRepository _cnfHardwareRepository;
        public ICnfHardwareRepository CnfHardwareRepository
        {
            get
            {
                if (_cnfHardwareRepository == null)
                {
                    _cnfHardwareRepository = new CBom.CnfHardwareRepository(_modelContext);
                }
                return _cnfHardwareRepository;
            }
        }
        private ICnfPriorityRepository _cnfPriorityRepository;
        public ICnfPriorityRepository CnfPriorityRepository
        {
            get
            {
                if (_cnfPriorityRepository == null)
                {
                    _cnfPriorityRepository = new CBom.CnfPriorityRepository(_modelContext);
                }
                return _cnfPriorityRepository;
            }
        }
        public IPodTypeInfoRepository _PodTypeInfoRepository;
        public IPodTypeInfoRepository PodTypeInfoRepository
        {
            get
            {
                if (_PodTypeInfoRepository == null)
                {
                    _PodTypeInfoRepository = new PodTypeInfoRepository(_modelContext);
                }
                return _PodTypeInfoRepository;
            }
        }

        private IFunctionStandardNameRepository _FunctionStandardNameRepository;
        public IFunctionStandardNameRepository FunctionStandardNameRepository
        {
            get
            {
                if (_FunctionStandardNameRepository == null)
                {
                    _FunctionStandardNameRepository = new FunctionStandardNameRepository(_modelContext);
                }
                return _FunctionStandardNameRepository;
            }
        }
        #endregion

        #region Vbom

        private IClusterNameRepository _clusterNameRepository;
        public IClusterNameRepository ClusterNameRepository
        {
            get
            {
                if (_clusterNameRepository == null)
                {
                    _clusterNameRepository = new ClusterNameRepository(_modelContext);
                }
                return _clusterNameRepository;
            }
        }

        private IVmTypeNameRepository _vmTypeNameRepository;
        public IVmTypeNameRepository VmTypeNameRepository
        {
            get
            {
                if (_vmTypeNameRepository == null)
                {
                    _vmTypeNameRepository = new VmTypeNameRepository(_modelContext);
                }
                return _vmTypeNameRepository;
            }
        }

        private IVnfInfoRepository _VnfInfoRepository;
        public IVnfInfoRepository VnfInfoRepository
        {
            get
            {
                if (_VnfInfoRepository == null)
                {
                    _VnfInfoRepository = new VnfInfoRepository(_modelContext);
                }
                return _VnfInfoRepository;
            }
        }
       
        private IVnfNameRepository _vnfNameRepository;
        public IVnfNameRepository VnfNameRepository
        {
            get
            {
                if (_vnfNameRepository == null)
                {
                    _vnfNameRepository = new VnfNameRepository(_modelContext);
                }
                return _vnfNameRepository;
            }
        }

        private IVnfVmCapacityRepository _vnfVmCapacityRepository;
        public IVnfVmCapacityRepository VnfVmCapacityRepository
        {
            get
            {
                if (_vnfVmCapacityRepository == null)
                {
                    _vnfVmCapacityRepository = new VnfVmCapacityRepository(_modelContext);
                }
                return _vnfVmCapacityRepository;
        }
        }
 
        private IVnfClusterInfoRepository _vnfClusterInfoRepository;
        public IVnfClusterInfoRepository VnfClusterInfoRepository
        {
            get
            {
                if (_vnfClusterInfoRepository == null)
                {
                    _vnfClusterInfoRepository = new VnfClusterInfoRepository(_modelContext);
                }
                return _vnfClusterInfoRepository;
            }
        }

        private IInterVmTypeRepository _interVmTypeRepository;
        public IInterVmTypeRepository InterVmTypeRepository
        {
            get
            {
                if (_interVmTypeRepository == null)
                {
                    _interVmTypeRepository = new InterVmTypeRepository(_modelContext);
                }
                return _interVmTypeRepository;
            }
        }

        private IIntraVmTypeRepository _intraVmTypeRepository;
        public IIntraVmTypeRepository IntraVmTypeRepository
        {
            get
            {
                if (_intraVmTypeRepository == null)
                {
                    _intraVmTypeRepository = new IntraVmTypeRepository(_modelContext);
                }
                return _intraVmTypeRepository;
            }
        }

        private IVmWorkLoadTypeRepository _vmWorkLoadTypeRepository;
        public IVmWorkLoadTypeRepository VnfWorkLoadTypeRepository
        {
            get
            {
                if (_vmWorkLoadTypeRepository == null)
                {
                    _vmWorkLoadTypeRepository = new VmWorkLoadTypeRepository(_modelContext);
                }
                return _vmWorkLoadTypeRepository;
            }
        }
        #endregion


        #region AssetHardwareConfig
        private IAssetClusterRepository _assetClusterRepository;
        public IAssetClusterRepository AssetClusterRepository
        {
            get
            {
                if (_assetClusterRepository == null)
                {
                    _assetClusterRepository = new AssetClusterRepository(_modelContext);
                }
                return _assetClusterRepository;
            }
        }

        private IAssetCapacityInfoRepository _assetCapacityInfoRepository;
        public IAssetCapacityInfoRepository AssetCapacityInfoRepository
        {
            get
            {
                if (_assetCapacityInfoRepository == null)
                {
                    _assetCapacityInfoRepository = new AssetCapacityInfoRepository(_modelContext);
                }
                return _assetCapacityInfoRepository;
            }
        }
        private IAssetClusterTypeRepository _assetClusterTypeRepository;
        public IAssetClusterTypeRepository AssetClusterTypeRepository
        {
            get
            {
                if (_assetClusterTypeRepository == null)
                {
                    _assetClusterTypeRepository = new AssetClusterTypeRepository(_modelContext);
                }
                return _assetClusterTypeRepository;
            }
        }

        private IAssetHardwareAncillaryRepository _assetHardwareAncillaryRepository;
        public IAssetHardwareAncillaryRepository AssetHardwareAncillaryRepository
        {
            get
            {
                if (_assetHardwareAncillaryRepository == null)
                {
                    _assetHardwareAncillaryRepository = new AssetHardwareAncillaryRepository(_modelContext);
                }
                return _assetHardwareAncillaryRepository;
            }
        }
        private IDataCenterRepository _dataCenterRepository;
        public IDataCenterRepository DataCenterRepository
        {
            get
            {
                if (_dataCenterRepository == null)
                {
                    _dataCenterRepository = new DataCenterRepository(_modelContext);
                }
                return _dataCenterRepository;
            }
        }

        #endregion


        #region  ExcelTempleteConfigration

        private IExcelTemplateConfigurationRepository _excelTemplateConfigurationRepository;
        public IExcelTemplateConfigurationRepository  ExcelTemplateConfigurationRepository
        {
            get
            {
                if (_excelTemplateConfigurationRepository == null)
                {
                    _excelTemplateConfigurationRepository = new ExcelTemplateConfigurationRepository(_modelContext);
                }
                return _excelTemplateConfigurationRepository;
            }
        }

        #endregion
        public ISiteRepository _siteRepository;
        public ISiteRepository SiteRepository
        {
            get
            {
                if(_siteRepository == null)
                {
                    _siteRepository = new SiteRepository(_modelContext);
                }
                return _siteRepository;
            }
        }

        private ISystemNamesRepository _systemNamesRepository;
        public ISystemNamesRepository SystemNamesRepository
        {
            get
            {
                if(_systemNamesRepository == null)
                {
                    _systemNamesRepository = new SystemNamesRepository(_modelContext);
                }
                return _systemNamesRepository;
            }
        }

        private IComponentManufacturersRepository _componentManufacturersRepository;
        public IComponentManufacturersRepository ComponentManufacturersRepository
        {
            get
            {
                if(_componentManufacturersRepository == null)
                {
                    _componentManufacturersRepository = new ComponentManufacturersRepository(_modelContext);
                }
                return _componentManufacturersRepository;
            }
        }
        #region // BGT 
        private IBudgetProjectTrackersRepository _budgetProjectTrackersRepository;
        public IBudgetProjectTrackersRepository BudgetProjectTrackersRepository
        {
            get
            {
                if (_budgetProjectTrackersRepository == null)
                {
                    _budgetProjectTrackersRepository = new BudgetProjectTrackersRepository(_modelContext);
                }
                return _budgetProjectTrackersRepository;
            }
        }
        
        private IPlannedActivityCategoryRepository _plannedActivityCategoryRepository;
        public IPlannedActivityCategoryRepository PlannedActivityCategoryRepository
        {
            get
            {
                if (_plannedActivityCategoryRepository == null)
                {
                    _plannedActivityCategoryRepository = new PlannedActivityCategoryRepository(_modelContext);
                }
                return _plannedActivityCategoryRepository;
            }
        }


        #endregion

        private IProgramRepository _programRepository;
        public IProgramRepository ProgramRepository
        {
            get
            {
                if(_programRepository == null)
                {
                    _programRepository = new ProgramRepository(_modelContext);
                }
                return _programRepository;
            }
        }

        private IProjectPlanRepository _projectPlanRepository;
        public IProjectPlanRepository ProjectPlanRepository
        {
            get
            {
                if(_projectPlanRepository == null)
                {
                    _projectPlanRepository = new ProjectPlanRepository(_modelContext);
                }
                return _projectPlanRepository;
            }
        }

        private IDaPlannedActivityDcfRepository _daPlannedActivityDcfRepository;
        public IDaPlannedActivityDcfRepository DaPlannedActivityDcfRepository
        {
            get
            {
                if (_daPlannedActivityDcfRepository == null)
                {
                    _daPlannedActivityDcfRepository = new DaPlannedActivityDcfRepository(_modelContext);
                }
                return _daPlannedActivityDcfRepository;
            }
        }

        private IProjectPlanAuditRepository _projectPlanAuditRepository;
        public IProjectPlanAuditRepository ProjectPlanAuditRepository
        {
            get
            {
                if (_projectPlanAuditRepository == null)
                {
                    _projectPlanAuditRepository = new ProjectPlanAuditRepository(_modelContext);
                }
                return _projectPlanAuditRepository;
            }
        }

        private IPassThroughRepository _passThroughRepository;
        public IPassThroughRepository PassThroughRepository
        {
            get
            {
                if (_passThroughRepository == null)
                {
                    _passThroughRepository = new PassThroughRepository(_modelContext);
                }
                return _passThroughRepository;
            }
        }

        public INonTemsPassThroughConfigRepository _nonTemsPassThroughConfigRepository;
        public INonTemsPassThroughConfigRepository NonTemsPassThroughConfigRepository
        {
            get
            {
                if (_nonTemsPassThroughConfigRepository == null)
                {
                    _nonTemsPassThroughConfigRepository = new NonTemsPassThroughConfigRepository(_modelContext);
                }
                return _nonTemsPassThroughConfigRepository;
            }
        }

        private ITemsFntReportRepository _temsFntReportRepository;
        public ITemsFntReportRepository TemsFntReportRepository
        {
            get
            {
                if(_temsFntReportRepository == null)
                {
                    _temsFntReportRepository = new TemsFntReportRepository(_modelContext);
                }
                return _temsFntReportRepository;
            }
        }

        private ISwPassThroughRepositoryLcm _swpassThroughRepositoryLcm;
        public ISwPassThroughRepositoryLcm SwPassThroughLcmRepository
        {
            get
            {
                if(_swpassThroughRepositoryLcm == null)
                {
                    _swpassThroughRepositoryLcm = new SwPassThroughLcmRepository(_modelContext);
                }
                return _swpassThroughRepositoryLcm;
            }
        }

        private IHwPassThroughRepositoryLcm _hwpassThroughRepositoryLcm;
        public IHwPassThroughRepositoryLcm HwPassThroughLcmRepository
        {
            get
            {
                if(_hwpassThroughRepositoryLcm == null)
                {
                    _hwpassThroughRepositoryLcm = new HwPassThroughLcmRepository(_modelContext);
                }
                return _hwpassThroughRepositoryLcm;
            }
        }

        public IAssetMapInfoRepository _assetMapInfoRepository;
        public IAssetMapInfoRepository AssetMapInfoRepository
        {
            get
            {
                if (_assetMapInfoRepository == null)
                {
                    _assetMapInfoRepository = new AssetMapInfoRepository(_modelContext);
                }
                return _assetMapInfoRepository;
            }
        }

        public IAssetAsIsSdiInfoRepository _assetAsIsSdiInfoRepository;
        public IAssetAsIsSdiInfoRepository AssetAsIsSdiInfoRepository 
        {
            get
            {
                if (_assetAsIsSdiInfoRepository == null)
                {
                    _assetAsIsSdiInfoRepository = new AssetAsIsSdiInfoRepository(_modelContext);
                }
                return _assetAsIsSdiInfoRepository;
            }
        }


        public IAssetAsIsHwAncillaryDataRepository _assetAsIsHwAncillaryDataRepository;
        public IAssetAsIsHwAncillaryDataRepository AssetAsIsHwAncillaryDataRepository
        {
            get
            {
                if (_assetAsIsHwAncillaryDataRepository == null)
                {
                    _assetAsIsHwAncillaryDataRepository = new AssetAsIsHwAncillaryDataRepository(_modelContext);
                }
                return _assetAsIsHwAncillaryDataRepository;
            }
        }

        public IAssetAsIsSdiSwitchInfoRepository _assetAsIsSdiSwitchInfoRepository;
        public IAssetAsIsSdiSwitchInfoRepository AssetAsIsSdiSwitchInfoRepository
        {
            get
            {
                if (_assetAsIsSdiSwitchInfoRepository == null)
                {
                    _assetAsIsSdiSwitchInfoRepository = new AssetAsIsSdiSwitchInfoRepository(_modelContext);
                }
                return _assetAsIsSdiSwitchInfoRepository;
            }
        }

        public IReportSchedulerRepository _reportSchedulerRepository;
        public IReportSchedulerRepository ReportSchedulerRepository
        {
            get
            {
                if(_reportSchedulerRepository == null)
                {
                    _reportSchedulerRepository = new ReportSchedulerRepository(_modelContext);
                }
                return _reportSchedulerRepository;
            }
        }

        private IServiceMasterRepository _serviceMasterRepository;
        public IServiceMasterRepository ServiceMasterRepository
        {
            get
            {
                if (_serviceMasterRepository == null)
                {
                    _serviceMasterRepository = new ServiceMasterRepository(_modelContext);
                }
                return _serviceMasterRepository;
            }
        }

        private IServicePlanRepository _servicePlanRepository;
        public IServicePlanRepository ServicePlanRepository
        {
            get
            {
                if(_servicePlanRepository == null)
                {
                    _servicePlanRepository = new ServicePlanRepository(_modelContext);
                }
                return (_servicePlanRepository);
            }
        }

        private IServicePlanDcfMappingRepository _servicePlanDcfMappingRepository;
        public IServicePlanDcfMappingRepository ServicePlanDcfMappingRepository
        {
            get
            {
                if(_servicePlanDcfMappingRepository == null)
                {
                    _servicePlanDcfMappingRepository = new ServicePlanDcfMappingRepository(_modelContext);    
                }
                return _servicePlanDcfMappingRepository;
            }
        }

        private IAspNetUserPreferenceRepository _aspNetUserPreferenceRepository;
        public IAspNetUserPreferenceRepository aspNetUserPreferenceRepository
        {
            get
            {
                if (_aspNetUserPreferenceRepository == null)
                {
                    _aspNetUserPreferenceRepository = new AspNetUserPreferenceRepository(_modelContext);
                }
                return _aspNetUserPreferenceRepository;
            }
        }

        private IAspNetUserOpcosRepository _aspNetUserOpcosRepository;
        public IAspNetUserOpcosRepository AspNetUserOpcosRepository
        {
            get
            {
                if(_aspNetUserOpcosRepository == null)
                {
                    _aspNetUserOpcosRepository = new AspNetUserOpcosRepository(_modelContext);
                }
                return _aspNetUserOpcosRepository;
            }
        }

        private IAspNetUserVerticalsRepository _aspNetUserVerticalsRepository;
        public IAspNetUserVerticalsRepository AspNetUserVerticalsRepository
        {
            get
            {
                if(_aspNetUserVerticalsRepository == null)
                {
                    _aspNetUserVerticalsRepository = new AspNetUserVerticalsRepository(_modelContext);
                }
                return _aspNetUserVerticalsRepository;
            }
        }

        private ITeamRepository _teamRepository;
        public ITeamRepository TeamRepository
        {
            get
            {
                if(_teamRepository == null)
                {
                    _teamRepository = new TeamRepository(_modelContext);
                }
                return _teamRepository;
            }
        }

        private ITeamMemberRepository _teamMemberRepository;
        public ITeamMemberRepository TeamMemberRepository
        {
            get
            {
                if (_teamMemberRepository == null)
                {
                    _teamMemberRepository = new TeamMemberRepository(_modelContext);
                }
                return _teamMemberRepository;
            }
        }

        private IExodusMilestoneAndActivityRepository _exodusMilestoneAndActivityRepository;
        public IExodusMilestoneAndActivityRepository ExodusMilestoneAndActivityRepository
        {
            get
            {
                if(_exodusMilestoneAndActivityRepository == null)
                {
                    _exodusMilestoneAndActivityRepository = new ExodusMilestoneAndActivityRepository(_modelContext);
                }
                return _exodusMilestoneAndActivityRepository;
            }
        }

        public RepositoryWrapper(ModelContext modelContext, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            //_repoContext = repoContext;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _modelContext = modelContext;
        }
        public void Save()
        {
            SetAuditableEntity();
            _modelContext.SaveChanges();
        }
        public async Task SaveAsync()
        {

            #region Ticket 709 Get Fkey Tables and Linked Id based on Pkey Id  - #708 Improve Perfomance
            // var dcEntity = await _modelContext.Designcomponents.Where(x => x.Designcomponentid == 6925).FirstOrDefaultAsync();

            //var tablenameWithColumnName = GetReferencingEntitieWorking(_modelContext, dcEntity);
            //var  linkedRecords = GetReferencingEntities(_modelContext, dcEntity);
            #endregion

            SetAuditableEntity();
            await _modelContext.SaveChangesAsync();
        }
 
        public static Dictionary<string,string> GetReferencingEntitieWorking(ModelContext context, object entity)
        {



            Dictionary<string, string> referencingEntities = new Dictionary<string, string>();

            var entityType = context.Model.FindEntityType(entity.GetType());
            var primaryKey = entityType.FindPrimaryKey();
            var foreignKeyList = entityType.GetReferencingForeignKeys().Select(x => x.Properties);
            // Get the primary key values
            var primaryKeyValues = primaryKey.Properties
                .ToDictionary(p => p.Name, p => p.PropertyInfo.GetValue(entity));

            var foreignKeys = context.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => fk.PrincipalEntityType == entityType); 

            foreach (var foreignKey in foreignKeys)
            {
                
                var dependentEntityType = foreignKey.DeclaringEntityType;
                var dynamicTableName = dependentEntityType.GetDefaultTableName(); 
                var query = context.Set<dynamic>(dynamicTableName).AsQueryable();

                foreach (var fkProperty in foreignKey.Properties)
                {
                    var principalProperty = primaryKey.Properties
                    .FirstOrDefault(p => p.Name == fkProperty.PropertyInfo.Name);
                    var principalValue = fkProperty.PropertyInfo.Name;

                    if(!referencingEntities.ContainsKey(dynamicTableName))
                    referencingEntities.Add(dynamicTableName, fkProperty.PropertyInfo.Name);


                    var wherecondtion = $" {fkProperty.PropertyInfo.Name} =  {primaryKeyValues.Values.FirstOrDefault()} ";

                    
                    var dbSet = context.GetType().GetProperty(dynamicTableName).GetValue(context, null) as IQueryable;                     

                    var dbSetList = dbSet.Cast<dynamic>().ToList() ;
                        

                }


            }

            return referencingEntities;
        }

        public static List<string> GetReferencingEntities(ModelContext context, object entity)
        { 
            var referencingEntities = new List<string>();
            var entityType = context.Model.FindEntityType(entity.GetType());
            var primaryKey = entityType.FindPrimaryKey();
            var foreignKeyList = entityType.GetReferencingForeignKeys().Select(x => x.Properties);
            // Get the primary key values
            var primaryKeyValues = primaryKey.Properties
                .ToDictionary(p => p.Name, p => p.PropertyInfo.GetValue(entity));

            var foreignKeys = context.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => fk.PrincipalEntityType == entityType);
 
            foreach (var foreignKey in foreignKeys)
            {
                
                var dependentEntityType = foreignKey.DeclaringEntityType;
                var dynamicTableName = dependentEntityType.GetDefaultTableName();// dependentEntityType.GetTableName();
 
                referencingEntities.Add(dynamicTableName);

                var query = context.Set<dynamic>(dynamicTableName).AsQueryable();

                foreach (var fkProperty in foreignKey.Properties)
                {
                    var principalProperty = primaryKey.Properties
                    .FirstOrDefault(p => p.Name == fkProperty.PropertyInfo.Name);
                    var principalValue = fkProperty.PropertyInfo.Name;

                    var wherecondtion = $" {fkProperty.PropertyInfo.Name} =  {primaryKeyValues.Values.FirstOrDefault()} ";
 

                    var dbSet = context.GetType().GetProperty(dynamicTableName).GetValue(context, null) as IQueryable;


                    long idValue = 2204; //  primaryKeyValues.Values.FirstOrDefault();
 

                    using (var conttext1 = new ModelContext())
                    {

                        var dynamicConditions = new List<string> {fkProperty.PropertyInfo.Name +" = "+ idValue };

                        string combinedConditions = dynamicConditions .ToString();// string.Join("", dynamicConditions);



                        string sqlQuery = $"SELECT * FROM {dynamicTableName}";

                        var result = context.Database.ExecuteSqlRaw(sqlQuery);

                        var t = result;
                    } 
                    query = (IQueryable<dynamic>)query.ToList();

                }


            }

            return referencingEntities;
        } 
        public IDbContextTransaction BeginTransaction()
        {
            return _modelContext.Database.BeginTransaction();
        }

        public async Task ClearTracker()
        {
            var changedEntriesCopy =   _modelContext.ChangeTracker.Entries().ToList();                 

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Unchanged;

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _modelContext.Database.BeginTransactionAsync();
        }
        private void SetPropertyValue(object entity, string propertyName, object value)
        {
            PropertyInfo fieldPropertyInfo = entity.GetType().GetProperties()
                                .FirstOrDefault(f => f.Name == propertyName);
 
            if (fieldPropertyInfo != null)
            {
                var businessObjectPropValue = fieldPropertyInfo.GetValue(entity, null);


                if (fieldPropertyInfo != null)
                {
                    fieldPropertyInfo.SetValue(entity, value, null);

                }
            }

        }
        private void SetAuditableEntity()
        {

            foreach (var entry in _modelContext.ChangeTracker.Entries())
            {
                SetPropertyValue(entry.Entity, "Modificationuser", _currentUserService.UserId);
                SetPropertyValue(entry.Entity, "Modificationdate", _dateTime.Now);

                //Ticket 273 - Application Timeout Issue: When a user is actively working, the application session is getting timed out automatically
                if (entry.Metadata.GetTableName() == "REFRESHTOKEN" && entry.State != EntityState.Added) break;
                

                switch (entry.State)
                {
                    case EntityState.Added:
                        SetPropertyValue(entry.Entity, "Creationuser", (entry.Metadata.GetTableName().ToLower().Trim() == "audithistory") ? 1:_currentUserService.UserId);
                        SetPropertyValue(entry.Entity, "Modificationuser", (entry.Metadata.GetTableName().ToLower().Trim() == "audithistory") ? 1 : _currentUserService.UserId);
                        SetPropertyValue(entry.Entity, "Creationdate", _dateTime.Now);
                        break;
                    case EntityState.Modified:
                        NoEditingFields(entry);
                        GenerateAuditEntry(entry);
                        break;
                    case EntityState.Deleted:

                        SetPropertyValue(entry.Entity, "Deleted", true);
                        SetPropertyValue(entry.Entity, "Deletiondate", _dateTime.Now);

                        #region//Ticket 741 - Unable to delete DC, getting internal server error
                        entry.State = EntityState.Unchanged;
                        GenerateAuditEntry(entry);
                        //entry.State = EntityState.Detached; 
                        #endregion
                        entry.State = EntityState.Modified;
                        NoEditingFields(entry);
                        
                        break;
                }



            }
 
        }
        private void NoEditingFields(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            entry.Property("Creationuser").IsModified = false;
            entry.Property("Creationdate").IsModified = false;
        }
        private void GenerateAuditEntry(EntityEntry entry)
        {
            var auditLog = new List<Auditlogs>();
            var AuditEntities = AuditTablesAndColumnsRepository.FindByCondition(x => x.Islogenabled == true).ToList();
            var currentValues = entry.CurrentValues;
            var primaryKeys = entry.Metadata.FindPrimaryKey().Properties;
            var primarykeyvalues = primaryKeys.Select(pk => entry.Property(pk.Name).CurrentValue).ToList();

            string primarykey = primarykeyvalues.Count == 1 ? primarykeyvalues.First().ToString() : string.Join("_", primarykeyvalues);
            var isTableAvailabeInAudit = AuditEntities.FirstOrDefault(a => a.Entityname == entry.Entity.GetType().Name);

            if (isTableAvailabeInAudit != null)
            {

                foreach (var propertyName in currentValues.Properties)
                {
                    var currentValue = currentValues[propertyName];
                    var DbValue = entry.GetDatabaseValues().GetValue<object>(propertyName.Name.ToString());
                    var auditEntity = AuditEntities.FirstOrDefault(a => a.Entityname == entry.Entity.GetType().Name && a.Entityfield == propertyName.Name);
                    if (currentValue != null)
                    {
                        if (auditEntity != null && !Equals(currentValue, DbValue)
                            && !(string.IsNullOrEmpty(currentValue.ToString()) && DbValue == null))
                        {
                            auditLog.Add(new Auditlogs()
                            {
                                Entityname = entry.Entity.GetType().Name,
                                Entityfield = propertyName.Name,
                                Entityid = Convert.ToInt64(primarykey),
                                //Ticket 741 - Unable to delete DC, getting internal server error
                                //Entitystate = (entry.State.ToString() == "Unchanged") ? "Deleted" : entry.State.ToString(),
                                Oldvalue = Convert.ToString(DbValue),
                                Newvalue = Convert.ToString(currentValue),
                                Creationuser = _currentUserService.UserId,
                                Creationdate = _dateTime.Now,
                                Modificationdate = _dateTime.Now,
                                Modificationuser = _currentUserService.UserId,
                                //Ticket 741 - Unable to delete DC, getting internal server error
                                Entitystate = (entry.State.ToString() == "Unchanged") ? "Deleted" : entry.State.ToString(),
                            });

                        }
                    }

                }

            }
            if (auditLog.Any())
            {
                _modelContext.Auditlogs.AddRange(auditLog);
                _modelContext.SaveChanges();
            }
        }
        public async Task<dynamic> GetLinkedReferenceDetails(string currentEntityName, long PKeyId)
        {
            List<List<ReferenceTable>> returnLinkedReferenceDetails = new List<List<ReferenceTable>>();
            var returnQueryResultDto = new QueryResultDto<List<ReferenceTable>>()
            {
            };

            currentEntityName = "OracleModels.DBModels." + currentEntityName;

            var currentEntityType = _modelContext.Model.FindEntityType(currentEntityName);
            if (currentEntityType == null)
            {
                Console.WriteLine($"Entity {currentEntityName} not found in the model.");
                returnQueryResultDto.Items = null;
                return returnQueryResultDto;
            }
            
            var currentEntityRelationTables = _modelContext.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => fk.PrincipalEntityType == currentEntityType);

            foreach (var foreignKey in currentEntityRelationTables)
            {

                var dependentFkeyEntityType = foreignKey.DeclaringEntityType;
                var principleEntityType = foreignKey.PrincipalEntityType;
               
                foreach (var fkProperty in foreignKey.Properties)
                {                    
                    if ((dependentFkeyEntityType.Name  != "OracleModels.DBModels.Dcflifecycle") && 
                        !(dependentFkeyEntityType.Name == "OracleModels.DBModels.Softwarebuildcompatibility" 
                        && foreignKey.Properties.FirstOrDefault()?.Name == "Majorsoftwarebuildid") )
                        returnLinkedReferenceDetails.Add(GetReferenceFilteredRecords(principleEntityType.ClrType.Name,  dependentFkeyEntityType.ClrType, foreignKey.Properties.FirstOrDefault()?.Name, PKeyId));
                    
                }


            }

            returnQueryResultDto.Items = returnLinkedReferenceDetails;
            return returnQueryResultDto;
        }

        private List<ReferenceTable> GetReferenceFilteredRecords(string ParentTableEntityName,Type refenceFKeyEntity, string foreignKeyPropertyName, object foreignKeyValue)
        {
            List<ReferenceTable> referenceTableList = new List<ReferenceTable>();
            try
            {
                #region -- Code lines is used to load Table records from DB
                var setMethod = typeof(DbContext).GetMethods().Where(m => m.Name == "Set" && m.IsGenericMethod && m.GetParameters().Length == 0)
            .FirstOrDefault()?.MakeGenericMethod(refenceFKeyEntity);

                if (setMethod == null) return null;

                // Convert Dbset to Iqueryable 
                var queryableFkeyRecordsSet = ((IQueryable<object>)setMethod.Invoke(_modelContext, null));

                #endregion
                // Create a parameter expression for the dependent entity
                var parameter = Expression.Parameter(refenceFKeyEntity, "e");

                #region Filter condition added
                // Create an expression for the foreign key property
                var property = Expression.Property(parameter, foreignKeyPropertyName);

                // Adjust the constant expression to match the property type, including handling nullable types
                var propertyType = property.Type;
                object typedForeignKeyValue;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    typedForeignKeyValue = Convert.ChangeType(foreignKeyValue, Nullable.GetUnderlyingType(propertyType));
                    typedForeignKeyValue = Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(Nullable.GetUnderlyingType(propertyType)), typedForeignKeyValue);
                }
                else
                {
                    typedForeignKeyValue = Convert.ChangeType(foreignKeyValue, propertyType);
                }

                var constant = Expression.Constant(typedForeignKeyValue, propertyType);

                // Create an equality expression
                var equality = Expression.Equal(property, constant);

                // Create a lambda expression for the where clause
                var lambda = Expression.Lambda(equality, parameter);

                #region deleted column Check
                //Expression condition = null;
                //condition = condition == null ? equality : Expression.AndAlso(condition, equality);


                //var deletedProperty = Expression.Property(parameter, "Deleted");              

                //var pe = Expression.Parameter(typeof(bool), "x");
                //var left = Expression.Property(pe, "Deleted" );
                //var right = Expression.Constant(false, typeof(bool?));
                //var predicateBody = Expression.Equal(left, right);

                //lambda = Expression.Lambda(equality, parameter);
                // condition = condition == null ? equality : Expression.AndAlso(condition, equality);

                //var deletedProperty = Expression.Property(parameter, "Deleted");
                //propertyType = deletedProperty.Type;
                //typedForeignKeyValue = 0;


                //if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                //{
                //    typedForeignKeyValue = Convert.ChangeType(typedForeignKeyValue, Nullable.GetUnderlyingType(propertyType));
                //    typedForeignKeyValue = Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(Nullable.GetUnderlyingType(propertyType)), typedForeignKeyValue);
                //}
                //else
                //{
                //    typedForeignKeyValue = Convert.ChangeType((short)typedForeignKeyValue, propertyType);
                //}

                //constant = Expression.Constant(typedForeignKeyValue, propertyType);// Expression.Constant(1, propertyType);
                //equality = Expression.Equal(deletedProperty, constant);
                //lambda = Expression.Lambda(equality, parameter);
                //condition = condition == null ? equality : Expression.AndAlso(condition, equality);
                #endregion deleted column Check

                #endregion
                // Get the 'Where' method info and make it generic
                var whereMethod = typeof(Queryable).GetMethods()
                    .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(refenceFKeyEntity);

                var filteredQuery = whereMethod.Invoke(null, new object[] { queryableFkeyRecordsSet, lambda });

                // Execute the query and print the results
                var filteredRecords = ((IQueryable<object>)filteredQuery).ToList();

                var referenceTableEntityType = _modelContext.Model.FindEntityType(refenceFKeyEntity.FullName);

                string referenceTablePkey = referenceTableEntityType.FindPrimaryKey()?.Properties[0]?.Name;  //.GetReferencingForeignKeys().Select(x => x.Properties); //referenceTablePkey.Properties[0].Name

                foreach (var record in filteredRecords)
                {                    
                    long currentRefPKey = 0;
                    bool archivedValue = false;
                    
                    bool isDeleted = (bool)refenceFKeyEntity.GetProperties().Where(x => x.Name == "Deleted").Select(x
                        => x.GetValue(record)).FirstOrDefault();

                    bool transientDcAllowToDelete = (bool)refenceFKeyEntity.GetProperties().Where(x => x.Name == "Visibleflag")
                        .Select(x => x.GetValue(record)).DefaultIfEmpty(true).FirstOrDefault();


                    if (refenceFKeyEntity.GetProperties().Where(x => x.Name == "Archived").FirstOrDefault() != null)
                    {
                        currentRefPKey = Convert.ToInt64(refenceFKeyEntity.GetProperties().Where(x => x.Name == referenceTablePkey.ToString()).Select(x
                        => x.GetValue(record)).FirstOrDefault());

                        archivedValue = (bool)refenceFKeyEntity.GetProperties().Where(x => x.Name == "Archived").Select(x
                        => x.GetValue(record)).FirstOrDefault();
                    }

                    else
                    {
                        if (refenceFKeyEntity.Name == "Softwarebuildcompatibility" && ParentTableEntityName == "Majorsoftwarebuilds")
                            referenceTablePkey = "Majorsoftwarebuildid";

                        currentRefPKey = Convert.ToInt64(  refenceFKeyEntity.GetProperties().Where(x => x.Name == referenceTablePkey.ToString()).Select(x
                        => x.GetValue(record)).FirstOrDefault());
                    }
                        

                    if (!archivedValue && !isDeleted &&  transientDcAllowToDelete )
                        referenceTableList.Add(new ReferenceTable()
                        {
                            ReferenceTableName = refenceFKeyEntity.FullName,
                            ReferencePrimaryKey = currentRefPKey,//(long)  (dependentType.GetProperties().Where(x => x.Name == referenceTablePkey.ToString())).GetValue(record),
                            ReferenceKey = (long)foreignKeyValue
                        });

                    Console.WriteLine(record);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return referenceTableList;
        }

    }
    public class ReferenceTable
    {
        public string ReferenceTableName { get; set; }
        public long ReferencePrimaryKey { get; set; }

        public long ReferenceKey { get; set; }
    }
}


