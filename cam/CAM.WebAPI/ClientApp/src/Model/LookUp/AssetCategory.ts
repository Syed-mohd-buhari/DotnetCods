import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface AssetCategoryDto
 */
export interface AssetCategoryDto {
  /**
   *
   * @type {number}
   * @memberof AssetCategoryDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof AssetCategoryDto
   */
  description?: string;
  /**
   *
   * @type {number}
   * @memberof AssetCategoryDto
   */
  assetClassId?: number;

  /**
   *
   * @type {number}
   * @memberof AssetCategoryDto
   */
  idAssetClass?: number;
  /**
   *
   * @type {boolean}
   * @memberof AssetCategoryDto
   */
  takeFromAssetTypeTable?: boolean;
  /**
   *
   * @type {Date}
   * @memberof AssetCategoryDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof AssetCategoryDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof AssetCategoryDto
   */
  assetClassResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface AssetCategoryDtoGrid
 */
export interface AssetCategoryDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof AssetCategoryDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof AssetCategoryDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof AssetCategoryDtoGrid
   */
  assetClassId?: string;
  /**
   *
   * @type {number}
   * @memberof AssetCategoryDtoGrid
   */
  idAssetClass?: number;
  /**
   *
   * @type {boolean}
   * @memberof AssetCategoryDtoGrid
   */
  takeFromAssetTypeTable?: boolean;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfAssetCategoryDtoGrid
 */
export interface QueryResultDtoOfAssetCategoryDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfAssetCategoryDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<AssetCategoryDtoGrid>}
   * @memberof QueryResultDtoOfAssetCategoryDtoGrid
   */
  items?: Array<AssetCategoryDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfAssetCategoryDtoGrid}
   * @memberof QueryResultDtoOfAssetCategoryDtoGrid
   */
  gridRender?: CustomGridRender;
}

export interface AssetCategoryQueryObjectGrid extends QueryObjectGrid {
  id?: Array<number>;
  description?: Array<string>;
  assetClassId?: Array<number>;
  sortBy?: string;
  isSortAscending?: boolean;
  page?: number;
  pageSize?: number;
  lastModifiedStartDate?: Date;
  lastModifiedEndDate?: Date;
  principalId?: number;
  takeFromAssetTypeTable?: boolean[];
}

export interface AssetCategoryGrid {
  LookUpGridResult: QueryResultDtoOfAssetCategoryDtoGrid | null;
  LookUpGridResultAll: QueryResultDtoOfAssetCategoryDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface AssetCategoryEdit {
  LookUpDtoEdit: AssetCategoryDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface AssetCategoryCreate {
  LookUpDtoCreate: AssetCategoryDto | null;
  ResultDtoCreate: ResultDto | null;
}
