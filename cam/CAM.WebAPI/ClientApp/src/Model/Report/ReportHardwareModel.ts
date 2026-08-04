import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  EOM_STATUS,
  QueryObjectGrid,
  ReportViewMode,
} from "../Common";
import { ReportQueryDto } from "./Export";
/**
 *
 * @export
 * @interface ReportDto
 */
export interface ReportDto {
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  reportId?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  localMarket?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  verticalEngineeringTeam?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  verticalSubDomain?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  engineeringContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  operationsContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  assetCategory?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  assetClass?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  assetType?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  assetDescription?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  productImportance?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  vendor?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  hardwareModel?: string;
  /**
   *
   * @type {number}
   * @memberof ReportDto
   */
  numberOfNodes?: number;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  operationsMaintenanceContract?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportDto
   */
  vendorEndOfMaintenanceDate?: Date;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  lcmStatus?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  identifiedAction?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  descriptionOfPlannedAction?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  plannedHardwareModel?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  projectStatus?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportDto
   */
  projectEndDate?: Date;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  trackingNumberProjectName?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  budgetEstimated?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  notes?: string;
  /**
   *
   * @type {number}
   * @memberof ReportDto
   */
  systemTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof ReportDto
   */
  designComponentId?: number;

  /**
   *
   * @type {number}
   * @memberof ReportDto
   */
  designComponentFamilyId?: number;
  /**
   *
   * @type {number}
   * @memberof ReportDto
   */
  majorSoftwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof ReportDto
   */
  majorHardwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof ReportDto
   */
  lcmEngineeringId?: number;
  /**
   *
   * @type {number}
   * @memberof ReportDto
   */
  plannedActivityId?: number;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  assetOutofScopeForReportingPurposes?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  managedByGdc?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  extendedSupportOptionOfferedByVendor?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  eomControl?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  engUpdateTracker?: string;
  /**
   *
   * @type {string}
   * @memberof ReportDto
   */
  opsUpdateTracker?: string;
}
/**
 *
 * @export
 * @interface ReportHardwareDtoGrid
 */
export interface ReportHardwareDtoGrid extends ReportDto {
  /**
   *
   * @type {string}
   * @memberof ReportHardwareDtoGrid
   */
  bundleBudget?: string;
  /**
   *
   * @type {string}
   * @memberof ReportHardwareDtoGrid
   */
  bundleId?: string;
  /**
   *
   * @type {string}
   * @memberof ReportHardwareDtoGrid
   */
  assetServiceFunctionality?: string;
  /**
   *
   * @type {string}
   * @memberof ReportHardwareDtoGrid
   */
  platform?: string;
  /**
   *
   * @type {string}
   * @memberof ReportHardwareDtoGrid
   */
  lcmStatusEngHardware?: string;
  /**
   *
   * @type {string}
   * @memberof ReportHardwareDtoGrid
   */
  lcmStatusOpsHardware?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportHardwareDtoGrid
   */
  opsMaintenanceConractEnd?: Date;
  /**
   *
   * @type {string}
   * @memberof ReportHardwareDtoGrid
   */
  engRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof ReportHardwareDtoGrid
   */
  engRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof ReportHardwareDtoGrid
   */
  opsRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof ReportHardwareDtoGrid
   */
  opsRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof ReportHardwareDtoGrid
   */
  overallRiskEvaluation?: string;

  eomStatus: EOM_STATUS;

  /**
   *
   * @type {boolean}
   * @memberof ReportHardwareDtoGrid
   */
  archived?: boolean;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfReportHardwareDtoGrid
 */
export interface QueryResultDtoOfReportHardwareDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfReportHardwareDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<ReportHardwareDtoGrid>}
   * @memberof QueryResultDtoOfReportHardwareDtoGrid
   */
  items?: Array<ReportHardwareDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface ReportHardwareQueryObjectGrid extends ReportQueryDto {
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
  ViewMode?: ReportViewMode;
  lcmExportDescription?: string;
}

export interface ReportHardwareGrid {
  ReportHardwareGridResult: QueryResultDtoOfReportHardwareDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_GRID_REPORT_HARDWARE = "GET_GRID_REPORT_HARDWARE";
export const GET_FILTER_REPORT_HARDWARE = "GET_FILTER_REPORT_HARDWARE";
