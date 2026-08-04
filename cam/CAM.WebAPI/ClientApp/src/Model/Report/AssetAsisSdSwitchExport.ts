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
 * @interface AssetAsisSdSwitchDto
 */
export interface AssetAsisSdSwitchDto {
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  vodafoneUniqueIdentifier?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  assetName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  assetDescriptionOrPurpose?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  businessOwner?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  supportOwner?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  supportTeam?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  supportTeamsPlaceInTheOrganisation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  assetFunction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  assetTypeTsr?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  deploymentOrLifeCycleStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  relatedriskidsFromRiskRegisters?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  regulatoryScope?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  countryWhereAssetisLocated?: string;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdSwitchDto
   */
  numberOfNodes?: number;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  geoLocation?: string;
  /**
   *
   * @type {Date}
   * @memberof AssetAsisSdSwitchDto
   */
  vendorEndOfMaintenanceDate?: Date;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  infrastructure?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  upstreamDependencies?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  downStreamDependencies?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  changesToTheAssetSinceDeployment?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  projectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  projectEndDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  cloudhostedAsset?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  cloudType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  cloudVendor?: string;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdSwitchDto
   */
  systemTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdSwitchDto
   */
  designComponentId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdSwitchDto
   */
  designComponentFamilyId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdSwitchDto
   */
  majorSoftwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdSwitchDto
   */
  majorHardwareBuildId?: number;

  /**
   *
   * @type {number}
   * @memberof AssetAsisSdSwitchDto
   */
  plannedActivityId?: number;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  equipmentName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  hostLocationWithinPhysicalLocation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  softwareVendorName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  firmwareVersionPatchLevel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  maintenanceSupportSupplier?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  dependantHardware?: string;

  // NEW PROPERTIES FROM JSON
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdSwitchDto
   */
  principalIds?: number;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisSdSwitchDto
   */
  deleteds?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisSdSwitchDto
   */
  orphan?: boolean;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  instanceType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  operatingSystemName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  operatingSystemswvVersion?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  operatingSystemswVersionPatchLevel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  systemNamedns?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  systemNameManagementIpaddress?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  systemNamenetbios?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  systemNameHostName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  hardwareVendorName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  maintenanceSupportSupplierSecond?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  dependantSystemSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  resilienceModel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  geographicSiteResilience?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  localSiteResilience?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  nameOfProductsDependantonAsset?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  technicalServiceNames?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  customer?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  privilegedAccessLogging?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  boardorModuleNamecomponentName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  exposedEdge?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  externallyFacingSystem?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  managementPlane?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  networkOverSightFunction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  pecn?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  pecs?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  securityCriticalFunction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  productImportance?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  critical?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  criticalityType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  partNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  descriptionofPlannedAction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  identifiedAction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  prodorLab?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  localmarketOwnership?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  budgetestimated?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  assuranceCall?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  commentonProjectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  serviceLevel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  lastPenTestRefNo?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  piData?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  encryptedPiData?: string;
  /**
   *
   * @type {number}
   * @memberof AssetAsisSdSwitchDto
   */
  recordclassifier?: number;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  vendorHardwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  maintenanceHardwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  dateAssetMovedtoliveStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  dateAssetDecommissioned?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  vendorSoftwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  maintenanceSoftwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  lastUpgradeDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  lastPenTestDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  model?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  firmwareVersion?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  boardorModuleTypeComponentSubType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  boardorModuleTypeComponentVersionNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  serialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  hardwareTypeofHardwareAsset?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  hwEndofSale?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  softwareProductType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  softwareProductVersion?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  applicationHostedonSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  uuidorSerialNumberofSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  swEndofSale?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  verticalEngineeringTeam?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  verticalSubDomain?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  platform?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  riskCluster?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  operationsContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  assetClass?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  operationsMaintenanceContract?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  vendorEndofMaintenanceDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  opsMaintenanceContractEndDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  incidentClass?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  occurrenceProbability?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  organizationorPersonGroup?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  typeofNetworkElement?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  application?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  physicalServerHostName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  physicalServerIpaddress?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  physicalServerSerialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  physicalServerHwModel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  physicalServerVendor?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  virtualServerHostedon?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  virtualServerManufacturer?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  virtualServerTypeofDevice?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  virtualMachineType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  virtualServerSerialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  virtualServerType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  osStartDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  osInstallationDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  osStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  softwareName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  version?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  release?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  language?: string;
}

/**
 *
 * @export
 * @interface AssetAsisSdSwitchDtoGrid
 */
export interface AssetAsisSdSwitchDtoGrid extends AssetAsisSdSwitchDto {
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDtoGrid
   */
  bundleBudget?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDtoGrid
   */
  bundleId?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDtoGrid
   */
  assetServiceFunctionality?: string;

  /**
   *
   * @type {Date}
   * @memberof AssetAsisSdSwitchDtoGrid
   */
  opsMaintenanceConractEnd?: Date;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDtoGrid
   */
  engRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDtoGrid
   */
  engRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDtoGrid
   */
  opsRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDtoGrid
   */
  opsRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDtoGrid
   */
  overallRiskEvaluation?: string;

  eomStatus: EOM_STATUS;

  /**
   *
   * @type {boolean}
   * @memberof AssetAsisSdSwitchDtoGrid
   */
  archived?: boolean;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfAssetAsisSdSwitchDtoGrid
 */
export interface QueryResultDtoOfAssetAsisSdSwitchDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfAssetAsisSdSwitchDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<AssetAsisSdSwitchDtoGrid>}
   * @memberof QueryResultDtoOfAssetAsisSdSwitchDtoGrid
   */
  items?: Array<AssetAsisSdSwitchDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface AssetAsisSdSwitchQueryObjectGrid
  extends AssetAsisSdSwitchQueryDto {
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  lastModifiedEndDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  lastModifiedStartDate?: DateFilter;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  changesToTheAssetSinceDeployment?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  tsrmodel?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  manufacturer?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  firmwareversion?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  swversion?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  datasourcetype?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  datasourcename?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchswversion?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchipaddress?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchmodel?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchmanufacturer?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchnetwork?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchopsstate?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchserialnumber?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchlabel?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchrack?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchrole?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchuniqueid?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchadminstate?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchid?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchDto
   */
  switchname?: Array<string>;

  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdSwitchHardwareQueryDto
   */
  appSettingsId?: Array<number>;

  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchHardwareQueryDto
   */
  outputToLcmHardware?: Array<string>;
  ViewMode?: ReportViewMode;
  lcmExportDescription?: string;
}

/**
 *
 * @export
 * @interface AssetAsisSdSwitchHardwareQueryDto
 */
export interface AssetAsisSdSwitchHardwareQueryDto
  extends AssetAsisSdSwitchQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchHardwareQueryDto
   */
  lcmStatusEngHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchHardwareQueryDto
   */
  lcmStatusOpsHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchHardwareQueryDto
   */
  outputToLcmHardware?: Array<string>;
}
/**
 *
 * @export
 * @interface AssetAsisSdSwitchQueryDto
 */
export interface AssetAsisSdSwitchQueryDto extends QueryObject {
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  vodafoneUniqueIdentifier?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  name?: string;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  isHistorical?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  isDefault?: boolean;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  assetName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  designComponentIndex?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  assetDescriptionOrPurpose?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  businessOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  supportOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  supportTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  supportTeamsPlaceInTheOrganisation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  assetFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  assetTypeTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  deploymentOrLifeCycleStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  relatedriskidsFromRiskRegisters?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  regulatoryScope?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  countryWhereAssetisLocated?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  numberOfNodes?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  geoLocation?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  vendorEndOfMaintenanceDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  infrastructure?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  plannedAction?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  downStreamDependencies?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  plannedSoftwareRelease?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  projectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  projectEndDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  cloudhostedAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  cloudVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  bundleBudget?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  bundleId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  assetServiceFunctionality?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  platform?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  opsMaintenanceConractEnd?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  engRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  engRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  opsRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  opsRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  overallRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  upstreamDependencies?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  cloudType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  equipmentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  hostLocationWithinPhysicalLocation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  softwareVendorName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  firmwareVersionPatchLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  maintenanceSupportSupplier?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  dependantHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  componentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  componentResourceKey?: Array<string>;

  // NEW QUERY PROPERTIES FROM JSON
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  principalIds?: Array<number>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  deleteds?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  orphans?: Array<boolean>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  lastModifiedValue?: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  assetasissdiswitchinfoid?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  nonTemsVertical?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  instanceType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  operatingSystemName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  operatingSystemswvVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  operatingSystemswVersionPatchLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  systemNamedns?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  systemNameManagementIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  systemNamenetbios?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  systemNameHostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  hardwareVendorName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  maintenanceSupportSupplierSecond?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  dependantSystemSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  resilienceModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  geographicSiteResilience?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  localSiteResilience?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  nameOfProductsDependantonAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  technicalServiceNames?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  customer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  privilegedAccessLogging?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  boardorModuleNamecomponentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  exposedEdge?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  externallyFacingSystem?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  managementPlane?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  networkOverSightFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  pecn?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  pecs?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  securityCriticalFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  productImportance?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  critical?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  criticalityType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  partNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  descriptionofPlannedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  identifiedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  prodorLab?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  localmarketOwnership?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  budgetestimated?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  assuranceCall?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  commentonProjectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  serviceLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  lastPenTestRefNo?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  piData?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  encryptedPiData?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  recordclassifier?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  vendorHardwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  maintenanceHardwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  dateAssetMovedtoliveStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  dateAssetDecommissioned?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  vendorSoftwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  maintenanceSoftwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  lastUpgradeDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  lastPenTestDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  model?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  firmwareVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  boardorModuleTypeComponentSubType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  boardorModuleTypeComponentVersionNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  serialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  hardwareTypeofHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  hwEndofSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  softwareProductType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  softwareProductVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  applicationHostedonSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  uuidorSerialNumberofSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  swEndofSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  verticalEngineeringTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  verticalSubDomain?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  riskCluster?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  operationsContactPoint?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  assetClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  operationsMaintenanceContract?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  vendorEndofMaintenanceDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  opsMaintenanceContractEndDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  incidentClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  occurrenceProbability?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  organizationorPersonGroup?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  typeofNetworkElement?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  application?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  physicalServerHostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  physicalServerIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  physicalServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  physicalServerHwModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  physicalServerVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  virtualServerHostedon?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  virtualServerManufacturer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  virtualServerTypeofDevice?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  virtualMachineType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  virtualServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  virtualServerType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  osStartDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  osInstallationDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  osStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  softwareName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  version?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  release?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisSdSwitchQueryDto
   */
  language?: Array<string>;
}

/**
 *
 * @export
 * @interface AssetAsisSdSwitchQueryAllDto
 */
export interface AssetAsisSdSwitchQueryAllDto {
  query?: QueryObjectGrid;
  /**
   *
   * @type {AssetAsisSdSwitchHardwareQueryDto}
   * @memberof AssetAsisSdSwitchQueryAllDto
   */
  queryPassThrough?: AssetAsisSdSwitchHardwareQueryDto;

  /**
   *
   * @type {ReportNetworkLevel2QueryObjectGrid}
   * @memberof AssetAsisSdSwitchQueryAllDto
   */
  querySoftwareLevelTwo?: ReportNetworkLevel2QueryObjectGrid;
  /**
   *
   * @type {ReportSubBoundHwSwQueryGrid}
   * @memberof AssetAsisSdSwitchQueryAllDto
   */
  querySubnetworkSoftware?: ReportSubBoundHwSwQueryGrid;
  /**
   *
   * @type {ReportSubBoundHwSwQueryGrid}
   * @memberof AssetAsisSdSwitchQueryAllDto
   */
  querySubnetworkHardware?: ReportSubBoundHwSwQueryGrid;
  /**
   *
   * @type {ReportHardwareConfigQueryObjectGrid}
   * @memberof AssetAsisSdSwitchQueryAllDto
   */
  queryHardwareConfiguration?: ReportHardwareConfigQueryObjectGrid;
  activeTab?: string;
  lcmExportDescription?: string;
}

export interface AssetAsisSdSwitchDownload {
  file: Blob | null;
}
export interface AssetAsisSdSwitchGrid {
  AssetAsisSdSwitchGridResult: QueryResultDtoOfAssetAsisSdSwitchDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const DOWNLOAD_ASSETASISSDSWITCH = "DOWNLOAD_ASSETASISSDSWITCH";
export const GET_GRID_ASSETASISSDSWITCH = "GET_GRID_ASSETASISSDSWITCH";
export const GET_FILTER_ASSETASISSDSWITCH = "GET_FILTER_ASSETASISSDSWITCH";
