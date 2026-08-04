import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface CNFFunctionStandardNameDto
 */
export interface CNFFunctionStandardNameDto {
  /**
   *
   * @type {number}
   * @memberof CNFFunctionStandardNameDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CNFFunctionStandardNameDto
   */
  functionName?: string;
  /**
   *
   * @type {number}
   * @memberof CNFFunctionStandardNameDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof CNFFunctionStandardNameDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof CNFFunctionStandardNameDto
   */
  intervmTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof CNFFunctionStandardNameDto
   */
  InterVMTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof CNFFunctionStandardNameDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof CNFFunctionStandardNameDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof CNFFunctionStandardNameDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof CNFFunctionStandardNameDto
   */
  interVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof CNFFunctionStandardNameDto
   */
  intervmTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface CNFFunctionStandardNameDtoGrid
 */
export interface CNFFunctionStandardNameDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CNFFunctionStandardNameDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CNFFunctionStandardNameDtoGrid
   */
  functionName?: string;
  /**
   *
   * @type {string}
   * @memberof CNFFunctionStandardNameDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof CNFFunctionStandardNameDtoGrid
   */
  intervmType?: string;
  /**
   *
   * @type {number}
   * @memberof CNFFunctionStandardNameDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof CNFFunctionStandardNameDtoGrid
   */
  functionStandardNameId?: number;
  /**
   *
   * @type {boolean}
   * @memberof CNFFunctionStandardNameDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfCNFFunctionStandardNameDtoGrid
 */
export interface CustomGridRenderOfCNFFunctionStandardNameDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfCNFFunctionStandardNameDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfCNFFunctionStandardNameDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfCNFFunctionStandardNameDtoGrid
 */
export interface QueryResultDtoOfCNFFunctionStandardNameDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfCNFFunctionStandardNameDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<CNFFunctionStandardNameDtoGrid>}
   * @memberof QueryResultDtoOfCNFFunctionStandardNameDtoGrid
   */
  items?: Array<CNFFunctionStandardNameDtoGrid>;
  /**
   *
   * @type {Array<CNFFunctionStandardNameDtoGrid>}
   * @memberof QueryResultDtoOfCNFFunctionStandardNameDtoGrid
   */

  allItems?: Array<CNFFunctionStandardNameDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfCNFFunctionStandardNameDtoGrid}
   * @memberof QueryResultDtoOfCNFFunctionStandardNameDtoGrid
   */
  gridRender?: CustomGridRenderOfCNFFunctionStandardNameDtoGrid;
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
   * @type {QueryResultDtoOfCNFFunctionStandardNameDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfCNFFunctionStandardNameDtoGrid;
}
// ------------Not AutoGen---------

export interface CNFFunctionStandardNameQueryObjectGrid extends QueryObject {
  functionStandardNameId?: number[];
  functionName?: string[];
}

export interface CNFFunctionStandardNameEdit {
  LookUpDtoEdit: CNFFunctionStandardNameDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface CNFFunctionStandardNameCreate {
  LookUpDtoCreate: CNFFunctionStandardNameDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface CNFFunctionStandardNameGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
