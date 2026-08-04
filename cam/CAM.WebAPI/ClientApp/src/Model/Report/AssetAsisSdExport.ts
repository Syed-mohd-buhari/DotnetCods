import { QueryObject, DateFilter } from "../Common";
import { ReportSubBoundHwSwQueryGrid } from "./LcmExportReport";
import {
  ReportHardwareConfigQueryObjectGrid,
  ReportNetworkLevel2QueryObjectGrid,
} from "./ReportLcmExportModel";
import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  EOM_STATUS,
  QueryObjectGrid,
  ReportViewMode,
} from "../Common";

/**
 *
 * @export
 * @interface AssetAsisSdDto
 */
export interface AssetAsisSdDto {
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  vodafoneUniqueIdentifier?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  assetName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  assetDescriptionOrPurpose?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  businessOwner?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  supportOwner?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  supportTeam?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  supportTeamsPlaceInTheOrganisation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  assetFunction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  assetTypeTsr?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  deploymentOrLifeCycleStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  relatedriskidsFromRiskRegisters?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  regulatoryScope?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  countryWhereAssetisLocated?: string;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdDto
   */
  numberOfNodes?: number;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  geoLocation?: string;
  /**
   *
   * @type {Date}
   * @memberof AssetAsisSdDto
   */
  vendorEndOfMaintenanceDate?: Date;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  infrastructure?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  upstreamDependencies?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  downStreamDependencies?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  changesToTheAssetSinceDeployment?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  projectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  projectEndDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  cloudhostedAsset?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  cloudType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  cloudVendor?: string;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdDto
   */
  systemTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdDto
   */
  designComponentId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdDto
   */
  designComponentFamilyId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdDto
   */
  majorSoftwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdDto
   */
  majorHardwareBuildId?: number;

  /**
   *
   * @type {number}
   * @memberof AssetAsisSdDto
   */
  plannedActivityId?: number;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  equipmentName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  hostLocationWithinPhysicalLocation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  softwareVendorName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  firmwareVersionPatchLevel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  maintenanceSupportSupplier?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  dependantHardware?: string;

  // NEW PROPERTIES FROM JSON
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdDto
   */
  principalIds?: number;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisSdDto
   */
  deleteds?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisSdDto
   */
  orphan?: boolean;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  instanceType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  operatingSystemName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  operatingSystemswvVersion?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  operatingSystemswVersionPatchLevel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  systemNamedns?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  systemNameManagementIpaddress?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  systemNamenetbios?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  systemNameHostName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  hardwareVendorName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  maintenanceSupportSupplierSecond?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  dependantSystemSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  resilienceModel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  geographicSiteResilience?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  localSiteResilience?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  nameOfProductsDependantonAsset?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  technicalServiceNames?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  customer?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  privilegedAccessLogging?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  boardorModuleNamecomponentName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  exposedEdge?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  externallyFacingSystem?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  managementPlane?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  networkOverSightFunction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  pecn?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  pecs?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  securityCriticalFunction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  productImportance?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  critical?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  criticalityType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  partNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  descriptionofPlannedAction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  identifiedAction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  prodorLab?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  localmarketOwnership?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  budgetestimated?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  assuranceCall?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  commentonProjectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  serviceLevel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  lastPenTestRefNo?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  piData?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  encryptedPiData?: string;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdDto
   */
  recordclassifier?: number;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  vendorHardwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  maintenanceHardwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  dateAssetMovedtoliveStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  dateAssetDecommissioned?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  vendorSoftwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  maintenanceSoftwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  lastUpgradeDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  lastPenTestDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  model?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  firmwareVersion?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  boardorModuleTypeComponentSubType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  boardorModuleTypeComponentVersionNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  serialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  hardwareTypeofHardwareAsset?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  hwEndofSale?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  softwareProductType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  softwareProductVersion?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  applicationHostedonSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  uuidorSerialNumberofSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  swEndofSale?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  verticalEngineeringTeam?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  verticalSubDomain?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  platform?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  riskCluster?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  operationsContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  assetClass?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  operationsMaintenanceContract?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  vendorEndofMaintenanceDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  opsMaintenanceContractEndDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  incidentClass?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  occurrenceProbability?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  organizationorPersonGroup?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  typeofNetworkElement?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  application?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  physicalServerHostName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  physicalServerIpaddress?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  physicalServerSerialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  physicalServerHwModel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  physicalServerVendor?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  virtualServerHostedon?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  virtualServerManufacturer?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  virtualServerTypeofDevice?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  virtualMachineType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  virtualServerSerialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  virtualServerType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  osStartDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  osInstallationDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  osStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  softwareName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  version?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  release?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  language?: string;
}

/**
 *
 * @export
 * @interface AssetAsisSdDtoGrid
 */
export interface AssetAsisSdDtoGrid extends AssetAsisSdDto {
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDtoGrid
   */
  bundleBudget?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDtoGrid
   */
  bundleId?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDtoGrid
   */
  assetServiceFunctionality?: string;

  /**
   *
   * @type {Date}
   * @memberof AssetAsisSdDtoGrid
   */
  opsMaintenanceConractEnd?: Date;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDtoGrid
   */
  engRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDtoGrid
   */
  engRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDtoGrid
   */
  opsRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDtoGrid
   */
  opsRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDtoGrid
   */
  overallRiskEvaluation?: string;

  eomStatus: EOM_STATUS;

  /**
   *
   * @type {boolean}
   * @memberof AssetAsisSdDtoGrid
   */
  archived?: boolean;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfAssetAsisSdDtoGrid
 */
export interface QueryResultDtoOfAssetAsisSdDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfAssetAsisSdDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<AssetAsisSdDtoGrid>}
   * @memberof QueryResultDtoOfAssetAsisSdDtoGrid
   */
  items?: Array<AssetAsisSdDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface AssetAsisSdQueryObjectGrid extends AssetAsisSdQueryDto {
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisSdQueryDto
   */
  lastModifiedEndDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisSdQueryDto
   */
  lastModifiedStartDate?: DateFilter;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  changesToTheAssetSinceDeployment?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  tsrmodel?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  manufacturer?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  firmwareversion?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  swversion?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  datasourcetype?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdDto
   */
  datasourcename?: Array<string>;

  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdHardwareQueryDto
   */
  appSettingsId?: Array<number>;

  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdHardwareQueryDto
   */
  outputToLcmHardware?: Array<string>;
  ViewMode?: ReportViewMode;
  lcmExportDescription?: string;
}

/**
 *
 * @export
 * @interface AssetAsisSdHardwareQueryDto
 */
export interface AssetAsisSdHardwareQueryDto extends AssetAsisSdQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdHardwareQueryDto
   */
  lcmStatusEngHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdHardwareQueryDto
   */
  lcmStatusOpsHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdHardwareQueryDto
   */
  outputToLcmHardware?: Array<string>;
}
/**
 *
 * @export
 * @interface AssetAsisSdQueryDto
 */
export interface AssetAsisSdQueryDto extends QueryObject {
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  vodafoneUniqueIdentifier?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdQueryDto
   */
  name?: string;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisSdQueryDto
   */
  isHistorical?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisSdQueryDto
   */
  isDefault?: boolean;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  assetName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  designComponentIndex?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  assetDescriptionOrPurpose?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  businessOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  supportOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  supportTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  supportTeamsPlaceInTheOrganisation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  assetFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  assetTypeTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  deploymentOrLifeCycleStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  relatedriskidsFromRiskRegisters?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  regulatoryScope?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  countryWhereAssetisLocated?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdQueryDto
   */
  numberOfNodes?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  geoLocation?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisSdQueryDto
   */
  vendorEndOfMaintenanceDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  infrastructure?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdQueryDto
   */
  plannedAction?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  downStreamDependencies?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  plannedSoftwareRelease?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  projectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  projectEndDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  cloudhostedAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  cloudVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  bundleBudget?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  bundleId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  assetServiceFunctionality?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdQueryDto
   */
  platform?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdQueryDto
   */
  opsMaintenanceConractEnd?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  engRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  engRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  opsRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  opsRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  overallRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  upstreamDependencies?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  cloudType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  equipmentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  hostLocationWithinPhysicalLocation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  softwareVendorName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  firmwareVersionPatchLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  maintenanceSupportSupplier?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  dependantHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  componentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  componentResourceKey?: Array<string>;

  // NEW QUERY PROPERTIES FROM JSON
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdQueryDto
   */
  principalIds?: Array<number>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof AssetAsisSdQueryDto
   */
  deleteds?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof AssetAsisSdQueryDto
   */
  orphans?: Array<boolean>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisSdQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisSdQueryDto
   */
  lastModifiedValue?: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdQueryDto
   */
  assetAsisSdinfoid?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdQueryDto
   */
  nonTemsVertical?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  instanceType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  operatingSystemName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  operatingSystemswvVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  operatingSystemswVersionPatchLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  systemNamedns?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  systemNameManagementIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  systemNamenetbios?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  systemNameHostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  hardwareVendorName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  maintenanceSupportSupplierSecond?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  dependantSystemSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  resilienceModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  geographicSiteResilience?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  localSiteResilience?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  nameOfProductsDependantonAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  technicalServiceNames?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  customer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  privilegedAccessLogging?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  boardorModuleNamecomponentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  exposedEdge?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  externallyFacingSystem?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  managementPlane?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  networkOverSightFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  pecn?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  pecs?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  securityCriticalFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  productImportance?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  critical?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  criticalityType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  partNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  descriptionofPlannedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  identifiedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  prodorLab?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  localmarketOwnership?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  budgetestimated?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  assuranceCall?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  commentonProjectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  serviceLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  lastPenTestRefNo?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  piData?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  encryptedPiData?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdQueryDto
   */
  recordclassifier?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  vendorHardwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  maintenanceHardwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  dateAssetMovedtoliveStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  dateAssetDecommissioned?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  vendorSoftwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  maintenanceSoftwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  lastUpgradeDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  lastPenTestDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  model?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  firmwareVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  boardorModuleTypeComponentSubType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  boardorModuleTypeComponentVersionNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  serialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  hardwareTypeofHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  hwEndofSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  softwareProductType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  softwareProductVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  applicationHostedonSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  uuidorSerialNumberofSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  swEndofSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  verticalEngineeringTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  verticalSubDomain?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  riskCluster?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  operationsContactPoint?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  assetClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  operationsMaintenanceContract?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  vendorEndofMaintenanceDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  opsMaintenanceContractEndDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  incidentClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  occurrenceProbability?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  organizationorPersonGroup?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  typeofNetworkElement?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  application?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  physicalServerHostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  physicalServerIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  physicalServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  physicalServerHwModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  physicalServerVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  virtualServerHostedon?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  virtualServerManufacturer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  virtualServerTypeofDevice?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  virtualMachineType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  virtualServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  virtualServerType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  osStartDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  osInstallationDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  osStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  softwareName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  version?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  release?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdQueryDto
   */
  language?: Array<string>;
}

/**
 *
 * @export
 * @interface AssetAsisSdQueryAllDto
 */
export interface AssetAsisSdQueryAllDto {
  query?: QueryObjectGrid;
  /**
   *
   * @type {AssetAsisSdHardwareQueryDto}
   * @memberof AssetAsisSdQueryAllDto
   */
  queryPassThrough?: AssetAsisSdHardwareQueryDto;

  /**
   *
   * @type {ReportNetworkLevel2QueryObjectGrid}
   * @memberof AssetAsisSdQueryAllDto
   */
  querySoftwareLevelTwo?: ReportNetworkLevel2QueryObjectGrid;
  /**
   *
   * @type {ReportSubBoundHwSwQueryGrid}
   * @memberof AssetAsisSdQueryAllDto
   */
  querySubnetworkSoftware?: ReportSubBoundHwSwQueryGrid;
  /**
   *
   * @type {ReportSubBoundHwSwQueryGrid}
   * @memberof AssetAsisSdQueryAllDto
   */
  querySubnetworkHardware?: ReportSubBoundHwSwQueryGrid;
  /**
   *
   * @type {ReportHardwareConfigQueryObjectGrid}
   * @memberof AssetAsisSdQueryAllDto
   */
  queryHardwareConfiguration?: ReportHardwareConfigQueryObjectGrid;
  activeTab?: string;
  lcmExportDescription?: string;
}

export interface AssetAsisSdDownload {
  file: Blob | null;
}
export interface AssetAsisSdGrid {
  AssetAsisSdGridResult: QueryResultDtoOfAssetAsisSdDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const DOWNLOAD_ASSETASISSD = "DOWNLOAD_ASSETASISSD";
export const GET_GRID_ASSETASISSD = "GET_GRID_ASSETASISSD";
export const GET_FILTER_ASSETASISSD = "GET_FILTER_ASSETASISSD";
