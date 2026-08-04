import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface VNFNameDto
 */
export interface VNFNameDto {
  /**
   *
   * @type {number}
   * @memberof VNFNameDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof VNFNameDto
   */
  vnfDescription?: string;
  /**
   *
   * @type {string}
   * @memberof VNFNameDto
   */
  product?: string;
  /**
   *
   * @type {number}
   * @memberof VNFNameDto
   */
  productId?: number;
  /**
   *
   * @type {boolean}
   * @memberof VNFNameDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof VNFNameDto
   */
  intervmTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof VNFNameDto
   */
  InterVMTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof VNFNameDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof VNFNameDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof VNFNameDto
   */
  productResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof VNFNameDto
   */
  interVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof VNFNameDto
   */
  intervmTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface VNFNameDtoGrid
 */
export interface VNFNameDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof VNFNameDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof VNFNameDtoGrid
   */
  vnfDescription?: string;
  /**
   *
   * @type {string}
   * @memberof VNFNameDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof VNFNameDtoGrid
   */
  intervmType?: string;
  /**
   *
   * @type {number}
   * @memberof VNFNameDtoGrid
   */
  productId?: number;
  /**
   *
   * @type {number}
   * @memberof VNFNameDtoGrid
   */
  vnfNameId?: number;
  /**
   *
   * @type {boolean}
   * @memberof VNFNameDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfVNFNameDtoGrid
 */
export interface CustomGridRenderOfVNFNameDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfVNFNameDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfVNFNameDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfVNFNameDtoGrid
 */
export interface QueryResultDtoOfVNFNameDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfVNFNameDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<VNFNameDtoGrid>}
   * @memberof QueryResultDtoOfVNFNameDtoGrid
   */
  items?: Array<VNFNameDtoGrid>;
  /**
   *
   * @type {Array<VNFNameDtoGrid>}
   * @memberof QueryResultDtoOfVNFNameDtoGrid
   */

  allItems?: Array<VNFNameDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfVNFNameDtoGrid}
   * @memberof QueryResultDtoOfVNFNameDtoGrid
   */
  gridRender?: CustomGridRenderOfVNFNameDtoGrid;
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
   * @type {QueryResultDtoOfVNFNameDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfVNFNameDtoGrid;
}
// ------------Not AutoGen---------

export interface VNFNameQueryObjectGrid extends QueryObject {
  vnfNameId?: number[];
  vnfDescription?: string[];
  productId?: number[];
  product?: string[];
}

export interface VNFNameEdit {
  LookUpDtoEdit: VNFNameDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface VNFNameCreate {
  LookUpDtoCreate: VNFNameDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface VNFNameGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
