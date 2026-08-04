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
 * @interface AssetAsisHwAncillaryDto
 */
export interface AssetAsisHwAncillaryDto {
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  vodafoneUniqueIdentifier?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  assetName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  assetDescriptionOrPurpose?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  businessOwner?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  supportOwner?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  supportTeam?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  supportTeamsPlaceInTheOrganisation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  assetFunction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  assetTypeTsr?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  deploymentOrLifeCycleStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  relatedriskidsFromRiskRegisters?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  regulatoryScope?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  countryWhereAssetisLocated?: string;
  /**
   *
   * @type {number}
   * @memberof AssetAsisHwAncillaryDto
   */
  numberOfNodes?: number;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  geoLocation?: string;
  /**
   *
   * @type {Date}
   * @memberof AssetAsisHwAncillaryDto
   */
  vendorEndOfMaintenanceDate?: Date;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  infrastructure?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  upstreamDependencies?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  downStreamDependencies?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  changesToTheAssetSinceDeployment?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  projectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  projectEndDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  cloudhostedAsset?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  cloudType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  cloudVendor?: string;
  /**
   *
   * @type {number}
   * @memberof AssetAsisHwAncillaryDto
   */
  systemTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisHwAncillaryDto
   */
  designComponentId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisHwAncillaryDto
   */
  designComponentFamilyId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisHwAncillaryDto
   */
  majorSoftwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof AssetAsisHwAncillaryDto
   */
  majorHardwareBuildId?: number;

  /**
   *
   * @type {number}
   * @memberof AssetAsisHwAncillaryDto
   */
  plannedActivityId?: number;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  equipmentName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  hostLocationWithinPhysicalLocation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  softwareVendorName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  firmwareVersionPatchLevel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  maintenanceSupportSupplier?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  dependantHardware?: string;

  // NEW PROPERTIES FROM JSON
  /**
   *
   * @type {number}
   * @memberof AssetAsisHwAncillaryDto
   */
  principalIds?: number;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisHwAncillaryDto
   */
  deleteds?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisHwAncillaryDto
   */
  orphan?: boolean;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  instanceType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  operatingSystemName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  operatingSystemswvVersion?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  operatingSystemswVersionPatchLevel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  systemNamedns?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  systemNameManagementIpaddress?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  systemNamenetbios?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  systemNameHostName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  hardwareVendorName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  maintenanceSupportSupplierSecond?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  dependantSystemSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  resilienceModel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  geographicSiteResilience?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  localSiteResilience?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  nameOfProductsDependantonAsset?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  technicalServiceNames?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  customer?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  privilegedAccessLogging?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  boardorModuleNamecomponentName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  exposedEdge?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  externallyFacingSystem?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  managementPlane?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  networkOverSightFunction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  pecn?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  pecs?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  securityCriticalFunction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  productImportance?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  critical?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  criticalityType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  partNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  descriptionofPlannedAction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  identifiedAction?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  prodorLab?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  localmarketOwnership?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  budgetestimated?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  assuranceCall?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  commentonProjectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  serviceLevel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  lastPenTestRefNo?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  piData?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  encryptedPiData?: string;
  /**
   *
   * @type {number}
   * @memberof AssetAsisHwAncillaryDto
   */
  recordclassifier?: number;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  vendorHardwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  maintenanceHardwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  dateAssetMovedtoliveStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  dateAssetDecommissioned?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  vendorSoftwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  maintenanceSoftwareEndofSupportDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  lastUpgradeDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  lastPenTestDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  model?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  firmwareVersion?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  boardorModuleTypeComponentSubType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  boardorModuleTypeComponentVersionNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  serialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  hardwareTypeofHardwareAsset?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  hwEndofSale?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  softwareProductType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  softwareProductVersion?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  applicationHostedonSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  uuidorSerialNumberofSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  swEndofSale?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  verticalEngineeringTeam?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  verticalSubDomain?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  platform?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  riskCluster?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  operationsContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  assetClass?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  operationsMaintenanceContract?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  vendorEndofMaintenanceDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  opsMaintenanceContractEndDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  incidentClass?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  occurrenceProbability?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  organizationorPersonGroup?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  typeofNetworkElement?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  application?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  physicalServerHostName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  physicalServerIpaddress?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  physicalServerSerialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  physicalServerHwModel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  physicalServerVendor?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  virtualServerHostedon?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  virtualServerManufacturer?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  virtualServerTypeofDevice?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  virtualMachineType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  virtualServerSerialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  virtualServerType?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  osStartDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  osInstallationDate?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  osStatus?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  softwareName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  version?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  release?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  language?: string;
}

/**
 *
 * @export
 * @interface AssetAsisHwAncillaryDtoGrid
 */
export interface AssetAsisHwAncillaryDtoGrid extends AssetAsisHwAncillaryDto {
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDtoGrid
   */
  bundleBudget?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDtoGrid
   */
  bundleId?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDtoGrid
   */
  assetServiceFunctionality?: string;

  /**
   *
   * @type {Date}
   * @memberof AssetAsisHwAncillaryDtoGrid
   */
  opsMaintenanceConractEnd?: Date;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDtoGrid
   */
  engRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDtoGrid
   */
  engRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDtoGrid
   */
  opsRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDtoGrid
   */
  opsRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDtoGrid
   */
  overallRiskEvaluation?: string;

  eomStatus: EOM_STATUS;

  /**
   *
   * @type {boolean}
   * @memberof AssetAsisHwAncillaryDtoGrid
   */
  archived?: boolean;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfAssetAsisHwAncillaryDtoGrid
 */
export interface QueryResultDtoOfAssetAsisHwAncillaryDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfAssetAsisHwAncillaryDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<AssetAsisHwAncillaryDtoGrid>}
   * @memberof QueryResultDtoOfAssetAsisHwAncillaryDtoGrid
   */
  items?: Array<AssetAsisHwAncillaryDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface AssetAsisHwAncillaryQueryObjectGrid
  extends AssetAsisHwAncillaryQueryDto {
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  lastModifiedEndDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  lastModifiedStartDate?: DateFilter;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  changesToTheAssetSinceDeployment?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  tsrmodel?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  manufacturer?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  firmwareversion?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  site?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  swversion?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  datasourcetype?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  datasourcename?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  provider?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  chassisdetails?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  kubernetesnodestate?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  kubernetesnodestatusupdatetime?: DateFilter;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  kubernetesnoderesourcetype?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  kubernetesnodeos?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  kubeletversion?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  kubernetesnodename?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  managementip?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  kubernetesnodetype?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  processorsummarymodel?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  sku?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  biosversion?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  systemtype?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  partnumber?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  clustername?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  consumerrole?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  consumertype?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  consumer?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  providertype?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryDto
   */
  host?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryHardwareQueryDto
   */
  memorycapacity?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryHardwareQueryDto
   */
  networkelementasisid?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryHardwareQueryDto
   */
  ephemeralstoragecapacity?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryHardwareQueryDto
   */
  cpucapacity?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryHardwareQueryDto
   */
  assetasishwancillarydataid?: Array<number>;

  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryHardwareQueryDto
   */
  outputToLcmHardware?: Array<string>;
  ViewMode?: ReportViewMode;
  lcmExportDescription?: string;
}

/**
 *
 * @export
 * @interface AssetAsisHwAncillaryHardwareQueryDto
 */
export interface AssetAsisHwAncillaryHardwareQueryDto
  extends AssetAsisHwAncillaryQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryHardwareQueryDto
   */
  lcmStatusEngHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryHardwareQueryDto
   */
  lcmStatusOpsHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryHardwareQueryDto
   */
  outputToLcmHardware?: Array<string>;
}
/**
 *
 * @export
 * @interface AssetAsisHwAncillaryQueryDto
 */
export interface AssetAsisHwAncillaryQueryDto extends QueryObject {
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  vodafoneUniqueIdentifier?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  name?: string;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  isHistorical?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  isDefault?: boolean;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  assetName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  designComponentIndex?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  assetDescriptionOrPurpose?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  businessOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  supportOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  supportTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  supportTeamsPlaceInTheOrganisation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  assetFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  assetTypeTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  deploymentOrLifeCycleStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  relatedriskidsFromRiskRegisters?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  regulatoryScope?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  countryWhereAssetisLocated?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  numberOfNodes?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  geoLocation?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  vendorEndOfMaintenanceDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  infrastructure?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  plannedAction?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  downStreamDependencies?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  plannedSoftwareRelease?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  projectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  projectEndDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  cloudhostedAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  cloudVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  bundleBudget?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  bundleId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  assetServiceFunctionality?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  platform?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  opsMaintenanceConractEnd?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  engRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  engRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  opsRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  opsRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  overallRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  upstreamDependencies?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  cloudType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  equipmentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  hostLocationWithinPhysicalLocation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  softwareVendorName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  firmwareVersionPatchLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  maintenanceSupportSupplier?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  dependantHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  componentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  componentResourceKey?: Array<string>;

  // NEW QUERY PROPERTIES FROM JSON
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  principalIds?: Array<number>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  deleteds?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  orphans?: Array<boolean>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  lastModifiedValue?: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  assetAsisSdinfoid?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  nonTemsVertical?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  instanceType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  operatingSystemName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  operatingSystemswvVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  operatingSystemswVersionPatchLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  systemNamedns?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  systemNameManagementIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  systemNamenetbios?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  systemNameHostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  hardwareVendorName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  maintenanceSupportSupplierSecond?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  dependantSystemSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  resilienceModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  geographicSiteResilience?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  localSiteResilience?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  nameOfProductsDependantonAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  technicalServiceNames?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  customer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  privilegedAccessLogging?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  boardorModuleNamecomponentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  exposedEdge?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  externallyFacingSystem?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  managementPlane?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  networkOverSightFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  pecn?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  pecs?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  securityCriticalFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  productImportance?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  critical?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  criticalityType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  partNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  descriptionofPlannedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  identifiedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  prodorLab?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  localmarketOwnership?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  budgetestimated?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  assuranceCall?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  commentonProjectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  serviceLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  lastPenTestRefNo?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  piData?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  encryptedPiData?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  recordclassifier?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  vendorHardwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  maintenanceHardwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  dateAssetMovedtoliveStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  dateAssetDecommissioned?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  vendorSoftwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  maintenanceSoftwareEndofSupportDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  lastUpgradeDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  lastPenTestDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  model?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  firmwareVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  boardorModuleTypeComponentSubType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  boardorModuleTypeComponentVersionNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  serialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  hardwareTypeofHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  hwEndofSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  softwareProductType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  softwareProductVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  applicationHostedonSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  uuidorSerialNumberofSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  swEndofSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  verticalEngineeringTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  verticalSubDomain?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  riskCluster?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  operationsContactPoint?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  assetClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  operationsMaintenanceContract?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  vendorEndofMaintenanceDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  opsMaintenanceContractEndDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  incidentClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  occurrenceProbability?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  organizationorPersonGroup?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  typeofNetworkElement?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  application?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  physicalServerHostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  physicalServerIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  physicalServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  physicalServerHwModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  physicalServerVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  virtualServerHostedon?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  virtualServerManufacturer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  virtualServerTypeofDevice?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  virtualMachineType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  virtualServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  virtualServerType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  osStartDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  osInstallationDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  osStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  softwareName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  version?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  release?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssetAsisHwAncillaryQueryDto
   */
  language?: Array<string>;
}

/**
 *
 * @export
 * @interface AssetAsisHwAncillaryQueryAllDto
 */
export interface AssetAsisHwAncillaryQueryAllDto {
  query?: QueryObjectGrid;
  /**
   *
   * @type {AssetAsisHwAncillaryHardwareQueryDto}
   * @memberof AssetAsisHwAncillaryQueryAllDto
   */
  queryPassThrough?: AssetAsisHwAncillaryHardwareQueryDto;

  /**
   *
   * @type {ReportNetworkLevel2QueryObjectGrid}
   * @memberof AssetAsisHwAncillaryQueryAllDto
   */
  querySoftwareLevelTwo?: ReportNetworkLevel2QueryObjectGrid;
  /**
   *
   * @type {ReportSubBoundHwSwQueryGrid}
   * @memberof AssetAsisHwAncillaryQueryAllDto
   */
  querySubnetworkSoftware?: ReportSubBoundHwSwQueryGrid;
  /**
   *
   * @type {ReportSubBoundHwSwQueryGrid}
   * @memberof AssetAsisHwAncillaryQueryAllDto
   */
  querySubnetworkHardware?: ReportSubBoundHwSwQueryGrid;
  /**
   *
   * @type {ReportHardwareConfigQueryObjectGrid}
   * @memberof AssetAsisHwAncillaryQueryAllDto
   */
  queryHardwareConfiguration?: ReportHardwareConfigQueryObjectGrid;
  activeTab?: string;
  lcmExportDescription?: string;
}

export interface AssetAsisHwAncillaryDownload {
  file: Blob | null;
}
export interface AssetAsisHwAncillaryGrid {
  AssetAsisHwAncillaryGridResult: QueryResultDtoOfAssetAsisHwAncillaryDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const DOWNLOAD_ASSETASISHWANCILLARY = "DOWNLOAD_ASSETASISHWANCILLARY";
export const GET_GRID_ASSETASISHWANCILLARY = "GET_GRID_ASSETASISHWANCILLARY";
export const GET_FILTER_ASSETASISHWANCILLARY =
  "GET_FILTER_ASSETASISHWANCILLARY";
