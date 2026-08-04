import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  GridDtoBase,
  RenderDetail,
  QueryObject,
  DateFilter,
  CustomGridRender,
  QueryObjectGrid,
} from "./Common";
import { ResultDto } from "./CommonModels";

/**
 *
 * @export
 * @interface NFVISwCompatibleQueryDto
 */
export interface NFVISwCompatibleQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof NFVISwCompatibleQueryDto
   */
  nfviSoftwareCompatibilityId: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof NFVISwCompatibleQueryDto
   */
  minimumSupportedVersion: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NFVISwCompatibleQueryDto
   */
  plaftFormVersion: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NFVISwCompatibleQueryDto
   */
  productName: Array<string>;
}

/**
 *
 * @export
 * @interface NFVISwCompatibleDtoGrid
 */
export interface NFVISwCompatibleDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof NFVISwCompatibleDtoGrid
   */
  nfviSoftwareCompatibilityId: number;
  /**
   *
   * @type {string}
   * @memberof NFVISwCompatibleDtoGrid
   */
  minimumSupportedVersion: string;
  /**
   *
   * @type {string}
   * @memberof NFVISwCompatibleDtoGrid
   */
  plaftFormVersion: string;
  /**
   *
   * @type {string}
   * @memberof NFVISwCompatibleDtoGrid
   */
  productName: string;
  /**
   *
   * @type {number}
   * @memberof NFVISwCompatibleDtoGrid
   */
  productId?: number;
  /**
   *
   * @type {number}
   * @memberof NFVISwCompatibleDtoGrid
   */
  vendorId?: number;
  /**
   *
   * @type {number}
   * @memberof NFVISwCompatibleDtoGrid
   */
  plaftFormId?: number;
}

/**
 *
 * @export
 * @interface NFVISwCompatibleDtoCreate
 */
export interface NFVISwCompatibleDtoCreate extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof NFVISwCompatibleDtoCreate
   */
  minimumSupportedVersion: string;
  /**
   *
   * @type {{[key: string]: string}}
   * @memberof NFVISwCompatibleDtoCreate
   */
  vmwareMswPlatform?: { [key: string]: string };
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof NFVISwCompatibleDtoCreate
   */
  vendorResource?: Array<{ key: number; text: string }>;
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof NFVISwCompatibleDtoCreate
   */
  productName?: Array<{ key: number; text: string }>;
  /**
   *
   * @type {number}
   * @memberof NFVISwCompatibleDtoCreate
   */
  productId?: number;
  /**
   *
   * @type {number}
   * @memberof NFVISwCompatibleDtoCreate
   */
  vendorId?: number;
  /**
   *
   * @type {number}
   * @memberof NFVISwCompatibleDtoCreate
   */
  plaftFormId?: number;
}
/**
 *
 * @export
 * @interface NFVISwCompatibleDtoUpdate
 */
export interface NFVISwCompatibleDtoUpdate extends NFVISwCompatibleDtoCreate {
  /**
   *
   * @type {number}
   * @memberof NFVISwCompatibleDtoUpdate
   */
  nfviSoftwareCompatibilityId?: number;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfNFVISwCompatibleDtoGrid
 */
export interface QueryResultDtoOfNFVISwCompatibleDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfNFVISwCompatibleDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NFVISwCompatibleDtoGrid>}
   * @memberof QueryResultDtoOfNFVISwCompatibleDtoGrid
   */
  items?: Array<NFVISwCompatibleDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface NFVISwCompatibleQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof NFVISwCompatibleQueryDto
   */
  nfviSoftwareCompatibilityId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof NFVISwCompatibleQueryDto
   */
  minimumSupportedVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NFVISwCompatibleQueryDto
   */
  plaftFormVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NFVISwCompatibleQueryDto
   */
  productName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NFVISwCompatibleQueryDto
   */
  vendor?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof SystemTypeQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  lastModifiedBy?: Array<string>;
}

export interface NFVISwCompatibleEdit {
  NFVISwCompatibleDtoEdit: NFVISwCompatibleDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface NFVISwCompatibleCreate {
  NFVISwCompatibleDtoCreate: NFVISwCompatibleDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface NFVISwCompatibleGrid {
  NFVISwCompatibleGridResult: QueryResultDtoOfNFVISwCompatibleDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_NFVI_SW_COMPATIBLE = "GET_CREATE_NFVI_SW_COMPATIBLE";
export const GET_EDIT_NFVI_SW_COMPATIBLE = "GET_EDIT_NFVI_SW_COMPATIBLE";
export const GET_GRID_NFVI_COMPATIBLE = "GET_GRID_NFVI_COMPATIBLE";
export const GET_FILTER_NFVI_COMPATIBLE = "GET_FILTER_NFVI_COMPATIBLE";
export const CREATE_NFVI_SW_COMPATIBLE = "CREATE_NFVI_SW_COMPATIBLE";
export const EDIT_NFVI_SW_COMPATIBLE = "EDIT_NFVI_SW_COMPATIBLE";
export const DELETE_NFVI_COMPATIBLE = "DELETE_NFVI_COMPATIBLE";
export const RESTORE_NFVI_COMPATIBLE = "RESTORE_NFVI_COMPATIBLE";
