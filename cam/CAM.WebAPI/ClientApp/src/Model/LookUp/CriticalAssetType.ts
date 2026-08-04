import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";
import { ResultDto } from "../CommonModels";
/**
 *
 * @export
 * @interface CriticalAssetTypeDto
 */
export interface CriticalAssetTypeDto {
  /**
   *
   * @type {number}
   * @memberof CriticalAssetTypeDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CriticalAssetTypeDto
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof CriticalAssetTypeDto
   */
  assetCategoryId?: string;
  /**
   *
   * @type {number}
   * @memberof CriticalAssetTypeDto
   */
  idAssetCategory?: number;
  /**
   *
   * @type {Date}
   * @memberof CriticalAssetTypeDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof CriticalAssetTypeDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof CriticalAssetTypeDto
   */
  assetCategoryResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface CriticalAssetTypeDtoGrid
 */
export interface CriticalAssetTypeDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CriticalAssetTypeDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CriticalAssetTypeDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof CriticalAssetTypeDtoGrid
   */
  idAssetCategory?: number;

  /**
   *
   * @type {string}
   * @memberof CriticalAssetTypeDtoGrid
   */
  assetCategoryId?: string;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfCriticalAssetTypeDtoGrid
 */
export interface QueryResultDtoOfCriticalAssetTypeDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfCriticalAssetTypeDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<CriticalAssetTypeDtoGrid>}
   * @memberof QueryResultDtoOfCriticalAssetTypeDtoGrid
   */
  items?: Array<CriticalAssetTypeDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfCriticalAssetTypeDtoGrid}
   * @memberof QueryResultDtoOfCriticalAssetTypeDtoGrid
   */
  gridRender?: CustomGridRender;
}

export interface CriticalAssetTypeQueryObjectGrid extends QueryObjectGrid {
  id?: Array<number>;
  description?: Array<string>;
  assetCategoryId?: Array<number>;
  sortBy?: string;
  isSortAscending?: boolean;
  page?: number;
  pageSize?: number;
  lastModifiedStartDate?: Date;
  lastModifiedEndDate?: Date;
  principalId?: number;
}

export interface CriticalAssetTypeGrid {
  LookUpGridResult: QueryResultDtoOfCriticalAssetTypeDtoGrid | null;
  LookUpGridResultAll: QueryResultDtoOfCriticalAssetTypeDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface CriticalAssetTypeEdit {
  LookUpDtoEdit: CriticalAssetTypeDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface CriticalAssetTypeCreate {
  LookUpDtoCreate: CriticalAssetTypeDto | null;
  ResultDtoCreate: ResultDto | null;
}
