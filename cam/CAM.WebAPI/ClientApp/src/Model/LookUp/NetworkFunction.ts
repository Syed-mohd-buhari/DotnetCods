import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";

/**
 *
 * @export
 * @interface NetworkFunctionDto
 */
export interface NetworkFunctionDto {
  /**
   *
   * @type {number}
   * @memberof NetworkFunctionDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof NetworkFunctionDto
   */
  description?: string;
  /**
   *
   * @type {Date}
   * @memberof NetworkFunctionDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof NetworkFunctionDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface NetworkFunctionDtoGrid
 */
export interface NetworkFunctionDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof NetworkFunctionDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof NetworkFunctionDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkFunctionDtoGrid
   */
  lastModifiedBy?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfNetworkFunctionDtoGrid
 */
export interface QueryResultDtoOfNetworkFunctionDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfNetworkFunctionDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkFunctionDtoGrid>}
   * @memberof QueryResultDtoOfNetworkFunctionDtoGrid
   */
  items?: Array<NetworkFunctionDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfAssetClassDtoGrid}
   * @memberof QueryResultDtoOfNetworkFunctionDtoGrid
   */
  gridRender?: CustomGridRender;
}

export interface NetworkFunctionQueryObjectGrid extends QueryObjectGrid {
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
