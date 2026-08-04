import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  DateFilter,
  EOM_STATUS,
  QueryObjectGrid,
  ReportViewMode,
} from "../Common";
import { ReportQueryDto } from "./Export";
import { ReportDto } from "./ReportHardwareModel";

/**
 *
 * @export
 * @interface ReportSoftwareDtoGrid
 */
export interface ReportSoftwareDtoGrid {
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  reportId?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  localMarket?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  verticalEngineeringTeam?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  verticalSubDomain?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  engineeringContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  operationsContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  assetCategory?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  assetClass?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  assetType?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  assetDescription?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  assetVirtualized?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  productImportance?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  vendor?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  hardwareModel?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  softwareRelease?: string;
  /**
   *
   * @type {number}
   * @memberof ReportSoftwareDtoGrid
   */
  numberOfNodes?: number;

  /**
   *
   * @type {number}
   * @memberof ReportSoftwareDtoGrid
   */
  designComponentFamilyId?: number;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  operationsMaintenanceContract?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportSoftwareDtoGrid
   */
  vendorEndOfMaintenanceDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof ReportSoftwareDtoGrid
   */
  vendorEndOfVulnerabilitySecuritySupportDate?: Date;

  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  vendorEndOfVulnerabilitySecuritySupportDateValue?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  outputToLcmSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  identifiedAction?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  descriptionOfPlannedAction?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  plannedSoftwareRelease?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  projectStatus?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportSoftwareDtoGrid
   */
  projectEndDate?: Date;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  trackingNumberProjectName?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  budgetEstimated?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  notes?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  cloudVersion?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  bundleBudget?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  bundleId?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  assetServiceFunctionality?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  platform?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  lcmStatusEngSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  lcmStatusOpsSoftware?: string;
  /**
   *
   * @type {Date}
   * @memberof ReportSoftwareDtoGrid
   */
  opsMaintenanceConractEnd?: Date;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  engRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  engRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  opsRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  opsRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  overallRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  assetOutofScopeForReportingPurposes?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  managedByGdc?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  extendedSupportOptionOfferedByVendor?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  eomControl?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  engUpdateTracker?: string;
  /**
   *
   * @type {string}
   * @memberof ReportSoftwareDtoGrid
   */
  opsUpdateTracker?: string;
  /**
   *
   * @type {number}
   * @memberof ReportSoftwareDtoGrid
   */
  systemTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof ReportSoftwareDtoGrid
   */
  designComponentId?: number;
  /**
   *
   * @type {number}
   * @memberof ReportSoftwareDtoGrid
   */
  majorSoftwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof ReportSoftwareDtoGrid
   */
  majorHardwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof ReportSoftwareDtoGrid
   */
  lcmEngineeringId?: number;
  /**
   *
   * @type {number}
   * @memberof ReportSoftwareDtoGrid
   */
  plannedActivityId?: number;

  /**
   *
   * @type {EOM_STATUS}
   * @memberof ReportSoftwareDtoGrid
   */
  eomStatus: EOM_STATUS;
  /**
   *
   * @type {boolean}
   * @memberof ReportSoftwareDtoGrid
   */
  archived?: boolean;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfReportSoftwareDtoGrid
 */
export interface QueryResultDtoOfReportSoftwareDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfReportSoftwareDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<ReportSoftwareDtoGrid>}
   * @memberof QueryResultDtoOfReportSoftwareDtoGrid
   */
  items?: Array<ReportSoftwareDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface ReportSoftwareQueryObjectGrid extends ReportQueryDto {
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
  ViewMode?: ReportViewMode;
  lcmExportDescription?: string;
}

export interface ReportSoftwareGrid {
  ReportSoftwareGridResult: QueryResultDtoOfReportSoftwareDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_GRID_REPORT_SOFTWARE = "GET_GRID_REPORT_SOFTWARE";
export const GET_FILTER_REPORT_SOFTWARE = "GET_FILTER_REPORT_SOFTWARE";
