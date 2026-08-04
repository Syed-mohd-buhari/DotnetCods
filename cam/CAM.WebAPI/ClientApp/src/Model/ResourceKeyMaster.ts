import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  QueryObjectGrid,
  GridDtoBase,
  RenderDetail,
  QueryObject,
  DateFilter,
} from "./Common";
import { RelatedResource, ResultDto } from "./CommonModels";
import { PlannedActivityNetworkElementDtoUpdate } from "./LookUp/PlannedActivityNetworkElement";
import { NetworkElementAsPlannedDtoGrid } from "./NetworkElementAsPlanned";
import { PlannedActivityDtoUpdate } from "./PlannedActivity";

/**
 *
 * @export
 * @interface NetworkElementAsIsDto
 */
export interface NetworkElementAsIsDto extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  elementDeploymentName?: string;

  linkedToNetworkAsIs: boolean;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  softwareProductNumber?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  elementManager?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  patchDetails?: string;
  /**
   *
   * @type {Date}
   * @memberof NetworkElementAsIsDto
   */
  softwareProductionDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof NetworkElementAsIsDto
   */
  softwareInstallDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof NetworkElementAsIsDto
   */
  dataAcquisitionDate?: Date;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  dataAcquisitionMethod?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  hardwareAcquisition?: string;
  /**
   *
   * @type {boolean}
   * @memberof NetworkElementAsIsDto
   */
  manualOverride?: boolean;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  nodeType?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  elementManagerExportFileFormat?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  networkFunction?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  hardwareSolution?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  platform?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  hardwareType?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  softwareReleaseInformation?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  otherHardwareInfo?: string;
  /**
   *
   * @type {Date}
   * @memberof NetworkElementAsIsDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface ResourceKeyMasterDtoCreate
 */
export interface ResourceKeyMasterDtoCreate extends NetworkElementAsIsDto {
  /**
   *
   * @type {number}
   * @memberof ResourceKeyMasterDtoCreate
   */
  originalEquipmentManufacturerId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ResourceKeyMasterDtoCreate
   */
  originalEquipmentManufacturerResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof ResourceKeyMasterDtoCreate
   */
  networkElementAsPlannedId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ResourceKeyMasterDtoCreate
   */
  networkElementAsPlannedResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof ResourceKeyMasterDtoCreate
   */
  locationId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ResourceKeyMasterDtoCreate
   */
  locationResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof ResourceKeyMasterDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ResourceKeyMasterDtoCreate
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof ResourceKeyMasterDtoCreate
   */
  systemTypeId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ResourceKeyMasterDtoCreate
   */
  systemTypeResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface ResourceKeyMasterDtoGrid
 */
export interface ResourceKeyMasterDtoGrid extends NetworkElementAsIsDto {
  /**
   *
   * @type {number}
   * @memberof ResourceKeyMasterDtoGrid
   */
  identityid?: number;
  /**
   *
   * @type {string}
   * @memberof ResourceKeyMasterDtoGrid
   */
  opCo?: string;
  /**
   *
   * @type {string}
   * @memberof ResourceKeyMasterDtoGrid
   */
  systemType?: string;
  /**
   *
   * @type {string}
   * @memberof ResourceKeyMasterDtoGrid
   */
  location?: string;
}
/**
 *
 * @export
 * @interface ResourceKeyMasterDtoUpdate
 */
export interface ResourceKeyMasterDtoUpdate extends ResourceKeyMasterDtoCreate {
  /**
   *
   * @type {number}
   * @memberof ResourceKeyMasterDtoUpdate
   */
  identityid?: number;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfResourceKeyMasterDtoGrid
 */
export interface QueryResultDtoOfResourceKeyMasterDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfResourceKeyMasterDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfResourceKeyMasterDtoGrid
   */
  items?: Array<ResourceKeyMasterDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfResourceKeyMasterDtoGrid}
   * @memberof QueryResultDtoOfResourceKeyMasterDtoGrid
   */
  gridRender?: CustomGridRenderOfResourceKeyMasterDtoGrid;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfResourceKeyMasterDtoGrid
 */
export interface CustomGridRenderOfResourceKeyMasterDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfResourceKeyMasterDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfResourceKeyMasterDtoGrid
   */
  render?: Array<RenderDetail>;
}

// -------------------------No auto Generated---------------------------------

export interface ResourceKeyMasterQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  identityid?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  opCo?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  oem?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  networkFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  nodeType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  elementDeploymentName?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  location?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  systemTypeId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  softwareReleaseInformationSystemLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  softwareProductNumberSystemLevel?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof NetworkElementsAsIsQueryDto
   */
  softwareProductionDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof NetworkElementsAsIsQueryDto
   */
  softwareInstallDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  hardwareSolution?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  platform?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  hardwareType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  otherHardwareInfo?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  hardwareAcquisition?: Array<string>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  manualOverride?: Array<boolean>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  hardwareSystemId?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof NetworkElementsAsIsQueryDto
   */
  dataAcquisitionDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  dataAcquisitionMethod?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  elementManager?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  elementManagerExportFileFormat?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  spareFieldsJson?: Array<string>;
}

/**
 *
 * @param {Array<number>} [identityid]
 * @param {Array<number>} [opCo]
 * @param {Array<number>} [oem]
 * @param {Array<string>} [networkFunction]
 * @param {Array<string>} [nodeType]
 * @param {Array<string>} [elementDeploymentName]
 * @param {Array<number>} [location]
 * @param {Array<number>} [systemTypeId]
 * @param {Array<string>} [softwareReleaseInformationSystemLevel]
 * @param {Array<string>} [softwareProductNumberSystemLevel]
 * @param {Date} [softwareProductionDateStartDate]
 * @param {Date} [softwareProductionDateEndDate]
 * @param {Date} [softwareInstallDateStartDate]
 * @param {Date} [softwareInstallDateEndDate]
 * @param {Array<string>} [hardwareSolution]
 * @param {Array<string>} [platform]
 * @param {Array<string>} [hardwareType]
 * @param {Array<string>} [otherHardwareInfo]
 * @param {Array<string>} [hardwareAcquisition]
 * @param {Array<boolean>} [manualOverrideHardwareSolution]
 * @param {Array<number>} [hardwareSystemId]
 * @param {Date} [dataAcquisitionDateStartDate]
 * @param {Date} [dataAcquisitionDateEndDate]
 * @param {Array<string>} [dataAcquisitionMethod]
 * @param {Array<string>} [elementManager]
 * @param {Array<string>} [elementManagerExportFileFormat]
 * @param {Array<string>} [spareFieldsJson]
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

export interface NetworkElementAsIsEdit {
  NetworkElementAsIsDtoEdit: ResourceKeyMasterDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface NetworkElementAsIsCreate {
  ResourceKeyMasterDtoCreate: ResourceKeyMasterDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface NetworkElementAsIsGrid {
  NetworkElementAsIsGridResult: QueryResultDtoOfResourceKeyMasterDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface ResourceKeyMasterGrid {
  ResourceKeyMasterGridResult: QueryResultDtoOfResourceKeyMasterDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface NetworkElementAsIsSystemTypeInfo {
  hardwareSolution?: string;
  platform?: string;
  hardwareType?: string;
  otherHardwareInfo?: string;
  softwareReleaseInformation?: string;
  networkFunction?: string;
}

export const GET_CREATE_NETWORK_ELEMENT_AS_IS =
  "GET_CREATE_NETWORK_ELEMENT_AS_IS";
export const GET_EDIT_NETWORK_ELEMENT_AS_IS = "GET_EDIT_NETWORK_ELEMENT_AS_IS";
export const GET_GRID_RESOURCE_KEY_MASTER = "GET_GRID_RESOURCE_KEY_MASTERr";
export const GET_FILTER_NETWORK_ELEMENT_AS_IS =
  "GET_FILTER_NETWORK_ELEMENT_AS_IS";
export const CREATE_NETWORK_ELEMENT_AS_IS = "CREATE_NETWORK_ELEMENT_AS_IS";
export const EDIT_NETWORK_ELEMENT_AS_IS = "EDIT_NETWORK_ELEMENT_AS_IS";
export const DELETE_NETWORK_ELEMENT_AS_IS = "DELETE_NETWORK_ELEMENT_AS_IS";
export const RESTORE_NETWORK_ELEMENT_AS_IS = "RESTORE_NETWORK_ELEMENT_AS_IS";
