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
 * @interface TestInfoQueryDto
 */
export interface TestInfoQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  systemVerificationProblemId: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  problemId: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  opCoId: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  systemTypeId: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  environmentId: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof TestInfoQueryDto
   */
  dateFound: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  problemCategoryId: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  problemDescription: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  maintenanceReference: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  severity: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  statusUrl: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  testReport: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  standardNir: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  ericssonSecReport: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  swAndStEntries: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  penTestingReport: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  mitigation: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  solutionDescription: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  patchReference: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  productUpgradeReference: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  suppleMental: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  vendorCsr: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  subNetwork: Array<string>;
}

/**
 *
 * @export
 * @interface TestInfoDtoGrid
 */
export interface TestInfoDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoGrid
   */
  systemVerificationProblemId: number;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  problemId: string;
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoGrid
   */
  opCoId: number;
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoGrid
   */
  systemTypeId: number;
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoGrid
   */
  environmentId: number;
  /**
   *
   * @type {Date}
   * @memberof TestInfoDtoGrid
   */
  dateFound: Date;
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoGrid
   */
  problemCategoryId: number;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  problemCategoryDescription: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  problemDescription: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  maintenanceReference: string;
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoGrid
   */
  severity: number;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  severityDescription: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  statusUrl: string;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  testReport: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  standardNir: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  ericssonSecReport: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  swAndStEntries: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  penTestingReport: Array<string>;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  mitigation: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  solutionDescription: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  patchReference: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  productUpgradeReference: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  suppleMental: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  vendorCsr: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  subNetwork: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  environment: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoGrid
   */
  opCoName: string;
}

/**
 *
 * @export
 * @interface TestInfoDtoCreate
 */
export interface TestInfoDtoCreate extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoCreate
   */
  problemCategoryId: number;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  dateFound: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof TestInfoDtoCreate
   */
  severityTypes?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  problemId: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof TestInfoDtoCreate
   */
  problemCategoryTypes?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof TestInfoDtoCreate
   */
  opCos?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof TestInfoDtoCreate
   */
  environmentTypes?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoCreate
   */
  systemTypeId: number;
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoCreate
   */
  environmentId: number;
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoCreate
   */
  severity?: number;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  problemCategoryDescription?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  problemDescription?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  maintenanceReference?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  severityDescription?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  statusUrl?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  testReport?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  standardNir?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  ericssonSecReport?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  swAndStEntries?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  penTestingReport?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  mitigation?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  solutionDescription?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  patchReference?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  productUpgradeReference?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  suppleMental?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  vendorCsr?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  subNetwork?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  opCoName?: string;
  /**
   *
   * @type {string}
   * @memberof TestInfoDtoCreate
   */
  environment?: string;
  /**
   *
   * @type {Array<{key: number, value: string}>}
   * @memberof TestInfoDtoCreate
   */
  systemTypes?: Array<{ key: number; value: string }>;
}
/**
 *
 * @export
 * @interface TestInfoDtoUpdate
 */
export interface TestInfoDtoUpdate extends TestInfoDtoCreate {
  /**
   *
   * @type {number}
   * @memberof TestInfoDtoUpdate
   */
  systemVerificationProblemId?: number;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfTestInfoDtoGrid
 */
export interface QueryResultDtoOfTestInfoDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfTestInfoDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<TestInfoDtoGrid>}
   * @memberof QueryResultDtoOfTestInfoDtoGrid
   */
  items?: Array<TestInfoDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface TestInfoQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  systemVerificationProblemId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  problemId?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  opCoId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  systemTypeId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  environmentId?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof TestInfoQueryDto
   */
  dateFound?: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  problemCategoryId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  problemDescription?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  maintenanceReference?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof TestInfoQueryDto
   */
  severity?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  statusUrl?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  testReport?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  standardNir?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  ericssonSecReport?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  swAndStEntries?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  penTestingReport?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  mitigation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  solutionDescription?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  patchReference?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  productUpgradeReference?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  suppleMental?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  vendorCsr?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof TestInfoQueryDto
   */
  subNetwork?: Array<string>;
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

export interface TestInfoEdit {
  TestInfoDtoEdit: TestInfoDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface TestInfoCreate {
  TestInfoDtoCreate: TestInfoDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface TestInfoGrid {
  TestInfoGridResult: QueryResultDtoOfTestInfoDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_TEST_INFO = "GET_CREATE_TEST_INFO";
export const GET_EDIT_TEST_INFO = "GET_EDIT_TEST_INFO";
export const GET_GRID_TEST_INFO = "GET_GRID_TEST_INFO";
export const GET_FILTER_TEST_INFO = "GET_FILTER_TEST_INFO";
export const CREATE_TEST_INFO = "CREATE_TEST_INFO";
export const EDIT_TEST_INFO = "EDIT_TEST_INFO";
export const DELETE_TEST_INFO = "DELETE_TEST_INFO";
export const RESTORE_TEST_INFO = "RESTORE_TEST_INFO";
