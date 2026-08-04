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
 * @interface SettingsUpdatePlannedActivityDto
 */
export interface SettingsUpdatePlannedActivityDto extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof SettingsUpdatePlannedActivityDto
   */
  settingsUpdatePlannedActivityDescription?: string;
  /**
   *
   * @type {string}
   * @memberof SettingsUpdatePlannedActivityDto
   */
  localApproval?: string;
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDto
   */
  order?: number;
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDto
   */
  maxOrder?: number;
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDto
   */
  rule?: number;
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDto
   */
  ruleElementCount?: number;
  /**
   *
   * @type {Date}
   * @memberof SettingsUpdatePlannedActivityDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof SettingsUpdatePlannedActivityDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface SettingsUpdatePlannedActivityDtoCreate
 */
export interface SettingsUpdatePlannedActivityDtoCreate
  extends SettingsUpdatePlannedActivityDto {
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  deliveryStatusId?: number;
  /**
   *
   * @type {string}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  msStatus?: string;
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  msStatusDuration?: number;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  deliveryStatusResource?: { [key: string]: string };

  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  plannedActivityTypeFor?: number;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  successorPlannedActivityTypeResource?: { [key: string]: string };

  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  successorPlannedActivityId?: number;

  /**
   *
   * @type {boolean}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  ruleforSuccessorPlannedActivityCreation?: boolean;
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  lcmDeploymentStatusIds?: Array<number>;

  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  assetDeploymentStatusIds?: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  lcmDeploymentStatusResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  planningActivityStatusId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  planningActivityStatusResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  budgetAvailabilityId?: number;

  planningActivityResource?: {
    [key: string]: string;
  };

  plannedActivityTypeDescription?: {
    [key: string]: string;
  };
  isMileStone?: string | boolean;
  planningActivityResourceId?: number;
  specifyDC?: boolean;
  isLiveStatusDateAvailable?: boolean;
  isDecommissionedDateAvailable?: boolean;
  needPlannedAsset?: boolean;
  isRollback?: boolean;
  isReleaseDetailsUnknown?: boolean;
  isPAReleaseDetailsUnknown?: boolean;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  budgetAvaibilityResource?: { [key: string]: string };
  /**
   *
   * @type {Array<RelatedResource>}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  crossSettingsOutIds?: Array<RelatedResource>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SettingsUpdatePlannedActivityDtoCreate
   */
  crossSettingscResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface SettingsUpdatePlannedActivityDtoGrid
 */
export interface SettingsUpdatePlannedActivityDtoGrid
  extends SettingsUpdatePlannedActivityDto {
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoGrid
   */
  settingsUpdatePlannedActivityId?: number;
  /**
   *
   * @type {string}
   * @memberof SettingsUpdatePlannedActivityDtoGrid
   */
  settingsUpdatePlannedActivityDescription?: string;
  /**
   *
   * @type {string}
   * @memberof SettingsUpdatePlannedActivityDtoGrid
   */
  plannedActivityTypeDescription?: string;
  /**
   *
   * @type {string}
   * @memberof SettingsUpdatePlannedActivityDtoGrid
   */
  localApproval?: string;
  /**
   *
   * @type {string}
   * @memberof SettingsUpdatePlannedActivityDtoGrid
   */
  deliveryStatus?: string;
  /**
   *
   * @type {string}
   * @memberof SettingsUpdatePlannedActivityDtoGrid
   */
  planningActivityStatus?: string;
  /**
   *
   * @type {string}
   * @memberof SettingsUpdatePlannedActivityDtoGrid
   */
  budgetAvailability?: string;
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoGrid
   */
  order?: number;
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoGrid
   */
  maxOrder?: number;
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoGrid
   */
  rule?: number;
  /**
   *
   * @type {string}
   * @memberof SettingsUpdatePlannedActivityDtoGrid
   */
  crossSetting?: string;
}
/**
 *
 * @export
 * @interface SettingsUpdatePlannedActivityDtoUpdate
 */
export interface SettingsUpdatePlannedActivityDtoUpdate
  extends SettingsUpdatePlannedActivityDtoCreate {
  /**
   *
   * @type {number}
   * @memberof SettingsUpdatePlannedActivityDtoUpdate
   */
  settingsUpdatePlannedActivityId?: number;
}

export interface SettingsUpdatePlannedActivityQueryObjectGrid
  extends QueryObjectGrid {
  planningActivityResource?: Array<string>;
  plannedActivityTypeDescription?: Array<string>;
  plannedActivityTypeFor?: Array<number>;
  successorPlannedActivityTypeResource?: Array<string>;
  ruleElementCount?: Array<string>;
  deliveryStatus?: Array<string>;
  maxOrder?: Array<number>;
  planningActivityStatus?: Array<number>;
  order?: Array<number>;
  budgetAvailability?: Array<string>;
  deliveryStatusId?: Array<number>;
  msStatus?: Array<string>;
  msStatusDuration?: Array<number>;
  lastModifiedBy?: Array<string>;
  crossSetting?: Array<string>;
  sortBy?: string;
  isSortAscending?: boolean;
  ruleforSuccessorPlannedActivityCreation?: Array<boolean>;
  rule?: Array<number>;
  page?: number;
  pageSize?: number;
  lastModifiedStartDate?: Date;
  lastModifiedEndDate?: Date;
  principalId?: number;
  deleted?: boolean;
  orphan?: boolean;
  settingsUpdatePlannedActivityDescription?: Array<string>;
  localApproval?: Array<string>;
  lcmDeploymentStatus?: Array<string>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid
 */
export interface QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<SettingsUpdatePlannedActivityDtoGrid>}
   * @memberof QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid
   */
  items?: Array<SettingsUpdatePlannedActivityDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfSettingsUpdatePlannedActivityDtoGrid}
   * @memberof QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid
   */
  gridRender?: CustomGridRenderOfSettingsUpdatePlannedActivityDtoGrid;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfSettingsUpdatePlannedActivityDtoGrid
 */
export interface CustomGridRenderOfSettingsUpdatePlannedActivityDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfSettingsUpdatePlannedActivityDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfSettingsUpdatePlannedActivityDtoGrid
   */
  render?: Array<RenderDetail>;
}

export interface SettingsUpdatePlannedActivityEdit {
  SettingsUpdatePlannedActivityDtoEdit: SettingsUpdatePlannedActivityDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface SettingsUpdatePlannedActivityCreate {
  SettingsUpdatePlannedActivityDtoCreate: SettingsUpdatePlannedActivityDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface SettingsUpdatePlannedActivityGrid {
  SettingsUpdatePlannedActivityGridResult: QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY =
  "GET_CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY";
export const GET_EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY =
  "GET_EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY";
export const GET_GRID_SETTINGS_UPDATE_PLANNED_ACTIVITY =
  "GET_GRID_SETTINGS_UPDATE_PLANNED_ACTIVITY";
export const GET_FILTER_SETTINGS_UPDATE_PLANNED_ACTIVITY =
  "GET_FILTER_SETTINGS_UPDATE_PLANNED_ACTIVITY";
export const CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY =
  "CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY";
export const EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY =
  "EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY";
export const DELETE_SETTINGS_UPDATE_PLANNED_ACTIVITY =
  "DELETE_SETTINGS_UPDATE_PLANNED_ACTIVITY";
export const RESTORE_SETTINGS_UPDATE_PLANNED_ACTIVITY =
  "RESTORE_SETTINGS_UPDATE_PLANNED_ACTIVITY";
export const GET_SETTINGS_UPDATE_PLANNED_ACTIVITY_NAME =
  "GET_SETTINGS_UPDATE_PLANNED_ACTIVITY_NAME";
export const GET_SETTINGS_UPDATE_PLANNED_ACTIVITY_CONSTRAINT_INFO =
  "GET_SETTINGS_UPDATE_PLANNED_ACTIVITY_CONSTRAINT_INFO";

export enum PlannedActivityTypeForEnum {
  LcmEngineering,
  AddAsset,
  EditAsset,
  DesignAspect,
  ServicePlan,
}
