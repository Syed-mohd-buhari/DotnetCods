import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface VNFHardwareTypeDto
 */
export interface VNFHardwareTypeDto {
  /**
   *
   * @type {number}
   * @memberof VNFHardwareTypeDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof VNFHardwareTypeDto
   */
  description?: string;
  /**
   *
   * @type {number}
   * @memberof VNFHardwareTypeDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof VNFHardwareTypeDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof VNFHardwareTypeDto
   */
  intervmTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof VNFHardwareTypeDto
   */
  InterVMTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof VNFHardwareTypeDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof VNFHardwareTypeDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof VNFHardwareTypeDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof VNFHardwareTypeDto
   */
  interVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof VNFHardwareTypeDto
   */
  intervmTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface VNFHardwareTypeDtoGrid
 */
export interface VNFHardwareTypeDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof VNFHardwareTypeDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof VNFHardwareTypeDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof VNFHardwareTypeDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof VNFHardwareTypeDtoGrid
   */
  intervmType?: string;
  /**
   *
   * @type {number}
   * @memberof VNFHardwareTypeDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof VNFHardwareTypeDtoGrid
   */
  vnfHardwareId?: number;
  /**
   *
   * @type {boolean}
   * @memberof VNFHardwareTypeDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfVNFHardwareTypeTypeDtoGrid
 */
export interface CustomGridRenderOfVNFHardwareTypeTypeDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfVNFHardwareTypeTypeDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfVNFHardwareTypeTypeDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfVNFHardwareTypeDtoGrid
 */
export interface QueryResultDtoOfVNFHardwareTypeDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfVNFHardwareTypeDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<VNFHardwareTypeDtoGrid>}
   * @memberof QueryResultDtoOfVNFHardwareTypeDtoGrid
   */
  items?: Array<VNFHardwareTypeDtoGrid>;
  /**
   *
   * @type {Array<VNFHardwareTypeDtoGrid>}
   * @memberof QueryResultDtoOfVNFHardwareTypeDtoGrid
   */

  allItems?: Array<VNFHardwareTypeDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfVNFHardwareTypeTypeDtoGrid}
   * @memberof QueryResultDtoOfVNFHardwareTypeDtoGrid
   */
  gridRender?: CustomGridRenderOfVNFHardwareTypeTypeDtoGrid;
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
   * @type {QueryResultDtoOfVNFHardwareTypeDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfVNFHardwareTypeDtoGrid;
}
// ------------Not AutoGen---------

export interface VNFHardwareTypeQueryObjectGrid extends QueryObject {
  vnfHardwareId?: number[];
  description?: string[];
}

export interface VNFHardwareTypeEdit {
  LookUpDtoEdit: VNFHardwareTypeDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface VNFHardwareTypeCreate {
  LookUpDtoCreate: VNFHardwareTypeDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface VNFHardwareTypeGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
