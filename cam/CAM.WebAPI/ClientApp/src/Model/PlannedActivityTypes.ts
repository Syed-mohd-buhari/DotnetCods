import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  QueryObjectGrid,
  GridDtoBase,
  RenderDetail,
} from "./Common";
import { RelatedResource, ResultDto } from "./CommonModels";

/**
 *
 * @export
 * @interface PlannedActivityTypesDto
 */
export interface PlannedActivityTypesDto extends GridDtoBase {
  /**
   *
   * @type {Date}
   * @memberof PlannedActivityTypesDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityTypesDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface PlannedActivityTypesDtoGrid
 */
export interface PlannedActivityTypesDtoGrid extends PlannedActivityTypesDto {
  /**
   *
   * @type {number}
   * @memberof PlannedActivityTypesDtoGrid
   */
  plannedActivityTypesId?: number;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityTypesDtoGrid
   */
  plannedActivityTypeDescription?: string;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoGrid
   */
  hwOem?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoGrid
   */
  hwSolution?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoGrid
   */
  hwPlatform?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoGrid
   */
  swOem?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoGrid
   */
  swVersion?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoGrid
   */
  swProductname?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoGrid
   */
  subNetworkService?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoGrid
   */
  linkedDcRule?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoGrid
   */
  forLcm?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoGrid
   */
  forAsset?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoGrid
   */
  forDesignAspect?: boolean;
}
/**
 *
 * @export
 * @interface PlannedActivityTypesDtoCreate
 */
export interface PlannedActivityTypesDtoCreate extends PlannedActivityTypesDto {
  /**
   *
   * @type {string}
   * @memberof PlannedActivityTypesDtoCreate
   */
  plannedActivityTypeDescription?: string;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoCreate
   */
  hwOem?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoCreate
   */
  hwSolution?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoCreate
   */
  hwPlatform?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoCreate
   */
  swOem?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoCreate
   */
  swVersion?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoCreate
   */
  swProductname?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoCreate
   */
  subNetworkService?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoCreate
   */
  linkedDcRule?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoCreate
   */
  forLcm?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoCreate
   */
  forAsset?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityTypesDtoCreate
   */
  forDesignAspect?: boolean;
}
/**
 *
 * @export
 * @interface PlannedActivityTypesDtoUpdate
 */
export interface PlannedActivityTypesDtoUpdate
  extends PlannedActivityTypesDtoCreate {
  /**
   *
   * @type {number}
   * @memberof PlannedActivityTypesDtoGrid
   */
  plannedActivityTypesId: number;
}

export interface PlannedActivityTypesQueryObjectGrid extends QueryObjectGrid {
  plannedActivityTypesId?: Array<number>;
  plannedActivityTypeDescription?: Array<string>;
  hwOem?: Array<boolean>;
  hwSolution?: Array<boolean>;
  hwPlatform?: Array<boolean>;
  swOem?: Array<boolean>;
  swProductname?: Array<boolean>;
  swVersion?: Array<boolean>;
  subNetworkService?: Array<boolean>;
  forDesignAspect?: Array<boolean>;
  forserviceplan?: Array<boolean>;
  forLcm?: Array<boolean>;
  forAsset?: Array<boolean>;
  linkedDcRule?: Array<boolean>;
  sortBy?: string;
  isSortAscending?: boolean;
  page?: number;
  pageSize?: number;
  lastModifiedStartDate?: Date;
  lastModifiedEndDate?: Date;
  lastModifiedBy?: Array<string>;
  principalId?: number;
  deleted?: boolean;
  orphan?: boolean;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfPlannedActivityTypesDtoGrid
 */
export interface QueryResultDtoOfPlannedActivityTypesDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfPlannedActivityTypesDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<PlannedActivityTypesDtoGrid>}
   * @memberof QueryResultDtoOfPlannedActivityTypesDtoGrid
   */
  items?: Array<PlannedActivityTypesDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfPlannedActivityTypesDtoGrid}
   * @memberof QueryResultDtoOfPlannedActivityTypesDtoGrid
   */
  gridRender?: CustomGridRenderOfPlannedActivityTypesDtoGrid;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfPlannedActivityTypesDtoGrid
 */
export interface CustomGridRenderOfPlannedActivityTypesDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfPlannedActivityTypesDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfPlannedActivityTypesDtoGrid
   */
  render?: Array<RenderDetail>;
}

export interface PlannedActivityTypesEdit {
  PlannedActivityTypesDtoEdit: PlannedActivityTypesDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface PlannedActivityTypesCreate {
  PlannedActivityTypesDtoCreate: PlannedActivityTypesDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface PlannedActivityTypesGrid {
  PlannedActivityTypesGridResult: QueryResultDtoOfPlannedActivityTypesDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_PLANNED_ACTIVITY_TYPES =
  "GET_CREATE_PLANNED_ACTIVITY_TYPES";
export const GET_EDIT_PLANNED_ACTIVITY_TYPES =
  "GET_EDIT_PLANNED_ACTIVITY_TYPES";
export const GET_GRID_PLANNED_ACTIVITY_TYPES =
  "GET_GRID_PLANNED_ACTIVITY_TYPES";
export const GET_FILTER_PLANNED_ACTIVITY_TYPES =
  "GET_FILTER_PLANNED_ACTIVITY_TYPES";
export const CREATE_PLANNED_ACTIVITY_TYPES = "CREATE_PLANNED_ACTIVITY_TYPES";
export const EDIT_PLANNED_ACTIVITY_TYPES = "EDIT_PLANNED_ACTIVITY_TYPES";
export const DELETE_PLANNED_ACTIVITY_TYPES = "DELETE_PLANNED_ACTIVITY_TYPES";
export const RESTORE_PLANNED_ACTIVITY_TYPES = "RESTORE_PLANNED_ACTIVITY_TYPES";
export const GET_PLANNED_ACTIVITY_TYPES_NAME =
  "GET_PLANNED_ACTIVITY_TYPES_NAME";
export const GET_PLANNED_ACTIVITY_TYPES_CONSTRAINT_INFO =
  "GET_PLANNED_ACTIVITY_TYPES_CONSTRAINT_INFO";
export const PLANNED_ACTIVITY_TYPES_ENABLE_LINKED_DC =
  "PLANNED_ACTIVITY_TYPES_ENABLE_LINKED_DC";

export enum PlannedActivityTypeForEnum {
  LcmEngineering,
  AddAsset,
  EditAsset,
  DesignAspect,
}
