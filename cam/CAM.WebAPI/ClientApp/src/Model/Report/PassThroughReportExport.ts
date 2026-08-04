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
 * @interface PassThroughReportDto
 */
export interface PassThroughReportDto {
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  vodafoneUniqueIdentifier?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  assetName?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  assetDescriptionOrPurpose?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  businessOwner?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  supportOwner?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  supportTeam?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  supportTeamsPlaceInTheOrganisation?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  assetFunction?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  assetTypeTsr?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  deploymentOrLifeCycleStatus?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  relatedriskidsFromRiskRegisters?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  regulatoryScope?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  countryWhereAssetisLocated?: string;
  /**
   *
   * @type {number}
   * @memberof PassThroughReportDto
   */
  numberOfNodes?: number;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  geoLocation?: string;
  /**
   *
   * @type {Date}
   * @memberof PassThroughReportDto
   */
  vendorEndOfMaintenanceDate?: Date;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  infrastructure?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  upstreamDependencies?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  downStreamDependencies?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  changesToTheAssetSinceDeployment?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  projectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  projectEndDate?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  cloudhostedAsset?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  cloudType?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  cloudVendor?: string;
  /**
   *
   * @type {number}
   * @memberof PassThroughReportDto
   */
  systemTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughReportDto
   */
  designComponentId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughReportDto
   */
  designComponentFamilyId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughReportDto
   */
  majorSoftwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughReportDto
   */
  majorHardwareBuildId?: number;

  /**
   *
   * @type {number}
   * @memberof PassThroughReportDto
   */
  plannedActivityId?: number;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  equipmentName?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  hostLocationWithinPhysicalLocation?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  softwareVendorName?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  firmwareVersionPatchLevel?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  maintenanceSupportSupplier?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  dependantHardware?: string;

  // NEW PROPERTIES FROM JSON
  /**
   *
   * @type {number}
   * @memberof PassThroughReportDto
   */
  principalIds?: number;
  /**
   *
   * @type {boolean}
   * @memberof PassThroughReportDto
   */
  deleteds?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PassThroughReportDto
   */
  orphan?: boolean;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  instanceType?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  operatingSystemName?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  operatingSystemswvVersion?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  operatingSystemswVersionPatchLevel?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  systemNamedns?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  systemNameManagementIpaddress?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  systemNamenetbios?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  systemNameHostName?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  hardwareVendorName?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  maintenanceSupportSupplierSecond?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  dependantSystemSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  resilienceModel?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  geographicSiteResilience?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  localSiteResilience?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  nameOfProductsDependantonAsset?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  technicalServiceNames?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  customer?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  privilegedAccessLogging?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  boardorModuleNamecomponentName?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  exposedEdge?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  externallyFacingSystem?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  managementPlane?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  networkOverSightFunction?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  pecn?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  pecs?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  securityCriticalFunction?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  productImportance?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  critical?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  criticalityType?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  partNumber?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  descriptionofPlannedAction?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  identifiedAction?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  prodorLab?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  localmarketOwnership?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  budgetestimated?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  assuranceCall?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  commentonProjectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  serviceLevel?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  lastPenTestRefNo?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  piData?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  encryptedPiData?: string;
  /**
   *
   * @type {number}
   * @memberof PassThroughReportDto
   */
  recordclassifier?: number;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  vendorHardwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  maintenanceHardwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  dateAssetMovedtoliveStatus?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  dateAssetDecommissioned?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  vendorSoftwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  maintenanceSoftwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  lastUpgradeDate?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  lastPenTestDate?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  model?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  firmwareVersion?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  boardorModuleTypeComponentSubType?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  boardorModuleTypeComponentVersionNumber?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  serialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  hardwareTypeofHardwareAsset?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  hwEndofSale?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  softwareProductType?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  softwareProductVersion?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  applicationHostedonSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  uuidorSerialNumberofSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  swEndofSale?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  verticalEngineeringTeam?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  verticalSubDomain?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  platform?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  riskCluster?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  operationsContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  assetClass?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  operationsMaintenanceContract?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  vendorEndofMaintenanceDate?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  opsMaintenanceContractEndDate?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  incidentClass?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  occurrenceProbability?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  organizationorPersonGroup?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  typeofNetworkElement?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  application?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  physicalServerHostName?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  physicalServerIpaddress?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  physicalServerSerialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  physicalServerHwModel?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  physicalServerVendor?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  virtualServerHostedon?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  virtualServerManufacturer?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  virtualServerTypeofDevice?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  virtualMachineType?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  virtualServerSerialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  virtualServerType?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  osStartDate?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  osInstallationDate?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  osStatus?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  softwareName?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  version?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  release?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  language?: string;
}

/**
 *
 * @export
 * @interface PassThroughReportDtoGrid
 */
export interface PassThroughReportDtoGrid extends PassThroughReportDto {
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDtoGrid
   */
  bundleBudget?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDtoGrid
   */
  bundleId?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDtoGrid
   */
  assetServiceFunctionality?: string;

  /**
   *
   * @type {Date}
   * @memberof PassThroughReportDtoGrid
   */
  opsMaintenanceConractEnd?: Date;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDtoGrid
   */
  engRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDtoGrid
   */
  engRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDtoGrid
   */
  opsRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDtoGrid
   */
  opsRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDtoGrid
   */
  overallRiskEvaluation?: string;

  eomStatus: EOM_STATUS;

  /**
   *
   * @type {boolean}
   * @memberof PassThroughReportDtoGrid
   */
  archived?: boolean;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfPassThroughReportDtoGrid
 */
export interface QueryResultDtoOfPassThroughReportDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfPassThroughReportDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<PassThroughReportDtoGrid>}
   * @memberof QueryResultDtoOfPassThroughReportDtoGrid
   */
  items?: Array<PassThroughReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface PassThroughReportQueryObjectGrid
  extends PassThroughReportQueryDto {
  /**
   *
   * @type {DateFilter}
   * @memberof PassThroughReportQueryDto
   */
  lastModifiedEndDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof PassThroughReportQueryDto
   */
  lastModifiedStartDate?: DateFilter;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportDto
   */
  changesToTheAssetSinceDeployment?: Array<string>;

  /**
   *
   * @type {Array<number>}
   * @memberof PassThroughReportHardwareQueryDto
   */
  appSettingsId?: Array<number>;

  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportHardwareQueryDto
   */
  outputToLcmHardware?: Array<string>;
  ViewMode?: ReportViewMode;
  lcmExportDescription?: string;
}

/**
 *
 * @export
 * @interface PassThroughReportHardwareQueryDto
 */
export interface PassThroughReportHardwareQueryDto
  extends PassThroughReportQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportHardwareQueryDto
   */
  lcmStatusEngHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportHardwareQueryDto
   */
  lcmStatusOpsHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportHardwareQueryDto
   */
  outputToLcmHardware?: Array<string>;
}
/**
 *
 * @export
 * @interface PassThroughReportQueryDto
 */
export interface PassThroughReportQueryDto extends QueryObject {
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  vodafoneUniqueIdentifier?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportQueryDto
   */
  name?: string;
  /**
   *
   * @type {boolean}
   * @memberof PassThroughReportQueryDto
   */
  isHistorical?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PassThroughReportQueryDto
   */
  isDefault?: boolean;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  assetName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  designComponentIndex?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  assetDescriptionOrPurpose?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  businessOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  supportOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  supportTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  supportTeamsPlaceInTheOrganisation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  assetFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  assetTypeTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  deploymentOrLifeCycleStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  relatedriskidsFromRiskRegisters?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  regulatoryScope?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  countryWhereAssetisLocated?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof PassThroughReportQueryDto
   */
  numberOfNodes?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  geoLocation?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof PassThroughReportQueryDto
   */
  vendorEndOfMaintenanceDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  infrastructure?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof PassThroughReportQueryDto
   */
  plannedAction?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  downStreamDependencies?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  plannedSoftwareRelease?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  projectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  projectEndDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  cloudhostedAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  cloudVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  bundleBudget?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  bundleId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  assetServiceFunctionality?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof PassThroughReportQueryDto
   */
  platform?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportQueryDto
   */
  opsMaintenanceConractEnd?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  engRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  engRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  opsRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  opsRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  overallRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  upstreamDependencies?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  cloudType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  equipmentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  hostLocationWithinPhysicalLocation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  softwareVendorName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  firmwareVersionPatchLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  maintenanceSupportSupplier?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  dependantHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  componentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  componentResourceKey?: Array<string>;

  // NEW QUERY PROPERTIES FROM JSON
  /**
   *
   * @type {Array<number>}
   * @memberof PassThroughReportQueryDto
   */
  principalIds?: Array<number>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof PassThroughReportQueryDto
   */
  deleteds?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof PassThroughReportQueryDto
   */
  orphans?: Array<boolean>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof PassThroughReportQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof PassThroughReportQueryDto
   */
  lastModifiedValue?: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof PassThroughReportQueryDto
   */
  passThroughIds?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PassThroughReportQueryDto
   */
  nonTemsVertical?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  instanceType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  operatingSystemName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  operatingSystemswvVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  operatingSystemswVersionPatchLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  systemNamedns?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  systemNameManagementIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  systemNamenetbios?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  systemNameHostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  hardwareVendorName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  maintenanceSupportSupplierSecond?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  dependantSystemSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  resilienceModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  geographicSiteResilience?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  localSiteResilience?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  nameOfProductsDependantonAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  technicalServiceNames?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  customer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  privilegedAccessLogging?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  boardorModuleNamecomponentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  exposedEdge?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  externallyFacingSystem?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  managementPlane?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  networkOverSightFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  pecn?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  pecs?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  securityCriticalFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  productImportance?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  critical?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  criticalityType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  partNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  descriptionofPlannedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  identifiedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  prodorLab?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  localmarketOwnership?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  budgetestimated?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  assuranceCall?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  commentonProjectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  serviceLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  lastPenTestRefNo?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  piData?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  encryptedPiData?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof PassThroughReportQueryDto
   */
  recordclassifier?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  vendorHardwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  maintenanceHardwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  dateAssetMovedtoliveStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  dateAssetDecommissioned?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  vendorSoftwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  maintenanceSoftwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  lastUpgradeDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  lastPenTestDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  model?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  firmwareVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  boardorModuleTypeComponentSubType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  boardorModuleTypeComponentVersionNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  serialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  hardwareTypeofHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  hwEndofSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  softwareProductType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  softwareProductVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  applicationHostedonSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  uuidorSerialNumberofSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  swEndofSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  verticalEngineeringTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  verticalSubDomain?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  riskCluster?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  operationsContactPoint?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  assetClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  operationsMaintenanceContract?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  vendorEndofMaintenanceDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  opsMaintenanceContractEndDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  incidentClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  occurrenceProbability?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  organizationorPersonGroup?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  typeofNetworkElement?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  application?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  physicalServerHostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  physicalServerIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  physicalServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  physicalServerHwModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  physicalServerVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  virtualServerHostedon?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  virtualServerManufacturer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  virtualServerTypeofDevice?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  virtualMachineType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  virtualServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  virtualServerType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  osStartDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  osInstallationDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  osStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  softwareName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  version?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  release?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  language?: Array<string>;
}

/**
 *
 * @export
 * @interface PassThroughReportQueryAllDto
 */
export interface PassThroughReportQueryAllDto {
  query?: QueryObjectGrid;
  /**
   *
   * @type {PassThroughReportHardwareQueryDto}
   * @memberof PassThroughReportQueryAllDto
   */
  queryPassThrough?: PassThroughReportHardwareQueryDto;

  /**
   *
   * @type {ReportNetworkLevel2QueryObjectGrid}
   * @memberof PassThroughReportQueryAllDto
   */
  querySoftwareLevelTwo?: ReportNetworkLevel2QueryObjectGrid;
  /**
   *
   * @type {ReportSubBoundHwSwQueryGrid}
   * @memberof PassThroughReportQueryAllDto
   */
  querySubnetworkSoftware?: ReportSubBoundHwSwQueryGrid;
  /**
   *
   * @type {ReportSubBoundHwSwQueryGrid}
   * @memberof PassThroughReportQueryAllDto
   */
  querySubnetworkHardware?: ReportSubBoundHwSwQueryGrid;
  /**
   *
   * @type {ReportHardwareConfigQueryObjectGrid}
   * @memberof PassThroughReportQueryAllDto
   */
  queryHardwareConfiguration?: ReportHardwareConfigQueryObjectGrid;
  activeTab?: string;
  lcmExportDescription?: string;
}

export interface PassThroughReportDownload {
  file: Blob | null;
}
export interface PassThroughReportGrid {
  PassThroughReportGridResult: QueryResultDtoOfPassThroughReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const DOWNLOAD_PASSTHROUGHREPORT = "DOWNLOAD_PASSTHROUGHREPORT";
export const GET_GRID_PASSTHROUGH_REPORT = "GET_GRID_PASSTHROUGH_REPORT";
export const GET_FILTER_PASSTHROUGH_REPORT = "GET_FILTER_PASSTHROUGH_REPORT";
