import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { DateFilter, QueryObject, RenderDetail } from "./Common";

export interface AuditTrailsGrid {
  AuditTrailsGridResult: QueryResultDtoOfAuditTrailsDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface AuditTrailsQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof AuditTrailsDtoGrid
   */
  auditLogId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof AuditTrailsDtoGrid
   */
  entityName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AuditTrailsDtoGrid
   */
  entityField?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AuditTrailsDtoGrid
   */
  oldValue?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AuditTrailsDtoGrid
   */
  newValue?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof AuditTrailsDtoGrid
   */
  creationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof AuditTrailsDtoGrid
   */
  creationDate?: DateFilter;
  /**
   *
   * @type {string}
   * @memberof AuditTrailsDtoGrid
   */
  modificationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof AuditTrailsDtoGrid
   */
  modificationDate?: DateFilter;
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

/**
 *
 * @export
 * @interface AuditTrailsDtoGrid
 */
export interface AuditTrailsDtoGrid {
  deleted: boolean;
  /**
   *
   * @type {boolean}
   * @memberof AuditTrailsDtoGrid
   */
  orphan: boolean;
  /**
   *
   * @type {string}
   * @memberof AuditTrailsDtoGrid
   */
  lastModified: string;
  /**
   *
   * @type {string}
   * @memberof AuditTrailsDtoGrid
   */
  lastModifiedBy: string;
  /**
   *
   * @type {number}
   * @memberof AuditTrailsDtoGrid
   */
  dynamicReportId: number;
  /**
   *
   * @type {number}
   * @memberof AuditTrailsDtoGrid
   */
  userId?: number;
  /**
   *
   * @type {string}
   * @memberof AuditTrailsDtoGrid
   */
  reportName: string;
  /**
   *
   * @type {string}
   * @memberof AuditTrailsDtoGrid
   */
  jsonGridCustomizationData: string;
  /**
   *
   * @type {string}
   * @memberof AuditTrailsDtoGrid
   */
  published: string;
  /**
   *
   * @type {string}
   * @memberof AuditTrailsDtoGrid
   */
  isScheduled: string;
  /**
   *
   * @type {string}
   * @memberof AuditTrailsDtoGrid
   */
  creationUser?: string;
  /**
   *
   * @type {string}
   * @memberof AuditTrailsDtoGrid
   */
  creationDate: string;
  /**
   *
   * @type {string}
   * @memberof AuditTrailsDtoGrid
   */
  modificationUser?: string;
  /**
   *
   * @type {string}
   * @memberof AuditTrailsDtoGrid
   */
  modificationDate?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfAuditTrailsDtoGrid
 */
export interface QueryResultDtoOfAuditTrailsDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfAuditTrailsDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfAuditTrailsDtoGrid
   */
  items?: Array<AuditTrailsDtoGrid>;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfAuditTrailsDtoGrid
   */
  gridRender?: Array<AuditTrailsDtoGrid>;
}

export const GET_GRID_AUDIT_TRAILS = "GET_GRID_AUDIT_TRAILS";
export const GET_FILTER_AUDIT_TRAILS = "GET_FILTER_AUDIT_TRAILS";
