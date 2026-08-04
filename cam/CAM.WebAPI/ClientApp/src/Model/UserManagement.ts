import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  QueryObjectGrid,
  GridDtoBase,
  RenderDetail,
  QueryObject,
  DateFilter,
  CustomGridRender,
} from "./Common";
import { ResultDto } from "./CommonModels";
import { OrganizationInfoDtoCreate } from "./OrganizationInfo";

export interface UserManagementQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof UserManagementQueryDto
   */
  roleId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof UserManagementQueryDto
   */
  userId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof UserManagementQueryDto
   */
  aspNetUserRoleId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof UserManagementDtoGrid
   */
  userName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof UserManagementDtoGrid
   */
  email?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof UserManagementQueryDto
   */
  tempCreationDate?: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof UserManagementQueryDto
   */
  opCoId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof UserManagementQueryDto
   */
  verticalResponsibleId?: Array<number>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof UserManagementQueryDto
   */
  active?: Array<boolean>;
  /**
   *
   * @type {Array<string>}
   * @memberof UserManagementQueryDto
   */
  opCo?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof UserManagementQueryDto
   */
  creationUser?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof UserManagementQueryDto
   */
  creationDate?: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof UserManagementQueryDto
   */
  modificationUser?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof UserManagementQueryDto
   */
  modificationDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof UserManagementQueryDto
   */
  verticalResponsible?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof UserManagementQueryDto
   */
  role?: Array<string>;
  /**
   *
   * @type {{ [key: string]: string }}
   * @memberof UserManagementQueryDto
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string }}
   * @memberof UserManagementQueryDto
   */
  verticalResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string }}
   * @memberof UserManagementQueryDto
   */
  roleResource?: { [key: string]: string };
  /**
   *
   * @type {Array<boolean>}
   * @memberof UserManagementQueryDto
   */
  archived?: boolean;
  /**
   *
   * @type {Array<DateFilter>}
   * @memberof UserManagementQueryDto
   */
  lastModified?: Array<DateFilter>;
  /**
   *
   * @type {Array<string>}
   * @memberof UserManagementQueryDto
   */
  lastModifiedBy?: Array<string>;
}
/**
 *
 * @export
 * @interface UserManagementDtoCreate
 */
export interface UserManagementDtoEdit {
  /**
   *
   * @type {number}
   * @memberof UserManagementDtoCreate
   */
  userId?: number;
  /**
   *
   * @type {number}
   * @memberof UserManagementDtoCreate
   */
  roleId?: number;
  /**
   *
   * @type {Date}
   * @memberof UserManagementDtoCreate
   */
  tempCreationDate?: Date;
  /**
   *
   * @type {number}
   * @memberof UserManagementDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {number}
   * @memberof UserManagementDtoCreate
   */
  verticalResponsibleId?: number;
  /**
   *
   * @type {boolean}
   * @memberof UserManagementDtoCreate
   */
  active?: boolean;
  /**
   *
   * @type {string}
   * @memberof UserManagementDtoCreate
   */
  opCo?: string;
  /**
   *
   * @type {number}
   * @memberof UserManagementDtoCreate
   */
  creationUser?: number;
  /**
   *
   * @type {Date}
   * @memberof UserManagementDtoCreate
   */
  creationDate?: Date;
  /**
   *
   * @type {number}
   * @memberof UserManagementDtoCreate
   */
  modificationUser?: number;
  /**
   *
   * @type {Date}
   * @memberof UserManagementDtoCreate
   */
  modificationDate?: Date;
  /**
   *
   * @type {string}
   * @memberof UserManagementDtoCreate
   */
  verticalResponsible?: string;
  /**
   *
   * @type {string}
   * @memberof UserManagementDtoCreate
   */
  role?: string;
  /**
   *
   * @type {{ [key: string]: string }}
   * @memberof UserManagementDtoCreate
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string }}
   * @memberof UserManagementDtoCreate
   */
  verticalResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string }}
   * @memberof UserManagementDtoCreate
   */
  roleResource?: { [key: string]: string };
  /**
   *
   * @type {Date}
   * @memberof UserManagementDtoCreate
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof UserManagementDtoCreate
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {string}
   * @memberof UserManagementDtoCreate
   */
  userName?: string;
  /**
   *
   * @type {string}
   * @memberof UserManagementDtoCreate
   */
  email: string;
}

/**
 *
 * @export
 * @interface UserManagementDtoEdit
 */
export interface UserManagementDtoCreate extends UserManagementDtoEdit {}

/**
 *
 * @export
 * @interface UserManagementRoleDtoCreate
 */
export interface UserManagementRoleDtoCreate {
  /**
   *
   * @type {number}
   * @memberof UserManagementRoleDtoCreate
   */
  userId: number;
  /**
   *
   * @type {string}
   * @memberof UserManagementRoleDtoCreate
   */
  email: string;
  /**
   *
   * @type {string}
   * @memberof UserManagementRoleDtoCreate
   */
  userName?: string;
  /**
   *
   * @type {boolean}
   * @memberof UserManagementRoleDtoCreate
   */
  active: boolean;
  /**
   *
   * @type {number}
   * @memberof UserManagementRoleDtoCreate
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof UserManagementRoleDtoCreate
   */
  verticalResponsibleId?: number;
  /**
   *
   * @type {number}
   * @memberof UserManagementRoleDtoCreate
   */
  roleId?: number;
}

/**
 *
 * @export
 * @interface UserManagementRoleDtoEdit
 */
export interface UserManagementRoleDtoEdit extends UserManagementRoleDtoCreate {
  /**
   *
   * @type {number}
   * @memberof UserManagementRoleDtoEdit
   */
  aspNetUserRoleId: number;
}

/**
 *
 * @export
 * @interface UserManagementDtoGrid
 */
export interface UserManagementDtoGrid {
  /**
   *
   * @type {number}
   * @memberof UserManagementDtoGrid
   */
  aspNetUserRoleId?: number;
  /**
   *
   * @type {number}
   * @memberof UserManagementDtoGrid
   */
  userId?: number;
  /**
   *
   * @type {string}
   * @memberof UserManagementDtoGrid
   */
  userName?: string;
  /**
   *
   * @type {string}
   * @memberof UserManagementDtoGrid
   */
  email?: string;
  /**
   *
   * @type {boolean}
   * @memberof UserManagementDtoGrid
   */
  active?: boolean;
  /**
   *
   * @type {string}
   * @memberof UserManagementDtoGrid
   */
  opCo?: string;
  /**
   *
   * @type {string}
   * @memberof UserManagementDtoGrid
   */
  role?: string;
  /**
   *
   * @type {string}
   * @memberof UserManagementDtoGrid
   */
  verticalResponsible?: string;
}
/**
 *
 * @export
 * @interface UserManagementRoleDtoGrid
 */
export interface UserManagementRoleDtoGrid {
  /**
   *
   * @type {number}
   * @memberof UserManagementRoleDtoGrid
   */
  aspNetUserRoleId?: number;
  /**
   *
   * @type {string}
   * @memberof UserManagementRoleDtoGrid
   */
  opCo?: string;
  /**
   *
   * @type {string}
   * @memberof UserManagementRoleDtoGrid
   */
  role?: string;
  /**
   *
   * @type {string}
   * @memberof UserManagementRoleDtoGrid
   */
  verticalResponsible?: string;
}
/**
 *
 * @export
 * @interface UserManagementRoleDtoGrid
 */
export interface UserManagementRoleDtoGrid {
  /**
   *
   * @type {number}
   * @memberof UserManagementRoleDtoGrid
   */
  aspNetUserRoleId?: number;
  /**
   *
   * @type {string}
   * @memberof UserManagementRoleDtoGrid
   */
  opCo?: string;
  /**
   *
   * @type {string}
   * @memberof UserManagementRoleDtoGrid
   */
  role?: string;
  /**
   *
   * @type {string}
   * @memberof UserManagementRoleDtoGrid
   */
  verticalResponsible?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfUserManagementDtoGrid
 */
export interface QueryResultDtoOfUserManagementDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfUserManagementDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<UserManagementDtoGrid>}
   * @memberof QueryResultDtoOfUserManagementDtoGrid
   */
  items?: Array<UserManagementDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof QueryResultDtoOfUserManagementDtoGrid
   */
  gridRender?: CustomGridRender;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfGetUserResourceListDtoGrid
 */
export interface QueryResultDtoOfGetUserResourceListDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfGetUserResourceListDtoGrid
   */
  userId?: number;
  /**
   *
   * @type {string}
   * @memberof QueryResultDtoOfGetUserResourceListDtoGrid
   */
  userName?: string;
  /**
   *
   * @type {string}
   * @memberof QueryResultDtoOfGetUserResourceListDtoGrid
   */
  email?: number;
  /**
   *
   * @type {boolean}
   * @memberof QueryResultDtoOfGetUserResourceListDtoGrid
   */
  active?: boolean;
  /**
   *
   * @type {{ [key: string]: string }}
   * @memberof QueryResultDtoOfGetUserResourceListDtoGrid
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string }}
   * @memberof QueryResultDtoOfGetUserResourceListDtoGrid
   */
  verticalResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string }}
   * @memberof QueryResultDtoOfGetUserResourceListDtoGrid
   */
  roleResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string }}
   * @memberof QueryResultDtoOfGetUserResourceListDtoGrid
   */
  roleDescriptionResource?: { [key: string]: string };
}

export interface OrganisationDetail {
  organisationId?: number;
  mainOrganisation?: string;
  practice?: string;
  practiceContact?: string;
  contact?: string;
  mainOrganisationId?: number;
  practiceId?: number;
  practiceContactId?: number;
  opCo?: string;
  verticalResponsible?: string;
  subdomainResponsible?: string;
  subdomainResponsibleId?: number;
  contactId?: number;
  isSubDomainSpoc?: boolean;
  isEduSpoc?: boolean;
  deleted?: boolean;
  orphan?: boolean | null;
  lastModified?: string;
  lastModifiedBy?: string;
}

export interface UserPreferenceDetail {
  id?: string;
  text?: string;
  path?: string;
  order?: number;
  default?: boolean;
  menu?: string;
  screenId?: number;
  screenPermission?: number;
  permissionLevel?: number;
}

export interface perissionResource {
  id: string;
  text: string;
  path: string;
  order: number;
  default: boolean;
  menu: string;
  screenId: number;
  screenPermission: number;
  permissionLevel?: number;
}

export interface UserManagementRoleResponse {
  aspNetUserRovDetails?: Array<UserManagementRoleDtoGrid>;
  organisationDetaial?: Array<OrganisationDetail>;
  organisationResource?: OrganizationInfoDtoCreate;
  userPrefrenceDetails?: Array<UserPreferenceDetail>;
  perissionResource?: Array<perissionResource>;
}

export interface QueryResultDtoOfUserManagementRoleDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfUserManagementRoleDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<UserManagementRoleDtoGrid>}
   * @memberof QueryResultDtoOfUserManagementRoleDtoGrid
   */
  data?: UserManagementRoleResponse;
  /**
   *
   * @type {CustomGridRender}
   * @memberof QueryResultDtoOfUserManagementRoleDtoGrid
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @param {Array<number>} [userId]
 * @param {Array<number>} [roleId]
 * @param {Array<string>} [userName]
 * @param {Array<string>} [email]
 * @param {Array<string>} [opCo]
 * @param {Array<number>} [creationUser]
 * @param {Array<string>} [role]
 * @param {Array<number>} [aspNetUserRoleId]
 * @param {Array<string>} [verticalResponsible]
 * @param {Array<number>} [modificationUser]
 * @param {Array<number>} [opCoId]
 * @param {Date} [tempCreationDate]
 * @param {Date} [creationDate]
 * @param {Date} [modificationDate]
 * @param {Array<boolean>} [active]
 * @param {Array<number>} [verticalResponsibleId]
 * @param {Date} [dataAcquisitionDateStartDate]
 * @param {Date} [dataAcquisitionDateEndDate]
 * @param {string} [sortBy]
 * @param {boolean} [isSortAscending]
 * @param {number} [page]
 * @param {number} [pageSize]
 * @param {Date} [lastModifiedStartDate]
 * @param {Date} [lastModifiedEndDate]
 * @param {number} [principalId]
 * @param {boolean} [deleted]
 * @param {boolean} [orphan]
 * @param {Array<string>} [lastModifiedBy]
 * @param {*} [options] Override http request option.
 * @throws {RequiredError}
 */

export interface UserManagementEdit {
  UserManagementDtoEdit: UserManagementDtoEdit | null;
  ResultDtoEdit: ResultDto | null;
}

export interface UserManagementCreate {
  UserManagementDtoCreate: UserManagementDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface UserManagementEditRole {
  UserManagementRoleDtoEdit: UserManagementRoleDtoEdit | null;
  ResultDtoEdit: ResultDto | null;
}

export interface UserManagementCreateRole {
  UserManagementRoleDtoCreate: UserManagementRoleDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface UserManagementGrid {
  UserManagementGridResult: QueryResultDtoOfUserManagementDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface UserManagementRoleGrid {
  UserManagementRoleGridResult: QueryResultDtoOfUserManagementRoleDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface GetUserResourceListGrid {
  GetUserResourceListGridResult: QueryResultDtoOfGetUserResourceListDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_USER_MANAGEMENT = "GET_CREATE_USER_MANAGEMENT";
export const GET_EDIT_USER_MANAGEMENT = "GET_EDIT_USER_MANAGEMENT";
export const GET_GRID_USER_MANAGEMENT = "GET_GRID_USER_MANAGEMENT";
export const GET_GRID_USER_RESOURCE = "GET_GRID_USER_RESOURCE";
export const GET_GRID_USER_MANAGEMENT_ROLE = "GET_GRID_USER_MANAGEMENT_ROLE";
export const GET_FILTER_USER_MANAGEMENT = "GET_FILTER_USER_MANAGEMENT";
export const GET_FILTER_USER_MANAGEMENT_ROLE =
  "GET_FILTER_USER_MANAGEMENT_ROLE";
export const CREATE_USER_MANAGEMENT = "CREATE_USER_MANAGEMENT";
export const EDIT_USER_MANAGEMENT = "EDIT_USER_MANAGEMENT";
export const DELETE_USER_MANAGEMENT = "DELETE_USER_MANAGEMENT";
export const RESTORE_USER_MANAGEMENT = "RESTORE_USER_MANAGEMENT";
export const GET_CREATE_USER_MANAGEMENT_ROLE =
  "GET_CREATE_USER_MANAGEMENT_ROLE";
export const GET_EDIT_USER_MANAGEMENT_ROLE = "GET_EDIT_USER_MANAGEMENT_ROLE";
export const DELETE_USER_MANAGEMENT_ROLE = "DELETE_USER_MANAGEMENT_ROLE";
export const RESTORE_USER_MANAGEMENT_ROLE = "RESTORE_USER_MANAGEMENT_ROLE";
export const ACTIVATION_USER_MANAGEMENT = "ACTIVATION_USER_MANAGEMENT";
export const CREATE_USER_MANAGEMENT_ROLE = "CREATE_USER_MANAGEMENT_ROLE";
export const ADD_NEW_USER_MANAGEMENT = "ADD_NEW_USER_MANAGEMENT";
