import { QueryObject, DateFilter } from "../Common";

/**
 *
 * @export
 * @interface GetDcfNamesQueryDto
 */
export interface GetDcfNamesQueryDto extends QueryObject {
  /**
   *
   * @type {Array<string>}
   * @memberof GetDcfNamesQueryDto
   */
  systemTypeIdentityName?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof GetDcfNamesQueryDto
   */
  designComponentFamilyName?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof GetDcfNamesQueryDto
   */
  description?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof GetDcfNamesQueryDto
   */
  subNetworkBoundary?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof GetDcfNamesQueryDto
   */
  subNetworkBoundaryId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof GetDcfNamesQueryDto
   */
  networkFunction?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof GetDcfNamesQueryDto
   */
  supportedServices?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof GetDcfNamesQueryDto
   */
  vodafoneName?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof GetDcfNamesQueryDto
   */
  designComponentFamilyId?: Array<number>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof GetDcfNamesQueryDto
   */
  implementation?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof GetDcfNamesQueryDto
   */
  systemIsShared?: Array<boolean>;
  /**
   *
   * @type {Array<number>}
   * @memberof GetDcfNamesQueryDto
   */
  criticalityRating?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof GetDcfNamesQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof GetDcfNamesQueryDto
   */
  sharingType?: Array<number>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof GetDcfNamesQueryDto
   */
  countrySpecificCriticality?: Array<boolean>;
  /**
   *
   * @type {Array<number>}
   * @memberof GetDcfNamesQueryDto
   */
  productName?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof GetDcfNamesQueryDto
   */
  lastModifiedValue?: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof GetDcfNamesQueryDto
   */
  systemTypeIdBasedVerticalId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof GetDcfNamesQueryDto
   */
  designContact?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof GetDcfNamesQueryDto
   */
  systemTypeIdBasedVerticalValue?: Array<string>;
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
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  previousReportId?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ReportQueryDto
   */
  name?: string;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  localMarket?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportQueryDto
   */
  localMarketId?: Array<number>;
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
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  assetClass?: Array<string>;
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
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  hardwareModel?: Array<string>;
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
  vendorEndOfMaintenanceDateValue?: DateFilter;
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
  plannedSoftwareVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  plannedHardwareModel?: Array<string>;
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
  projectEndDateValue?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportQueryDto
   */
  vendorEndOfVulnerabilitySecuritySupportDateValueLcm?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportQueryDto
   */
  opsMaintenanceConractEndValueLcm?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  assetStatusFY26?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  assetStatusFY28?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  applicationOrOperationgSys?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  requestIDLcm?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  eoslKpiTarget?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  securityRiskOverallLcm?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  riskComment?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  programLcm?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  trackingNumberProjectNameLcm?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  eoslKpiFrozen?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  eoslKpiForecast?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  exceptionFlag?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  numberOfNodesLcm?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  operationsMaintenanceContractLcm?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  dataSourceLcm?: Array<string>;
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
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  platform?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportQueryDto
   */
  opsMaintenanceConractEndValue?: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportQueryDto
   */
  engRiskEvaluation?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  engRiskEvaluationNotes?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportQueryDto
   */
  opsRiskEvaluation?: Array<number>;
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
  engkpI2?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  expLCMstatusatendofFY24?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportQueryDto
   */
  riskCluster?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  criticality?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  gdprRelevant?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  deliveryPlanAvailable?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  hostname?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  ipAddress?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  ragStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  wbsCode?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  bptID?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  ppmID?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  serialNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  assetStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  program?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  projectOwner?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  reasonfornoPlan?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  commentonProjectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  securityRiskPotential?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  securityRiskEffective?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  vulnerabilityRating?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  securityMitigation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  securityRiskOverall?: Array<string>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof ReportQueryDto
   */
  includedinSecurityScanning?: Array<boolean>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  raId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  cyberRiskRequestId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  requestID?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportQueryDto
   */
  lastScanDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportQueryDto
   */
  lastScanDateValue?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  assetOutofScopeForReportingPurposes?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportQueryDto
   */
  lastUpgradeDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportQueryDto
   */
  lastUpgradeDateValue?: DateFilter;
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
  exNetworks?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  newopsRiskEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  occurrenceProbability?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  incidentClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  productCode?: Array<string>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof ReportQueryDto
   */
  handedOverToOperation?: Array<boolean>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  contractRenewalPlan?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  dataSource?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  scopeOfSimplification?: Array<string>;
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
   * @type {Array<number>}
   * @memberof ReportQueryDto
   */
  verticalEngineeringTeamId?: Array<number>;
  /**
   *
   * @type {Array<string>}
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
   * @type {Array<string>}
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
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  infrastructureLocation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  category?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportQueryDto
   */
  interfaceType?: Array<string>;
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
 * @interface ReportNetworkLevel2QueryGrid
 */
export interface ReportNetworkLevel2QueryGrid extends ReportQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  assetVirtualized?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  softwareVersion?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  vendorEndOfVulnerabilitySecuritySupportDateValue?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  outputToLcmSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  lcmStatusOpsSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  lcmStatusEngSoftware?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  typeOfNetworkElement?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  originalLCMSpreadsheetID?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  isExtendedSupportOfferedByVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  labSWRelease?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  originalSwLcmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  lcmStatusEngHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  lcmStatusOpsHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  outputToLcmHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  originalHwLcmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  hwIsExtendedSupportOfferedByVendor?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  designComponentFamily?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportNetworkLevel2QueryGrid
   */
  supportedService?: Array<number>;
}
/**
 *
 * @export
 * @interface ReportSubBoundHwSwQueryGrid
 */
export interface ReportSubBoundHwSwQueryGrid extends ReportQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  assetVirtualized?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  softwareVersion?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  vendorEndOfVulnerabilitySecuritySupportDateValue?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  outputToLcmSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  lcmStatusOpsSoftware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  lcmStatusEngSoftware?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  typeOfNetworkElement?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  originalLCMSpreadsheetID?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  isExtendedSupportOfferedByVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  labSWRelease?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  originalSwLcmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  lcmStatusEngHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  lcmStatusOpsHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  outputToLcmHardware?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  originalHwLcmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  hwIsExtendedSupportOfferedByVendor?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  designComponentFamily?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportSubBoundHwSwQueryGrid
   */
  supportedService?: Array<number>;
}

export interface ExportDownload {
  file: Blob | null;
}

export const DOWNLOAD_REPORT = "DOWNLOAD_REPORT";
