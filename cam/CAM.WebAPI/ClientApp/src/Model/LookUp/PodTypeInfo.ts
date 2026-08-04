import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface PodTypeInfoDto
 */
export interface PodTypeInfoDto {
  /**
   *
   * @type {number}
   * @memberof PodTypeInfoDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof PodTypeInfoDto
   */
  podRoleDescription?: string;
  /**
   *
   * @type {string}
   * @memberof PodTypeInfoDto
   */
  podTypeInfoName?: string;
  /**
   *
   * @type {number}
   * @memberof PodTypeInfoDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof PodTypeInfoDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof PodTypeInfoDto
   */
  intervmTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof PodTypeInfoDto
   */
  InterVMTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof PodTypeInfoDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof PodTypeInfoDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PodTypeInfoDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PodTypeInfoDto
   */
  interVMTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof PodTypeInfoDto
   */
  intervmTypeIdsList?: Array<number>;
}
/**
 *
 * @export
 * @interface PodTypeInfoDtoGrid
 */
export interface PodTypeInfoDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof PodTypeInfoDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof PodTypeInfoDtoGrid
   */
  podRoleDescription?: string;
  /**
   *
   * @type {string}
   * @memberof PodTypeInfoDtoGrid
   */
  podTypeInfoName?: string;
  /**
   *
   * @type {string}
   * @memberof PodTypeInfoDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof PodTypeInfoDtoGrid
   */
  intervmType?: string;
  /**
   *
   * @type {number}
   * @memberof PodTypeInfoDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof PodTypeInfoDtoGrid
   */
  podTypeInfoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof PodTypeInfoDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfPodTypeInfoDtoGrid
 */
export interface CustomGridRenderOfPodTypeInfoDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfPodTypeInfoDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfPodTypeInfoDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfPodTypeInfoDtoGrid
 */
export interface QueryResultDtoOfPodTypeInfoDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfPodTypeInfoDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<PodTypeInfoDtoGrid>}
   * @memberof QueryResultDtoOfPodTypeInfoDtoGrid
   */
  items?: Array<PodTypeInfoDtoGrid>;
  /**
   *
   * @type {Array<PodTypeInfoDtoGrid>}
   * @memberof QueryResultDtoOfPodTypeInfoDtoGrid
   */

  allItems?: Array<PodTypeInfoDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfPodTypeInfoDtoGrid}
   * @memberof QueryResultDtoOfPodTypeInfoDtoGrid
   */
  gridRender?: CustomGridRenderOfPodTypeInfoDtoGrid;
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
   * @type {QueryResultDtoOfPodTypeInfoDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfPodTypeInfoDtoGrid;
}
// ------------Not AutoGen---------

export interface PodTypeInfoQueryObjectGrid extends QueryObject {
  podTypeInfoId?: number[];
  podTypeInfoName?: string[];
  podRoleDescription?: string[];
}

export interface PodTypeInfoEdit {
  LookUpDtoEdit: PodTypeInfoDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface PodTypeInfoCreate {
  LookUpDtoCreate: PodTypeInfoDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface PodTypeInfoGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
