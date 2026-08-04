import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { DateFilter, QueryObject, RenderDetail } from "./Common";

export interface UserDefinedReportsLogsGrid {
  UserDefinedReportsLogsGridResult: QueryResultDtoOfUserDefinedReportsLogsDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface UserDefinedReportsLogsQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  userDefinedReportsLogId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  reportName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  reportDownloadedPath?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  reportStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  newValue?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  creationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  creationDate?: DateFilter;
  /**
   *
   * @type {string}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  modificationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof UserDefinedReportsLogsDtoGrid
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
 * @interface UserDefinedReportsLogsDtoGrid
 */
export interface UserDefinedReportsLogsDtoGrid {
  deleted: boolean;
  /**
   *
   * @type {boolean}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  orphan: boolean;
  /**
   *
   * @type {string}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  lastModified: string;
  /**
   *
   * @type {string}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  lastModifiedBy: string;
  /**
   *
   * @type {number}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  dynamicReportId: number;
  /**
   *
   * @type {number}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  userId?: number;
  /**
   *
   * @type {string}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  reportName: string;
  /**
   *
   * @type {string}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  jsonGridCustomizationData: string;
  /**
   *
   * @type {string}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  published: string;
  /**
   *
   * @type {string}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  isScheduled: string;
  /**
   *
   * @type {string}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  creationUser?: string;
  /**
   *
   * @type {string}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  creationDate: string;
  /**
   *
   * @type {string}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  modificationUser?: string;
  /**
   *
   * @type {string}
   * @memberof UserDefinedReportsLogsDtoGrid
   */
  modificationDate?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfUserDefinedReportsLogsDtoGrid
 */
export interface QueryResultDtoOfUserDefinedReportsLogsDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfUserDefinedReportsLogsDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfUserDefinedReportsLogsDtoGrid
   */
  items?: Array<UserDefinedReportsLogsDtoGrid>;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfUserDefinedReportsLogsDtoGrid
   */
  gridRender?: Array<UserDefinedReportsLogsDtoGrid>;
}

export const GET_GRID_USER_DEFINED_REPORTS_LOGS =
  "GET_GRID_USER_DEFINED_REPORTS_LOGS";
export const GET_FILTER_USER_DEFINED_REPORTS_LOGS =
  "GET_FILTER_USER_DEFINED_REPORTS_LOGS";
