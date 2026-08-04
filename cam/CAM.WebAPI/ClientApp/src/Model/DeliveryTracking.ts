import { ResultDto } from "./CommonModels";
import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  DateFilter,
  EOM_STATUS,
  GridDtoBase,
  QueryObject,
  QueryObjectGrid,
} from "./Common";
/**
 *
 * @export
 * @interface DeliveryTrackingDto
 */
export interface DeliveryTrackingDto extends GridDtoBase {
  activity?: string;
  ms1EventType?: number;
  ms1BaseLineDate?: Date;
  ms1LatestPlanningDate?: Date;
  ms1status?: number;

  ms2EventType?: number;
  ms2BaseLineDate?: Date;
  ms2LatestPlanningDate?: Date;
  ms2status?: number;

  ms3EventType?: number;
  ms3BaseLineDate?: Date;
  ms3LatestPlanningDate?: Date;
  ms3status?: number;

  ms4EventType?: number;
  ms4BaseLineDate?: Date;
  ms4LatestPlanningDate?: Date;
  ms4status?: number;
  plannedActivityId?: number;
  ppmImportDate?: Date;
  ppmID?: number;
  notes1?: string;
  notes2?: string;

  /**
   *
   * @type {Date}
   * @memberof DeliveryTrackingDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof DeliveryTrackingDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface DeliveryTrackingDtoCreate
 */
export interface DeliveryTrackingDtoCreate extends DeliveryTrackingDto {
  id?: number;
  ms1StatusResource?: { [key: string]: string };
  mS1EventTypeResource?: { [key: string]: string };
  mS2EventTypeResource?: { [key: string]: string };
  ms2StatusResource?: { [key: string]: string };
  mS3EventTypeResource?: { [key: string]: string };
  ms3StatusResource?: { [key: string]: string };
  mS4EventTypeResource?: { [key: string]: string };
  ms4StatusResource?: { [key: string]: string };
  msStatusResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface DeliveryTrackingDtoGrid
 */
export interface DeliveryTrackingDtoGrid extends DeliveryTrackingDto {
  id?: number;
}
/**
 *
 * @export
 * @interface DeliveryTrackingDtoUpdate
 */
export interface DeliveryTrackingDtoUpdate extends DeliveryTrackingDtoCreate {}
/**
 *
 * @export
 * @interface QueryResultDtoOfDeliveryTrackingDtoGrid
 */
export interface QueryResultDtoOfDeliveryTrackingDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfDeliveryTrackingDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<DeliveryTrackingDtoGrid>}
   * @memberof QueryResultDtoOfDeliveryTrackingDtoGrid
   */
  items?: Array<DeliveryTrackingDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @export
 * @interface ResultDtoOfDeliveryTrackingToCloneDto
 */
export interface ResultDtoOfDeliveryTrackingToCloneDto {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfDeliveryTrackingToCloneDto
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfDeliveryTrackingToCloneDto
   */
  info?: string;
  /**
   *
   * @type {DeliveryTrackingToCloneDto}
   * @memberof ResultDtoOfDeliveryTrackingToCloneDto
   */
  data?: DeliveryTrackingToCloneDto;
}
/**
 *
 * @export
 * @interface DeliveryTrackingToCloneDto
 */
export interface DeliveryTrackingToCloneDto {}

/**
 *
 * @export
 * @interface CloneDeliveryTrackingDto
 */
export interface CloneDeliveryTrackingDto {}

// ------------------------ Not generated  ------------------------

export interface DeliveryTrackingEdit {
  DeliveryTrackingDtoEdit: DeliveryTrackingDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface DeliveryTrackingCreate {
  DeliveryTrackingDtoCreate: DeliveryTrackingDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface DeliveryTrackingGrid {
  DeliveryTrackingGridResult: QueryResultDtoOfDeliveryTrackingDtoGrid | null;
  filter: FilterValueDto[] | null;
}
export interface DeliveryTrackingQueryObjectGrid extends QueryObject {
  id?: Array<number>;
  activity?: Array<string>;
  ms1EventType?: Array<string>;
  ms1BaseLineDate?: Array<Date>;
  ms1LatestPlanningDate?: Array<Date>;
  ms1status?: Array<string>;

  ms2EventType?: Array<string>;
  ms2BaseLineDate?: Array<Date>;
  ms2LatestPlanningDate?: Array<Date>;
  ms2status?: Array<string>;

  ms3EventType?: Array<string>;
  ms3BaseLineDate?: Array<Date>;
  ms3LatestPlanningDate?: Array<Date>;
  ms3status?: Array<string>;

  ms4EventType?: Array<string>;
  ms4BaseLineDate?: Array<Date>;
  ms4LatestPlanningDate?: Array<Date>;
  ms4status?: Array<string>;

  ppmImportDate?: Array<Date>;
  ppmID?: number;
  notes1?: Array<string>;
  notes2?: Array<string>;
}

export const GET_CREATE_DELIVERY_TRACKING = "GET_CREATE_DELIVERY_TRACKING";
export const GET_EDIT_DELIVERY_TRACKING = "GET_EDIT_DELIVERY_TRACKING";
export const GET_GRID_DELIVERY_TRACKING = "GET_GRID_DELIVERY_TRACKING";
export const GET_FILTER_DELIVERY_TRACKING = "GET_FILTER_DELIVERY_TRACKING";
export const CREATE_DELIVERY_TRACKING = "CREATE_DELIVERY_TRACKING";
export const EDIT_DELIVERY_TRACKING = "EDIT_DELIVERY_TRACKING";
export const DELETE_DELIVERY_TRACKING = "DELETE_DELIVERY_TRACKING";
export const RESTORE_DELIVERY_TRACKING = "RESTORE_DELIVERY_TRACKING";
