import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";

/**
 *
 * @export
 * @interface SharedLookupDto
 */
export interface SharedLookupDto {
  /**
   *
   * @type {number}
   * @memberof SharedLookupDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof SharedLookupDto
   */
  description?: string;
  /**
   *
   * @type {Date}
   * @memberof SharedLookupDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof SharedLookupDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface SharedLookupDtoGrid
 */
export interface SharedLookupDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof SharedLookupDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof SharedLookupDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof SharedLookupDtoGrid
   */
  lastModifiedBy?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfSharedLookupDtoGrid
 */
export interface QueryResultDtoOfSharedLookupDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfSharedLookupDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<SharedLookupDtoGrid>}
   * @memberof QueryResultDtoOfSharedLookupDtoGrid
   */
  items?: Array<SharedLookupDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfAssetClassDtoGrid}
   * @memberof QueryResultDtoOfSharedLookupDtoGrid
   */
  gridRender?: CustomGridRender;
}

export interface SharedLookupQueryObjectGrid extends QueryObjectGrid {
  id?: Array<number>;
  description?: Array<string>;
  sortBy?: string;
  isSortAscending?: boolean;
  page?: number;
  pageSize?: number;
  lastModifiedStartDate?: Date;
  lastModifiedEndDate?: Date;
  principalId?: number;
}
