import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  GridDtoBase,
  RenderDetail,
  QueryObject,
  DateFilter,
  CustomGridRender,
  QueryObjectGrid,
} from "./Common";
import { ResultDto } from "./CommonModels";

/**
 *
 * @export
 * @interface TSRReportQueryDto
 */
export interface TSRReportQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof TSRReportQueryDto
   */
  tsrPassThroughId: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assetDescriptionOrPurpose: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assetFunction: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assetId: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assetName: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assetTypeTsr: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assuranceCall: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  boardOrModuleNameComponentName: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  boardOrModuleTypeComponentSubtype: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  boardOrModuleTypeComponentVersionNumber: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  budgetEstimatedTsr: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  bundleBudgetTsr: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  businessOwner: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  changesToTheassetSinceDeployment: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  cloudHostedAsset: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  cloudType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  cloudVendor: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  commentOnProjectStatusTsr: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  countryWhereAssetIsLocated: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  critical: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  criticalitytype: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  customer: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  dateAssetDecommissioned: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  dateAssetMovedToLiveStatus: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  dependantHardware: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  dependantSystemSoftware: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  deploymentOrLifeCycleStatus: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  descriptionOfPlannedaction: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  downStreamDependencies: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  encryptedPiData: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  equipmentName: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  exposedEdge: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  externallyFacingSystem: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  firmwareVersion: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  firmwareVersionPatchLevel: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  geographicSiteResilience: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  geolocation: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  hardwareVendorName: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  hostLocationWithInPhysicalLocation: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  identifiedActionTsr: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  infrastructure: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  instanceType: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  lastPenTestDateTsr: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  lastPenTestRefNo: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  lastUpgradeDateTsr: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  localMarketOwnerShip: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  localSiteResilience: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  maintenanceHardwareEndOfSupportDate: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  maintenanceSoftwareEndOfSupportDate: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  maintenanceSupportSupplier: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  managementPlane: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  model: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  nameOfProductsDependantOnAsset: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  networkOverSightFunction: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  operatingSystemName: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  operatingSystemSwVersionPatchLevel: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  operatingSystemSwvVersion: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  partNumber: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  pecn: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  pecs: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  piData: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  privilegedAccessLogging: Array<string>;
  /**
   *
   * @type {string}
   * @memberof TSRReportQueryDto
   */
  activeTab?: string;

  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  prodOrLab: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  productImportanceTsr: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  projectEndDateTsr: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  projectStatusTsr: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  regulatoryScope: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  relatedRiskIdsFromRiskRegisters: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  resilienceModel: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  securityCriticalFunction: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  serialNumberTsr: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  nonTemsVertical: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  serviceLevel: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  softwareVendorName: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  supportOwner: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  supportTeam: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  supportTeamsPlaceInTheOrganisation: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  systemNameDns: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  systemNameHostName: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  systemNameManagementIpAddress: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  systemNameNetBios: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  technicalServiceNames: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  upStreamDependencies: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  vendorHardwareEndOfSupportDate: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  vendorSoftwareEndOfSupportDate: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  vodafoneUniqueIdentifier: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof TSRReportQueryDto
   */
  recordClassifier?: Array<number>;
}

/**
 *
 * @export
 * @interface TSRReportDtoGrid
 */
export interface TSRReportDtoGrid extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  isScheduled?: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  reportVertical?: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  exportFilePath?: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  exportFileFormat?: string;
  /**
   *
   * @type {number}
   * @memberof TSRReportDtoGrid
   */
  reportSchedulerId?: number;
  /**
   *
   * @type {number}
   * @memberof TSRReportDtoGrid
   */
  scheduledDate?: number;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  scheduledDayInWeek?: string;

  /**
   *
   * @type {number}
   * @memberof TSRReportDtoGrid
   */
  tsrPassThroughId: number;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  assetDescriptionOrPurpose: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  assetFunction: string;

  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  assetId: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  assetName: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  assetTypeTsr: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  assuranceCall: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  boardOrModuleNameComponentName: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  problemCategoryDescription: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  boardOrModuleTypeComponentSubtype: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  boardOrModuleTypeComponentVersionNumber: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  budgetEstimatedTsr: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  severityDescription: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  bundleBudgetTsr: string;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  businessOwner: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  changesToTheassetSinceDeployment: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  cloudHostedAsset: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  cloudType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  cloudVendor: Array<string>;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  commentOnProjectStatusTsr: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  countryWhereAssetIsLocated: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  critical: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  criticalitytype: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  customer: string;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportDtoGrid
   */
  dateAssetDecommissioned: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportDtoGrid
   */
  dateAssetMovedToLiveStatus: DateFilter;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  dependantHardware: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  dependantSystemSoftware: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  deploymentOrLifeCycleStatus: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  descriptionOfPlannedaction: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  downStreamDependencies: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  encryptedPiData: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  equipmentName: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  exposedEdge: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  externallyFacingSystem: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  firmwareVersion: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  firmwareVersionPatchLevel: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  geographicSiteResilience: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  geolocation: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  hardwareVendorName: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  hostLocationWithInPhysicalLocation: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  identifiedActionTsr: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  infrastructure: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  instanceType: string;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportDtoGrid
   */
  lastPenTestDateTsr: DateFilter;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  lastPenTestRefNo: string;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportDtoGrid
   */
  lastUpgradeDateTsr: DateFilter;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  localMarketOwnerShip: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  localSiteResilience: string;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportDtoGrid
   */
  maintenanceHardwareEndOfSupportDate: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportDtoGrid
   */
  maintenanceSoftwareEndOfSupportDate: DateFilter;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  maintenanceSupportSupplier: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  managementPlane: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  model: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  nameOfProductsDependantOnAsset: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  networkOverSightFunction: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  operatingSystemName: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  operatingSystemSwVersionPatchLevel: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  operatingSystemSwvVersion: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  partNumber: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  pecn: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  pecs: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  piData: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  privilegedAccessLogging: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  prodOrLab: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  productImportanceTsr: string;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportDtoGrid
   */
  projectEndDateTsr: DateFilter;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  projectStatusTsr: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  regulatoryScope: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  relatedRiskIdsFromRiskRegisters: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  resilienceModel: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  securityCriticalFunction: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  serialNumberTsr: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  nonTemsVertical: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  serviceLevel: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  softwareVendorName: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  supportOwner: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  supportTeam: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  supportTeamsPlaceInTheOrganisation: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  systemNameDns: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  systemNameHostName: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  systemNameManagementIpAddress: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  systemNameNetBios: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  technicalServiceNames: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  upStreamDependencies: string;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportDtoGrid
   */
  vendorHardwareEndOfSupportDate: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportDtoGrid
   */
  vendorSoftwareEndOfSupportDate: DateFilter;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  vodafoneUniqueIdentifier: string;

  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  environment: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  opCoName: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfTSRReportDtoGrid
 */
export interface QueryResultDtoOfTSRReportDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfTSRReportDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<TSRReportDtoGrid>}
   * @memberof QueryResultDtoOfTSRReportDtoGrid
   */
  items?: Array<TSRReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface TSRReportQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {number}
   * @memberof TSRReportDtoGrid
   */
  reportSchedulerId?: number;
  /**
   *
   * @type {string[]}
   * @memberof TSRReportDtoGrid
   */
  reportName?: string[];
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  isScheduled?: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  scheduledDayInWeek?: string;
  /**
   *
   * @type {number}
   * @memberof TSRReportDtoGrid
   */
  scheduledDate?: number;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  exportFileFormat?: string;
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  exportFilePath?: string;
  /**
   *
   * @type {Array<number>}
   * @memberof TSRReportQueryDto
   */
  tsrPassThroughId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assetDescriptionOrPurpose?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assetFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assetId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assetName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assetTypeTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  assuranceCall?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  boardOrModuleNameComponentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  boardOrModuleTypeComponentSubtype?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  boardOrModuleTypeComponentVersionNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  budgetEstimatedTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  bundleBudgetTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  businessOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  changesToTheassetSinceDeployment?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  cloudHostedAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  cloudType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  cloudVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  commentOnProjectStatusTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  countryWhereAssetIsLocated?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  critical?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  criticalitytype?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  customer?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  dateAssetDecommissioned?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  dateAssetMovedToLiveStatus?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  dependantHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  dependantSystemSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  deploymentOrLifeCycleStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  descriptionOfPlannedaction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  downStreamDependencies?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  encryptedPiData?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  equipmentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  exposedEdge?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  externallyFacingSystem?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  firmwareVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  firmwareVersionPatchLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  geographicSiteResilience?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  geolocation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  hardwareVendorName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  hostLocationWithInPhysicalLocation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  identifiedActionTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  infrastructure?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  instanceType?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  lastPenTestDateTsr?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  lastPenTestRefNo?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  lastUpgradeDateTsr?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  localMarketOwnerShip?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  localSiteResilience?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  maintenanceHardwareEndOfSupportDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  maintenanceSoftwareEndOfSupportDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  maintenanceSupportSupplier?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  managementPlane?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  model?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  nameOfProductsDependantOnAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  networkOverSightFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  operatingSystemName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  operatingSystemSwVersionPatchLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  operatingSystemSwvVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  partNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  pecn?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  pecs?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  piData?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  privilegedAccessLogging?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof TSRReportQueryDto
   */
  activeTab?: string;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  prodOrLab?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  productImportanceTsr?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  projectEndDateTsr?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  projectStatusTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  regulatoryScope?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  relatedRiskIdsFromRiskRegisters?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  resilienceModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  securityCriticalFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  serialNumberTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  nonTemsVertical?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  serviceLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  softwareVendorName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  supportOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  supportTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  supportTeamsPlaceInTheOrganisation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  systemNameDns?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  systemNameHostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  systemNameManagementIpAddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  systemNameNetBios?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  technicalServiceNames?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  upStreamDependencies?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  vendorHardwareEndOfSupportDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  vendorSoftwareEndOfSupportDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  vodafoneUniqueIdentifier?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof TSRReportQueryDto
   */
  recordClassifier?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  lastModifiedBy?: Array<string>;

  isGlossary?: boolean;
}

export interface TSRReportVerticalQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof TSRReportQueryDto
   */
  appSettingsConfigurationId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof TSRReportQueryDto
   */
  appSettingsId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  settingsValue?: Array<string>;
  /**
   *
   * @type {boolean}
   * @memberof TSRReportQueryDto
   */
  deleted?: boolean;
  /**
   *
   *@type {boolean}
   * @memberof TSRReportQueryDto
   */
  orphan?: boolean;
  /**
   *
   * @type {DateFilter}
   * @memberof TSRReportQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof TSRReportQueryDto
   */
  lastModifiedBy?: Array<string>;
}

export interface TemsTSRReportGrid {
  TemsTSRReportGridResult: QueryResultDtoOfTSRReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface NonTemsTSRReportGrid {
  NonTemsTSRReportGridResult: QueryResultDtoOfTSRReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface TemsTSRReportVerticalGrid {
  TemsTSRReportVerticalGridResult: QueryResultDtoOfTSRReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface TemsTSRReportVerticalCreate {
  TemsTSRReportVerticalDtoCreate: TSRReportVerticalQueryObjectGrid | null;
  ResultDtoCreate: ResultDto | null;
}

export interface LookUpEdit {
  LookUpDtoEdit: TSRReportQueryObjectGrid | null;
  ResultDtoEdit: ResultDto | null;
}
export interface LookUpCreate {
  LookUpDtoCreate: TSRReportQueryObjectGrid | null;
  ResultDtoCreate: ResultDto | null;
}

export const GET_CREATE_TEST_INFO = "GET_CREATE_TEST_INFO";
export const GET_EDIT_TEST_INFO = "GET_EDIT_TEST_INFO";
export const GET_GRID_TEMS_TSR_REPORT = "GET_GRID_TEMS_TSR_REPORT";
export const GET_GRID_REPORT_SCHEDULER = "GET_GRID_REPORT_SCHEDULER";
export const GET_FILTER_TEMS_TSR_REPORT = "GET_FILTER_TEMS_TSR_REPORT";
export const GET_GRID_NON_TEMS_TSR_REPORT = "GET_GRID_NON_TEMS_TSR_REPORT";
export const GET_GRID_VERTICAL_TEMS_TSR_REPORT =
  "GET_GRID_VERTICAL_TEMS_TSR_REPORT";
export const GET_FILTER_NON_TEMS_TSR_REPORT = "GET_FILTER_NON_TEMS_TSR_REPORT";
export const CREATE_TEST_INFO = "CREATE_TEST_INFO";
export const EDIT_TEST_INFO = "EDIT_TEST_INFO";
export const DELETE_TEST_INFO = "DELETE_TEST_INFO";
export const RESTORE_TEST_INFO = "RESTORE_TEST_INFO";
