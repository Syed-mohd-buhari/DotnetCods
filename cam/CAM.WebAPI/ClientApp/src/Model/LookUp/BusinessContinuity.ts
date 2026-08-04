import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";

/**
 *
 * @export
 * @interface BusinessContinuityMethodDto
 */
export interface BusinessContinuityMethodDto {
  /**
   *
   * @type {number}
   * @memberof BusinessContinuityMethodDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof BusinessContinuityMethodDto
   */
  description?: string;
  /**
   *
   * @type {Date}
   * @memberof BusinessContinuityMethodDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof BusinessContinuityMethodDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface BusinessContinuityMethodDtoGrid
 */
export interface BusinessContinuityMethodDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof BusinessContinuityMethodDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof BusinessContinuityMethodDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof BusinessContinuityMethodDtoGrid
   */
  lastModifiedBy?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfBusinessContinuityMethodDtoGrid
 */
export interface QueryResultDtoOfBusinessContinuityMethodDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfBusinessContinuityMethodDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<BusinessContinuityMethodDtoGrid>}
   * @memberof QueryResultDtoOfBusinessContinuityMethodDtoGrid
   */
  items?: Array<BusinessContinuityMethodDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfAssetClassDtoGrid}
   * @memberof QueryResultDtoOfBusinessContinuityMethodDtoGrid
   */
  gridRender?: CustomGridRender;
}

export interface BusinessContinuityMethodQueryObjectGrid extends QueryObjectGrid {
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
