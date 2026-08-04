import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface ServiceMasterDto
 */
export interface ServiceMasterDto {
  /**
   *
   * @type {number}
   * @memberof ServiceMasterDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof ServiceMasterDto
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof ServiceMasterDto
   */
  intraDescription?: string;

  /**
   *
   * @type {number}
   * @memberof ServiceMasterDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof ServiceMasterDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof ServiceMasterDto
   */
  serviceMasterIndex?: number;
  /**
   *
   * @type {number}
   * @memberof ServiceMasterDto
   */
  intravmTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof ServiceMasterDto
   */
  IntraVMTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof ServiceMasterDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof ServiceMasterDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServiceMasterDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServiceMasterDto
   */
  intraVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof ServiceMasterDto
   */
  intravmTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface ServiceMasterDtoGrid
 */
export interface ServiceMasterDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof ServiceMasterDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof ServiceMasterDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof ServiceMasterDtoGrid
   */
  intraDescription?: string;
  /**
   *
   * @type {string}
   * @memberof ServiceMasterDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof ServiceMasterDtoGrid
   */
  intravmType?: string;
  /**
   *
   * @type {number}
   * @memberof ServiceMasterDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof ServiceMasterDtoGrid
   */
  intraVmTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof ServiceMasterDtoGrid
   */
  serviceMasterIndex?: number;
  /**
   *
   * @type {boolean}
   * @memberof ServiceMasterDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfServiceMasterDtoGrid
 */
export interface CustomGridRenderOfServiceMasterDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfServiceMasterDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfServiceMasterDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfServiceMasterDtoGrid
 */
export interface QueryResultDtoOfServiceMasterDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfServiceMasterDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<ServiceMasterDtoGrid>}
   * @memberof QueryResultDtoOfServiceMasterDtoGrid
   */
  items?: Array<ServiceMasterDtoGrid>;
  /**
   *
   * @type {Array<ServiceMasterDtoGrid>}
   * @memberof QueryResultDtoOfServiceMasterDtoGrid
   */
  allItems?: Array<ServiceMasterDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfServiceMasterDtoGrid}
   * @memberof QueryResultDtoOfServiceMasterDtoGrid
   */
  gridRender?: CustomGridRenderOfServiceMasterDtoGrid;
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
   * @type {QueryResultDtoOfServiceMasterDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfServiceMasterDtoGrid;
}

// ------------Not AutoGen---------

export interface ServiceMasterQueryObjectGrid extends QueryObject {
  serviceMasterIndex?: number[];
  description?: string[];
}

export interface ServiceMasterEdit {
  LookUpDtoEdit: ServiceMasterDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface ServiceMasterCreate {
  LookUpDtoCreate: ServiceMasterDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface ServiceMasterGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
