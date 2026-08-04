import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { DateFilter, QueryObject, RenderDetail } from "./Common";

export interface GenericReportGrid {
  GenericReportGridResult: QueryResultDtoOfGenericReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface GenericAggregatedReportGrid {
  GenericAggregatedReportGridResult: any | null;
  filter: any;
}

export interface GenericDisAggregatedReportGrid {
  GenericDisAggregatedReportGridResult: any | null;
  filter: any;
}

export interface GenericPreviewReportGrid {
  GenericPreviewReportGridResult: any;
  filter: any;
}

/**
 *
 * @export
 * @interface ViewGenericRenderDetail
 */
export interface ViewGenericRenderDetail {
  /**
   *
   * @type {string}
   * @memberof ViewGenericRenderDetail
   */
  tableName: string;
  /**
   *
   * @type {string}
   * @memberof ViewGenericRenderDetail
   */
  propertyName: string;
  /**
   *
   * @type {string}
   * @memberof ViewGenericRenderDetail
   */
  updatedPropertyName: string;
  /**
   *
   * @type {boolean}
   * @memberof ViewGenericRenderDetail
   */
  show: boolean;
  /**
   *
   * @type {number}
   * @memberof ViewGenericRenderDetail
   */
  order: number;
  /**
   *
   * @type {string}
   * @memberof ViewGenericRenderDetail
   */
  colorHeader?: string;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfPreviewGenericReportDtoGrid
 */
export interface QueryResultDtoOfPreviewGenericReportDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfPreviewGenericReportDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<any>}
   * @memberof QueryResultDtoOfPreviewGenericReportDtoGrid
   */
  items?: Array<any>;
  /**
   *
   * @type {CustomGridRenderOfPreviewGenericReportDtoGrid}
   * @memberof QueryResultDtoOfPreviewGenericReportDtoGrid
   */
  gridRender?: CustomGridRenderOfPreviewGenericReportDtoGrid;
}

export interface GenericViewReportQueryObjectGrid
  extends GenericReportQueryObjectGrid {
  lcmEngineeringId?: Array<number>;
  designComponentId?: Array<number>;
  lcmAncillaryDataId?: Array<number>;
  productCode?: Array<string>;
  lastScanDate?: DateFilter;
  lastUpgradeDate?: DateFilter;
  opCoId?: Array<number>;
  opCo?: Array<string>;
  softwareEndOfWarrantyDate?: DateFilter;
  numberOfNodes?: Array<number>;
  productImportanceId?: Array<number>;
  warranty?: Array<boolean>;
  numberOfNodesInLab?: Array<number>;
  hardwareSheetIndex?: Array<string>;
  softwareSheetIndex?: Array<string>;
  onHardware?: Array<boolean>;
  onSoftware?: Array<boolean>;
  fullOrPartialSupportId?: Array<number>;
  hardwareEndOfSupportContract?: DateFilter;
  hardwareSupportedId?: Array<number>;
  renewalinProgress?: Array<boolean>;
  softwareEndOfSupportContract?: DateFilter;
  softwareSupportedId?: Array<number>;
  sparesProvisioned?: Array<boolean>;
  vendorEndMntDateHw?: DateFilter;
  vendorEndMnteDateSw?: DateFilter;
  elementCount?: Array<boolean>;
  hardwareSupportProvider?: Array<string>;
  hardwareSupportType?: Array<string>;
  lcmStatusEngHardware?: Array<string>;
  lcmStatusEngSoftware?: Array<string>;
  lcmStatusHardware?: Array<string>;
  lcmStatusOpsHardware?: Array<string>;
  lcmStatusOpsSoftware?: Array<string>;
  lcmStatusSoftware?: Array<string>;
  outputToLcmHardware?: Array<string>;
  outputToLcmSoftware?: Array<string>;
  softwareSupportProvider?: Array<string>;
  softwareSupportType?: Array<string>;
  fullOrPartialSupportHwId?: Array<number>;
  operationalContract?: Array<string>;
  lcmSpreadSheetHwId?: Array<string>;
  lcmSpreadSheetSwId?: Array<string>;
  archived?: Array<boolean>;
  lcmDeploymentStatus?: Array<string>;
  resourceKey?: Array<string>;
  previousResourceKey?: Array<string>;
  isExtendedSupportOfferedByVendor?: Array<boolean>;
  hwIsExtendedSupportOfferedByVendor?: Array<boolean>;
  engKpi2?: Array<string>;
  ipAddress?: Array<string>;
  managedByGdc?: Array<string>;
  expLcmStatusAtEndOfFY24?: Array<string>;
  engineeringContactPoint?: Array<string>;
  operationsContactPoint?: Array<string>;
  vendorEndOfVulnerabilitySecuritySupportDateValue?: DateFilter;
  assetServiceFunctionality?: Array<string>;
  networkElementAsPlannedId?: Array<string>;
  automatedFeedback?: Array<boolean>;
  plannedAction?: Array<boolean>;
  environment?: Array<string>;
  deploymentStatusId?: Array<number>;
  deploymentTypeId?: Array<number>;
  locationName?: Array<string>;
  nfviBundleidId?: Array<number>;
  orgEqpManufacturerId?: Array<number>;
  capacityPlanReference?: Array<string>;
  elementName?: Array<string>;
  additionalInformation1?: Array<string>;
  networkConstruct?: Array<string>;
  additionalInformation2?: Array<string>;
  hwResourceKey?: Array<string>;
  previousHwResourceKey?: Array<string>;
  swResourceKey?: Array<string>;
  previousSwResourceKey?: Array<string>;
  meStatus?: Array<string>;
  meVirtualFlg?: Array<string>;
  meDeploymentType?: Array<string>;
  meType?: Array<string>;
  meSerialNumber?: Array<string>;
  meExternalConnectionFlg?: Array<string>;
  hardwareModules?: Array<string>;
  hardwareManfacturer?: Array<string>;
  lastCheckedTime?: Array<string>;
  plannedActivityId?: Array<number>;
  plannedImplementationYear?: Array<number>;
  activityStatusId?: Array<number>;
  planningActivityStatusId?: Array<number>;
  plannedActivity?: Array<string>;
  activityDetails?: Array<string>;
  ragStatus?: Array<string>;
  deliveryProjectName?: Array<string>;
  localApproval?: Array<string>;
  deliveryStatusId?: Array<number>;
  responsibilityPhaseId?: Array<number>;
  plannedCompletion?: DateFilter;
  plannedSpareFieldsJson?: Array<string>;
  relatestoId?: Array<number>;
  budgetValue?: Array<number>;
  plannedActivityResource?: Array<string>;
  activityDetailsText?: Array<string>;
  budgetAvailabilityId?: Array<number>;
  engineeringRiskId?: Array<number>;
  operationalRiskId?: Array<number>;
  linkedToPlannedActivityId?: Array<number>;
  isNewServiceArchitecture?: Array<boolean>;
  isReplacementExistingSolution?: Array<boolean>;
  benefitId?: Array<number>;
  driverId?: Array<number>;
  planningRiskId?: Array<number>;
  currency?: Array<string>;
  notes?: Array<string>;
  overAllRiskEvaluation?: Array<string>;
  deliveryProjectId?: Array<string>;
  planningRisk?: Array<string>;
  projectStatus?: Array<string>;
  riskEngineeringNotes?: Array<string>;
  riskOperationalNotes?: Array<string>;
  budgetTrackingId?: Array<string>;
  designAspectId?: Array<number>;
  designComponentFamilyId?: Array<number>;
  startDate?: DateFilter;
  forAddAsset?: Array<boolean>;
  forEditAsset?: Array<boolean>;
  originalLcmEngineeringId?: Array<number>;
  deliveryPlanAvailable?: Array<boolean>;
  program?: Array<string>;
  projectOwner?: Array<string>;
  budgetEstimated?: Array<string>;
  bundleBudget?: Array<string>;
  bundleId?: Array<string>;
  systemTypeId?: Array<number>;
  systemTypeNameVodafone?: Array<string>;
  systemTypeName3gpp?: Array<string>;
  systemTypeNameOem?: Array<string>;
  majorSoftwareBuildsId?: Array<number>;
  constraintsCaling?: DateFilter;
  endOfMaintenanceValue?: DateFilter;
  verticalResponsible?: Array<string>;
  buildConstruction?: Array<string>;
  subDomainResponsible?: Array<string>;
  subDomainsPoc?: Array<string>;
  assetCategoryId?: Array<number>;
  assetClassId?: Array<number>;
  assetTypeId?: Array<number>;
  assetClass?: Array<string>;
  constraintLcm?: Array<string>;
  vodafoneName?: Array<string>;
  assetCategory?: Array<string>;
  takeFromAssetTypeTable?: Array<boolean>;
  id?: Array<number>;
  subDescription?: Array<string>;
  default?: Array<boolean>;
  alias?: Array<string>;
  order?: Array<number>;
  swApplicationName?: Array<string>;
  gdprRelevant?: Array<boolean>;
  internetFacing?: Array<boolean>;
  lcmPolicy?: Array<string>;
  criticality?: Array<string>;
  securityElement?: Array<boolean>;
  gdprClassification?: Array<number>;
  pciSox?: Array<boolean>;
  c3C4?: Array<boolean>;
  missionCritical?: Array<boolean>;
  productNameId?: Array<number>;
  vodafoneNameId?: Array<number>;
  productImportance?: Array<string>;
  orgEqpmanufacturer?: Array<string>;
  softwareVersion?: Array<string>;
  lastTimeBuyNew?: DateFilter;
  lastTimeBuyUpgrades?: DateFilter;
  lastTimeBuyExpansions?: DateFilter;
  majEndOfMaintenance?: DateFilter;
  endOfSupport?: DateFilter;
  generaAvailableDate?: DateFilter;
  deliveryMethod?: Array<string>;
  majSpareFieldsJson?: Array<string>;
  operatingSystemId?: Array<number>;
  vulnerabilityStatus?: Array<string>;
  eomStatus?: Array<number>;
  criticalAssetType?: Array<string>;
  productName?: Array<string>;
  majorDescription?: Array<string>;
  platform?: Array<string>;
  majorHardwareId?: Array<number>;
  hardwareSolution?: Array<string>;
  otherHardwareInfo?: Array<string>;
  hardwareLastTimeBuyNew?: DateFilter;
  hardwareLastTimeBuyUpgrades?: DateFilter;
  hardwareLastTimeBuyExpansions?: DateFilter;
  hardwareEndofmaintenance?: DateFilter;
  hardwareEndofsupport?: DateFilter;
  proprietaryHardware?: Array<boolean>;
  platformId?: Array<number>;
  buildconstructionid?: Array<number>;
  hardwareType?: Array<string>;
  operatingSystem?: Array<string>;
  typeOfProcessor?: Array<string>;
  hardwareVulnerabilityStatus?: Array<string>;
  hardwareEomStatus?: Array<number>;
  generalAvailabledate?: DateFilter;
  hardwareModel?: Array<string>;
  hardwareDescription?: Array<string>;
  riskId?: Array<number>;
  severity?: Array<number>;
  riskDescription?: Array<string>;
  reportName?: Array<string>;
  published?: Array<boolean>;
  render?: Array<any>;
  ViewMode?: number;
}
export interface GenericReportQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof GenericReportDtoGrid
   */
  dynamicReportId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof GenericReportDtoGrid
   */
  userId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof GenericReportDtoGrid
   */
  jsonGridCustomizationData?: Array<string>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof GenericReportDtoGrid
   */
  published?: Array<boolean>;
  /**
   *
   * @type {DateFilter}
   * @memberof GenericReportDtoGrid
   */
  isScheduled?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof GenericReportDtoGrid
   */
  creationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof GenericReportDtoGrid
   */
  creationDate?: DateFilter;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  modificationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof GenericReportDtoGrid
   */
  modificationDate?: DateFilter;
}
export interface GenericPreviewReportQueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof GenericReportDtoGrid
   */
  dynamicReportId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof GenericReportDtoGrid
   */
  ViewMode?: number;
}
/**
 *
 * @param {Array<number>} [dynamicReportsId]
 * @param {number} [ViewMode]
 * @param {Array<number>} [userId]
 * @param {Array<string>} [jsonGridCustomizationData]
 * @param {Array<boolean>} [published]
 * @param {string} [isScheduled]
 * @param {Array<string>} [creationUser]
 * @param {Array<string>} [modificationUser]
 * @param {string} [sortBy]
 * @param {boolean} [isSortAscending]
 * @param {number} [page]
 * @param {number} [pageSize]
 * @param {Date} [lastModifiedStartDate]
 * @param {Date} [lastModifiedEndDate]
 * @param {number} [principalId]
 * @param {boolean} [deleted]
 * @param {boolean} [orphan]
 * @param {Array<string>} [lastModifiedBy]
 * @param {*} [options] Override http request option.
 * @throws {RequiredError}
 */

export interface GenericReportAssociated {
  /**
   *
   * @type {string}
   * @memberof GenericReportAssociated
   */
  tableName?: string;
  /**
   *
   * @type {boolean}
   * @memberof GenericReportAssociated
   */
  checked?: boolean;
  /**
   *
   * @type {string}
   * @memberof GenericReportAssociated
   */
  propertyName?: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportAssociated
   */
  updatedPropertyName?: string;
  /**
   *
   * @type {boolean}
   * @memberof GenericReportAssociated
   */
  archive: boolean;
  /**
   *
   * @type {boolean}
   * @memberof GenericReportAssociated
   */
  ignore: boolean;
  /**
   *
   * @type {boolean}
   * @memberof GenericReportAssociated
   */
  show: boolean;
  /**
   *
   * @type {string}
   * @memberof GenericReportAssociated
   */
  tab: string;
  /**
   *
   * @type {number}
   * @memberof GenericReportAssociated
   */
  type: number;
  /**
   *
   * @type {boolean}
   * @memberof GenericReportAssociated
   */
  fieldEdit: boolean;
}

export interface GenericReportResponse {
  push(arg0: {
    order: any;
    tableName: any;
    propertyName: any;
    updatedPropertyName: any;
    colorHeader: any;
    show: any;
  }): unknown;
  /**
   *
   * @type {string}
   * @memberof GenericReportResponse
   */
  order?: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportResponse
   */
  tableName?: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportResponse
   */
  propertyName?: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportResponse
   */
  updatedPropertyName?: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportResponse
   */
  colorHeader?: string;
  /**
   *
   * @type {boolean}
   * @memberof GenericReportResponse
   */
  show?: boolean;
}

export interface TableAndPropertiesResponse {
  [key: string]: Array<RenderDetail>;
}
export interface GetGenericReportResponse {
  getAllOpcos: { [key: string]: any };
  userReportTablesAndProperties: TableAndPropertiesResponse;
}

export interface CreateGenericReportBody {
  scheduledDate: any;
  scheduledDayInWeek: any;
  reportId?: number;
  reportName: Array<string>;
  published?: Array<boolean>;
  dynamicReportId?: Array<number>;
  scheduled?: Date;
  render: Array<Reports>;
  opco: Array<string>;
}
export interface Reports {
  order: number;
  tableName: string;
  columnName: string;
  updatedColumnName: string;
  colorHeader: string;
  show: boolean;
}
/**
 *
 * @export
 * @interface GenericReportDtoGrid
 */
export interface GenericReportDtoGrid {
  deleted: boolean;
  /**
   *
   * @type {boolean}
   * @memberof GenericReportDtoGrid
   */
  orphan: boolean;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  lastModified: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  lastModifiedBy: string;
  /**
   *
   * @type {number}
   * @memberof GenericReportDtoGrid
   */
  dynamicReportId: number;
  /**
   *
   * @type {number}
   * @memberof GenericReportDtoGrid
   */
  userId?: number;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  reportName: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  jsonGridCustomizationData: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  published: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  isScheduled: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  creationUser?: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  creationDate: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  modificationUser?: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  modificationDate?: string;
}
/**
 *
 * @export
 * @interface CustomGridRenderOfGenericReportDtoGrid
 */
export interface CustomGridRenderOfGenericReportDtoGrid {
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfGenericReportDtoGrid
   */
  render?: Array<RenderDetail>;
}
/**
 *
 * @export
 * @interface CustomGridRenderOfPreviewGenericReportDtoGrid
 */
export interface CustomGridRenderOfPreviewGenericReportDtoGrid {
  /**
   *
   * @type {Array<ViewGenericRenderDetail>}
   * @memberof CustomGridRenderOfPreviewGenericReportDtoGrid
   */
  render?: Array<ViewGenericRenderDetail>;
  reportName?: string | undefined;
  exportFilePath?: string;
  exportType?: string;
  exportFileFormat?: string;
  isExportReport?: boolean;
  scheduledDate?: string;
  /**
   *
   * @type {boolean}
   * @memberof CustomGridRenderOfPreviewGenericReportDtoGrid
   */
  published?: boolean;
  /**
   *
   * @type {number}
   * @memberof CustomGridRenderOfPreviewGenericReportDtoGrid
   */
  dynamicReportId?: number;
  /**
   *
   * @type {number}
   * @memberof CustomGridRenderOfPreviewGenericReportDtoGrid
   */
  ViewMode?: number;
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfPreviewGenericReportDtoGrid
   */
  opcoId?: Array<string>;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfGenericReportDtoGrid
 */
export interface QueryResultDtoOfGenericReportDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfGenericReportDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfGenericReportDtoGrid
   */
  items?: Array<GenericReportDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfGenericReportDtoGrid}
   * @memberof QueryResultDtoOfGenericReportDtoGrid
   */
  gridRender?: CustomGridRenderOfGenericReportDtoGrid;
}
export const genericPaginationQuery: GenericViewReportQueryObjectGrid = {
  lcmEngineeringId: [],
  designComponentId: [],
  lcmAncillaryDataId: [],
  productCode: [],
  lastScanDate: undefined,
  lastUpgradeDate: undefined,
  opCoId: [],
  opCo: [],
  softwareEndOfWarrantyDate: undefined,
  numberOfNodes: [],
  productImportanceId: [],
  warranty: [],
  numberOfNodesInLab: [],
  hardwareSheetIndex: [],
  softwareSheetIndex: [],
  onHardware: [],
  onSoftware: [],
  fullOrPartialSupportId: [],
  hardwareEndOfSupportContract: undefined,
  hardwareSupportedId: [],
  renewalinProgress: [],
  softwareEndOfSupportContract: undefined,
  softwareSupportedId: [],
  sparesProvisioned: [],
  vendorEndMntDateHw: undefined,
  vendorEndMnteDateSw: undefined,
  elementCount: [],
  hardwareSupportProvider: [],
  hardwareSupportType: [],
  lcmStatusEngHardware: [],
  lcmStatusEngSoftware: [],
  lcmStatusHardware: [],
  lcmStatusOpsHardware: [],
  lcmStatusOpsSoftware: [],
  lcmStatusSoftware: [],
  outputToLcmHardware: [],
  outputToLcmSoftware: [],
  softwareSupportProvider: [],
  softwareSupportType: [],
  fullOrPartialSupportHwId: [],
  operationalContract: [],
  lcmSpreadSheetHwId: [],
  lcmSpreadSheetSwId: [],
  archived: [],
  lcmDeploymentStatus: [],
  resourceKey: [],
  previousResourceKey: [],
  isExtendedSupportOfferedByVendor: [],
  hwIsExtendedSupportOfferedByVendor: [],
  engKpi2: [],
  ipAddress: [],
  managedByGdc: [],
  expLcmStatusAtEndOfFY24: [],
  engineeringContactPoint: [],
  operationsContactPoint: [],
  vendorEndOfVulnerabilitySecuritySupportDateValue: undefined,
  assetServiceFunctionality: [],
  networkElementAsPlannedId: [],
  automatedFeedback: [],
  plannedAction: [],
  environment: [],
  deploymentStatusId: [],
  deploymentTypeId: [],
  locationName: [],
  nfviBundleidId: [],
  orgEqpManufacturerId: [],
  capacityPlanReference: [],
  elementName: [],
  additionalInformation1: [],
  networkConstruct: [],
  additionalInformation2: [],
  hwResourceKey: [],
  previousHwResourceKey: [],
  swResourceKey: [],
  previousSwResourceKey: [],
  meStatus: [],
  meVirtualFlg: [],
  meDeploymentType: [],
  meType: [],
  meSerialNumber: [],
  meExternalConnectionFlg: [],
  hardwareModules: [],
  hardwareManfacturer: [],
  lastCheckedTime: [],
  plannedActivityId: [],
  plannedImplementationYear: [],
  activityStatusId: [],
  planningActivityStatusId: [],
  plannedActivity: [],
  activityDetails: [],
  ragStatus: [],
  deliveryProjectName: [],
  localApproval: [],
  deliveryStatusId: [],
  responsibilityPhaseId: [],
  plannedCompletion: undefined,
  plannedSpareFieldsJson: [],
  relatestoId: [],
  budgetValue: [],
  plannedActivityResource: [],
  activityDetailsText: [],
  budgetAvailabilityId: [],
  engineeringRiskId: [],
  operationalRiskId: [],
  linkedToPlannedActivityId: [],
  isNewServiceArchitecture: [],
  isReplacementExistingSolution: [],
  benefitId: [],
  driverId: [],
  planningRiskId: [],
  currency: [],
  notes: [],
  overAllRiskEvaluation: [],
  deliveryProjectId: [],
  planningRisk: [],
  projectStatus: [],
  riskEngineeringNotes: [],
  riskOperationalNotes: [],
  budgetTrackingId: [],
  designAspectId: [],
  designComponentFamilyId: [],
  startDate: undefined,
  forAddAsset: [],
  forEditAsset: [],
  originalLcmEngineeringId: [],
  deliveryPlanAvailable: [],
  program: [],
  projectOwner: [],
  budgetEstimated: [],
  bundleBudget: [],
  bundleId: [],
  systemTypeId: [],
  systemTypeNameVodafone: [],
  systemTypeName3gpp: [],
  systemTypeNameOem: [],
  majorSoftwareBuildsId: [],
  constraintsCaling: undefined,
  endOfMaintenanceValue: undefined,
  verticalResponsible: [],
  buildConstruction: [],
  subDomainResponsible: [],
  subDomainsPoc: [],
  assetCategoryId: [],
  assetClassId: [],
  assetTypeId: [],
  assetClass: [],
  constraintLcm: [],
  vodafoneName: [],
  assetCategory: [],
  takeFromAssetTypeTable: [],
  id: [],
  subDescription: [],
  default: [],
  alias: [],
  order: [],
  swApplicationName: [],
  gdprRelevant: [],
  internetFacing: [],
  lcmPolicy: [],
  criticality: [],
  securityElement: [],
  gdprClassification: [],
  pciSox: [],
  c3C4: [],
  missionCritical: [],
  productNameId: [],
  vodafoneNameId: [],
  productImportance: [],
  orgEqpmanufacturer: [],
  softwareVersion: [],
  lastTimeBuyNew: undefined,
  lastTimeBuyUpgrades: undefined,
  lastTimeBuyExpansions: undefined,
  majEndOfMaintenance: undefined,
  endOfSupport: undefined,
  generaAvailableDate: undefined,
  deliveryMethod: [],
  majSpareFieldsJson: [],
  operatingSystemId: [],
  vulnerabilityStatus: [],
  eomStatus: [],
  criticalAssetType: [],
  productName: [],
  majorDescription: [],
  platform: [],
  majorHardwareId: [],
  hardwareSolution: [],
  otherHardwareInfo: [],
  hardwareLastTimeBuyNew: undefined,
  hardwareLastTimeBuyUpgrades: undefined,
  hardwareLastTimeBuyExpansions: undefined,
  hardwareEndofmaintenance: undefined,
  hardwareEndofsupport: undefined,
  proprietaryHardware: [],
  platformId: [],
  buildconstructionid: [],
  hardwareType: [],
  operatingSystem: [],
  typeOfProcessor: [],
  hardwareVulnerabilityStatus: [],
  hardwareEomStatus: [],
  generalAvailabledate: undefined,
  hardwareModel: [],
  hardwareDescription: [],
  riskId: [],
  severity: [],
  riskDescription: [],
  sortBy: "",
  isSortAscending: true,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: [],
  dynamicReportId: [],
  userId: [],
  jsonGridCustomizationData: [],
  published: [],
  isScheduled: undefined,
  creationUser: [],
  creationDate: undefined,
  modificationUser: [],
  modificationDate: undefined,
  reportName: [],
  ViewMode: undefined,
};

export const GET_GRID_GENERIC_REPORT = "GET_GRID_GENERIC_REPORT";
export const GET_FILTER_GENERIC_REPORT = "GET_FILTER_GENERIC_REPORT";
export const GET_GRID_AGGREGATED_GENERIC_REPORT =
  "GET_GRID_AGGREGATED_GENERIC_REPORT";
export const GET_GRID_DISAGGREGATED_GENERIC_REPORT =
  "GET_GRID_DISAGGREGATED_GENERIC_REPORT";
export const GET_GRID_GENERIC_PREVIEW_REPORT =
  "GET_GRID_GENERIC_PREVIEW_REPORT";
export const GET_FILTER_AGGREGATED_GENERIC_REPORT =
  "GET_FILTER_AGGREGATED_GENERIC_REPORT";
export const GET_FILTER_DISAGGREGATED_GENERIC_REPORT =
  "GET_FILTER_DISAGGREGATED_GENERIC_REPORT";
export const GET_FILTER_GENERIC_PREVIEW_REPORT =
  "GET_FILTER_GENERIC_PREVIEW_REPORT";
