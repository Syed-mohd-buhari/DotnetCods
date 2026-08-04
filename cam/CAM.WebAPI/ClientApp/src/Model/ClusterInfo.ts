import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  GridDtoBase,
  RenderDetail,
  QueryObject,
  DateFilter,
  CustomGridRender,
  QueryObjectGrid,
} from "./Common";
import { ResultDto } from "./CommonModels";

export interface ClusterInfoDtoGrid extends GridDtoBase {
  infraClusterAsPlannedId?: number;
  applicationName?: string;
  applicationId?: number;
  opCoId?: number;
  verticalResponsibleValue?: string;
  deploymentStatusValue?: string;
  hardwareTypeValue?: string;
  clusterName?: string;
  clustertypeValue?: string;
  platformValue?: string;
  site?: string;
  locationValue?: string;
  opCoValue?: string;
  appClusterName?: string;
}

export interface QueryResultDtoOfClusterInfoDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfClusterInfoDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<ClusterInfoDtoGrid>}
   * @memberof QueryResultDtoOfClusterInfoDtoGrid
   */
  items?: Array<ClusterInfoDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface ClusterInfoQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {number}
   * @memberof ClusterInfoQueryObjectGrid
   */
  infraClusterAsPlannedId?: Array<number>;
  /**
   *
   * @type {number}
   * @memberof ClusterInfoQueryObjectGrid
   */
  applicationId?: Array<number>;
  /**
   *
   * @type {number}
   * @memberof ClusterInfoQueryObjectGrid
   */
  paId?: Array<number>;
  /**
   *
   * @type {number}
   * @memberof ClusterInfoQueryObjectGrid
   */
  opCoId?: Array<number>;

  /**
   *
   * @type {string}
   * @memberof ClusterInfoQueryObjectGrid
   */
  applicationName?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ClusterInfoQueryObjectGrid
   */
  verticalResponsibleValue?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ClusterInfoQueryObjectGrid
   */
  deploymentStatusValue?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ClusterInfoQueryObjectGrid
   */
  hardwareTypeValue?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ClusterInfoQueryObjectGrid
   */
  clusterName?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ClusterInfoQueryObjectGrid
   */
  clustertypeValue?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ClusterInfoQueryObjectGrid
   */
  platformValue?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ClusterInfoQueryObjectGrid
   */
  site?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ClusterInfoQueryObjectGrid
   */
  locationValue?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ClusterInfoQueryObjectGrid
   */
  opCoValue?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof ClusterInfoQueryObjectGrid
   */
  appClusterName?: Array<string>;
}

export interface ClusterInfoGrid {
  ClusterInfoGridResult: QueryResultDtoOfClusterInfoDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_GRID_CLUSTER_INFO = "GET_GRID__CLUSTER_INFO";
export const GET_FILTER_CLUSTER_INFO = "GET_FILTER_CLUSTER_INFO";
