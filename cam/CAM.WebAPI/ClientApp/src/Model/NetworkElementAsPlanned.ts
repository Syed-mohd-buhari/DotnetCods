import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  QueryObjectGrid,
  GridDtoBase,
  RenderDetail,
  QueryObject,
} from "./Common";
import { RelatedResource, ResultDto } from "./CommonModels";
import { DeploymentStatusDto } from "./LookUp/DeploymentStatus";
import { LocationDto } from "./LookUp/Location";
import { PlannedActivityNetworkElementDtoUpdate } from "./LookUp/PlannedActivityNetworkElement";
import { PlannedActivityDtoUpdate } from "./PlannedActivity";

/**
 *
 * @export
 * @interface NetworkElementAsPlannedDto
 */
export interface NetworkElementAsPlannedDto extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDto
   */
  nodeIndex?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDto
   */
  elementName?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDto
   */
  capacityPlanReference?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDto
   */
  additionalInformation1?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDto
   */
  additionalInformation2?: string;

  linkedToNetworkAsIs?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof NetworkElementAsPlannedDto
   */
  automatedFeedback?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof NetworkElementAsPlannedDto
   */
  plannedAction?: boolean;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDto
   */
  networkConstruct?: string;
  /**
   *
   * @type {boolean}
   * @memberof NetworkElementAsPlannedDto
   */
  isNewServiceArchitecture?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof NetworkElementAsPlannedDto
   */
  isReplacementExistingSolution?: boolean;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDto
   */
  originalEquipmentManufacturerId?: number;
}
/**
 *
 * @export
 * @interface NetworkElementAsPlannedDtoCreate
 */
export interface NetworkElementAsPlannedDtoCreate
  extends NetworkElementAsPlannedDto {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  originalEquipmentManufacturerResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  buildBagId?: number;
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  buildBagResources?: Array<{ key: number; text: string }>;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  deploymentStatusId?: number;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  deploymentTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  locationId?: number;

  locationTypeId?: number;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  opCoReosurce?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  eduSpocResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  designComponentId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  designComponentReosurce?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  environmentId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  environmentReosurce?: { [key: string]: string };
  elementDomianName?: string;
  systemNames?: string;
  /**
   *
   * @type {{ [key: string]: DeploymentStatusDto; }}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  deploymentStatusReosurce?: { [key: string]: DeploymentStatusDto };
  /**
   *
   * @type {{ [key: string]: RelatedResource; }}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  deploymentTypeReosurce?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: LocationDto; }}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  locationReosurce?: { [key: string]: LocationDto };

  hwResourceKey?: string;
  previousHWResourceKey?: string;
  swResourceKey?: string;
  previousSWResourceKey?: string;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  assetsStatusId?: number;
  /**
   *
   * @type {{ [key: string]: DeploymentStatusDto; }}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  assetsStatus?: { [key: string]: DeploymentStatusDto };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  nfviBundleIDReosurce?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  subDomainSpocIds?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  eduSpocIds?: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  subDomainSpocResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  nfviBundleIDId?: number;
  /**
   *
   * @type {Date}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  assetLiveStatusDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  dateAssetDecommissionedAsset?: Date;
  /**
   *
   * @type {Array<PlannedActivityDtoUpdate>}
   * @memberof NetworkElementAsPlannedDtoCreate
   */
  plannedActivityDto?: Array<PlannedActivityDtoUpdate>;
}
/**
 *
 * @export
 * @interface NetworkElementAsPlannedDtoGrid
 */
export interface NetworkElementAsPlannedDtoGrid
  extends NetworkElementAsPlannedDto {
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  networkElementAsPlannedId?: number;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  opCo?: string;
  opCoId?: number;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  designComponentIndex?: number;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  designComponentFamilyIndex?: number;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  designComponent?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  environment?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  deploymentStatus?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  deploymentType?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  location?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  nfviBundleID?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  plannedActivity?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  subDomainSpoc?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsPlannedDtoGrid
   */
  eduspoc?: string;

  isVirtualizedOrContanarized?: boolean;
}
/**
 *
 * @export
 * @interface NetworkElementAsPlannedDtoUpdate
 */
export interface NetworkElementAsPlannedDtoUpdate
  extends NetworkElementAsPlannedDtoCreate {
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsPlannedDtoUpdate
   */
  networkElementAsPlannedId?: number;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfNetworkElementAsPlannedDtoGrid
 */
export interface QueryResultDtoOfNetworkElementAsPlannedDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfNetworkElementAsPlannedDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfNetworkElementAsPlannedDtoGrid
   */
  items?: Array<NetworkElementAsPlannedDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfNetworkElementAsPlannedDtoGrid}
   * @memberof QueryResultDtoOfNetworkElementAsPlannedDtoGrid
   */
  gridRender?: CustomGridRenderOfNetworkElementAsPlannedDtoGrid;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfNetworkElementAsPlannedDtoGrid
 */
export interface CustomGridRenderOfNetworkElementAsPlannedDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfNetworkElementAsPlannedDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfNetworkElementAsPlannedDtoGrid
   */
  render?: Array<RenderDetail>;
}

export interface AssestOverviewByMarketQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  subDomainResponseCeFunctionId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  subDomainResponseCeFunction?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  oemVendorId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  oemVendor?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  productNameNeInstances?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  networkElementsAsPlannedId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  networkElementsPlannedName?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  opcoId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  opCoDescrption?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  verticalId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  systemTypetId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  verticalDescrption?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  networkElementCount?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  dcfId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  hwBuild?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  supportService?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssestOverviewByMarketQueryObjectGrid
   */
  environmentId?: Array<number>;
}
// -------------------------No auto Generated---------------------------------

export interface NetworkElementAsPlannedQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  nodeIndex?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  opCo?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  designComponent?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  plannedActivity?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  designComponentIndex?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  elementName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  capacityPlanReference?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  additionalInformation1?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  additionalInformation2?: Array<string>;
  /**
   *
   * @type {boolean}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  automatedFeedback?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  plannedAction?: boolean;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  networkConstruct?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  environment?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  deploymentStatus?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  deploymentType?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  location?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  nfviBundleID?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  subDomainSpoc?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementAsPlannedQueryDto
   */
  eduspoc?: Array<string>;

  hwResourceKey?: Array<string>;
  previousHWResourceKey?: Array<string>;
  swResourceKey?: Array<string>;
  previousSWResourceKey?: Array<string>;
}

/**
 *
 * @param {string} [propertyName]
 * @param {string} [propertyFilter]
 * @param {Array<number>} [networkElementAsPlannedId]
 * @param {Array<number>} [opCo]
 * @param {Array<number>} [designComponent]
 * @param {Array<string>} [elementName]
 * @param {Array<string>} [capacityPlanReference]
 * @param {Array<string>} [AdditionalInformation1]
 * @param {Array<string>} [AdditionalInformation2]
 * @param {boolean} [automatedFeedback]
 * @param {boolean} [plannedAction]
 * @param {Array<string>} [networkConstruct]
 * @param {Array<number>} [environment]
 * @param {Array<number>} [deploymentStatus]
 * @param {Array<number>} [deploymentType]
 * @param {Array<number>} [location]
 * @param {Array<number>} [nfviBundleID]
 * @param {Array<string>} [subDomainSpoc]
 * @param {Array<string>} [eduspoc]
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
 * @memberof NetworkElementAsPlannedApi
 */

export interface NetworkElementAsPlannedEdit {
  NetworkElementAsPlannedDtoEdit: NetworkElementAsPlannedDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface NetworkElementAsPlannedCreate {
  NetworkElementAsPlannedDtoCreate: NetworkElementAsPlannedDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface NetworkElementAsPlannedGrid {
  NetworkElementAsPlannedGridResult: QueryResultDtoOfNetworkElementAsPlannedDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface DaAssetMigrationDto {
  deleted: boolean;
  orphan: boolean;
  lastModified: string;
  lastModifiedBy: string;
  daAssetMigrationId: number;
  plannedActivityId: number;
  networkElementAsPlannedId: number;
  oldAssetName: string;
  newelEmentName: string;
  targetDesignComponenet: number;
  targetDesignComponenetId: number;
  newEnvironmentId: number;
  newEnvironment: string;
  newDeploymentStatusId: number;
  newDeploymentStatus: string;
  opcoId: number;
  opcoDesc: string;
  locationId: number;
  location: string;
  rfoDate: string;
  rfsDate: string;
  migrationCompletionDate: string;
  trafficNodePercentage: string;
  oldEnvironment: string;
  oldDeploymentType: string;
  oldDeploymentStatus: string;
}
export interface DaAssetMigrationDtoCreate {
  daAssetMigrationDtoGrid: DaAssetMigrationDto[];
  environmentReosurce?: { [key: string]: string };
  deploymentStatusReosurce?: { [key: string]: DeploymentStatusDto };
  locationReosurce?: { [key: string]: LocationDto };
  targetDesignComponentResource?: Array<{ key: number; value: string }>;
}
export interface CreateDaAssetMigration {
  DaAssetMigrationDtoCreate: DaAssetMigrationDtoCreate;
  ResultDtoCreate?: ResultDto;
}

export interface KeyValueItem {
  physicalServerVendor?: string;
  physicalServerHwModel?: string;
  key?: number;
  text?: string;
}

export interface AssetCapacityInfoGridDto {
  assetCapacityInfoId?: number;
  physicalServerHostName?: string;
  physicalServerIpAddress?: string;
  physicalServerSerialNumber?: string;
  noOfInstances?: number;
  vcpu?: number;
  memory?: number;
  storage?: number;
  physicalServerHwModelId?: number;
  physicalServerVendorId?: number;
  physicalServerHwModel?: string;
  physicalServerVendor?: string;
}

export interface AssetHardwareAncillaryGridDto {
  assetHardwareAncillaryId?: number;
  networkElementAsPlannedId?: number;

  elementName?: string;

  majorHardwareId?: number;
  majorHardwareName?: string;

  dataCenterId?: number;
  dataCeterName?: string;

  clusterNameId?: number;
  clusterNameDesc?: string;

  assetClusterTypeId?: number;
  assetClusterTypeDesc?: string;

  assetClusterId?: number;
  assetClusterDesc?: string;

  assetCapacityInfoGridDtos?: AssetCapacityInfoGridDto[];

  assetCapacityInfoId?: number;
  physicalServerHostName?: string;
  physicalServerIpAddress?: string;
  physicalServerSerialNumber?: string;
  noOfInstances?: number;
  vcpu?: number;
  memory?: number;
  storage?: number;
  physicalServerHwModelId?: number;
  physicalServerVendorId?: number;
  physicalServerHwModel?: string;
  physicalServerVendor?: string;

  deleted?: boolean;
  orphan?: boolean;

  lastModified?: string;
  lastModifiedBy?: string;
}

export interface AssetHardwareAncillariesResponse {
  assetHardwareAncillaryGridDto?: AssetHardwareAncillaryGridDto;

  majorHardwareResource?: KeyValueItem[];
  clusterNameResource?: KeyValueItem[];
  dataCenterResource?: KeyValueItem[];
  assetClusterResource?: KeyValueItem[];
  assetClusterTypeResource?: KeyValueItem[];

  opCoId?: number;
  assetId?: number;
}

export interface AssetHardwareAncillariesState {
  assetHardwareAncillariesResult: AssetHardwareAncillariesResponse | null;
}

export const GET_CREATE_NETWORK_ELEMENT_AS_PLANNED =
  "GET_CREATE_NETWORK_ELEMENT_AS_PLANNED";
export const GET_EDIT_NETWORK_ELEMENT_AS_PLANNED =
  "GET_EDIT_NETWORK_ELEMENT_AS_PLANNED";
export const GET_GRID_NETWORK_ELEMENT_AS_PLANNED =
  "GET_GRID_NETWORK_ELEMENT_AS_PLANNED";
export const GET_FILTER_NETWORK_ELEMENT_AS_PLANNED =
  "GET_FILTER_NETWORK_ELEMENT_AS_PLANNED";
export const CREATE_NETWORK_ELEMENT_AS_PLANNED =
  "CREATE_NETWORK_ELEMENT_AS_PLANNED";
export const EDIT_NETWORK_ELEMENT_AS_PLANNED =
  "EDIT_NETWORK_ELEMENT_AS_PLANNED";
export const DELETE_NETWORK_ELEMENT_AS_PLANNED =
  "DELETE_NETWORK_ELEMENT_AS_PLANNED";
export const RESTORE_NETWORK_ELEMENT_AS_PLANNED =
  "RESTORE_NETWORK_ELEMENT_AS_PLANNED";

export const CREATE_ASSET_MIGRATION = "CREATE_ASSET_MIGRATION";
export const GET_ASSET_HARDWARE_ANCILLARIES = "GET_ASSET_HARDWARE_ANCILLARIES";
