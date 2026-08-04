import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";

/**
 *
 * @export
 * @interface CustomerWheelDto
 */
export interface CustomerWheelDto {
  /**
   *
   * @type {number}
   * @memberof CustomerWheelDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CustomerWheelDto
   */
  description?: string;
  /**
   *
   * @type {Date}
   * @memberof CustomerWheelDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof CustomerWheelDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface CustomerWheelDtoGrid
 */
export interface CustomerWheelDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CustomerWheelDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CustomerWheelDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof CustomerWheelDtoGrid
   */
  lastModifiedBy?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfCustomerWheelDtoGrid
 */
export interface QueryResultDtoOfCustomerWheelDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfCustomerWheelDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<CustomerWheelDtoGrid>}
   * @memberof QueryResultDtoOfCustomerWheelDtoGrid
   */
  items?: Array<CustomerWheelDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfAssetClassDtoGrid}
   * @memberof QueryResultDtoOfCustomerWheelDtoGrid
   */
  gridRender?: CustomGridRender;
}

export interface CustomerWheelQueryObjectGrid extends QueryObjectGrid {
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
