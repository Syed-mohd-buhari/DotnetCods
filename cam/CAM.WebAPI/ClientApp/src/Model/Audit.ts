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
 * @interface AuditDtoCreate
 */
export interface AuditDtoCreate extends NetworkElementAsIsDto {
  /**
   *
   * @type {number}
   * @memberof AuditDtoCreate
   */
  originalEquipmentManufacturerId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof AuditDtoCreate
   */
  originalEquipmentManufacturerResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof AuditDtoCreate
   */
  networkElementAsPlannedId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof AuditDtoCreate
   */
  networkElementAsPlannedResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof AuditDtoCreate
   */
  locationId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof AuditDtoCreate
   */
  locationResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof AuditDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof AuditDtoCreate
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof AuditDtoCreate
   */
  systemTypeId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof AuditDtoCreate
   */
  systemTypeResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface AuditDtoGrid
 */
export interface AuditDtoGrid extends NetworkElementAsIsDto {
  auditHistoryId: number | null | undefined;
  /**
   *
   * @type {number}
   * @memberof AuditDtoGrid
   */
  identityid?: number;
  /**
   *
   * @type {string}
   * @memberof AuditDtoGrid
   */
  opCo?: string;
  /**
   *
   * @type {string}
   * @memberof AuditDtoGrid
   */
  systemType?: string;
  /**
   *
   * @type {string}
   * @memberof AuditDtoGrid
   */
  location?: string;
  /**
   *
   * @type {string}
   * @memberof AuditDtoGrid
   */
  status?: string;
}
/**
 *
 * @export
 * @interface AuditDtoUpdate
 */
export interface AuditDtoUpdate extends AuditDtoCreate {
  /**
   *
   * @type {number}
   * @memberof AuditDtoUpdate
   */
  identityid?: number;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfAuditDtoGrid
 */
export interface QueryResultDtoOfAuditDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfAuditDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfAuditDtoGrid
   */
  items?: Array<AuditDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfAuditDtoGrid}
   * @memberof QueryResultDtoOfAuditDtoGrid
   */
  gridRender?: CustomGridRenderOfAuditDtoGrid;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfAuditDtoGrid
 */
export interface CustomGridRenderOfAuditDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfAuditDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfAuditDtoGrid
   */
  render?: Array<RenderDetail>;
}

// -------------------------No auto Generated---------------------------------

export interface AuditQueryObjectGrid extends QueryObject {
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
  NetworkElementAsIsDtoEdit: AuditDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface NetworkElementAsIsCreate {
  AuditDtoCreate: AuditDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface NetworkElementAsIsGrid {
  NetworkElementAsIsGridResult: QueryResultDtoOfAuditDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface AuditGrid {
  AuditGridResult: QueryResultDtoOfAuditDtoGrid | null;
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
export const GET_GRID_AUDIT = "GET_GRID_AUDIT";
export const GET_FILTER_NETWORK_ELEMENT_AS_IS =
  "GET_FILTER_NETWORK_ELEMENT_AS_IS";
export const CREATE_NETWORK_ELEMENT_AS_IS = "CREATE_NETWORK_ELEMENT_AS_IS";
export const EDIT_NETWORK_ELEMENT_AS_IS = "EDIT_NETWORK_ELEMENT_AS_IS";
export const DELETE_NETWORK_ELEMENT_AS_IS = "DELETE_NETWORK_ELEMENT_AS_IS";
export const RESTORE_NETWORK_ELEMENT_AS_IS = "RESTORE_NETWORK_ELEMENT_AS_IS";
