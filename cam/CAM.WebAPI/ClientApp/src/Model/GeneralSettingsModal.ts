import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  GridDtoBase,
  QueryObject,
  DateFilter,
  CustomGridRender,
  QueryObjectGrid
} from "./Common";
import { ResultDto } from "./CommonModels";

/**
 *
 * @export
 * @interface GeneralSettingsQueryDto
 */
export interface GeneralSettingsQueryDto extends QueryObject {
  /**
   * @type {string}
   */
  sortBy: string;

  /**
   * @type {boolean}
   */
  isSortAscending: boolean;

  /**
   * @type {number}
   */
  page: number;

  /**
   * @type {number}
   */
  pageSize: number;

  /**
   * @type {DateFilter}
   */
  lastModified: DateFilter;

  /**
   * @type {number}
   */
  principalId: number;

  /**
   * @type {boolean}
   */
  deleted: boolean;

  /**
   * @type {boolean}
   */
  orphan: boolean;

  /**
   * @type {Array<string>}
   */
  lastModifiedBy: string[];

  /**
   * @type {DateFilter}
   */
  lastModifiedValue: DateFilter;

  /**
   * @type {Array<number>}
   */
  appSettingsConfiguartionId: number[];

  /**
   * @type {Array<number>}
   */
  appSettingsId: number[];

  /**
   * @type {Array<string>}
   */
  settingsValue: string[];
}

/**
 *
 * @export
 * @interface GeneralSettingsDtoGrid
 */
export interface GeneralSettingsDtoGrid extends GridDtoBase {


  /**
   * @type {number}
   */
  appSettingsConfigurationId: number;

  /**
   * @type {number}
   */
  appSettingsId: number;

  /**
   * @type {string}
   */
  settingsValue: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfGeneralSettingsDtoGrid
 */
export interface QueryResultDtoOfGeneralSettingsDtoGrid {
  /**
   * @type {number}
   */
  totalItems?: number;

  /**
   * @type {Array<GeneralSettingsDtoGrid>}
   */
  items?: Array<GeneralSettingsDtoGrid>;

  /**
   * @type {CustomGridRender}
   */
  gridRender?: CustomGridRender;
}

export interface GeneralSettingsQueryObjectGrid extends QueryObjectGrid {
  appSettingsConfiguartionId?: number[];
  appSettingsId?: number[];
  settingsValue?: string[];

  lastModified?: DateFilter;
  lastModifiedValue?: DateFilter; 
    
  }


 /**
 * @export
 * @interface GeneralSettingsDtoUpdate
 */
export interface GeneralSettingsDtoUpdate extends GeneralSettingsDtoGrid {


}
 
/**
 *
 * @export
 * @interface GeneralSettingsGrid
 */
export interface GeneralSettingsGrid {
  GeneralSettingsGridResult: QueryResultDtoOfGeneralSettingsDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface GeneralSettingsEdit {
  GeneralSettingsDtoEdit: GeneralSettingsDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}


export const GET_CREATE_GENERAL_SETTINGS = "GET_CREATE_GENERAL_SETTINGS";
export const GET_EDIT_GENERAL_SETTINGS = "GET_EDIT_GENERAL_SETTINGS";
export const GET_GRID_GENERAL_SETTINGS = "GET_GRID_GENERAL_SETTINGS";
export const GET_FILTER_GENERAL_SETTINGS = "GET_FILTER_GENERAL_SETTINGS";
export const CREATE_GENERAL_SETTINGS = "CREATE_GENERAL_SETTINGS";
export const EDIT_GENERAL_SETTINGS = "EDIT_GENERAL_SETTINGS";
export const DELETE_GENERAL_SETTINGS = "DELETE_GENERAL_SETTINGS";
export const RESTORE_GENERAL_SETTINGS = "RESTORE_GENERAL_SETTINGS";

