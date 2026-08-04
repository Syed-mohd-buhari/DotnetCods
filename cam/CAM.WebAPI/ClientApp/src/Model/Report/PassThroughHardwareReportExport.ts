import { FilterValueDto } from "../../Business/Common/CommonBusiness";

import {
  CustomGridRender,
  EOM_STATUS,
  QueryObjectGrid,
  DateFilter,
  ReportViewMode,
  QueryObject,
} from "../Common";
import { ReportQueryDto } from "./Export";
import { PassThroughReportQueryDto } from "./PassThroughReportExport";
/**
 *
 * @export
 * @interface PassThroughHardwareReportDto
 */
export interface PassThroughHardwareReportDto {
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  reportId?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  localMarket?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  verticalEngineeringTeam?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  verticalSubDomain?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  engineeringContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  operationsContactPoint?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  assetCategory?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  assetClass?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  assetType?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  assetDescription?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  productImportance?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  vendor?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  hardwareModel?: string;
  /**
   *
   * @type {number}
   * @memberof PassThroughHardwareReportDto
   */
  numberOfNodes?: number;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  operationsMaintenanceContract?: string;
  /**
   *
   * @type {Date}
   * @memberof PassThroughHardwareReportDto
   */
  vendorEndOfMaintenanceDate?: Date;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  lcmStatus?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  identifiedAction?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  descriptionOfPlannedAction?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  plannedHardwareModel?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  projectStatus?: string;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughReportQueryDto
   */
  projectEndDate?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  trackingNumberProjectName?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  budgetEstimated?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  notes?: string;
  /**
   *
   * @type {number}
   * @memberof PassThroughHardwareReportDto
   */
  systemTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughHardwareReportDto
   */
  designComponentId?: number;

  /**
   *
   * @type {number}
   * @memberof PassThroughHardwareReportDto
   */
  designComponentFamilyId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughHardwareReportDto
   */
  majorSoftwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughHardwareReportDto
   */
  majorHardwareBuildId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughHardwareReportDto
   */
  lcmEngineeringId?: number;
  /**
   *
   * @type {number}
   * @memberof PassThroughHardwareReportDto
   */
  plannedActivityId?: number;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  assetOutofScopeForReportingPurposes?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  managedByGdc?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  extendedSupportOptionOfferedByVendor?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  eomControl?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  engUpdateTracker?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDto
   */
  opsUpdateTracker?: string;
}
/**
 *
 * @export
 * @interface PassThroughHardwareReportDtoGrid
 */
export interface PassThroughHardwareReportDtoGrid
  extends PassThroughHardwareReportDto {
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  bundleBudget?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  bundleId?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  assetServiceFunctionality?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  platform?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  lcmStatusEngHardware?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  lcmStatusOpsHardware?: string;
  /**
   *
   * @type {Date}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  opsMaintenanceConractEnd?: Date;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  engRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  engRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  opsRiskEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  opsRiskEvaluationNotes?: string;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  overallRiskEvaluation?: string;

  eomStatus: EOM_STATUS;

  /**
   *
   * @type {boolean}
   * @memberof PassThroughHardwareReportDtoGrid
   */
  archived?: boolean;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfPassThroughHardwareReportDtoGrid
 */
export interface QueryResultDtoOfPassThroughHardwareReportDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfPassThroughHardwareReportDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<PassThroughHardwareReportDtoGrid>}
   * @memberof QueryResultDtoOfPassThroughHardwareReportDtoGrid
   */
  items?: Array<PassThroughHardwareReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface PassThroughHardwareReportQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {string}
   * @memberof QueryObject
   */
  sortBy?: string;
  /**
   *
   * @type {boolean}
   * @memberof QueryObject
   */
  isSortAscending?: boolean;
  /**
   *
   * @type {number}
   * @memberof QueryObject
   */
  page?: number;
  /**
   *
   * @type {number}
   * @memberof QueryObject
   */
  pageSize?: number;
  /**
   *
   * @type {Array<number>}
   * @memberof PassThroughReportQueryDto
   */
  passThroughLcmId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  reportId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  localMarket?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  verticalEngineeringTeam?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  verticalSubDomain?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  engineeringContactPoint?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  assetCategory?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  assetClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  assetType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  assetDescription?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  assetVirtualized?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  productImportance?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  hardwareModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  productCode?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  numberOfNodes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  handedOverToOperation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  contractRenewalPlan?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  lcmStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  identifiedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  descriptionOfPlannedAction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  plannedSoftwareVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  projectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  reasonfornoPlan?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  commentonProjectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  projectEndDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  ragStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  trackingNumberProjectName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  program?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  wbsCode?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  bptID?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  ppmID?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  scopeOfSimplification?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  dataSource?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  projectOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  budgetEstimated?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  notes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  bundleBudget?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  bundleId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  assetServiceFunctionality?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  platform?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  engRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  engRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  opsRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  opsRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  incidentClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  occurrenceProbability?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  newopsRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  overallRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  riskCluster?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  securityRiskPotential?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  vulnerabilityScore?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  comments?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  qId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  requestID?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  vulnerabilityRating?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  securityRiskEffective?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  securityMitigation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  securityRiskOverall?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  includedinSecurityScanning?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  raId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  lcmCumulativeRiskId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  lcmCumulativeRiskLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  cyberRiskRequestId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  criticality?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  assetStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  gdprRelevant?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof PassThroughHardwareReportQueryDto
   */
  lastScanDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  lastUpgradeDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  assetOutofScopeForReportingPurposes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  mainOrganization?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  isExtendedSupportOfferedByVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  eomControl?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  engUpdateTracker?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  opsUpdateTracker?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  typeOfNetworkElement?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  engKpi2?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  ipAddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  serialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  hostname?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  exNetworks?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  originalLcmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  hwOperationsContactPoint?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  hwVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  hwOperationsMaintenanceContract?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  hwVendorEndOfMaintenanceDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  plannedHWModel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  hwopsMaintenanceContractEndDate?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  lcmStatusEngHardware?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof PassThroughReportQueryDto
   */
  lastModifiedValue?: string;
  /**
   *
   * @type {Array<number>}
   * @memberof PassThroughReportQueryDto
   */
  nonTemsVertical?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  lcmStatusOpsHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PassThroughHardwareReportQueryDto
   */
  outputToLcmHardware?: Array<string>;
  ViewMode?: ReportViewMode;
  lcmExportDescription?: string;
}
export interface PassThroughHardwareReportDownload {
  file: Blob | null;
}
export interface PassThroughHardwareReportGrid {
  PassThroughHardwareReportGridResult: QueryResultDtoOfPassThroughHardwareReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_GRID_PASSTHROUGH_HARDWARE_REPORT =
  "GET_GRID_PASSTHROUGH_HARDWARE_REPORT";
export const GET_FILTER_PASSTHROUGH_HARDWARE_REPORT =
  "GET_FILTER__PASSTHROUGH_HARDWARE_REPORT";
export const DOWNLOAD_PASSTHROUGH_HARDWARE_REPORT =
  "DOWNLOAD_PASSTHROUGH_HARDWARE_REPORT";
