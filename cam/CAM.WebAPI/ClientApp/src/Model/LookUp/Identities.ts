import { ResultDto } from "../CommonModels";
import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  DateFilter,
  EOM_STATUS,
  GridDtoBase,
  QueryObject,
  QueryObjectGrid,
} from "../Common";
/**
 *
 * @export
 * @interface IdentityAsIsDto
 */
export interface IdentityAsIsDto extends GridDtoBase {
  id?: number;
  value?: string;
  resourceKey?: string;
  previousResourceKey?: string;
  categoryId?: number;
  classId?: number;
  typeId?: number;
  assetId?: number;
  categoryDescription?: string;
  classDescription?: string;
  typeDescription?: string;
  opcoId?: number;
  dcfId?: number;

  /**
   *
   * @type {Date}
   * @memberof IdentityAsIsDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof IdentityAsIsDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface IdentityAsIsDtoCreate
 */
export interface IdentityAsIsDtoCreate extends IdentityAsIsDto {
  assetResource?: { [key: string]: string };
  opCoResource?: { [key: string]: string };
  typeResource?: { [key: string]: string };
  classResource?: { [key: string]: string };
  categoryResource?: { [key: string]: string };
  dcfResource?: { [key: string]: string };
  interfaceTypes?: { [key: string]: string };
  interfaceType?: number;
  interfaceName?: string;
}
/**
 *
 * @export
 * @interface IdentityAsIsDtoGrid
 */
export interface IdentityAsIsDtoGrid extends IdentityAsIsDto {}
/**
 *
 * @export
 * @interface IdentityAsIsDtoUpdate
 */
export interface IdentityAsIsDtoUpdate extends IdentityAsIsDtoCreate {}
/**
 *
 * @export
 * @interface QueryResultDtoOfIdentityAsIsDtoGrid
 */
export interface QueryResultDtoOfIdentityAsIsDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfIdentityAsIsDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<IdentityAsIsDtoGrid>}
   * @memberof QueryResultDtoOfIdentityAsIsDtoGrid
   */
  items?: Array<IdentityAsIsDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface IdentityAsIsGrid {
  IdentityAsIsGridResult: QueryResultDtoOfIdentityAsIsDtoGrid | null;
  filter: FilterValueDto[] | null;
}
export interface IdentityAsIsQueryObjectGrid extends QueryObject {
  id?: Array<number>;
  value?: string;
  resourceKey?: string;
  previousResourceKey?: string;
  categoryId?: number;
  classId?: number;
  typeId?: number;
  assetId?: number;
  categoryDescription?: string;
  classDescription?: string;
  typeDescription?: string;
  opcoId?: number;
  dcfId?: number;
  lastModifiedBy?: Array<string>;

  designComponentFamily?: Array<number>;
}

export interface IdentityAsIsEdit {
  IdentityAsIsDtoEdit: IdentityAsIsDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface IdentityAsIsCreate {
  IdentityAsIsDtoCreate: IdentityAsIsDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface IdentityAsIsGrid {
  IdentityAsIsGridResult: QueryResultDtoOfIdentityAsIsDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface IdentityAsIsQueryDto {}

export const GET_CREATE_IDENTiTYASIS = "GET_CREATE_IDENTiTYASIS";
export const GET_EDIT_IDENTiTYASIS = "GET_EDIT_IDENTiTYASIS";
export const GET_GRID_IDENTITYASIS = "GET_GRID_IDENTITYASIS";
export const GET_FILTER_IDENTITYASIS = "GET_FILTER_IDENTITYASIS";
export const CREATE_IDENTITYASIS = "CREATE_IDENTITYASIS";
export const EDIT_IDENTiTYASIS = "EDIT_IDENTiTYASIS";
export const RESTORE_IDENTiTYASIS = "RESTORE_IDENTiTYASIS";
export const DELETE_IDENTiTYASIS = "DELETE_IDENTiTYASIS";
// export const GET_RELATED_RECORDS_IDENTiTYASIS= "GET_RELATED_RECORDS_MAJOR_HARDWARE_BUILD";
