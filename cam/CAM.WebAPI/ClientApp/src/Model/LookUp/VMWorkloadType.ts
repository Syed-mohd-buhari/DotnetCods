import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface VMWorkloadTypeDto
 */
export interface VMWorkloadTypeDto {
  /**
   *
   * @type {number}
   * @memberof VMWorkloadTypeDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof VMWorkloadTypeDto
   */
  description?: string;
  /**
   *
   * @type {number}
   * @memberof VMWorkloadTypeDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof VMWorkloadTypeDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof VMWorkloadTypeDto
   */
  vmWorkloadTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof VMWorkloadTypeDto
   */
  VMWorkloadTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof VMWorkloadTypeDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof VMWorkloadTypeDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof VMWorkloadTypeDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof VMWorkloadTypeDto
   */
  VMWorkloadTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof VMWorkloadTypeDto
   */
  vmWorkloadTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface VMWorkloadTypeDtoGrid
 */
export interface VMWorkloadTypeDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof VMWorkloadTypeDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof VMWorkloadTypeDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof VMWorkloadTypeDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof VMWorkloadTypeDtoGrid
   */
  vmWorkloadType?: string;
  /**
   *
   * @type {number}
   * @memberof VMWorkloadTypeDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof VMWorkloadTypeDtoGrid
   */
  vmWorkLoadTypeId?: number;
  /**
   *
   * @type {boolean}
   * @memberof VMWorkloadTypeDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfVMWorkloadTypeDtoGrid
 */
export interface CustomGridRenderOfVMWorkloadTypeDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfVMWorkloadTypeDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfVMWorkloadTypeDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfVMWorkloadTypeDtoGrid
 */
export interface QueryResultDtoOfVMWorkloadTypeDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfVMWorkloadTypeDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<VMWorkloadTypeDtoGrid>}
   * @memberof QueryResultDtoOfVMWorkloadTypeDtoGrid
   */
  items?: Array<VMWorkloadTypeDtoGrid>;
  /**
   *
   * @type {Array<VMWorkloadTypeDtoGrid>}
   * @memberof QueryResultDtoOfVMWorkloadTypeDtoGrid
   */
  allItems?: Array<VMWorkloadTypeDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfVMWorkloadTypeDtoGrid}
   * @memberof QueryResultDtoOfVMWorkloadTypeDtoGrid
   */
  gridRender?: CustomGridRenderOfVMWorkloadTypeDtoGrid;
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
   * @type {QueryResultDtoOfVMWorkloadTypeDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfVMWorkloadTypeDtoGrid;
}

// ------------Not AutoGen---------

export interface VMWorkloadTypeQueryObjectGrid extends QueryObject {
  vmWorkLoadTypeId?: number[];
  description?: string[];
}

export interface VMWorkloadTypeEdit {
  LookUpDtoEdit: VMWorkloadTypeDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface VMWorkloadTypeCreate {
  LookUpDtoCreate: VMWorkloadTypeDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface VMWorkloadTypeGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
