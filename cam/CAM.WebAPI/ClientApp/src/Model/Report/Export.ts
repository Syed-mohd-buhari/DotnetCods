import { QueryObject, DateFilter } from "../Common";
import { ReportSubBoundHwSwQueryGrid } from "./LcmExportReport";
import {
  ReportHardwareConfigQueryObjectGrid,
  ReportNetworkLevel2QueryObjectGrid,
} from "./ReportLcmExportModel";

/**
 *
 * @export
 * @interface ReportSoftwareQueryDto
 */
export interface ReportSoftwareQueryDto extends ReportQueryDto {
  /**
   *
   * @type {Array<number>}
   * @memberof ReportSoftwareQueryDto
   */
  nonTemsVertical?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSoftwareQueryDto
   */
  assetVirtualized?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSoftwareQueryDto
   */
  softwareRelease?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportSoftwareQueryDto
   */
  vendorEndOfVulnerabilitySecuritySupportDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSoftwareQueryDto
   */
  cloudVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSoftwareQueryDto
   */
  outputToLcmSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSoftwareQueryDto
   */
  lcmStatusOpsSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSoftwareQueryDto
   */
  lcmStatusEngSoftware?: Array<string>;
}
/**
 *
 * @export
 * @interface ReportHardwareQueryDto
 */
export interface ReportHardwareQueryDto extends ReportQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareQueryDto
   */
  lcmStatusEngHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareQueryDto
   */
  lcmStatusOpsHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareQueryDto
   */
  outputToLcmHardware?: Array<string>;
}
/**
 *
 * @export
 * @interface ReportQueryDto
 */
export interface ReportQueryDto extends QueryObject {
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  reportId?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  name?: string;
  /**
   *
   * @type {boolean}
   * @memberof ReportQueryDto
   */
  isHistorical?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof ReportQueryDto
   */
  isDefault?: boolean;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  localMarket?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  designComponentIndex?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  verticalEngineeringTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  verticalSubDomain?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  engineeringContactPoint?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  operationsContactPoint?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  assetCategory?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportQueryDto
   */
  assetClass?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  assetType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  assetDescription?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  productImportance?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  vendor?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportQueryDto
   */
  hardwareModel?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportQueryDto
   */
  numberOfNodes?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  operationsMaintenanceContract?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportQueryDto
   */
  vendorEndOfMaintenanceDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  lcmStatus?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportQueryDto
   */
  plannedAction?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  descriptionOfPlannedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  plannedSoftwareRelease?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  projectStatus?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportQueryDto
   */
  projectEndDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  trackingNumberProjectName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  notes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  bundleBudget?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  bundleId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  assetServiceFunctionality?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportQueryDto
   */
  platform?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportQueryDto
   */
  opsMaintenanceConractEnd?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  engRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  engRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  opsRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  opsRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  overallRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  identifiedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  budgetEstimated?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  assetOutofScopeForReportingPurposes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  managedByGdc?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportQueryDto
   */
  extendedSupportOptionOfferedByVendor?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  eomControl?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  engUpdateTracker?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  opsUpdateTracker?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  componentName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  componentResourceKey?: Array<string>;
}

/**
 *
 * @export
 * @interface ReportQueryAllDto
 */
export interface ReportQueryAllDto {
  /**
   *
   * @type {ReportHardwareQueryDto}
   * @memberof ReportQueryAllDto
   */
  queryHardware?: ReportHardwareQueryDto;
  /**
   *
   * @type {ReportSoftwareQueryDto}
   * @memberof ReportQueryAllDto
   */
  querySoftware?: ReportSoftwareQueryDto;
  /**
   *
   * @type {ReportNetworkLevel2QueryObjectGrid}
   * @memberof ReportQueryAllDto
   */
  querySoftwareLevelTwo?: ReportNetworkLevel2QueryObjectGrid;
  /**
   *
   * @type {ReportSubBoundHwSwQueryGrid}
   * @memberof ReportQueryAllDto
   */
  querySubnetworkSoftware?: ReportSubBoundHwSwQueryGrid;
  /**
   *
   * @type {ReportSubBoundHwSwQueryGrid}
   * @memberof ReportQueryAllDto
   */
  querySubnetworkHardware?: ReportSubBoundHwSwQueryGrid;
  /**
   *
   * @type {ReportHardwareConfigQueryObjectGrid}
   * @memberof ReportQueryAllDto
   */
  queryHardwareConfiguration?: ReportHardwareConfigQueryObjectGrid;
  activeTab?: string;
  lcmExportDescription?: string;
}

export interface ExportDownload {
  file: Blob | null;
}

export const DOWNLOAD_REPORT = "DOWNLOAD_REPORT";
