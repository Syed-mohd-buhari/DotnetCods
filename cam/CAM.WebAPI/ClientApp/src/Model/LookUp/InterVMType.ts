import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface InterVMTypeDto
 */
export interface InterVMTypeDto {
  /**
   *
   * @type {number}
   * @memberof InterVMTypeDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof InterVMTypeDto
   */
  interDescription?: string;
  /**
   *
   * @type {number}
   * @memberof InterVMTypeDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof InterVMTypeDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof InterVMTypeDto
   */
  intervmTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof InterVMTypeDto
   */
  InterVMTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof InterVMTypeDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof InterVMTypeDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof InterVMTypeDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof InterVMTypeDto
   */
  interVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof InterVMTypeDto
   */
  intervmTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface InterVMTypeDtoGrid
 */
export interface InterVMTypeDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof InterVMTypeDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof InterVMTypeDtoGrid
   */
  interDescription?: string;
  /**
   *
   * @type {string}
   * @memberof InterVMTypeDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof InterVMTypeDtoGrid
   */
  intervmType?: string;
  /**
   *
   * @type {number}
   * @memberof InterVMTypeDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof InterVMTypeDtoGrid
   */
  interVmTypeId?: number;
  /**
   *
   * @type {boolean}
   * @memberof InterVMTypeDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfInterVMTypeDtoGrid
 */
export interface CustomGridRenderOfInterVMTypeDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfInterVMTypeDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfInterVMTypeDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfInterVMTypeDtoGrid
 */
export interface QueryResultDtoOfInterVMTypeDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfInterVMTypeDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<InterVMTypeDtoGrid>}
   * @memberof QueryResultDtoOfInterVMTypeDtoGrid
   */
  items?: Array<InterVMTypeDtoGrid>;
  /**
   *
   * @type {Array<InterVMTypeDtoGrid>}
   * @memberof QueryResultDtoOfInterVMTypeDtoGrid
   */

  allItems?: Array<InterVMTypeDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfInterVMTypeDtoGrid}
   * @memberof QueryResultDtoOfInterVMTypeDtoGrid
   */
  gridRender?: CustomGridRenderOfInterVMTypeDtoGrid;
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
   * @type {QueryResultDtoOfInterVMTypeDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfInterVMTypeDtoGrid;
}
// ------------Not AutoGen---------

export interface InterVMTypeQueryObjectGrid extends QueryObject {
  interVmTypeId?: number[];
  interDescription?: string[];
}

export interface InterVMTypeEdit {
  LookUpDtoEdit: InterVMTypeDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface InterVMTypeCreate {
  LookUpDtoCreate: InterVMTypeDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface InterVMTypeGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
