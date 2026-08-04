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
  hardwareInstallDate?: Date;
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
 * @interface NetworkElementAsIsDtoCreate
 */
export interface NetworkElementAsIsDtoCreate extends NetworkElementAsIsDto {
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsIsDtoCreate
   */
  originalEquipmentManufacturerId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsIsDtoCreate
   */
  originalEquipmentManufacturerResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsIsDtoCreate
   */
  networkElementAsPlannedId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsIsDtoCreate
   */
  networkElementAsPlannedResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsIsDtoCreate
   */
  locationId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsIsDtoCreate
   */
  locationResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsIsDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsIsDtoCreate
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsIsDtoCreate
   */
  systemTypeId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof NetworkElementAsIsDtoCreate
   */
  systemTypeResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface NetworkElementAsIsDtoGrid
 */
export interface NetworkElementAsIsDtoGrid extends NetworkElementAsIsDto {
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsIsDtoGrid
   */
  networkElementAsIsId?: number;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDtoGrid
   */
  opCo?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDtoGrid
   */
  systemType?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAsIsDtoGrid
   */
  location?: string;
}
/**
 *
 * @export
 * @interface NetworkElementAsIsDtoUpdate
 */
export interface NetworkElementAsIsDtoUpdate
  extends NetworkElementAsIsDtoCreate {
  /**
   *
   * @type {number}
   * @memberof NetworkElementAsIsDtoUpdate
   */
  networkElementAsIsId?: number;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfNetworkElementAsIsDtoGrid
 */
export interface QueryResultDtoOfNetworkElementAsIsDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfNetworkElementAsIsDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfNetworkElementAsIsDtoGrid
   */
  items?: Array<NetworkElementAsIsDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfNetworkElementAsIsDtoGrid}
   * @memberof QueryResultDtoOfNetworkElementAsIsDtoGrid
   */
  gridRender?: CustomGridRenderOfNetworkElementAsIsDtoGrid;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfNetworkElementAsIsDtoGrid
 */
export interface CustomGridRenderOfNetworkElementAsIsDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfNetworkElementAsIsDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfNetworkElementAsIsDtoGrid
   */
  render?: Array<RenderDetail>;
}

// -------------------------No auto Generated---------------------------------

export interface NetworkElementAsIsQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  networkElementAsIsId?: Array<number>;
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
 * @param {Array<number>} [networkElementAsIsId]
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
  NetworkElementAsIsDtoEdit: NetworkElementAsIsDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface NetworkElementAsIsCreate {
  NetworkElementAsIsDtoCreate: NetworkElementAsIsDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface NetworkElementAsIsGrid {
  NetworkElementAsIsGridResult: QueryResultDtoOfNetworkElementAsIsDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface NewNetworkElementAsIsGrid {
  NewNetworkElementAsIsGridResult: QueryResultDtoOfNetworkElementAsIsDtoGrid | null;
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
export const GET_GRID_NETWORK_ELEMENT_AS_IS = "GET_GRID_NETWORK_ELEMENT_AS_IS";
export const GET_FILTER_NETWORK_ELEMENT_AS_IS =
  "GET_FILTER_NETWORK_ELEMENT_AS_IS";
export const CREATE_NETWORK_ELEMENT_AS_IS = "CREATE_NETWORK_ELEMENT_AS_IS";
export const EDIT_NETWORK_ELEMENT_AS_IS = "EDIT_NETWORK_ELEMENT_AS_IS";
export const DELETE_NETWORK_ELEMENT_AS_IS = "DELETE_NETWORK_ELEMENT_AS_IS";
export const RESTORE_NETWORK_ELEMENT_AS_IS = "RESTORE_NETWORK_ELEMENT_AS_IS";
