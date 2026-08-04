import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  QueryObjectGrid,
  GridDtoBase,
  RenderDetail,
  QueryObject,
} from "./Common";
import { ResultDto } from "./CommonModels";
/**
 *
 * @export
 * @interface DesignComponentFamilyQueryDto
 */
export interface DesignComponentFamilyQueryDto extends QueryObject {
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentFamilyQueryDto
   */
  systemTypeIdentityName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentFamilyQueryDto
   */
  description?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  serviceBoundary?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  systemFunction?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  designComponentFamilyId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  gdprRelevant?: Array<number>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof DesignComponentFamilyQueryDto
   */
  internetFacing?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof DesignComponentFamilyQueryDto
   */
  implementation?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof DesignComponentFamilyQueryDto
   */
  systemIsShared?: Array<boolean>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentFamilyQueryDto
   */
  criticality?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentFamilyQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof DesignComponentFamilyQueryDto
   */
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  sharingType?: Array<number>;
  /**
   *
   * @type {number}
   * @memberof DesignComponentFamilyDto
   */
  platformId?: number;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfDesignComponentFamilyDtoGrid
 */
export interface QueryResultDtoOfDesignComponentFamilyDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfDesignComponentFamilyDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<DesignComponentFamilyDtoGrid>}
   * @memberof QueryResultDtoOfDesignComponentFamilyDtoGrid
   */
  items?: Array<DesignComponentFamilyDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfDesignComponentFamilyDtoGrid}
   * @memberof QueryResultDtoOfDesignComponentFamilyDtoGrid
   */
  gridRender?: CustomGridRenderOfDesignComponentFamilyDtoGrid;
}
/**
 *
 * @export
 * @interface CustomGridRenderOfDesignComponentFamilyDtoGrid
 */
export interface CustomGridRenderOfDesignComponentFamilyDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfDesignComponentFamilyDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfDesignComponentFamilyDtoGrid
   */
  render?: Array<RenderDetail>;
}
/**
 *
 * @export
 * @interface DesignComponentFamilyDto
 */
export interface DesignComponentFamilyDto extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof DesignComponentFamilyDto
   */
  systemTypeIdentityName?: string;
  /**
   *
   * @type {string}
   * @memberof DesignComponentFamilyDto
   */
  description?: string;
  /**
   *
   * @type {number}
   * @memberof DesignComponentFamilyDto
   */
  serviceBoundaryId?: number;
  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDto
   */
  gdprRelevant?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDto
   */
  internetFacing?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDto
   */
  implementation?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDto
   */
  lcmPolicy?: number;
  vodafoneName?:string;
  vodafoneNameId?:number;
  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDto
   */
  systemIsShared?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDto
   */
  /**
   *
   * @type {string}
   * @memberof DesignComponentFamilyDto
   */
  criticality?: string;
  /**
   *
   * @type {number}
   * @memberof DesignComponentFamilyDto
   */
  /**
   *
   * @type {number}
   * @memberof DesignComponentFamilyDto
   */
  sharingTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof DesignComponentFamilyDto
   */
  majorSoftwareOemId?: number;
  /**
   *
   * @type {string}
   * @memberof DesignComponentFamilyDto
   */
  majorProductName?: string;
  /**
   *
   * @type {number}
   * @memberof DesignComponentFamilyDto
   */
  majorHardwareOemId?: number;
  /**
   *
   * @type {number}
   * @memberof DesignComponentFamilyDto
   */
  platformId?: number;
  /**
   *
   * @type {Date}
   * @memberof DesignComponentFamilyDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof DesignComponentFamilyDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface DesignComponentFamilyDtoCreate
 */
export interface DesignComponentFamilyDtoCreate
  extends DesignComponentFamilyDto {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignComponentFamilyDtoCreate
   */
  serviceBoundariesResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignComponentFamilyDtoCreate
   */
  subNetworkBoundariesResource?: { [key: string]: string };

  /**
   *
   * @type {number}
   * @memberof DesignComponentFamilyDtoCreate
   */
  subNetworkBoundaryId: number;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignComponentFamilyDtoCreate
   */
  systemFunctionsResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignComponentFamilyDtoCreate
   */
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignComponentFamilyDtoCreate
   */
  sharingTypeResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyDtoCreate
   */
  systemFunctionsIds?: Array<number>;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignComponentFamilyDtoCreate
   */
  networkFunctionsResource?: { [key: string]: string };

  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyDtoCreate
   */
  networkFunctionsIds?: Array<number>;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignComponentFamilyDtoCreate
   */
  criticalAssetTypeResource?: { [key: string]: string };

  /**
   *
   * @type {nummber}
   * @memberof DesignComponentFamilyDtoCreate
   */
  criticalAssetTypeId?: number;

  /**
   *
   * @type {nummber}
   * @memberof DesignComponentFamilyDtoCreate
   */
  gdprClassification?: number;

  /**
   *
   * @type {nummber}
   * @memberof DesignComponentFamilyDtoCreate
   */
  criticalityRating?: number;

  /**
   *
   * @type {string}
   * @memberof DesignComponentFamilyDtoCreate
   */
  gdprClassificationValue?: string;

  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDtoCreate
   */
  pcisox?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDtoCreate
   */
  securityElement?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDtoCreate
   */
  c3C4?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDtoCreate
   */
  countrySpecificCriticality?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDtoCreate
   */
  missionCritical?: boolean;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignComponentFamilyDtoCreate
   */
  customerWheelsResource?: { [key: string]: string };

  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyDtoCreate
   */
  customerWheelsIds?: Array<number>;
}
/**
 *
 * @export
 * @interface DesignComponentFamilyDtoGrid
 */
export interface DesignComponentFamilyDtoGrid extends DesignComponentFamilyDto {
  /**
   *
   * @type {number}
   * @memberof DesignComponentFamilyDtoGrid
   */
  designComponentFamilyId?: number;
  /**
   *
   * @type {string}
   * @memberof DesignComponentFamilyDtoGrid
   */
  serviceBoundary?: string;
  /**
   *
   * @type {string}
   * @memberof DesignComponentFamilyDtoGrid
   */
  systemFunction?: string;
  /**
   *
   * @type {string}
   * @memberof DesignComponentFamilyDtoGrid
   */
  /**
   *
   * @type {string}
   * @memberof DesignComponentFamilyDtoGrid
   */
  sharingType?: string;

  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDtoGrid
   */
  c3C4?: boolean;
}
/**
 *
 * @export
 * @interface DesignComponentFamilyDtoUpdate
 */
export interface DesignComponentFamilyDtoUpdate
  extends DesignComponentFamilyDtoCreate {
  /**
   *
   * @type {number}
   * @memberof DesignComponentFamilyDtoUpdate
   */
  designComponentFamilyId?: number;
}

export interface DesignComponentFamilyQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentFamilyQueryDto
   */
  systemTypeIdentityName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentFamilyQueryDto
   */
  description?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  serviceBoundary?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  systemFunction?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  designComponentFamilyId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  gdprRelevant?: Array<number>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof DesignComponentFamilyQueryDto
   */
  internetFacing?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof DesignComponentFamilyQueryDto
   */
  implementation?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof DesignComponentFamilyQueryDto
   */
  systemIsShared?: Array<boolean>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentFamilyQueryDto
   */
  criticality?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentFamilyQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof DesignComponentFamilyQueryDto
   */
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentFamilyQueryDto
   */
  sharingType?: Array<number>;
}

/**
 *
 * @param {Array<string>} [systemTypeIdentityName]
 * @param {Array<string>} [description]
 * @param {Array<number>} [serviceBoundary]
 * @param {Array<number>} [systemFunction]
 * @param {Array<number>} [designComponentFamilyId]
 * @param {Array<number>} [gdprRelevant]
 * @param {Array<boolean>} [internetFacing]
 * @param {Array<boolean>} [implementation]
 * @param {Array<boolean>} [systemIsShared]
 * @param {Array<string>} [criticality]
 * @param {Array<string>} [lastModifiedBy]
 * @param {string} [sortBy]
 * @param {boolean} [isSortAscending]
 * @param {number} [page]
 * @param {number} [pageSize]
 * @param {Date} [lastModifiedStartDate]
 * @param {Date} [lastModifiedEndDate]
 * @param {number} [principalId]
 * @param {boolean} [deleted]
 * @param {boolean} [orphan]
 * @param {*} [options] Override http request option.
 * @throws {RequiredError}
 * @memberof DesignComponentFamilyApi
 */

export interface DesignComponentFamilyEdit {
  DesignComponentFamilyDtoEdit: DesignComponentFamilyDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface DesignComponentFamilyCreate {
  DesignComponentFamilyDtoCreate: DesignComponentFamilyDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface DesignComponentFamilyGrid {
  DesignComponentFamilyGridResult: QueryResultDtoOfDesignComponentFamilyDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_DESIGN_COMPONENT_FAMILY =
  "GET_CREATE_DESIGN_COMPONENT_FAMILY";
export const GET_EDIT_DESIGN_COMPONENT_FAMILY =
  "GET_EDIT_DESIGN_COMPONENT_FAMILY";
export const GET_GRID_DESIGN_COMPONENT_FAMILY =
  "GET_GRID_DESIGN_COMPONENT_FAMILY";
export const GET_FILTER_DESIGN_COMPONENT_FAMILY =
  "GET_FILTER_DESIGN_COMPONENT_FAMILY";
export const CREATE_DESIGN_COMPONENT_FAMILY = "CREATE_DESIGN_COMPONENT_FAMILY";
export const EDIT_DESIGN_COMPONENT_FAMILY = "EDIT_DESIGN_COMPONENT_FAMILY";
export const DELETE_DESIGN_COMPONENT_FAMILY = "DELETE_DESIGN_COMPONENT_FAMILY";
export const RESTORE_DESIGN_COMPONENT_FAMILY =
  "RESTORE_DESIGN_COMPONENT_FAMILY";
export const GET_DESIGN_COMPONENT_FAMILY_NAME =
  "GET_DESIGN_COMPONENT_FAMILY_NAME";
export const GET_DESIGN_COMPONENT_FAMILY_CONSTRAINT_INFO =
  "GET_DESIGN_COMPONENT_FAMILY_CONSTRAINT_INFO";
