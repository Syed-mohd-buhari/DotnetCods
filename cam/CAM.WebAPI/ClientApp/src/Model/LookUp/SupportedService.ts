import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";

/**
 *
 * @export
 * @interface SupportedServiceDto
 */
export interface SupportedServiceDto {
  /**
   *
   * @type {number}
   * @memberof SupportedServiceDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof SupportedServiceDto
   */
  description?: string;
  /**
   *
   * @type {Date}
   * @memberof SupportedServiceDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof SupportedServiceDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface SupportedServiceDtoGrid
 */
export interface SupportedServiceDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof SupportedServiceDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof SupportedServiceDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof SupportedServiceDtoGrid
   */
  lastModifiedBy?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfSupportedServiceDtoGrid
 */
export interface QueryResultDtoOfSupportedServiceDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfSupportedServiceDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<SupportedServiceDtoGrid>}
   * @memberof QueryResultDtoOfSupportedServiceDtoGrid
   */
  items?: Array<SupportedServiceDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfSupportedServiceDtoGrid}
   * @memberof QueryResultDtoOfSupportedServiceDtoGrid
   */
  gridRender?: CustomGridRender;
}

export interface SupportedServiceQueryObjectGrid extends QueryObjectGrid {
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
