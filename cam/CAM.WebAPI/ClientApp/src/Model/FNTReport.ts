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
 * @interface FNTReportQueryDto
 */
export interface FNTReportQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof FNTReportQueryDto
   */
  temsFntReportId: Array<number>;

  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  operatingSystemSwVersionPatchLevel: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  operatingSystemSwvVersion: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  partNumber: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  pecn: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  pecs: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  piData: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  privilegedAccessLogging: Array<string>;
  /**
   *
   * @type {string}
   * @memberof FNTReportQueryDto
   */
  activeTab?: string;

  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  prodOrLab: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  productImportanceTsr: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  projectEndDateTsr: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  projectStatusTsr: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  regulatoryScope: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  relatedRiskIdsFromRiskRegisters: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  resilienceModel: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  securityCriticalFunction: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  serialNumberTsr: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  nonTemsVertical: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  serviceLevel: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareVendorName: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  supportOwner: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  supportTeam: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  supportTeamsPlaceInTheOrganisation: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  systemNameDns: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  systemNameHostName: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  systemNameManagementIpAddress: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  systemNameNetBios: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  technicalServiceNames: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  upStreamDependencies: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  vendorHardwareEndOfSupportDate: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  vendorSoftwareEndOfSupportDate: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  vodafoneUniqueIdentifier: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof FNTReportQueryDto
   */
  recordClassifier?: Array<number>;

  // New fields from the provided JSON

  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  serialNumberOfHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  locationOfHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hardwareTypeOfHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  vendorFnt?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  ipAddressOfHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  market?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hwEndOfLife?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hwEndOfSupport?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hwEndOfSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hardwareModules?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareProductType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareProductVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareIsVirtualized?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  operatingSystemOfVirtualMachine?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  applicationHostedOnSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  uuidSerialNumberOfSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  locationOfSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  serviceType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  swEndOfLife?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  swEndOfSupport?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  swEndOfSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  verticalSubdomain?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  assetCategory?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  assetDescriptionFnt?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  identifiedActionFnt?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  descriptionOfPlannedActionFnt?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  plannedHwModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  businessServiceName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  opMaintenanceContractendDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  meverticalResposible?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  assetStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  localMarket?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  application?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  cloud?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  dataCenterocation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerHostname?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerHwModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualServerHostedOn?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualServerManufacturer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualServerTypeOfDevice?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualServerIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  osName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  osVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  osStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  manufacturer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  creationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  creationDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  modificationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  modificationDate?: DateFilter;
}

/**
 *
 * @export
 * @interface FNTReportDtoGrid
 */
export interface FNTReportDtoGrid extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof TSRReportDtoGrid
   */
  isScheduled?: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  reportVertical?: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  exportFilePath?: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  exportFileFormat?: string;
  /**
   *
   * @type {number}
   * @memberof FNTReportDtoGrid
   */
  reportSchedulerId?: number;
  /**
   *
   * @type {number}
   * @memberof FNTReportDtoGrid
   */
  scheduledDate?: number;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  scheduledDayInWeek?: string;

  /**
   *
   * @type {number}
   * @memberof FNTReportDtoGrid
   */
  temsFntReportId: number;

  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  problemCategoryDescription: string;

  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  severityDescription: string;

  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  model: string;

  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  operatingSystemSwVersionPatchLevel: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  operatingSystemSwvVersion: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  partNumber: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  pecn: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  pecs: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  piData: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  privilegedAccessLogging: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  prodOrLab: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  productImportanceTsr: string;
  /**
   *
   * @type {Date}
   * @memberof FNTReportDtoGrid
   */
  projectEndDateTsr: Date;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  projectStatusTsr: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  regulatoryScope: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  relatedRiskIdsFromRiskRegisters: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  resilienceModel: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  securityCriticalFunction: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  serialNumberTsr: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  nonTemsVertical: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  serviceLevel: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  softwareVendorName: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  supportOwner: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  supportTeam: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  supportTeamsPlaceInTheOrganisation: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  systemNameDns: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  systemNameHostName: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  systemNameManagementIpAddress: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  systemNameNetBios: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  technicalServiceNames: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  upStreamDependencies: string;
  /**
   *
   * @type {Date}
   * @memberof FNTReportDtoGrid
   */
  vendorHardwareEndOfSupportDate: Date;
  /**
   *
   * @type {Date}
   * @memberof FNTReportDtoGrid
   */
  vendorSoftwareEndOfSupportDate: Date;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  vodafoneUniqueIdentifier: string;

  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  environment: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  opCoName: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfFNTReportDtoGrid
 */
export interface QueryResultDtoOfFNTReportDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfFNTReportDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<FNTReportDtoGrid>}
   * @memberof QueryResultDtoOfFNTReportDtoGrid
   */
  items?: Array<FNTReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface FNTReportQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {number}
   * @memberof FNTReportDtoGrid
   */
  reportSchedulerId?: number;
  /**
   *
   * @type {string[]}
   * @memberof FNTReportDtoGrid
   */
  reportName?: string[];
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  isScheduled?: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  scheduledDayInWeek?: string;
  /**
   *
   * @type {number}
   * @memberof FNTReportDtoGrid
   */
  scheduledDate?: number;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  exportFileFormat?: string;
  /**
   *
   * @type {string}
   * @memberof FNTReportDtoGrid
   */
  exportFilePath?: string;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerOsName?: Array<string>;

  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerOsVersion?: Array<string>;

  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  physicalServerOsStartDate?: DateFilter;

  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  physicalServerOsInstallationDate?: DateFilter;

  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerOsStatus?: Array<string>;

  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hwOperationsContactPoint?: Array<string>;

  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  hwOpsContractEndDate?: DateFilter;

  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hwOpsContractStatus?: Array<string>;

  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  swOperationsContactPoint?: Array<string>;

  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  swOpsContractEndDate?: DateFilter;

  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  swOpsContractStatus?: Array<string>;

  /**
   *
   * @type {Array<number>}
   * @memberof FNTReportQueryDto
   */
  passThroughId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  operationsContactPoint?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  riskCluster?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  platform?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hardwareTypeOfHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  verticalEngineeringTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  vendorFnt?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  ipAddressOfHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  market?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hwEndOfLife?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hwEndOfSupport?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hwEndOfSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hardwareModules?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareProductType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareProductVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareIsVirtualized?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  operatingSystemOfVirtualMachine?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  applicationHostedOnSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  uuidSerialNumberOfSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  locationOfSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  serviceType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  swEndOfLife?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  swEndOfSupport?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  swEndOfSale?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  verticalSubdomain?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  assetCategory?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  assetClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  assetTypeFnt?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  assetDescriptionFnt?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  assetStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  productImportanceFnt?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  operationsMaintenanceContract?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  vendorEndOfMaintenanceDateFnt?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  opMaintenanceContractendDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  identifiedActionFnt?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  descriptionOfPlannedActionFnt?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  plannedHwModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  businessServiceName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  incidentClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  occurrenceProbability?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  meverticalResposible?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  typeOfNetworkElement?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  localMarket?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  application?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  cloud?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  dataCenterocation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  locationOfHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  serialNumberOfHardwareAsset?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  hostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerHostname?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerHwModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  physicalServerVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualServerHostedOn?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualServerManufacturer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualServerTypeOfDevice?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualMachineType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualServerIpaddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualServerSerialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  virtualServerType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  osName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  osVersion?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  osStartDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  osInstallationDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  osStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  version?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  release?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  manufacturer?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  language?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  creationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  creationDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  modificationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  modificationDate?: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof FNTReportQueryDto
   */
  temsFntReportId?: Array<number>;

  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  model?: Array<string>;

  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  operatingSystemSwVersionPatchLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  operatingSystemSwvVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  partNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  pecn?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  pecs?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  piData?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  privilegedAccessLogging?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof FNTReportQueryDto
   */
  activeTab?: string;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  prodOrLab?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  productImportanceTsr?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  projectEndDateTsr?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  projectStatusTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  regulatoryScope?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  relatedRiskIdsFromRiskRegisters?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  resilienceModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  securityCriticalFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  serialNumberTsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  nonTemsVertical?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  serviceLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  softwareVendorName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  supportOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  supportTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  supportTeamsPlaceInTheOrganisation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  systemNameDns?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  systemNameHostName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  systemNameManagementIpAddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  systemNameNetBios?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  technicalServiceNames?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  upStreamDependencies?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  vendorHardwareEndOfSupportDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  vendorSoftwareEndOfSupportDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  vodafoneUniqueIdentifier?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof FNTReportQueryDto
   */
  recordClassifier?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  lastModifiedBy?: Array<string>;

  isGlossary?: boolean;
}

export interface FNTReportVerticalQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof FNTReportQueryDto
   */
  appSettingsConfigurationId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof FNTReportQueryDto
   */
  appSettingsId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  settingsValue?: Array<string>;
  /**
   *
   * @type {boolean}
   * @memberof FNTReportQueryDto
   */
  deleted?: boolean;
  /**
   *
   *@type {boolean}
   * @memberof FNTReportQueryDto
   */
  orphan?: boolean;
  /**
   *
   * @type {DateFilter}
   * @memberof FNTReportQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof FNTReportQueryDto
   */
  lastModifiedBy?: Array<string>;
}

export interface TemsFNTReportGrid {
  TemsFNTReportGridResult: QueryResultDtoOfFNTReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface NonTemsFNTReportGrid {
  NonTemsFNTReportGridResult: QueryResultDtoOfFNTReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface TemsFNTReportVerticalGrid {
  TemsFNTReportVerticalGridResult: QueryResultDtoOfFNTReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface TemsFNTReportVerticalCreate {
  TemsFNTReportVerticalDtoCreate: FNTReportVerticalQueryObjectGrid | null;
  ResultDtoCreate: ResultDto | null;
}

export interface LookUpEdit {
  LookUpDtoEdit: FNTReportQueryObjectGrid | null;
  ResultDtoEdit: ResultDto | null;
}
export interface LookUpCreate {
  LookUpDtoCreate: FNTReportQueryObjectGrid | null;
  ResultDtoCreate: ResultDto | null;
}

export const GET_CREATE_TEST_INFO = "GET_CREATE_TEST_INFO";
export const GET_EDIT_TEST_INFO = "GET_EDIT_TEST_INFO";
export const GET_GRID_TEMS_FNT_REPORT = "GET_GRID_TEMS_FNT_REPORT";
export const GET_FILTER_TEMS_FNT_REPORT = "GET_FILTER_TEMS_FNT_REPORT";
export const GET_GRID_NON_TEMS_FNT_REPORT = "GET_GRID_NON_TEMS_FNT_REPORT";
export const GET_GRID_VERTICAL_TEMS_FNT_REPORT =
  "GET_GRID_VERTICAL_TEMS_FNT_REPORT";
export const GET_FILTER_NON_TEMS_FNT_REPORT = "GET_FILTER_NON_TEMS_FNT_REPORT";
export const CREATE_TEST_INFO = "CREATE_TEST_INFO";
export const EDIT_TEST_INFO = "EDIT_TEST_INFO";
export const DELETE_TEST_INFO = "DELETE_TEST_INFO";
export const RESTORE_TEST_INFO = "RESTORE_TEST_INFO";
