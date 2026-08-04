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
 * @interface SoftwareComponentDtoCreate
 */
export interface SoftwareComponentDtoCreate extends NetworkElementAsIsDto {
  /**
   *
   * @type {number}
   * @memberof SoftwareComponentDtoCreate
   */
  originalEquipmentManufacturerId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SoftwareComponentDtoCreate
   */
  originalEquipmentManufacturerResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof SoftwareComponentDtoCreate
   */
  networkElementAsPlannedId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SoftwareComponentDtoCreate
   */
  networkElementAsPlannedResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof SoftwareComponentDtoCreate
   */
  locationId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SoftwareComponentDtoCreate
   */
  locationResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof SoftwareComponentDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SoftwareComponentDtoCreate
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof SoftwareComponentDtoCreate
   */
  systemTypeId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SoftwareComponentDtoCreate
   */
  systemTypeResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface SoftwareComponentDtoGrid
 */
export interface SoftwareComponentDtoGrid extends NetworkElementAsIsDto {
  /**
   *
   * @type {number}
   * @memberof SoftwareComponentDtoGrid
   */
  softwarewarecomponentid?: number;
  /**
   *
   * @type {string}
   * @memberof SoftwareComponentDtoGrid
   */
  opCo?: string;
  /**
   *
   * @type {string}
   * @memberof SoftwareComponentDtoGrid
   */
  systemType?: string;
  /**
   *
   * @type {string}
   * @memberof SoftwareComponentDtoGrid
   */
  location?: string;
}
/**
 *
 * @export
 * @interface SoftwareComponentDtoUpdate
 */
export interface SoftwareComponentDtoUpdate
  extends SoftwareComponentDtoCreate {
  /**
   *
   * @type {number}
   * @memberof SoftwareComponentDtoUpdate
   */
  softwarewarecomponentid?: number;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfSoftwareComponentDtoGrid
 */
export interface QueryResultDtoOfSoftwareComponentDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfSoftwareComponentDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfSoftwareComponentDtoGrid
   */
  items?: Array<SoftwareComponentDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfSoftwareComponentDtoGrid}
   * @memberof QueryResultDtoOfSoftwareComponentDtoGrid
   */
  gridRender?: CustomGridRenderOfSoftwareComponentDtoGrid;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfSoftwareComponentDtoGrid
 */
export interface CustomGridRenderOfSoftwareComponentDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfSoftwareComponentDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfSoftwareComponentDtoGrid
   */
  render?: Array<RenderDetail>;
}

// -------------------------No auto Generated---------------------------------

export interface SoftwareComponentQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  softwarewarecomponentid?: Array<number>;
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
 * @param {Array<number>} [softwarewarecomponentid]
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
  NetworkElementAsIsDtoEdit: SoftwareComponentDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface NetworkElementAsIsCreate {
  SoftwareComponentDtoCreate: SoftwareComponentDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface NetworkElementAsIsGrid {
  NetworkElementAsIsGridResult: QueryResultDtoOfSoftwareComponentDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface SoftwareComponentGrid {
  SoftwareComponentGridResult: QueryResultDtoOfSoftwareComponentDtoGrid | null;
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
export const GET_GRID_SOFTWARE_COMPONENT = "GET_GRID_SOFTWARE_COMPONENT";
export const GET_FILTER_NETWORK_ELEMENT_AS_IS =
  "GET_FILTER_NETWORK_ELEMENT_AS_IS";
export const CREATE_NETWORK_ELEMENT_AS_IS = "CREATE_NETWORK_ELEMENT_AS_IS";
export const EDIT_NETWORK_ELEMENT_AS_IS = "EDIT_NETWORK_ELEMENT_AS_IS";
export const DELETE_NETWORK_ELEMENT_AS_IS = "DELETE_NETWORK_ELEMENT_AS_IS";
export const RESTORE_NETWORK_ELEMENT_AS_IS = "RESTORE_NETWORK_ELEMENT_AS_IS";
