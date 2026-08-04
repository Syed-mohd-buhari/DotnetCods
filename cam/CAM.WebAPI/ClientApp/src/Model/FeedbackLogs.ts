import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { DateFilter, QueryObject, RenderDetail } from "./Common";

export interface FeedbackLogsGrid {
  FeedbackLogsGridResult: QueryResultDtoOfFeedbackLogsDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface FeedbackLogsQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof FeedbackLogsDtoGrid
   */
  feedBackLoopAuditId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof FeedbackLogsDtoGrid
   */
  opCo?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FeedbackLogsDtoGrid
   */
  nodeType?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof FeedbackLogsDtoGrid
   */
  fileCount?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof FeedbackLogsDtoGrid
   */
  oem?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof FeedbackLogsDtoGrid
   */
  creationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FeedbackLogsDtoGrid
   */
  creationDate?: DateFilter;
  /**
   *
   * @type {string}
   * @memberof FeedbackLogsDtoGrid
   */
  modificationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof FeedbackLogsDtoGrid
   */
  modificationDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof FeedbackLogsDtoGrid
   */
  processStartTime?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof FeedbackLogsDtoGrid
   */
  processEndTime?: DateFilter;
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
 * @interface FeedbackLogsDtoGrid
 */
export interface FeedbackLogsDtoGrid {
  deleted: boolean;
  /**
   *
   * @type {boolean}
   * @memberof FeedbackLogsDtoGrid
   */
  orphan: boolean;
  /**
   *
   * @type {string}
   * @memberof FeedbackLogsDtoGrid
   */
  lastModified: string;
  /**
   *
   * @type {string}
   * @memberof FeedbackLogsDtoGrid
   */
  lastModifiedBy: string;
  /**
   *
   * @type {number}
   * @memberof FeedbackLogsDtoGrid
   */
  dynamicReportId: number;
  /**
   *
   * @type {number}
   * @memberof FeedbackLogsDtoGrid
   */
  userId?: number;
  /**
   *
   * @type {string}
   * @memberof FeedbackLogsDtoGrid
   */
  reportName: string;
  /**
   *
   * @type {string}
   * @memberof FeedbackLogsDtoGrid
   */
  jsonGridCustomizationData: string;
  /**
   *
   * @type {string}
   * @memberof FeedbackLogsDtoGrid
   */
  published: string;
  /**
   *
   * @type {string}
   * @memberof FeedbackLogsDtoGrid
   */
  isScheduled: string;
  /**
   *
   * @type {string}
   * @memberof FeedbackLogsDtoGrid
   */
  creationUser?: string;
  /**
   *
   * @type {string}
   * @memberof FeedbackLogsDtoGrid
   */
  creationDate: string;
  /**
   *
   * @type {string}
   * @memberof FeedbackLogsDtoGrid
   */
  modificationUser?: string;
  /**
   *
   * @type {string}
   * @memberof FeedbackLogsDtoGrid
   */
  modificationDate?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfFeedbackLogsDtoGrid
 */
export interface QueryResultDtoOfFeedbackLogsDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfFeedbackLogsDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfFeedbackLogsDtoGrid
   */
  items?: Array<FeedbackLogsDtoGrid>;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfFeedbackLogsDtoGrid
   */
  gridRender?: Array<FeedbackLogsDtoGrid>;
}

export const GET_GRID_FEEDBACK_LOGS = "GET_GRID_FEEDBACK_LOGS";
export const GET_FILTER_FEEDBACK_LOGS = "GET_FILTER_FEEDBACK_LOGS";
