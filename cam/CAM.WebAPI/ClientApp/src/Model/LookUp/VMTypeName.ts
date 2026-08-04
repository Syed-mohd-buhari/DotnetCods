import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface VMTypeNameDto
 */
export interface VMTypeNameDto {
  /**
   *
   * @type {number}
   * @memberof VMTypeNameDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof VMTypeNameDto
   */
  vmTypeDescription?: string;
  /**
   *
   * @type {string}
   * @memberof VMTypeNameDto
   */
  vnfName?: string;
  /**
   *
   * @type {number}
   * @memberof VMTypeNameDto
   */
  vnfNameId?: number;
  /**
   *
   * @type {boolean}
   * @memberof VMTypeNameDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof VMTypeNameDto
   */
  intervmTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof VMTypeNameDto
   */
  InterVMTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof VMTypeNameDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof VMTypeNameDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof VMTypeNameDto
   */
  vnfNamResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof VMTypeNameDto
   */
  interVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof VMTypeNameDto
   */
  intervmTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface VMTypeNameDtoGrid
 */
export interface VMTypeNameDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof VMTypeNameDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof VMTypeNameDtoGrid
   */
  vmTypeDescription?: string;
  /**
   *
   * @type {string}
   * @memberof VMTypeNameDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof VMTypeNameDtoGrid
   */
  intervmType?: string;
  /**
   *
   * @type {number}
   * @memberof VMTypeNameDtoGrid
   */
  vnfNameId?: number;
  /**
   *
   * @type {number}
   * @memberof VMTypeNameDtoGrid
   */
  vmTypeNameId?: number;
  /**
   *
   * @type {boolean}
   * @memberof VMTypeNameDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfVMTypeNameDtoGrid
 */
export interface CustomGridRenderOfVMTypeNameDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfVMTypeNameDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfVMTypeNameDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfVMTypeNameDtoGrid
 */
export interface QueryResultDtoOfVMTypeNameDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfVMTypeNameDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<VMTypeNameDtoGrid>}
   * @memberof QueryResultDtoOfVMTypeNameDtoGrid
   */
  items?: Array<VMTypeNameDtoGrid>;
  /**
   *
   * @type {Array<VMTypeNameDtoGrid>}
   * @memberof QueryResultDtoOfVMTypeNameDtoGrid
   */

  allItems?: Array<VMTypeNameDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfVMTypeNameDtoGrid}
   * @memberof QueryResultDtoOfVMTypeNameDtoGrid
   */
  gridRender?: CustomGridRenderOfVMTypeNameDtoGrid;
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
   * @type {QueryResultDtoOfVMTypeNameDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfVMTypeNameDtoGrid;
}
// ------------Not AutoGen---------

export interface VMTypeNameQueryObjectGrid extends QueryObject {
  vmTypeNameId?: number[];
  vmTypeDescription?: string[];
  vnfNameId?: number[];
  vnfName?: string[];
}

export interface VMTypeNameEdit {
  LookUpDtoEdit: VMTypeNameDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface VMTypeNameCreate {
  LookUpDtoCreate: VMTypeNameDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface VMTypeNameGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
