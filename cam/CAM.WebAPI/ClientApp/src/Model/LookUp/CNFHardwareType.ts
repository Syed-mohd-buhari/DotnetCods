import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface CNFHardwareTypeDto
 */
export interface CNFHardwareTypeDto {
  /**
   *
   * @type {number}
   * @memberof CNFHardwareTypeDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CNFHardwareTypeDto
   */
  description?: string;
  /**
   *
   * @type {number}
   * @memberof CNFHardwareTypeDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof CNFHardwareTypeDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof CNFHardwareTypeDto
   */
  cnfHardwareTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof CNFHardwareTypeDto
   */
  CnfHardwareTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof CNFHardwareTypeDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof CNFHardwareTypeDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof CNFHardwareTypeDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof CNFHardwareTypeDto
   */
  interVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof CNFHardwareTypeDto
   */
  cnfHardwareTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface CNFHardwareTypeDtoGrid
 */
export interface CNFHardwareTypeDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CNFHardwareTypeDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CNFHardwareTypeDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof CNFHardwareTypeDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof CNFHardwareTypeDtoGrid
   */
  intervmType?: string;
  /**
   *
   * @type {number}
   * @memberof CNFHardwareTypeDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof CNFHardwareTypeDtoGrid
   */
  cnfHardwareId?: number;
  /**
   *
   * @type {boolean}
   * @memberof CNFHardwareTypeDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfCNFHardwareTypeDtoGrid
 */
export interface CustomGridRenderOfCNFHardwareTypeDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfCNFHardwareTypeDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfCNFHardwareTypeDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfCNFHardwareTypeDtoGrid
 */
export interface QueryResultDtoOfCNFHardwareTypeDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfCNFHardwareTypeDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<CNFHardwareTypeDtoGrid>}
   * @memberof QueryResultDtoOfCNFHardwareTypeDtoGrid
   */
  items?: Array<CNFHardwareTypeDtoGrid>;
  /**
   *
   * @type {Array<CNFHardwareTypeDtoGrid>}
   * @memberof QueryResultDtoOfCNFHardwareTypeDtoGrid
   */

  allItems?: Array<CNFHardwareTypeDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfCNFHardwareTypeDtoGrid}
   * @memberof QueryResultDtoOfCNFHardwareTypeDtoGrid
   */
  gridRender?: CustomGridRenderOfCNFHardwareTypeDtoGrid;
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
   * @type {QueryResultDtoOfCNFHardwareTypeDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfCNFHardwareTypeDtoGrid;
}
// ------------Not AutoGen---------

export interface CNFHardwareTypeQueryObjectGrid extends QueryObject {
  cnfHardwareId?: number[];
  description?: string[];
}

export interface CNFHardwareTypeEdit {
  LookUpDtoEdit: CNFHardwareTypeDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface CNFHardwareTypeCreate {
  LookUpDtoCreate: CNFHardwareTypeDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface CNFHardwareTypeGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
