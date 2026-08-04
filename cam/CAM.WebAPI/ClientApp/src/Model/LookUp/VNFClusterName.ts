import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface VNFClusterNameDto
 */
export interface VNFClusterNameDto {
  /**
   *
   * @type {number}
   * @memberof VNFClusterNameDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof VNFClusterNameDto
   */
  clusterDescription?: string;
  /**
   *
   * @type {number}
   * @memberof VNFClusterNameDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof VNFClusterNameDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof VNFClusterNameDto
   */
  clusternameId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof VNFClusterNameDto
   */
  ClusterNameIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof VNFClusterNameDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof VNFClusterNameDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof VNFClusterNameDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof VNFClusterNameDto
   */
  clusterNameResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof VNFClusterNameDto
   */
  clusterNameIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface VNFClusterNameDtoGrid
 */
export interface VNFClusterNameDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof VNFClusterNameDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof VNFClusterNameDtoGrid
   */
  clusterDescription?: string;
  /**
   *
   * @type {string}
   * @memberof VNFClusterNameDtoGrid
   */
  opco?: string;

  /**
   *
   * @type {number}
   * @memberof VNFClusterNameDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof VNFClusterNameDtoGrid
   */
  clusterNameId?: number;
  /**
   *
   * @type {number}
   * @memberof VNFClusterNameDtoGrid
   */
  clusterType?: number;
  /**
   *
   * @type {boolean}
   * @memberof VNFClusterNameDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfVNFClusterNameDtoGrid
 */
export interface CustomGridRenderOfVNFClusterNameDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfVNFClusterNameDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfVNFClusterNameDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfVNFClusterNameDtoGrid
 */
export interface QueryResultDtoOfVNFClusterNameDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfVNFClusterNameDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<VNFClusterNameDtoGrid>}
   * @memberof QueryResultDtoOfVNFClusterNameDtoGrid
   */
  items?: Array<VNFClusterNameDtoGrid>;
  /**
   *
   * @type {Array<VNFClusterNameDtoGrid>}
   * @memberof QueryResultDtoOfVNFClusterNameDtoGrid
   */

  allItems?: Array<VNFClusterNameDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfVNFClusterNameDtoGrid}
   * @memberof QueryResultDtoOfVNFClusterNameDtoGrid
   */
  gridRender?: CustomGridRenderOfVNFClusterNameDtoGrid;
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
   * @type {QueryResultDtoOfVNFClusterNameDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfVNFClusterNameDtoGrid;
}
// ------------Not AutoGen---------

export interface VNFClusterNameQueryObjectGrid extends QueryObject {
  clusterNameId?: number[];
  clusterDescription?: string[];
  clusterType?: number[];
}

export interface VNFClusterNameEdit {
  LookUpDtoEdit: VNFClusterNameDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface VNFClusterNameCreate {
  LookUpDtoCreate: VNFClusterNameDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface VNFClusterNameGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
