import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface IntraVMTypeDto
 */
export interface IntraVMTypeDto {
  /**
   *
   * @type {number}
   * @memberof IntraVMTypeDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof IntraVMTypeDto
   */
  intraDescription?: string;
  /**
   *
   * @type {number}
   * @memberof IntraVMTypeDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof IntraVMTypeDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof IntraVMTypeDto
   */
  intravmTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof IntraVMTypeDto
   */
  IntraVMTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof IntraVMTypeDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof IntraVMTypeDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof IntraVMTypeDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof IntraVMTypeDto
   */
  intraVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof IntraVMTypeDto
   */
  intravmTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface IntraVMTypeDtoGrid
 */
export interface IntraVMTypeDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof IntraVMTypeDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof IntraVMTypeDtoGrid
   */
  intraDescription?: string;
  /**
   *
   * @type {string}
   * @memberof IntraVMTypeDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof IntraVMTypeDtoGrid
   */
  intravmType?: string;
  /**
   *
   * @type {number}
   * @memberof IntraVMTypeDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof IntraVMTypeDtoGrid
   */
  intraVmTypeId?: number;
  /**
   *
   * @type {boolean}
   * @memberof IntraVMTypeDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfIntraVMTypeDtoGrid
 */
export interface CustomGridRenderOfIntraVMTypeDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfIntraVMTypeDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfIntraVMTypeDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfIntraVMTypeDtoGrid
 */
export interface QueryResultDtoOfIntraVMTypeDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfIntraVMTypeDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<IntraVMTypeDtoGrid>}
   * @memberof QueryResultDtoOfIntraVMTypeDtoGrid
   */
  items?: Array<IntraVMTypeDtoGrid>;
  /**
   *
   * @type {Array<IntraVMTypeDtoGrid>}
   * @memberof QueryResultDtoOfIntraVMTypeDtoGrid
   */
  allItems?: Array<IntraVMTypeDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfIntraVMTypeDtoGrid}
   * @memberof QueryResultDtoOfIntraVMTypeDtoGrid
   */
  gridRender?: CustomGridRenderOfIntraVMTypeDtoGrid;
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
   * @type {QueryResultDtoOfIntraVMTypeDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfIntraVMTypeDtoGrid;
}

// ------------Not AutoGen---------

export interface IntraVMTypeQueryObjectGrid extends QueryObject {
  intraVmTypeId?: number[];
  intraDescription?: string[];
}

export interface IntraVMTypeEdit {
  LookUpDtoEdit: IntraVMTypeDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface IntraVMTypeCreate {
  LookUpDtoCreate: IntraVMTypeDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface IntraVMTypeGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
