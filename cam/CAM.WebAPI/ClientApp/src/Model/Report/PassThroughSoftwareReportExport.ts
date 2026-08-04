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
 * @interface PassThroughSoftwareReportDtoGrid
 */
export interface PassThroughSoftwareReportDtoGrid {
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  reportId?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  localMarket?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  verticalEngineeringTeam?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  verticalSubDomain?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  engineeringContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  operationsContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  assetCategory?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  assetClass?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  assetType?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  assetDescription?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  assetVirtualized?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  productImportance?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  vendor?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  hardwareModel?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  softwareRelease?: string;
  /**
   *
   * @type {number}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  numberOfNodes?: number;

  /**
   *
   * @type {number}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  designComponentFamilyId?: number;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  operationsMaintenanceContract?: string;
  /**
   *
   * @type {Date}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  vendorEndOfMaintenanceDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  vendorEndOfVulnerabilitySecuritySupportDate?: Date;

  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  vendorEndOfVulnerabilitySecuritySupportDateValue?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  outputToLcmSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  identifiedAction?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  descriptionOfPlannedAction?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  plannedSoftwareRelease?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  projectStatus?: string;
  /**
   *
   * @type {Date}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  projectEndDate?: Date;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  trackingNumberProjectName?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  budgetEstimated?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  notes?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  cloudVersion?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  bundleBudget?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  bundleId?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  assetServiceFunctionality?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  platform?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  lcmStatusEngSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  lcmStatusOpsSoftware?: string;
  /**
   *
   * @type {Date}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  opsMaintenanceConractEnd?: Date;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  engRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  engRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  opsRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  opsRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  overallRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  assetOutofScopeForReportingPurposes?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  managedByGdc?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  extendedSupportOptionOfferedByVendor?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  eomControl?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  engUpdateTracker?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  opsUpdateTracker?: string;
  /**
   *
   * @type {number}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  systemTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  designComponentId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  majorSoftwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  majorHardwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  lcmEngineeringId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  plannedActivityId?: number;

  /**
   *
   * @type {EOM_STATUS}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  eomStatus: EOM_STATUS;
  /**
   *
   * @type {boolean}
   * @memberof PassThroughSoftwareReportDtoGrid
   */
  archived?: boolean;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfPassThroughSoftwareReportDtoGrid
 */
export interface QueryResultDtoOfPassThroughSoftwareReportDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfPassThroughSoftwareReportDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<PassThroughSoftwareReportDtoGrid>}
   * @memberof QueryResultDtoOfPassThroughSoftwareReportDtoGrid
   */
  items?: Array<PassThroughSoftwareReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface PassThroughSoftwareReportQueryObjectGrid
  extends ReportQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughSoftwareReportQueryDto
   */
  assetVirtualized?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof PassThroughSoftwareReportQueryDto
   */
  nonTemsVertical?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughSoftwareReportQueryDto
   */
  softwareRelease?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof PassThroughSoftwareReportQueryDto
   */
  vendorEndOfVulnerabilitySecuritySupportDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughSoftwareReportQueryDto
   */
  cloudVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughSoftwareReportQueryDto
   */
  outputToLcmSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughSoftwareReportQueryDto
   */
  lcmStatusOpsSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughSoftwareReportQueryDto
   */
  lcmStatusEngSoftware?: Array<string>;
  ViewMode?: ReportViewMode;
  lcmExportDescription?: string;
}
export interface PassThroughSoftwareReportDownload {
  file: Blob | null;
}

export interface PassThroughSoftwareReportGrid {
  PassThroughSoftwareReportGridResult: QueryResultDtoOfPassThroughSoftwareReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_GRID_PASSTHROUGH_SOFTWARE_REPORT =
  "GET_GRID_PASSTHROUGH_SOFTWARE_REPORT";
export const GET_FILTER_PASSTHROUGH_SOFTWARE_REPORT =
  "GET_FILTER_PASSTHROUGH_SOFTWARE_REPORT";
export const DOWNLOAD_PASSTHROUGH_SOFTWARE_REPORT =
  "DOWNLOAD_PASSTHROUGH_SOFTWARE_REPORT";
