import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface CNFPriorityDto
 */
export interface CNFPriorityDto {
  /**
   *
   * @type {number}
   * @memberof CNFPriorityDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CNFPriorityDto
   */
  description?: string;
  /**
   *
   * @type {number}
   * @memberof CNFPriorityDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof CNFPriorityDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof CNFPriorityDto
   */
  intervmTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof CNFPriorityDto
   */
  InterVMTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof CNFPriorityDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof CNFPriorityDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof CNFPriorityDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof CNFPriorityDto
   */
  interVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof CNFPriorityDto
   */
  intervmTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface CNFPriorityDtoGrid
 */
export interface CNFPriorityDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CNFPriorityDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CNFPriorityDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof CNFPriorityDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof CNFPriorityDtoGrid
   */
  intervmType?: string;
  /**
   *
   * @type {number}
   * @memberof CNFPriorityDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof CNFPriorityDtoGrid
   */
  cnfPriorityId?: number;
  /**
   *
   * @type {boolean}
   * @memberof CNFPriorityDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfCNFPriorityDtoGrid
 */
export interface CustomGridRenderOfCNFPriorityDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfCNFPriorityDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfCNFPriorityDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfCNFPriorityDtoGrid
 */
export interface QueryResultDtoOfCNFPriorityDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfCNFPriorityDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<CNFPriorityDtoGrid>}
   * @memberof QueryResultDtoOfCNFPriorityDtoGrid
   */
  items?: Array<CNFPriorityDtoGrid>;
  /**
   *
   * @type {Array<CNFPriorityDtoGrid>}
   * @memberof QueryResultDtoOfCNFPriorityDtoGrid
   */

  allItems?: Array<CNFPriorityDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfCNFPriorityDtoGrid}
   * @memberof QueryResultDtoOfCNFPriorityDtoGrid
   */
  gridRender?: CustomGridRenderOfCNFPriorityDtoGrid;
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
   * @type {QueryResultDtoOfCNFPriorityDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfCNFPriorityDtoGrid;
}
// ------------Not AutoGen---------

export interface CNFPriorityQueryObjectGrid extends QueryObject {
  cnfPriorityId?: number[];
  description?: string[];
}

export interface CNFPriorityEdit {
  LookUpDtoEdit: CNFPriorityDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface CNFPriorityCreate {
  LookUpDtoCreate: CNFPriorityDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface CNFPriorityGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
