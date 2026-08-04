import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface CNFNameDto
 */
export interface CNFNameDto {
  /**
   *
   * @type {number}
   * @memberof CNFNameDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CNFNameDto
   */
  cnfDescription?: string;
  /**
   *
   * @type {string}
   * @memberof CNFNameDto
   */
  product?: string;
  /**
   *
   * @type {number}
   * @memberof CNFNameDto
   */
  productId?: number;
  /**
   *
   * @type {boolean}
   * @memberof CNFNameDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof CNFNameDto
   */
  intervmTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof CNFNameDto
   */
  InterVMTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof CNFNameDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof CNFNameDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof CNFNameDto
   */
  productResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof CNFNameDto
   */
  interVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof CNFNameDto
   */
  intervmTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface CNFNameDtoGrid
 */
export interface CNFNameDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CNFNameDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CNFNameDtoGrid
   */
  cnfDescription?: string;
  /**
   *
   * @type {string}
   * @memberof CNFNameDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof CNFNameDtoGrid
   */
  intervmType?: string;
  /**
   *
   * @type {number}
   * @memberof CNFNameDtoGrid
   */
  productId?: number;
  /**
   *
   * @type {number}
   * @memberof CNFNameDtoGrid
   */
  cnfNameId?: number;
  /**
   *
   * @type {boolean}
   * @memberof CNFNameDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfCNFNameDtoGrid
 */
export interface CustomGridRenderOfCNFNameDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfCNFNameDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfCNFNameDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfCNFNameDtoGrid
 */
export interface QueryResultDtoOfCNFNameDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfCNFNameDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<CNFNameDtoGrid>}
   * @memberof QueryResultDtoOfCNFNameDtoGrid
   */
  items?: Array<CNFNameDtoGrid>;
  /**
   *
   * @type {Array<CNFNameDtoGrid>}
   * @memberof QueryResultDtoOfCNFNameDtoGrid
   */

  allItems?: Array<CNFNameDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfCNFNameDtoGrid}
   * @memberof QueryResultDtoOfCNFNameDtoGrid
   */
  gridRender?: CustomGridRenderOfCNFNameDtoGrid;
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
   * @type {QueryResultDtoOfCNFNameDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfCNFNameDtoGrid;
}
// ------------Not AutoGen---------

export interface CNFNameQueryObjectGrid extends QueryObject {
  cnfNameId?: number[];
  cnfDescription?: string[];
  productId?: number[];
  product?: string[];
}

export interface CNFNameEdit {
  LookUpDtoEdit: CNFNameDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface CNFNameCreate {
  LookUpDtoCreate: CNFNameDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface CNFNameGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
