import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  DateFilter,
  EOM_STATUS,
  QueryObject,
  QueryObjectGrid,
  ReportViewMode,
} from "../Common";
import { ReportQueryDto } from "./LcmExportReport";

/**
 *
 * @export
 * @interface ReportDtoGrid
 */
export interface ReportDtoGrid extends QueryObject {
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  reportId?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  previousReportId?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  name?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  localMarket?: string;
  /**
   *
   * @type {number}
   * @memberof ReportQueryDto
   */
  localMarketId?: number;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  designComponentIndex?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  verticalEngineeringTeam?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  verticalSubDomain?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  engineeringContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  operationsContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  assetCategory?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  assetClass?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  assetType?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  assetDescription?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  productImportance?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  vendor?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  hardwareModel?: string;
  /**
   *
   * @type {number}
   * @memberof ReportQueryDto
   */
  numberOfNodes?: number;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  operationsMaintenanceContract?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportQueryDto
   */
  vendorEndOfMaintenanceDateValue?: Date;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  lcmStatus?: string;
  /**
   *
   * @type {number}
   * @memberof ReportQueryDto
   */
  plannedAction?: number;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  descriptionOfPlannedAction?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  plannedSoftwareVersion?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  plannedHardwareModel?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  projectStatus?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportQueryDto
   */
  projectEndDateValue?: Date;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  trackingNumberProjectName?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  notes?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  bundleBudget?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  bundleId?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  assetServiceFunctionality?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  platform?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportQueryDto
   */
  opsMaintenanceConractEndValue?: Date;
  /**
   *
   * @type {number}
   * @memberof ReportQueryDto
   */
  engRiskEvaluation?: number;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  engRiskEvaluationNotes?: string;
  /**
   *
   * @type {number}
   * @memberof ReportQueryDto
   */
  opsRiskEvaluation?: number;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  opsRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  overallRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  identifiedAction?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  budgetEstimated?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  managedByGdc?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportQueryDto
   */
  extendedSupportOptionOfferedByVendor?: Date;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  engkpI2?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  expLCMstatusatendofFY24?: string;
  /**
   *
   * @type {number}
   * @memberof ReportQueryDto
   */
  riskCluster?: number;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  criticality?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  gdprRelevant?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  deliveryPlanAvailable?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  hostname?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  ipAddress?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  ragStatus?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  wbsCode?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  bptID?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  ppmID?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  serialNumber?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  assetStatus?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  program?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  projectOwner?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  reasonfornoPlan?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  commentonProjectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  securityRiskPotential?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  securityRiskEffective?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  vulnerabilityRating?: string;

  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  securityMitigation?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  securityRiskOverall?: string;
  /**
   *
   * @type {Array<boolean>}
   * @memberof ReportQueryDto
   */
  includedinSecurityScanning?: Array<boolean>;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  raId?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  cyberRiskRequestId?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  requestID?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportQueryDto
   */
  lastScanDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof ReportQueryDto
   */
  lastScanDateValue?: Date;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  assetOutofScopeForReportingPurposes?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportQueryDto
   */
  lastUpgradeDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof ReportQueryDto
   */
  lastUpgradeDateValue?: Date;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  eomControl?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  engUpdateTracker?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  opsUpdateTracker?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  exNetworks?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  newopsRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  occurrenceProbability?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  incidentClass?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  productCode?: string;
  /**
   *
   * @type {Array<boolean>}
   * @memberof ReportQueryDto
   */
  handedOverToOperation?: Array<boolean>;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  contractRenewalPlan?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  dataSource?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  scopeOfSimplification?: string;
  /**
   *
   * @type {number}
   * @memberof ReportQueryDto
   */
  viewMode?: number;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  lcmExportDescription?: string;
  /**
   *
   * @type {number}
   * @memberof ReportQueryDto
   */
  verticalEngineeringTeamId?: number;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  isPecn?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof ReportQueryDto
   */
  isPecs?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof ReportQueryDto
   */
  isScf?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof ReportQueryDto
   */
  isNof?: Array<boolean>;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  exposedEdgeFlag?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof ReportQueryDto
   */
  externalFacingFlag?: Array<boolean>;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  infrastructureLocation?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  category?: string;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  interfaceType?: string;
  /**
   *
   * @type {boolean}
   * @memberof ReportQueryDto
   */
  archived?: boolean;
}

/**
 *
 * @export
 * @interface NetworkLevel2ReportDtoGrid
 */
export interface NetworkLevel2ReportDtoGrid extends ReportDtoGrid {
  /**
   *
   * @type {string}
   * @memberof NetworkLevel2ReportDtoGrid
   */
  assetVirtualized?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkLevel2ReportDtoGrid
   */
  softwareVersion?: string;
  /**
   *
   * @type {Date}
   * @memberof NetworkLevel2ReportDtoGrid
   */
  vendorEndOfVulnerabilitySecuritySupportDateValue?: Date;
  /**
   *
   * @type {string}
   * @memberof NetworkLevel2ReportDtoGrid
   */
  outputToLcmSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkLevel2ReportDtoGrid
   */
  lcmStatusOpsSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkLevel2ReportDtoGrid
   */
  lcmStatusEngSoftware?: string;
  /**
   *
   * @type {number}
   * @memberof NetworkLevel2ReportDtoGrid
   */
  typeOfNetworkElement?: number;
  /**
   *
   * @type {string}
   * @memberof NetworkLevel2ReportDtoGrid
   */
  originalLCMSpreadsheetID?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkLevel2ReportDtoGrid
   */
  isExtendedSupportOfferedByVendor?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkLevel2ReportDtoGrid
   */
  labSWRelease?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkLevel2ReportDtoGrid
   */
  originalSwLcmId?: string;
  /**
   *
   * @type {string}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  lcmStatusEngHardware?: string;
  /**
   *
   * @type {string}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  lcmStatusOpsHardware?: string;
  /**
   *
   * @type {string}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  outputToLcmHardware?: string;
  /**
   *
   * @type {string}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  originalHwLcmId?: string;
  /**
   *
   * @type {string}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  hwIsExtendedSupportOfferedByVendor?: string;
  /**
   *
   * @type {number}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  designComponentFamily?: number;
  /**
   *
   * @type {number}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  supportedService?: number;
}
/**
 *
 * @export
 * @interface SubBoundHWSwReportDtoGrid
 */
export interface SubBoundHWSwReportDtoGrid extends ReportDtoGrid {
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  assetVirtualized?: string;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  softwareVersion?: string;
  /**
   *
   * @type {Date}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  vendorEndOfVulnerabilitySecuritySupportDateValue?: Date;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  outputToLcmSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  lcmStatusOpsSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  lcmStatusEngSoftware?: string;
  /**
   *
   * @type {number}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  typeOfNetworkElement?: number;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  originalLCMSpreadsheetID?: string;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  isExtendedSupportOfferedByVendor?: string;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  labSWRelease?: string;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  originalSwLcmId?: string;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  lcmStatusEngHardware?: string;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  lcmStatusOpsHardware?: string;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  outputToLcmHardware?: string;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  originalHwLcmId?: string;
  /**
   *
   * @type {string}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  hwIsExtendedSupportOfferedByVendor?: string;
  /**
   *
   * @type {number}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  designComponentFamily?: number;
  /**
   *
   * @type {number}
   * @memberof SubBoundHWSwReportDtoGrid
   */
  supportedService?: number;
}
/**
 *
 * @export
 * @interface HardwareConfigReportDtoGrid
 */
export interface HardwareConfigReportDtoGrid extends ReportDtoGrid {
  /**
   *
   * @type {string}
   * @memberof HardwareConfigReportDtoGrid
   */
  outputToLcmHardware?: string;
  /**
   *
   * @type {string}
   * @memberof HardwareConfigReportDtoGrid
   */
  lcmStatusOpsHardware?: string;
  /**
   *
   * @type {string}
   * @memberof HardwareConfigReportDtoGrid
   */
  lcmStatusEngHardware?: string;
  /**
   *
   * @type {string}
   * @memberof HardwareConfigReportDtoGrid
   */
  hwIsExtendedSupportOfferedByVendor?: string;
  /**
   *
   * @type {string}
   * @memberof HardwareConfigReportDtoGrid
   */
  originalHwLcmId?: string;
  /**
   *
   * @type {string}
   * @memberof HardwareConfigReportDtoGrid
   */
  hardwareProfile?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfReportNetworkLevel2DtoGrid
 */
export interface QueryResultDtoOfReportNetworkLevel2DtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfReportNetworkLevel2DtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkLevel2ReportDtoGrid>}
   * @memberof QueryResultDtoOfReportNetworkLevel2DtoGrid
   */
  items?: Array<NetworkLevel2ReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfReportHardwareConfigDtoGrid
 */
export interface QueryResultDtoOfReportHardwareConfigDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfReportHardwareConfigDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<HardwareConfigReportDtoGrid>}
   * @memberof QueryResultDtoOfReportHardwareConfigDtoGrid
   */
  items?: Array<HardwareConfigReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfReportSubBoundHardwareDtoGrid
 */
export interface QueryResultDtoOfReportSubBoundHardwareDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfReportSubBoundHardwareDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<SubBoundHWSwReportDtoGrid>}
   * @memberof QueryResultDtoOfReportSubBoundHardwareDtoGrid
   */
  items?: Array<SubBoundHWSwReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfReportSubBoundSoftwareDtoGrid
 */
export interface QueryResultDtoOfReportSubBoundSoftwareDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfReportSubBoundSoftwareDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<SubBoundHWSwReportDtoGrid>}
   * @memberof QueryResultDtoOfReportSubBoundSoftwareDtoGrid
   */
  items?: Array<SubBoundHWSwReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface ReportNetworkLevel2Grid {
  ReportNetworkLevel2GridResult: QueryResultDtoOfReportNetworkLevel2DtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface ReportHardwareConfigGrid {
  ReportHardwareConfigGridResult: QueryResultDtoOfReportHardwareConfigDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface ReportSubBoundHardwareGrid {
  ReportSubBoundHardwareGridResult: QueryResultDtoOfReportSubBoundHardwareDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface ReportSubBoundSoftwareGrid {
  ReportSubBoundSoftwareGridResult: QueryResultDtoOfReportSubBoundSoftwareDtoGrid | null;
  filter: FilterValueDto[] | null;
}
/**
 *
 * @export
 * @interface ReportNetworkLevel2QueryObjectGrid
 */
export interface ReportNetworkLevel2QueryObjectGrid extends ReportQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  assetVirtualized?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  softwareVersion?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  vendorEndOfVulnerabilitySecuritySupportDateValue?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  outputToLcmSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  lcmStatusOpsSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  lcmStatusEngSoftware?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  typeOfNetworkElement?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  originalLCMSpreadsheetID?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  isExtendedSupportOfferedByVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  labSWRelease?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  originalSwLcmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  lcmStatusEngHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  lcmStatusOpsHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  outputToLcmHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  originalHwLcmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  hwIsExtendedSupportOfferedByVendor?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  designComponentFamily?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportNetworkLevel2QueryObjectGrid
   */
  supportedService?: Array<number>;
}
export interface ReportHwSwQueryObjectGrid extends ReportQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  assetVirtualized?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  softwareVersion?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportHwSwQueryObjectGrid
   */
  vendorEndOfVulnerabilitySecuritySupportDateValue?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  outputToLcmSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  lcmStatusOpsSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  lcmStatusEngSoftware?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  typeOfNetworkElement?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  originalLCMSpreadsheetID?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  isExtendedSupportOfferedByVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  labSWRelease?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  originalSwLcmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  lcmStatusEngHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  lcmStatusOpsHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  outputToLcmHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  originalHwLcmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  hwIsExtendedSupportOfferedByVendor?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  designComponentFamily?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportHwSwQueryObjectGrid
   */
  supportedService?: Array<number>;
}
export interface ReportHardwareConfigQueryObjectGrid extends ReportQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareConfigQueryObjectGrid
   */
  lcmStatusEngHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareConfigQueryObjectGrid
   */
  lcmStatusOpsHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareConfigQueryObjectGrid
   */
  outputToLcmHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareConfigQueryObjectGrid
   */
  originalHwLcmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareConfigQueryObjectGrid
   */
  hwIsExtendedSupportOfferedByVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareConfigQueryObjectGrid
   */
  hardwareProfile?: Array<string>;
}

export const GET_GRID_REPORT_NETWORKLEVEL2 = "GET_GRID_REPORT_NETWORKLEVEL2";
export const GET_FILTER_REPORT_NETWORKLEVEL2 =
  "GET_FILTER_REPORT_NETWORKLEVEL2";
export const GET_GRID_REPORT_SUBBOUND_HARDWARE =
  "GET_GRID_REPORT_SUBBOUND_HARDWARE";
export const GET_FILTER_REPORT_SUBBOUND_HARDWARE =
  "GET_FILTER_REPORT_SUBBOUND_HARDWARE";
export const GET_GRID_REPORT_SUBBOUND_SOFTWARE =
  "GET_GRID_REPORT_SUBBOUND_SOFTWARE";
export const GET_FILTER_REPORT_SUBBOUND_SOFTWARE =
  "GET_FILTER_REPORT_SUBBOUND_SOFTWARE";
export const GET_GRID_REPORT_HARDWARE_CONFIG =
  "GET_GRID_REPORT_HARDWARE_CONFIG";
export const GET_FILTER_REPORT_HARDWARE_CONFIG =
  "GET_FILTER_REPORT_HARDWARE_CONFIG";
