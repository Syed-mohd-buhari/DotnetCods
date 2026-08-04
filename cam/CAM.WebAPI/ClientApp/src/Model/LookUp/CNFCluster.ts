import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface CNFClusterDto
 */
export interface CNFClusterDto {
  /**
   *
   * @type {number}
   * @memberof CNFClusterDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CNFClusterDto
   */
  cnfClusterName?: string;
  /**
   *
   * @type {string}
   * @memberof CNFClusterDto
   */
  cnfName?: string;
  /**
   *
   * @type {string}
   * @memberof CNFClusterDto
   */
  alaisName?: string;

  /**
   *
   * @type {string}
   * @memberof CNFClusterDto
   */
  nodePool?: string;
  /**
   *
   * @type {number}
   * @memberof CNFClusterDto
   */
  cnfNameId?: number;

  /**
   *
   * @type {boolean}
   * @memberof CNFClusterDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof CNFClusterDto
   */
  intervmTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof CNFClusterDto
   */
  InterVMTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof CNFClusterDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof CNFClusterDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof CNFClusterDto
   */
  cnfNameResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof CNFClusterDto
   */
  interVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof CNFClusterDto
   */
  intervmTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface CNFClusterDtoGrid
 */
export interface CNFClusterDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CNFClusterDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof CNFClusterDtoGrid
   */
  cnfClusterName?: string;
  /**
   *
   * @type {string}
   * @memberof CNFClusterDtoGrid
   */
  cnfName?: string;
  /**
   *
   * @type {string}
   * @memberof CNFClusterDtoGrid
   */
  alaisName?: string;
  /**
   *
   * @type {string}
   * @memberof CNFClusterDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof CNFClusterDtoGrid
   */
  intervmType?: string;
  /**
   *
   * @type {number}
   * @memberof CNFClusterDtoGrid
   */
  cnfNameId?: number;
  /**
   *
   * @type {number}
   * @memberof CNFClusterDtoGrid
   */
  cnfClusterId?: number;
  /**
   *
   * @type {boolean}
   * @memberof CNFClusterDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfCNFClusterDtoGrid
 */
export interface CustomGridRenderOfCNFClusterDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfCNFClusterDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfCNFClusterDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfCNFClusterDtoGrid
 */
export interface QueryResultDtoOfCNFClusterDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfCNFClusterDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<CNFClusterDtoGrid>}
   * @memberof QueryResultDtoOfCNFClusterDtoGrid
   */
  items?: Array<CNFClusterDtoGrid>;
  /**
   *
   * @type {Array<CNFClusterDtoGrid>}
   * @memberof QueryResultDtoOfCNFClusterDtoGrid
   */

  allItems?: Array<CNFClusterDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfCNFClusterDtoGrid}
   * @memberof QueryResultDtoOfCNFClusterDtoGrid
   */
  gridRender?: CustomGridRenderOfCNFClusterDtoGrid;
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
   * @type {QueryResultDtoOfCNFClusterDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfCNFClusterDtoGrid;
}
// ------------Not AutoGen---------

export interface CNFClusterQueryObjectGrid extends QueryObject {
  cnfClusterId?: number[];
  cnfClusterName?: string[];
  nodePool?: string[];
  alaisName?: string[];
  cnfName?: string[];
  cnfNameId?: number[];
}

export interface CNFClusterEdit {
  LookUpDtoEdit: CNFClusterDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface CNFClusterCreate {
  LookUpDtoCreate: CNFClusterDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface CNFClusterGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
