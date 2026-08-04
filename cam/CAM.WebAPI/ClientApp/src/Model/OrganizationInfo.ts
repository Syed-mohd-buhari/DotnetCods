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

/**
 *
 * @export
 * @interface OrganizationInfoQueryDto
 */
export interface OrganizationInfoQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  systemVerificationProblemId: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  contactId: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  opCoId: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  subPracticeContactId: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  organisationId: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  headOfOrganisationId: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  mainOrganisation: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  headOfOrganisation: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  subdomainResponsibleId: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  opCo: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  verticalResponsibleId: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  solutionDescription: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  contact: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  subdomainResponsible: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  subPractice: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  subPracticeContact: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  verticalResponsible: Array<string>;
}

/**
 *
 * @export
 * @interface OrganizationInfoDtoGrid
 */
export interface OrganizationInfoDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoGrid
   */
  systemVerificationProblemId: number;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoGrid
   */
  contactId: number;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoGrid
   */
  opCoId: number;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoGrid
   */
  subPracticeContactId: number;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoGrid
   */
  organisationId: number;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoGrid
   */
  practiceContactId: number;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoGrid
   */
  practiceId: number;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoGrid
   */
  mainOrganisationId: number;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoGrid
   */
  headOfOrganisationId: number;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  problemCategoryDescription: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  mainOrganisation: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  headOfOrganisation: string;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoGrid
   */
  subdomainResponsibleId: number;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  severityDescription: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  opCo: string;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoGrid
   */
  verticalResponsibleId: number;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  solutionDescription: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  contact: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  subdomainResponsible: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  subPractice: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  subPracticeContact: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  verticalResponsible: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  environment: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoGrid
   */
  opCoName: string;
  /**
   *
   * @type {boolean}
   * @memberof OrganizationInfoDtoGrid
   */
  isEduSpoc: boolean;
  /**
   *
   * @type {boolean}
   * @memberof OrganizationInfoDtoGrid
   */
  isSubDomainSpoc: boolean;
}

/**
 *
 * @export
 * @interface OrganizationInfoDtoCreate
 */
export interface OrganizationInfoDtoCreate extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoCreate
   */
  headOfOrganisationId: number;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof OrganizationInfoDtoCreate
   */
  severityTypes?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  contact?: string;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoCreate
   */
  contactId: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof OrganizationInfoDtoCreate
   */
  contacts?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof OrganizationInfoDtoCreate
   */
  practiceMethods?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  practice?: string;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoCreate
   */
  practiceId: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof OrganizationInfoDtoCreate
   */
  practices?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  practiceContact?: string;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoCreate
   */
  practiceContactId: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof OrganizationInfoDtoCreate
   */
  practiceContacts?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  subDomainResponsible?: string;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoCreate
   */
  subDomainResponsibleId: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof OrganizationInfoDtoCreate
   */
  subDomainResponsibles?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof OrganizationInfoDtoCreate
   */
  problemCategoryTypes?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof OrganizationInfoDtoCreate
   */
  opCos?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof OrganizationInfoDtoCreate
   */
  environmentTypes?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoCreate
   */
  subPracticeContactId: number;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoCreate
   */
  organisationId: number;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoCreate
   */
  subdomainResponsibleId?: number;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  problemCategoryDescription?: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  mainOrganisation?: string;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoCreate
   */
  mainOrganisationId: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof OrganizationInfoDtoCreate
   */
  mainOrganisations?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  headOfOrganisation?: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  severityDescription?: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  opCo?: string;
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoCreate
   */
  verticalResponsibleId?: number;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  solutionDescription?: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  subdomainResponsible?: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  subPractice?: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  subPracticeContact?: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  verticalResponsible?: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  opCoName?: string;
  /**
   *
   * @type {string}
   * @memberof OrganizationInfoDtoCreate
   */
  environment?: string;
  /**
   *
   * @type {Array<{key: number, value: string}>}
   * @memberof OrganizationInfoDtoCreate
   */
  systemTypes?: Array<{ key: number; value: string }>;
  /**
   *
   * @type {boolean}
   * @memberof OrganizationInfoDtoCreate
   */
  isEduSpoc: boolean;
  /**
   *
   * @type {boolean}
   * @memberof OrganizationInfoDtoCreate
   */
  isSubDomainSpoc: boolean;
}
/**
 *
 * @export
 * @interface OrganizationInfoDtoUpdate
 */
export interface OrganizationInfoDtoUpdate extends OrganizationInfoDtoCreate {
  /**
   *
   * @type {number}
   * @memberof OrganizationInfoDtoUpdate
   */
  systemVerificationProblemId?: number;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfOrganizationInfoDtoGrid
 */
export interface QueryResultDtoOfOrganizationInfoDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfOrganizationInfoDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<OrganizationInfoDtoGrid>}
   * @memberof QueryResultDtoOfOrganizationInfoDtoGrid
   */
  items?: Array<OrganizationInfoDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface OrganizationInfoQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  systemVerificationProblemId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  contactId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  opCoId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  subPracticeContactId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  organisationId?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  headOfOrganisationId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  mainOrganisation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  headOfOrganisation?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  subdomainResponsibleId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  opCo?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof OrganizationInfoQueryDto
   */
  verticalResponsibleId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  solutionDescription?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  contact?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  subdomainResponsible?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  subPractice?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  subPracticeContact?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof OrganizationInfoQueryDto
   */
  verticalResponsible?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof SystemTypeQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  lastModifiedBy?: Array<string>;
}

export interface OrganizationInfoEdit {
  OrganizationInfoDtoEdit: OrganizationInfoDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface OrganizationInfoCreate {
  OrganizationInfoDtoCreate: OrganizationInfoDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface OrganizationInfoGrid {
  OrganizationInfoGridResult: QueryResultDtoOfOrganizationInfoDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_ORGANIZATION_INFO = "GET_CREATE_ORGANIZATION_INFO";
export const GET_EDIT_ORGANIZATION_INFO = "GET_EDIT_ORGANIZATION_INFO";
export const GET_GRID_ORGANIZATION_INFO = "GET_GRID_ORGANIZATION_INFO";
export const GET_FILTER_ORGANIZATION_INFO = "GET_FILTER_ORGANIZATION_INFO";
export const CREATE_ORGANIZATION_INFO = "CREATE_ORGANIZATION_INFO";
export const EDIT_ORGANIZATION_INFO = "EDIT_ORGANIZATION_INFO";
export const DELETE_ORGANIZATION_INFO = "DELETE_ORGANIZATION_INFO";
export const RESTORE_ORGANIZATION_INFO = "RESTORE_ORGANIZATION_INFO";
