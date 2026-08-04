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

/**
 *
 * @export
 * @interface QueryDtoforTeam
 */
export interface QueryDtoforTeam extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof QueryDtoforTeam
   */
  teamId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof QueryDtoforTeam
   */
  teamName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof QueryDtoforTeam
   */
  teamDescription?: Array<string>;
  /**
   *
   * @type {Array<DateFilter>}
   * @memberof QueryDtoforTeam
   */
  lastModified?: Array<DateFilter>;
  /**
   *
   * @type {Array<string>}
   * @memberof QueryDtoforTeam
   */
  lastModifiedBy?: Array<string>;
}

/**
 *
 * @export
 * @interface QueryDtoforTeamMember
 */
export interface QueryDtoforTeamMember extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof QueryDtoforTeamMember
   */
  teamMemberId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof QueryDtoforTeamMember
   */
  user?: Array<number>;
  /**
   *
   * @type {Array<DateFilter>}
   * @memberof QueryDtoforTeamMember
   */
  lastModified?: Array<DateFilter>;
  /**
   *
   * @type {Array<string>}
   * @memberof QueryDtoforTeamMember
   */
  lastModifiedBy?: Array<string>;
}

/**
 *
 * @export
 * @interface TeamsGridDto
 */
export interface TeamsGridDto {
  /**
   *
   * @type {number}
   * @memberof TeamsGridDto
   */
  teamId?: number;
  /**
   *
   * @type {string}
   * @memberof TeamsGridDto
   */
  teamName?: string;
  /**
   *
   * @type {string}
   * @memberof TeamsGridDto
   */
  teamDescription?: string;
}

/**
 *
 * @export
 * @interface TeamMemberGridDto
 */
export interface TeamMemberGridDto {
  /**
   *
   * @type {number}
   * @memberof TeamMemberGridDto
   */
  teamMemberId?: number;
  /**
   *
   * @type {string}
   * @memberof TeamMemberGridDto
   */
  user?: string;
}

/**
 *
 * @export
 * @interface TeamsDtoCreate
 */
export interface TeamsDtoCreate {
  /**
   *
   * @type {number}
   * @memberof TeamsDtoCreate
   */
  teamId?: number;
  /**
   *
   * @type {string}
   * @memberof TeamsDtoCreate
   */
  teamName?: string;
  /**
   *
   * @type {string}
   * @memberof TeamsDtoCreate
   */
  teamDescription?: string;
  /**
   *
   * @type {Array<number>}
   * @memberof TeamsDtoCreate
   */
  userIds?: Array<number>;
  /**
   *
   * @type {string}
   * @memberof TeamsDtoCreate
   */
  userName?: string;
  /**
   *
   * @type {{ [key: string]: string }}
   * @memberof TeamsDtoCreate
   */
  userResources?: { [key: string]: string };
}

/**
 *
 * @export
 * @interface TeamsDtoEdit
 */
export interface TeamsDtoEdit extends TeamsDtoCreate {}

/**
 *
 * @export
 * @interface TeamMemberDtoCreate
 */
export interface TeamMemberDtoCreate {
  /**
   *
   * @type {number}
   * @memberof TeamMemberDtoCreate
   */
  teamMemberId?: number;
  /**
   *
   * @type {number}
   * @memberof TeamMemberDtoCreate
   */
  user?: number;
}

/**
 *
 * @export
 * @interface TeamMemberDtoEdit
 */
export interface TeamMemberDtoEdit extends TeamMemberDtoCreate {}

/**
 *
 * @export
 * @interface QueryResultDtoOfTeamsGridDto
 */
export interface QueryResultDtoOfTeamsGridDto {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfTeamsGridDto
   */
  totalItems?: number;
  /**
   *
   * @type {Array<TeamsGridDto>}
   * @memberof QueryResultDtoOfTeamsGridDto
   */
  items?: Array<TeamsGridDto>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof QueryResultDtoOfTeamsGridDto
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfTeamMemberGridDto
 */
export interface QueryResultDtoOfTeamMemberGridDto {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfTeamMemberGridDto
   */
  totalItems?: number;
  /**
   *
   * @type {Array<TeamMemberGridDto>}
   * @memberof QueryResultDtoOfTeamMemberGridDto
   */
  items?: Array<TeamMemberGridDto>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof QueryResultDtoOfTeamMemberGridDto
   */
  gridRender?: CustomGridRender;
}

export interface TeamsEdit {
  TeamsDtoEdit: TeamsDtoEdit | null;
  ResultDtoEdit: ResultDto | null;
}

export interface TeamsCreate {
  TeamsDtoCreate: TeamsDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface TeamMemberEdit {
  TeamMemberDtoEdit: TeamMemberDtoEdit | null;
  ResultDtoEdit: ResultDto | null;
}

export interface TeamMemberCreate {
  TeamMemberDtoCreate: TeamMemberDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface TeamsGrid {
  TeamsGridResult: QueryResultDtoOfTeamsGridDto | null;
  filter: FilterValueDto[] | null;
}

export interface TeamMemberGrid {
  TeamMemberGridResult: QueryResultDtoOfTeamMemberGridDto | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_TEAMS = "GET_CREATE_TEAMS";
export const GET_EDIT_TEAMS = "GET_EDIT_TEAMS";
export const GET_GRID_TEAMS = "GET_GRID_TEAMS";
export const GET_FILTER_TEAMS = "GET_FILTER_TEAMS";
export const CREATE_TEAMS = "CREATE_TEAMS";
export const EDIT_TEAMS = "EDIT_TEAMS";
