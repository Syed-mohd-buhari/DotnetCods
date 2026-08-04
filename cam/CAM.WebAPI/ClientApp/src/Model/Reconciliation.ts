import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { GridDtoBase, RenderDetail, QueryObject } from "./Common";
import { ResultDto } from "./CommonModels";

/**
 *
 * @export
 * @interface ReconciliationDto
 */
export interface ReconciliationDto extends GridDtoBase {
  /**
   *
   * @type {Date}
   * @memberof ReconciliationDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof ReconciliationDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface ReconciliationDtoGrid
 */
export interface ReconciliationDtoGrid extends ReconciliationDto {
  /**
   *
   * @type {string}
   * @memberof ReconciliationQueryDto
   */
  opCo?: string;
  /**
   *
   * @type {string}
   * @memberof ReconciliationQueryDto
   */
  oem?: string;
  /**
   *
   * @type {string}
   * @memberof ReconciliationQueryDto
   */
  newSWVersion?: string;
  /**
   *
   * @type {string}
   * @memberof ReconciliationQueryDto
   */
  currentSWVersion?: string;
  /**
   *
   * @type {string}
   * @memberof ReconciliationQueryDto
   */
  newHWType?: string;
  /**
   *
   * @type {string}
   * @memberof ReconciliationQueryDto
   */
  currentHWType?: string;
  /**
   *
   * @type {string}
   * @memberof ReconciliationQueryDto
   */
  elementName?: string;
  /**
   *
   * @type {string}
   * @memberof ReconciliationQueryDto
   */
  deploymentStatus?: string;
  /**
   *
   * @type {string}
   * @memberof ReconciliationQueryDto
   */
  status?: string;
  /**
   *
   * @type {number}
   * @memberof ReconciliationQueryDto
   */
  assetsId?: number;
  /**
   *
   * @type {number}
   * @memberof ReconciliationQueryDto
   */
  lcmId?: number;
  /**
   *
   * @type {number}
   * @memberof ReconciliationQueryDto
   */
  reconciliationId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof ReconciliationQueryDto
   */
  plannedActivityId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReconciliationQueryDto
   */
  plannedActivityTypeFor?: Array<number>;
}
/**
 *
 * @export
 * @interface ReconciliationDtoCreate
 */
export interface ReconciliationDtoCreate extends ReconciliationDto {}

/**
 *
 * @export
 * @interface ReconciliationDtoUpdate
 */
export interface ReconciliationDtoUpdate extends ReconciliationDtoCreate {}
/**
 *
 * @export
 * @interface QueryResultDtoOfReconciliationDtoGrid
 */
export interface QueryResultDtoOfReconciliationDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfReconciliationDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<ReconciliationDtoGrid>}
   * @memberof QueryResultDtoOfReconciliationDtoGrid
   */
  items?: Array<ReconciliationDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfReconciliationDtoGrid}
   * @memberof QueryResultDtoOfReconciliationDtoGrid
   */
  gridRender?: CustomGridRenderOfReconciliationDtoGrid;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfReconciliationDtoGrid
 */
export interface CustomGridRenderOfReconciliationDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfReconciliationDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfReconciliationDtoGrid
   */
  render?: Array<RenderDetail>;
}

export interface ReconciliationQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<string>}
   * @memberof ReconciliationQueryDto
   */
  opCo?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReconciliationQueryDto
   */
  oem?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReconciliationQueryDto
   */
  newSWVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReconciliationQueryDto
   */
  currentSWVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReconciliationQueryDto
   */
  newHWType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReconciliationQueryDto
   */
  currentHWType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReconciliationQueryDto
   */
  elementName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReconciliationQueryDto
   */
  deploymentStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReconciliationQueryDto
   */
  status?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReconciliationQueryDto
   */
  assetsId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReconciliationQueryDto
   */
  lcmId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReconciliationQueryDto
   */
  plannedActivityId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReconciliationQueryDto
   */
  plannedActivityTypeFor?: Array<number>;
}

/**
 *
 * @param {Array<string>} [opCo]
 * @param {Array<string>} [oem]
 * @param {Array<string>} [currentSWVersion]
 * @param {Array<string>} [newSWVersion]
 * @param {Array<string>} [currentHWType]
 * @param {Array<string>} [newHWType]
 * @param {Array<string>} [elementName]
 * @param {Array<string>} [deploymentStatus]
 * @param {Array<string>} [status]
 * @param {Array<number>} [assetsId]
 * @param {Array<number>} [lcmId]
 * @param {Array<string>} [plannedActivityId]
 * @param {Array<number>} [plannedActivityTypeFor]
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

export interface ReconciliationEdit {
  ReconciliationDtoEdit: ReconciliationDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface ReconciliationCreate {
  ReconciliationDtoCreate: ReconciliationDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface ReconciliationGrid {
  ReconciliationGridResult: QueryResultDtoOfReconciliationDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_RECONCILIATION = "GET_CREATE_RECONCILIATION";
export const GET_EDIT_RECONCILIATION = "GET_EDIT_RECONCILIATION";
export const GET_GRID_RECONCILIATION = "GET_GRID_RECONCILIATION";
export const GET_FILTER_RECONCILIATION = "GET_FILTER_RECONCILIATION";
export const CREATE_RECONCILIATION = "CREATE_RECONCILIATION";
export const EDIT_RECONCILIATION = "EDIT_RECONCILIATION";
export const DELETE_RECONCILIATION = "DELETE_RECONCILIATION";
export const RESTORE_RECONCILIATION = "RESTORE_RECONCILIATION";
