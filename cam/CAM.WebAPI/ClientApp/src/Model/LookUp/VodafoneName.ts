import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";

/**
 *
 * @export
 * @interface VodafoneNAmeDto
 */
export interface VodafoneNAmeDto {
  /**
   *
   * @type {number}
   * @memberof VodafoneNAmeDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof VodafoneNAmeDto
   */
  description?: string;

  /**
   *
   * @type {string}
   * @memberof VodafoneNAmeDto
   */
  productNames?: string;
  /**
   *
   * @type {Date}
   * @memberof VodafoneNAmeDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof VodafoneNAmeDto
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

  /**
   *
   * @type {string}
   * @memberof VodafoneNAmeDto
   */
  productNames?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfVodafoneNameDtoGrid
 */
export interface QueryResultDtoOfVodafoneNameDtoGrid {
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

export interface VodafoneNameQueryObjectGrid extends QueryObjectGrid {
  id?: Array<number>;
  description?: Array<string>;
  productName?: Array<string>;
  sortBy?: string;
  isSortAscending?: boolean;
  page?: number;
  pageSize?: number;
  lastModifiedStartDate?: Date;
  lastModifiedEndDate?: Date;
  principalId?: number;
}
