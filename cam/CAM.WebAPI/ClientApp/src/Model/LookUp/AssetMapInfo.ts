import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface AssetMapInfoDto
 */
export interface AssetMapInfoDto {
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof AssetMapInfoDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDto
   */
  omcAssetName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDto
   */
  temsAssetName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDto
   */
  enmAssetName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDto
   */
  site?: string;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDto
   */
  dataSourceName?: string;
  /**
   *
   * @type {Date}
   * @memberof AssetMapInfoDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {boolean}
   * @memberof AssetMapInfoDto
   */
  deleted?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof AssetMapInfoDto
   */
  orphan?: boolean;
}

/**
 *
 * @export
 * @interface AssetMapInfoDtoGrid
 */
export interface AssetMapInfoDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof AssetMapInfoDtoGrid
   */
  id?: number;
  /**
   *
   * @type {number}
   * @memberof AssetMapInfoDtoGrid
   */
  assetMapInfoId?: number;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDtoGrid
   */
  omcAssetName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDtoGrid
   */
  temsAssetName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDtoGrid
   */
  enmAssetName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDtoGrid
   */
  site?: string;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDtoGrid
   */
  dataSourceName?: string;
  /**
   *
   * @type {Date}
   * @memberof AssetMapInfoDtoGrid
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof AssetMapInfoDtoGrid
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {boolean}
   * @memberof AssetMapInfoDtoGrid
   */
  deleted?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof AssetMapInfoDtoGrid
   */
  orphan?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfAssetMapInfoDtoGrid
 */
export interface CustomGridRenderOfAssetMapInfoDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfAssetMapInfoDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfAssetMapInfoDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfAssetMapInfoDtoGrid
 */
export interface QueryResultDtoOfAssetMapInfoDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfAssetMapInfoDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<AssetMapInfoDtoGrid>}
   * @memberof QueryResultDtoOfAssetMapInfoDtoGrid
   */
  items?: Array<AssetMapInfoDtoGrid>;
  /**
   *
   * @type {Array<AssetMapInfoDtoGrid>}
   * @memberof QueryResultDtoOfAssetMapInfoDtoGrid
   */
  allItems?: Array<AssetMapInfoDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfAssetMapInfoDtoGrid}
   * @memberof QueryResultDtoOfAssetMapInfoDtoGrid
   */
  gridRender?: CustomGridRenderOfAssetMapInfoDtoGrid;
}

/**
 *
 * @export
 * @interface ResultDtoOfObject
 */
export interface ResultDtoOfObject {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfObject
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfObject
   */
  info?: string;
  /**
   *
   * @type {QueryResultDtoOfAssetMapInfoDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfAssetMapInfoDtoGrid;
}

// ------------Not AutoGen---------

export interface AssetMapInfoQueryObjectGrid extends QueryObject {
  assetMapInfoId?: number[];
  omcAssetName?: string[];
  temsAssetName?: string[];
  enmAssetName?: string[];
  site?: string[];
  dataSourceName?: string[];
}

export interface AssetMapInfoEdit {
  LookUpDtoEdit: AssetMapInfoDto | null;
  ResultDtoEdit: ResultDto | null;
}

export interface AssetMapInfoCreate {
  LookUpDtoCreate: AssetMapInfoDto | null;
  ResultDtoCreate: ResultDto | null;
}

export interface AssetMapInfoGrid {
  LookUpGridResult: QueryResultDtoOfAssetMapInfoDtoGrid | null;
  LookUpGridResultAll: QueryResultDtoOfAssetMapInfoDtoGrid | null;
  filter: FilterValueDto[] | null;
}
export const DOWNLOAD_ASSETMAPINFO = "DOWNLOAD_ASSETMAPINFO";
