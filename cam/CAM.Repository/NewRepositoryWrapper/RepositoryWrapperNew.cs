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
using CAM.Repository.NewRepositoryWrapper.AssetHardwareConfig;
using CAM.Repository.NewRepositoryWrapper.ComponentSoftware;
using CAM.Repository.NewRepositoryWrapper.Cross;
using CAM.Repository.NewRepositoryWrapper.Entity;
using CAM.Repository.NewRepositoryWrapper.ForeignIndex;
using CAM.Repository.NewRepositoryWrapper.LookUp;
using CAM.Repository.NewRepositoryWrapper.Mail;
using CAM.Repository.NewRepositoryWrapper.RBAC;
using CAM.Repository.NewRepositoryWrapper.Settings;
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
 
namespace CAM.Repository.NewRepositoryWrapper
{
    public class RepositoryWrapperNew : IRepositoryWrapper
    {
        
        private readonly ModelContextNew _modelContext;
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
                    _GlossaryItems = new CAM.Repository.NewRepositoryWrapper.Entity.GlossaryItemsRepository(_modelContext);
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
        private ILcmDeploymentStatusRepository _LcmDeploymentStatusRepository;
        public ILcmDeploymentStatusRepository LcmDeploymentStatusRepository
        {
            get
            {
                if (_LcmDeploymentStatusRepository == null)
                {
                    _LcmDeploymentStatusRepository = new LookUp.LcmDeploymentStatusRepository(_modelContext);
                }
                return _LcmDeploymentStatusRepository;
            }
        }
        private IProductNameRepository _ProductNameRepository;
        public IProductNameRepository ProductNameRepository
        {
            get
            {
                if (_ProductNameRepository == null)
                {
                    _ProductNameRepository = new LookUp.ProductNameRepository(_modelContext);
                }
                return _ProductNameRepository;
            }
        }
        public INfviSoftwareCompatibilityRepository _nfviSoftwareCompatibilityRepository;
        public INfviSoftwareCompatibilityRepository NfviSoftwareCompatibilityRepository
        {
            get
            {
                if (_nfviSoftwareCompatibilityRepository == null)
                {
                    _nfviSoftwareCompatibilityRepository = new CAM.Repository.NewRepositoryWrapper.Entity.NfviSoftwareCompatibilityRepository(_modelContext);
                }
                return _nfviSoftwareCompatibilityRepository;
            }
        }
        private IVodafoneNameRepository _VodafoneNameRepository;
        public IVodafoneNameRepository VodafoneNameRepository
        {
            get
            {
                if (_VodafoneNameRepository == null)
                {
                    _VodafoneNameRepository = new LookUp.VodafoneNameRepository(_modelContext);
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
                    _UsersLoggingLevelRepository = new LookUp.UsersLoggingLevelRepository(_modelContext);
                }
                return _UsersLoggingLevelRepository;
            }
        }
        private IDriverRepository _Driver;
        public IDriverRepository Driver
        {
            get
            {
                if (_Driver == null)
                {
                    _Driver = new LookUp.DriverRepository(_modelContext);
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

        #region  ExcelTempleteConfigration

        private IExcelTemplateConfigurationRepository _excelTemplateConfigurationRepository;
        public IExcelTemplateConfigurationRepository ExcelTemplateConfigurationRepository
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
                    _VolteKPI = new CAM.Repository.NewRepositoryWrapper.Entity.VolteKPIRepository(_modelContext);
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
                    _VolteKPIWorklog = new CAM.Repository.NewRepositoryWrapper.Entity.VolteKPIWorklogRepository(_modelContext);
                }
                return _VolteKPIWorklog;
            }
        }

        private ISoftwareBuildCompatibilityRepository _softwareBuildCompatibility;
        public ISoftwareBuildCompatibilityRepository SoftwareBuildCompatibility
        {
            get
            {
                if (_softwareBuildCompatibility == null)
                {
                    _softwareBuildCompatibility = new CAM.Repository.NewRepositoryWrapper.Entity.SoftwareBuildCompatibilityRepository(_modelContext);
                }
                return _softwareBuildCompatibility;
            }
        }

        private INetworkElementAsIsRepository _NetworkElementAsIs;
        public INetworkElementAsIsRepository NetworkElementAsIs
        {
            get
            {
                if (_NetworkElementAsIs == null)
                {
                    _NetworkElementAsIs = new CAM.Repository.NewRepositoryWrapper.Entity.NetworkElementAsIsRepository(_modelContext);
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
                    _PlanningRisk = new LookUp.PlanningRiskRepository(_modelContext);
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
                    _Benefit = new LookUp.BenefitRepository(_modelContext);
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
                    _ActivityDetails = new LookUp.ActivityDetailsRepository(_modelContext);
                }
                return _ActivityDetails;
            }
        }

        private ILocationRepository _Location;
        public ILocationRepository Location
        {
            get
            {
                if (_Location == null)
                {
                    _Location = new LookUp.LocationRepository(_modelContext);
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
                    _NetworkConstruct = new LookUp.NetworkConstructRepository(_modelContext);
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
                    _Environment = new LookUp.EnvironmentRepository(_modelContext);
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
                    _DeploymentStatus = new LookUp.DeploymentStatusRepository(_modelContext);
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
                    _DeploymentType = new LookUp.DeploymentTypeRepository(_modelContext);
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
                    _BudgetAvailability = new LookUp.BudgetAvailabilityRepository(_modelContext);
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
                    _NetworkElementAsPlanned = new Entity.NetworkElementAsPlannedRepository(_modelContext);
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
                    _NetworkElementAsPlannedEduSpoc = new LookUp.NetworkElementAsPlannedEduSpocRepository(_modelContext);
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
                    _NetworkElementAsPlannedSubDomainSpoc = new LookUp.NetworkElementAsPlannedSubDomainSpocRepository(_modelContext);
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

        public IPlannedActivityTypesRepository _PlannedActivityTypesRepository;
        public IPlannedActivityTypesRepository PlannedActivityTypesRepository
        {
            get
            {
                if (_PlannedActivityTypesRepository == null)
                {
                    _PlannedActivityTypesRepository = new PlannedActivityTypesRepository(_modelContext);
                }
                return _PlannedActivityTypesRepository;
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
                    _FullOrPartialResource = new LookUp.FullOrPartialResourceRepository(_modelContext);
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
                    _SupportedResource = new LookUp.SupportedResourceRepository(_modelContext);
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
                    _ReasonCheckboxResource = new LookUp.ReasonCheckboxResourceRepository(_modelContext);
                }
                return _ReasonCheckboxResource;
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
                    _componentSoftwareBuildBagRepository = new CAM.Repository.NewRepositoryWrapper.ComponentSoftware.ComponentSoftwareBuildBagRepository(_modelContext);
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
                    _componentSoftwareBuildsDesignContactRepository = new CAM.Repository.NewRepositoryWrapper.ComponentSoftware.ComponentSoftwareBuildsDesignContactRepository(_modelContext);
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
                    _BuildBagRepository = new CAM.Repository.NewRepositoryWrapper.ComponentSoftware.BuildBagRepository(_modelContext);
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
                    _componentSoftwareBuildRepository = new CAM.Repository.NewRepositoryWrapper.ComponentSoftware.ComponentSoftwareBuildRepository(_modelContext);
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
                    _networkElementClusterAsPlannedRepository = new CAM.Repository.NewRepositoryWrapper.CluserLevelPA.NetworkElementClusterAsPlannedRepository(_modelContext);
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
                    _clusterUpGradeStatusRepository = new CAM.Repository.NewRepositoryWrapper.CluserLevelPA.ClusterUpGradeStatusRepository(_modelContext);
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
                    _infraClusterAsPlannedRepository = new CAM.Repository.NewRepositoryWrapper.CluserLevelPA.InfraClusterAsPlannedRepository(_modelContext);
                }
                return _infraClusterAsPlannedRepository;
            }
        }

        #endregion
        #region RBAC 


        private IAspNetUserRolePermissionsRepository _AspNetUserRolePermissionsRepository;
        public IAspNetUserRolePermissionsRepository AspNetUserRolePermissionsRepository
        {
            get
            {
                if (_AspNetUserRolePermissionsRepository == null)
                {
                    _AspNetUserRolePermissionsRepository = new CAM.Repository.NewRepositoryWrapper.RBAC.AspNetUserRolePermissionsRepository(_modelContext);
                }
                return _AspNetUserRolePermissionsRepository;
            }
        }

        private IAspNetModulesRepository _AspNetModulesRepository;
        public IAspNetModulesRepository AspNetModulesRepository
        {
            get
            {
                if (_AspNetModulesRepository == null)
                {
                    _AspNetModulesRepository = new CAM.Repository.NewRepositoryWrapper.RBAC.AspNetModulesRepository(_modelContext);
                }
                return _AspNetModulesRepository;
            }
        }
 
        #endregion
        private IRiskRepository _OperationalRisk;
        public IRiskRepository Risk
        {
            get
            {
                if (_OperationalRisk == null)
                {
                    _OperationalRisk = new LookUp.OperationalRiskRepository(_modelContext);
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
                    _LcmEngineeringEduSpoc = new LookUp.LcmEngineeringEduSpocRepository(_modelContext);
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
                    _LcmEngineeringSubDomainSpoc = new LookUp.LcmEngineeringSubDomainSpocRepository(_modelContext);
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
                    _SubDomainSpoc = new LookUp.SubDomainSpocRepository(_modelContext);
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
                    _OperationalContracts = new LookUp.OperationalContractsRepository(_modelContext);
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

        #region Vbom

        private IClusterNameRepository _clusterNameRepository;

        public IClusterNameRepository ClusterNameRepository
        {
            get
            {
                if (_clusterNameRepository == null)
                {
                    _clusterNameRepository = new CAM.Repository.NewRepositoryWrapper.VBom.ClusterNameRepository(_modelContext);
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
                    _vmTypeNameRepository = new CAM.Repository.NewRepositoryWrapper.VBom.VmTypeNameRepository(_modelContext);
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
                    _VnfInfoRepository = new CAM.Repository.NewRepositoryWrapper.VBom.VnfInfoRepository(_modelContext);
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
                    _vnfNameRepository = new CAM.Repository.NewRepositoryWrapper.VBom.VnfNameRepository(_modelContext);
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
                    _vnfVmCapacityRepository = new CAM.Repository.NewRepositoryWrapper.VBom.VnfVmCapacityRepository(_modelContext);
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
                    _vnfClusterInfoRepository = new CAM.Repository.NewRepositoryWrapper.VBom.VnfClusterInfoRepository(_modelContext);
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
                    _interVmTypeRepository = new CAM.Repository.NewRepositoryWrapper.VBom.InterVmTypeRepository(_modelContext);
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
                    _intraVmTypeRepository = new CAM.Repository.NewRepositoryWrapper.VBom.IntraVmTypeRepository(_modelContext);
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
                    _vmWorkLoadTypeRepository = new CAM.Repository.NewRepositoryWrapper.VBom.VmWorkLoadTypeRepository(_modelContext);
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
                    _assetClusterRepository = new  AssetClusterRepository(_modelContext);
                }
                return _assetClusterRepository;
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
                    _NFVITransitionRepository = new LookUp.NFVITransitionRepository(_modelContext);
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
                    _PlannedActivityRepository = new CAM.Repository.NewRepositoryWrapper.Entity.PlannedActivityRepository(_modelContext);
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
                    _OpCo = new LookUp.OpCoRepository(_modelContext);
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
                    _HardwareSolutionResource = new LookUp.HardwareSolutionResourceRepository(_modelContext);
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
                    _ProductImportance = new LookUp.ProductImportanceRepository(_modelContext);
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
                    _ActivityStatus = new LookUp.ActivityStatusRepository(_modelContext);
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
                    _SystemType = new CAM.Repository.NewRepositoryWrapper.Entity.SystemTypeRepository(_modelContext);
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
                    _AssetClass = new LookUp.AssetClassRepository(_modelContext);
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
                    _DeliveryStatus = new LookUp.DeliveryStatusRepository(_modelContext);
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
                    _Lcmengineering = new CAM.Repository.NewRepositoryWrapper.Entity.LcmengineeringRepository(_modelContext);
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
                    _VNFTransition = new CAM.Repository.NewRepositoryWrapper.Entity.VNFTransitionRepository(_modelContext);
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
                    _DesignComponent = new CAM.Repository.NewRepositoryWrapper.Entity.DesignComponentRepository(_modelContext);
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
                    _DesignComponentFamily = new CAM.Repository.NewRepositoryWrapper.Entity.DesignComponentFamilyRepository(_modelContext);
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
                    _AssetType = new LookUp.AssetTypeRepository(_modelContext);
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
                    _AssetCategory = new LookUp.AssetCategoryRepository(_modelContext);
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
                    _MajorSoftwareBuild = new CAM.Repository.NewRepositoryWrapper.Entity.MajorSoftwareBuildRepository(_modelContext);
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
                    _MajorHardwareBuild = new CAM.Repository.NewRepositoryWrapper.Entity.MajorHardwareBuildRepository(_modelContext);
                }
                return _MajorHardwareBuild;
            }
        }

        public IBundleUpgradeInitiativeRepository BundleUpgradeInitiative
        {
            get
            {
                if (_BundleUpgradeInitiative == null)
                {
                    _BundleUpgradeInitiative = new LookUp.BundleUpgradeInitiativeRepository(_modelContext);
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
                    _VerticalResponsible = new LookUp.VerticalResponsibleRepository(_modelContext);
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
                    _OriginalEquipmentManufacture = new LookUp.OriginalEquipmentManufacturerRepository(_modelContext);
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
                    _VulnerabilityStatus = new LookUp.VulnerabilityStatusRepository(_modelContext);
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
                    _SubDomainResponsible = new LookUp.SubDomainResponsibleRepository(_modelContext);
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
                    _OperatingSystem = new LookUp.OperatingSystemRepository(_modelContext);
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
                    _PlannedActivityResourceRepository = new LookUp.PlannedActivityResourceRepository(_modelContext);
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
                    _PlanningActivityStatus = new LookUp.PlanningActivityStatusRepository(_modelContext);
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
                    _ResponsibilityPhase = new LookUp.ResponsibilityPhaseRepository(_modelContext);
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
                    _VNFDesignComponent = new LookUp.VNFDesignComponentRepository(_modelContext);
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
                    _EquipmentStatus = new LookUp.EquipmentStatusRepository(_modelContext);
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
                    _Platform = new LookUp.PlatformRepository(_modelContext);
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
                    _HardwareType = new LookUp.HardwareTypeRepository(_modelContext);
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
                    _SystemFunction = new LookUp.SystemFunctionRepository(_modelContext);
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
                    _NFVIStatus = new LookUp.NFVIStatusRepository(_modelContext);
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
                    _NFVIBundleID = new CAM.Repository.NewRepositoryWrapper.Entity.NFVIBundleIDRepository(_modelContext);
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
                    _BuildConstruction = new LookUp.BuildConstructionRepository(_modelContext);
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
                    _LcmDBExportUpdateHistory = new LookUp.RepositoryLcmDBExportUpdateHistory(_modelContext);
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
                    _EndOfSupportContract = new LookUp.EndOfSupportContractRepository(_modelContext);
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
                    _SubNetworkBoundaries = new LookUp.SubNetworkBoundaryRepository(_modelContext);
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
                    _SecurityTireZone = new LookUp.SecurityTireZoneRepository(_modelContext);
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
                    _SharingType = new LookUp.SharingTypeRepository(_modelContext);
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
                    _UserRepository = new CAM.Repository.NewRepositoryWrapper.Entity.UserRepository(_modelContext);
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
                    _ClaimsRepository = new CAM.Repository.NewRepositoryWrapper.Entity.ClaimsRepository(_modelContext);
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
                    _UserRoleRepository = new CAM.Repository.NewRepositoryWrapper.Entity.UserRoleRepository(_modelContext);
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
                    _TokenRepository = new CAM.Repository.NewRepositoryWrapper.Entity.TokenRepository(_modelContext);
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
                    _SupportedServiceRepository = new LookUp.SupportedServiceRepository(_modelContext);
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
                    _SubnetworkSupportedServiceRepository = new LookUp.SubnetworkSupportedServiceRepository(_modelContext);
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
                    _SubnetworkCustomerWheelsRepository = new LookUp.SubnetworkCustomerWheelsRepository(_modelContext);
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
                    _SubnetworkSystemFunctionsRepository = new LookUp.SubnetworkSystemFunctionsRepository(_modelContext);
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
                    _NetworkFunctionRepository = new LookUp.NetworkFunctionRepository(_modelContext);
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
                    _CustomerWheelRepository = new LookUp.CustomerWheelRepository(_modelContext);
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
                    _CriticalAssetTypeRepository = new LookUp.CriticalAssetTypeRepository(_modelContext);
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
                    _AuthenicationTypeRepository = new LookUp.AuthenicationTypeRepository(_modelContext);
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
                    _BusinessContinuityMethodRepository = new LookUp.BusinessContinuityMethodRepository(_modelContext);
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
                    _SiteResilienceRepository = new LookUp.SiteResilienceRepository(_modelContext);
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
                    _InstanceResilienceRepository = new LookUp.InstanceResilienceRepository(_modelContext);
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
                    _SecurityManagerRepository = new LookUp.SecurityManagerRepository(_modelContext);
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
                    _ThirdPartyAccessTypeRepository = new LookUp.ThirdPartyAccessTypeRepository(_modelContext);
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
                    _SWDeliveryLifeCycleRepository = new LookUp.SWDeliveryLifeCycleRepository(_modelContext);
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
                    _LicenseModelRepository = new LookUp.LicenseModelRepository(_modelContext);
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
                    _DesignAspectRepository = new CAM.Repository.NewRepositoryWrapper.Entity.DesignAspectRepository(_modelContext);
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
                    _locationDeploymentTypeRepository = new LookUp.LocationDeploymentTypeRepository(_modelContext);
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
                    _NetworkElement = new CAM.Repository.NewRepositoryWrapper.Entity.NetworkElementRepository(_modelContext);
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
                    _Identity = new CAM.Repository.NewRepositoryWrapper.Entity.IdentityRepository(_modelContext);
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
                    _HardwareConfiguration = new CAM.Repository.NewRepositoryWrapper.Entity.HardwareConfigurationRepository(_modelContext);
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
                    _softwareComponent = new CAM.Repository.NewRepositoryWrapper.Entity.SoftwareComponentRepository(_modelContext);
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
                    _SoftwareConfiguration = new CAM.Repository.NewRepositoryWrapper.Entity.SoftwareConfigurationRepository(_modelContext);
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
                    _Component = new CAM.NewRepositoryWrapper.LookUp.ComponentsRepository(_modelContext);
                }
                return _Component;
            }
        }

        private IFunctionRepository _function;
        public IFunctionRepository Function
        {
            get
            {
                if (_function == null)
                {
                    _function = new CAM.NewRepositoryWrapper.LookUp.FunctionRepository(_modelContext);
                }
                return _function;
            }
        }

        private IFunctionAreaRepository _functionArea;
        public IFunctionAreaRepository FunctionArea
        {
            get
            {
                if (_functionArea == null)
                {
                    _functionArea = new CAM.NewRepositoryWrapper.LookUp.FunctionAreaRepository(_modelContext);
                }
                return _functionArea;
            }
        }

        private ISubFunctionRepository _subFunction;
        public ISubFunctionRepository SubFunction
        {
            get
            {
                if (_subFunction == null)
                {
                    _subFunction = new CAM.NewRepositoryWrapper.LookUp.SubFunctionRepository(_modelContext);
                }
                return _subFunction;
            }
        }

        private ISubFunctionAreaRepository _subFunctionArea;
        public ISubFunctionAreaRepository SubFunctionArea
        {
            get
            {
                if (_subFunctionArea == null)
                {
                    _subFunctionArea = new CAM.NewRepositoryWrapper.LookUp.SubFunctionAreaRepository(_modelContext);
                }
                return _subFunctionArea;
            }
        }
        private IAuditHistoryRepository _auditHistory;
        public IAuditHistoryRepository AuditHistory
        {
            get
            {
                if (_auditHistory == null)
                {
                    _auditHistory = new CAM.Repository.NewRepositoryWrapper.Entity.AuditHistoryRepository(_modelContext);
                }
                return _auditHistory;
            }
        }

        private IDeliveryTrackingRepository _deliveryTrackingRepository;
        public IDeliveryTrackingRepository DeliveryTrackingRepository
        {
            get
            {
                if (_deliveryTrackingRepository == null)
                {
                    _deliveryTrackingRepository = new LookUp.DeliveryTrackingRepository(_modelContext);
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
                    _CategoryRepository = new LookUp.CategoryRepository(_modelContext);
                }
                return _CategoryRepository;
            }
        }

        private ILcmExportSettingRepository _LcmExportSettingRepository;
        public ILcmExportSettingRepository LcmExportSettingRepository
        {
            get
            {
                if (_LcmExportSettingRepository == null)
                {
                    _LcmExportSettingRepository = new LookUp.LcmExportSettingRepository(_modelContext);
                }
                return _LcmExportSettingRepository;
            }
        }

        private IClassRepository _ClassRepository;
        public IClassRepository ClassRepository
        {
            get
            {
                if (_ClassRepository == null)
                {
                    _ClassRepository = new LookUp.ClassRepository(_modelContext);
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
                    _TypeRepository = new LookUp.TypeRepository(_modelContext);
                }
                return _TypeRepository;
            }
        }
        private IIdentityAsIsRepository _IdentityRepository;
        public IIdentityAsIsRepository IdentityAsIsRepository
        {
            get
            {
                if (_IdentityRepository == null)
                {
                    _IdentityRepository = new LookUp.IdentityAsIsRepository(_modelContext);
                }
                return _IdentityRepository;
            }
        }

        private IResourceKeyMasterRepository _ResourceKeyMaster;
        public IResourceKeyMasterRepository ResourceKeyMaster
        {
            get
            {
                if (_ResourceKeyMaster == null)
                {
                    _ResourceKeyMaster = new CAM.Repository.NewRepositoryWrapper.Entity.ResourceKeyMasterRepository(_modelContext);
                }
                return _ResourceKeyMaster;
            }
        }

        private IResourceTypeRepository _ResourceType;
        public IResourceTypeRepository ResourceType
        {
            get
            {
                if (_ResourceType == null)
                {
                    _ResourceType = new LookUp.ResourceTypeRepository(_modelContext);
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
                    _DesignComponentFamilyLifeCycleRepository = new CAM.Repository.NewRepositoryWrapper.Entity.DesignComponentFamilyLifeCycleRepository(_modelContext);
                }
                return _DesignComponentFamilyLifeCycleRepository;
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
        
        // add round 9
        private ILcmAncillaryDataRepository _LcmAncillaryDataRepository;      
        public ILcmAncillaryDataRepository LcmAncillaryData
        {
            get
            {
                if (_LcmAncillaryDataRepository == null)
                {
                    _LcmAncillaryDataRepository = new LookUp.LcmAncillaryDataRepository(_modelContext);
                }
                return _LcmAncillaryDataRepository;
            }
        }

        private IRiskClusterRepository _RiskClusterRepository;
        public IRiskClusterRepository RiskClusterRepository
        {
            get
            {
                if(_RiskClusterRepository == null)
                {
                    _RiskClusterRepository = new LookUp.RiskClusterRepository(_modelContext);
                }
                return _RiskClusterRepository;
            }
        }

        public IRiskClusterVodafoneNamesRepository _RiskClusterVodafoneNamesRepository;
        public IRiskClusterVodafoneNamesRepository RiskClusterVodafoneNamesRepository
        {
            get
            {
                if (_RiskClusterVodafoneNamesRepository == null)
                {
                    _RiskClusterVodafoneNamesRepository = new LookUp.RiskClusterVodafoneNameRepository(_modelContext);
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
                    _reconciliationRepository =new ReconsiliationRepository(_modelContext);
                }
                return _reconciliationRepository;
            }
        }

        private IAuditTablesAndColumnsRepository _auditTablesAndColumnsRepository;
        public IAuditTablesAndColumnsRepository AuditTablesAndColumnsRepository
        {
            get
            {
                if(_auditTablesAndColumnsRepository == null)
                {
                    _auditTablesAndColumnsRepository = new Entity.AuditTablesAndColumnsRepository(_modelContext);
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
                    _auditLogRepository = new Entity.AuditLogRepositary(_modelContext);
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
                    _nodeParseHistoryRepository = new LookUp.NodeParseHistoryRepository(_modelContext);
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
                    _feedBackLoopAuditRepository = new Entity.FeedBackLoopAuditRepository(_modelContext);
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
                    _swConfigFunctionAreaRepository = new Entity.SWConfigFunctionAreaRepository(_modelContext);
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
                    _systemVerificationProblemRepository = new Entity.SystemVerificationProblemRepository(_modelContext);
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
                    _problemCategoryRepository = new Entity.ProblemCategoryRepository(_modelContext);
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
                    _severityRepository = new Entity.SeverityRepository(_modelContext);
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
                    _sWConfigSubFunctionRepository = new CAM.NewRepositoryWrapper.LookUp.SWConfigSubFunctionRepository(_modelContext);
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
                    _sWConfigSubFunctionAreasRepository = new CAM.NewRepositoryWrapper.LookUp.SWConfigSubFunctionAreasRepository(_modelContext);
                }
                return _sWConfigSubFunctionAreasRepository;
            }
        }

        private IOrganisationRepository _organisationRepository;
        public IOrganisationRepository OrganisationRepository
        {
            get
            {
                if(_organisationRepository == null)
                {
                    _organisationRepository = new CAM.Repository.NewRepositoryWrapper.Entity.OrganisationRepository(_modelContext);
                }
                return _organisationRepository;
            }
        }

        private IUserDefinedReportsLogsRepository _userDefinedReportsLogsRepository;
        public IUserDefinedReportsLogsRepository UserDefinedReportsLogsRepository
        {
            get
            {
                if (_userDefinedReportsLogsRepository == null)
                {
                    _userDefinedReportsLogsRepository = new CAM.Repository.NewRepositoryWrapper.Entity.UserDefinedReportsLogsRepository(_modelContext);
                }
                return _userDefinedReportsLogsRepository;
            }
        }

        public IPracticeRepository _practiceRepository;
        public IPracticeRepository PracticeRepository
        {
            get
            {
                if (_practiceRepository == null)
                {
                    _practiceRepository = new CAM.Repository.NewRepositoryWrapper.Entity.PracticeRepository(_modelContext);
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
                    _mainOrganisationRepository = new CAM.Repository.NewRepositoryWrapper.Entity.MainOrganisationRepository(_modelContext);
                }
                return _mainOrganisationRepository;
            }

        }

        public IMajorSwBuidlsDesignContactsRepository _majorSwBuidlsDesignContactsRepository;
        public IMajorSwBuidlsDesignContactsRepository MajorSwBuidlsDesignContactsRepository
        {
            get
            {
                if(_majorSwBuidlsDesignContactsRepository == null)
                {
                    _majorSwBuidlsDesignContactsRepository = new CAM.Repository.NewRepositoryWrapper.LookUp.MajorSwBuidlsDesignContactsRepository(_modelContext);
                }
                return _majorSwBuidlsDesignContactsRepository;
            }
        }

        public IMajorHwBuidlsDesignContactsRepository _majorHwBuidlsDesignContactsRepository;
        public IMajorHwBuidlsDesignContactsRepository MajorHwBuidlsDesignContactsRepository
        {
            get
            {
                if (_majorHwBuidlsDesignContactsRepository == null)
                {
                    _majorHwBuidlsDesignContactsRepository = new CAM.Repository.NewRepositoryWrapper.LookUp.MajorHwBuidlsDesignContactsRepository(_modelContext);
                }
                return _majorHwBuidlsDesignContactsRepository;
            }
        }

        public ITsrPassThroughRepository _tsrPassThroughRepository;
        public ITsrPassThroughRepository TsrPassThroughRepository
        {
            get
            {
                if (_tsrPassThroughRepository == null)
                {
                    _tsrPassThroughRepository = new CAM.Repository.NewRepositoryWrapper.Entity.TSRReportPassThroughRepository(_modelContext);
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
                    _nonTemsNweAsPlannedPassThroughRepository = new CAM.Repository.NewRepositoryWrapper.Entity.NonTemsNweAsPlannedPassThroughRepository(_modelContext);
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
                    _tsrLogRepository = new CAM.Repository.NewRepositoryWrapper.Entity.TsrLogRepository(_modelContext);
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
                    _appConfigurationSettingsRepository = new CAM.Repository.NewRepositoryWrapper.Entity.AppSettingsConfigurationRepository(_modelContext);
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
                    _appConfigurationRepository = new CAM.Repository.NewRepositoryWrapper.Entity.AppSettingsRepository(_modelContext);
                }
                return _appConfigurationRepository;
            }
        }

        public ISiteRepository _siteRepository;  
        public ISiteRepository SiteRepository
        {
            get
            {
                if(_siteRepository == null)
                {
                    _siteRepository = new CAM.Repository.NewRepositoryWrapper.LookUp.SiteRepository(_modelContext);
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
                    _systemNamesRepository = new CAM.Repository.NewRepositoryWrapper.LookUp.SystemNamesRepository(_modelContext);
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
        #region Exodus 
        private IDaMigrationStatusRepository _daMigrationStatusRepository;
        public IDaMigrationStatusRepository DaMigrationStatusRepository
        {
            get
            {
                if (_daMigrationStatusRepository == null)
                {
                    _daMigrationStatusRepository = new CAM.Repository.NewRepositoryWrapper.Entity.DaMigrationStatusRepository(_modelContext);
                }
                return _daMigrationStatusRepository;
            }
        }
        private IDaAssetMigrationRepository _daAssetMigrationRepository;
        public IDaAssetMigrationRepository DaAssetMigrationRepository
        {
            get
            {
                if (_daAssetMigrationRepository == null)
                {
                    _daAssetMigrationRepository = new CAM.Repository.NewRepositoryWrapper.Entity.DaAssetMigrationRepository(_modelContext);
                }
                return _daAssetMigrationRepository;
            }
        }

        private IVnfHardwareRepository _vnfHardwareRepository;
        public IVnfHardwareRepository VnfHardwareRepository
        {
            get
            {
                if (_vnfHardwareRepository == null)
                {
                    _vnfHardwareRepository = new CAM.Repository.NewRepositoryWrapper.Entity.VnfHardwareRepository(_modelContext);
                }
                return _vnfHardwareRepository;
            }
        }

        private IMajorHardwareBuildAsIsRepository _majorHardwareBuildAsIs;
        public IMajorHardwareBuildAsIsRepository MajorHardwareBuildAsIs
        {
            get
            {
                if (_majorHardwareBuildAsIs == null)
                {
                    _majorHardwareBuildAsIs = new CAM.Repository.NewRepositoryWrapper.Entity.MajorHardwareBuildAsIsRepository(_modelContext);
                }
                return _majorHardwareBuildAsIs;
            }
        }


        #endregion
        #region CBOM

        private ICnfCapacityRepository _cnfCapacityRepository;
        public ICnfCapacityRepository CnfCapacityRepository
        {
            get
            {
                if (_cnfCapacityRepository == null)
                {
                    _cnfCapacityRepository = new CBom.CnfCapacityRepository(_modelContext);
                }
                return _cnfCapacityRepository;
            }
        }

        private ICnfClusterInfoRepository _cnfClusterInfoRepository;
        public ICnfClusterInfoRepository CnfClusterInfoRepository
        {
            get
            {
                if (_cnfClusterInfoRepository == null)
                {
                    _cnfClusterInfoRepository = new CBom.CnfClusterInfoRepository(_modelContext);
                }
                return _cnfClusterInfoRepository;
            }
        }

      private ICnfClusterRepository _cnfClusterRepository;
        public ICnfClusterRepository CnfClusterRepository
        {
            get
            {
                if (_cnfClusterRepository == null)
                {
                    _cnfClusterRepository = new CBom.CnfClusterRepository(_modelContext);
                }
                return _cnfClusterRepository;
            }
        }
        private ICnfPodInfoRepository _cnfPodInfoRepository;
        public ICnfPodInfoRepository CnfPodInfoRepository
        {
            get
            {
                if (_cnfPodInfoRepository == null)
                {
                    _cnfPodInfoRepository = new CBom.CnfPodInfoRepository(_modelContext);
                }
                return _cnfPodInfoRepository;
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

        private ICnfNameRepository  _cnfNameRepository;
        public ICnfNameRepository CnfNameRepository
        {
            get
            {
                if (_cnfNameRepository == null)
                {
                    _cnfNameRepository = new CBom.CnfNameRepository(_modelContext);
                }
                return _cnfNameRepository;
            }
        }

        private IPodTypeInfoRepository _ipodTypeInfoRepository; 
        public IPodTypeInfoRepository PodTypeInfoRepository
        {
            get
            {
                if (_ipodTypeInfoRepository == null)
                {
                    _ipodTypeInfoRepository = new CBom.PodTypeInfoRepository(_modelContext);
                }
                return _ipodTypeInfoRepository;
            }
        }

        private IFunctionStandardNameRepository _functionStandardNameRepository;
        public IFunctionStandardNameRepository FunctionStandardNameRepository
        {
            get
            {
                if (_functionStandardNameRepository == null)
                {
                    _functionStandardNameRepository = new CBom.FunctionStandardNameRepository(_modelContext);
                }
                return _functionStandardNameRepository;
            }
        }
        #endregion

        #region // BGT 
        private IBudgetProjectTrackersRepository _budgetProjectTrackersRepository;
        public IBudgetProjectTrackersRepository BudgetProjectTrackersRepository
        {
            get
            {
                if (_budgetProjectTrackersRepository == null)
                {
                    _budgetProjectTrackersRepository = new CAM.Repository.NewRepositoryWrapper.Entity.BudgetProjectTrackersRepository(_modelContext);
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
                    _plannedActivityCategoryRepository = new LookUp.PlannedActivityCategoryRepository(_modelContext);
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
                    _programRepository = new LookUp.ProgramRepository(_modelContext);
                }
                return _programRepository;
            }
        }

        public IProjectPlanRepository _projectPlanRepository;
        public IProjectPlanRepository ProjectPlanRepository
        {
            get
            {
                if(_projectPlanRepository == null)
                {
                    _projectPlanRepository = new CAM.Repository.NewRepositoryWrapper.Entity.ProjectPlanRepository(_modelContext);
                }
                return _projectPlanRepository;
            }
        }

        public IDaPlannedActivityDcfRepository _daPlannedActivityDcfRepository;
        public IDaPlannedActivityDcfRepository DaPlannedActivityDcfRepository
        {
            get
            {
                if (_daPlannedActivityDcfRepository == null)
                {
                    _daPlannedActivityDcfRepository = new CAM.Repository.NewRepositoryWrapper.LookUp.DaPlannedActivityDcfRepository(_modelContext);
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
                    _projectPlanAuditRepository = new CAM.Repository.NewRepositoryWrapper.LookUp.ProjectPlanAuditRepository(_modelContext);
                }
                return _projectPlanAuditRepository;
            }
        }

        public IPassThroughRepository _passThroughRepository;
        public IPassThroughRepository PassThroughRepository             
        {
            get
            {
                if (_passThroughRepository == null)
                {
                    _passThroughRepository = new CAM.Repository.NewRepositoryWrapper.Entity.PassThroughRepository(_modelContext);
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
                    _nonTemsPassThroughConfigRepository = new CAM.Repository.NewRepositoryWrapper.Entity.NonTemsPassThroughConfigRepository(_modelContext);
                }
                return _nonTemsPassThroughConfigRepository;
            }
        }

        private ITemsFntReportRepository _temsFntReportRepository;

        public ITemsFntReportRepository TemsFntReportRepository
        {
            get
            {
                if (_temsFntReportRepository == null)
                {
                    _temsFntReportRepository = new CAM.Repository.NewRepositoryWrapper.Entity.TemsFntReportRepository(_modelContext);
                }
                return _temsFntReportRepository;
            }
        }

        public ISwPassThroughRepositoryLcm _swpassThroughRepositoryLcm;
        public ISwPassThroughRepositoryLcm SwPassThroughLcmRepository
        {
            get
            {
                if (_swpassThroughRepositoryLcm == null)
                {
                    _swpassThroughRepositoryLcm = new CAM.Repository.NewRepositoryWrapper.Entity.SwPassThroughLcmRepository(_modelContext);
                }
                return _swpassThroughRepositoryLcm;
            }
        }

        public IHwPassThroughRepositoryLcm _hwpassThroughRepositoryLcm;
        public IHwPassThroughRepositoryLcm HwPassThroughLcmRepository
        {
            get
            {
                if (_hwpassThroughRepositoryLcm == null)
                {
                    _hwpassThroughRepositoryLcm = new CAM.Repository.NewRepositoryWrapper.Entity.HwPassThroughLcmRepository(_modelContext);
                }
                return _hwpassThroughRepositoryLcm;
            }
        }

        private IAssetMapInfoRepository _assetMapInfoRepository;
        public IAssetMapInfoRepository AssetMapInfoRepository 
        {
            get
            {
                if (_assetMapInfoRepository == null)
                {
                    _assetMapInfoRepository = new CAM.Repository.NewRepositoryWrapper.OMC.AssetMapInfoRepository(_modelContext);
                }
                return _assetMapInfoRepository;
            }
        }

        private IAssetAsIsSdiInfoRepository _assetAsIsSdiInfoRepository;
        public IAssetAsIsSdiInfoRepository AssetAsIsSdiInfoRepository
        {
            get
            {
                if (_assetAsIsSdiInfoRepository == null)
                {
                    _assetAsIsSdiInfoRepository = new CAM.Repository.NewRepositoryWrapper.OMC.AssetAsIsSdiInfoRepository(_modelContext);
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
                    _assetAsIsHwAncillaryDataRepository = new CAM.Repository.NewRepositoryWrapper.OMC.AssetAsIsHwAncillaryDataRepository(_modelContext);
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
                    _assetAsIsSdiSwitchInfoRepository = new CAM.Repository.NewRepositoryWrapper.OMC.AssetAsIsSdiSwitchInfoRepository(_modelContext);
                }
                return _assetAsIsSdiSwitchInfoRepository;
            }
        }

        public IReportSchedulerRepository _reportSchedulerRepository;
        public IReportSchedulerRepository ReportSchedulerRepository
        {
            get
            {
                if (_reportSchedulerRepository == null)
                {
                    _reportSchedulerRepository = new CAM.Repository.NewRepositoryWrapper.Entity.ReportSchedulerRepository(_modelContext);
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
                return _servicePlanRepository;
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

        public IAspNetUserVerticalsRepository _aspNetUserVerticalsRepository;
        public IAspNetUserVerticalsRepository AspNetUserVerticalsRepository
        {
            get
            {
                if (_aspNetUserVerticalsRepository == null)
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
                if (_teamRepository == null)
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


        public RepositoryWrapperNew(ModelContextNew modelContext, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            //_repoContext = repoContext;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _modelContext = modelContext;
        }

        public RepositoryWrapperNew(ModelContextNew contextNew)
        {
            _modelContext = contextNew;
        }

        public void Save()
        {
            SetAuditableEntity();
            _modelContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            SetAuditableEntity();
            await _modelContext.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _modelContext.Database.BeginTransaction();
        }

        public async Task ClearTracker()
        {
            var changedEntriesCopy = _modelContext.ChangeTracker.Entries().ToList();

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
                        SetPropertyValue(entry.Entity, "Creationuser", _currentUserService.UserId);
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
            //foreach (var entry in _modelContext.ChangeTracker.Entries<AuditableEntity>())
            //{
            //    var now = _dateTime.Now;
            //    entry.Entity.ModificationDate = now;
            //    entry.Entity.ModificationUser = 14;  // _currentUserService.UserId;  //need to be modified after adding identity

            //    switch (entry.State)
            //    {
            //        case EntityState.Added:
            //            entry.Entity.CreationUser = 14; //_currentUserService.UserId;  //need to be modified after adding identity
            //            entry.Entity.CreationDate = now;
            //            break;
            //        case EntityState.Modified:
            //            NoEditingFields(entry);
            //            break;
            //        case EntityState.Deleted:
            //            entry.Entity.Deleted = true;
            //            entry.Entity.DeletionDate = now;
            //            entry.State = EntityState.Modified;
            //            NoEditingFields(entry);
            //            break;
            //    }
            //}
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
                            // Entitystate = (entry.State.ToString() == "Unchanged") ? "Deleted" : entry.State.ToString(),
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
            //var foreignKeyList = currentEntityType.GetReferencingForeignKeys().Select(x => x.Properties);
            var currentEntityRelationTables = _modelContext.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => fk.PrincipalEntityType == currentEntityType);

            foreach (var foreignKey in currentEntityRelationTables)
            {

                var dependentFkeyEntityType = foreignKey.DeclaringEntityType;
                var principleEntityType = foreignKey.PrincipalEntityType;
                //var dynamicTableName = dependentFkeyEntityType.GetDefaultTableName();// dependentEntityType.GetTableName();

                //var query = _modelContext.Set<dynamic>(currentEntityName).AsQueryable();

                foreach (var fkProperty in foreignKey.Properties)
                {
                    // get table records
                    //var dbSet = _modelContext.GetType().GetProperty(dynamicTableName).GetValue(_modelContext, null) as IQueryable;
                    //var dbSetList = dbSet.Cast<dynamic>().ToList();

                    //var foreignKeyValue = new object[] { PKeyId, true };
                    if ((dependentFkeyEntityType.Name != "OracleModels.DBModels.Dcflifecycle") &&
                        !(dependentFkeyEntityType.Name == "OracleModels.DBModels.Softwarebuildcompatibility"
                        && foreignKey.Properties.FirstOrDefault()?.Name == "Majorsoftwarebuildid"))
                        returnLinkedReferenceDetails.Add(GetReferenceFilteredRecords(principleEntityType.ClrType.Name, dependentFkeyEntityType.ClrType, foreignKey.Properties.FirstOrDefault()?.Name, PKeyId));

                }


            }

            returnQueryResultDto.Items = returnLinkedReferenceDetails;
            return returnQueryResultDto;
        }

        private List<ReferenceTable> GetReferenceFilteredRecords(string ParentTableEntityName, Type refenceFKeyEntity, string foreignKeyPropertyName, object foreignKeyValue)
        {
            List<ReferenceTable> referenceTableList = new List<ReferenceTable>();
            try
            {
                #region -- Code lines is used to load Table records from DB
                var setMethod = typeof(DbContext).GetMethods().Where(m => m.Name == "Set" && m.IsGenericMethod && m.GetParameters().Length == 0)
            .FirstOrDefault()?.MakeGenericMethod(refenceFKeyEntity);

                if (setMethod == null) return null;

                //var dependentFkeyTableRecordsSet = setMethod.Invoke(_modelContext, null);

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
                    // **** Get record Value 
                    //foreach (var item in dependentType.GetProperties().Where(x => x.Name == referenceTablePkey.ToString()))
                    //{
                    //    var value = item.GetValue(record);
                    //    Console.WriteLine($" : {value}");
                    //}
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

                        currentRefPKey = Convert.ToInt64(refenceFKeyEntity.GetProperties().Where(x => x.Name == referenceTablePkey.ToString()).Select(x
                        => x.GetValue(record)).FirstOrDefault());
                    }


                    if (!archivedValue && !isDeleted && transientDcAllowToDelete)
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

