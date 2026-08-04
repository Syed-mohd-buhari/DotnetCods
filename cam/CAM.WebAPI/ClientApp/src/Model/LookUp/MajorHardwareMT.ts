import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface MajorHardwareMTDto
 * This is used for CREATE/EDIT operations (single record)
 */
export interface MajorHardwareMTDto {
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDto
   */
  orgEqpManuFacturerDesc?: string;
  /**
   *
   * @type {number}
   * @memberof MajorHardwareMTDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDto
   */
  intraDescription?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDto
   */
  buildConstructionDesc?: string;
  /**
   *
   * @type {number}
   * @memberof MajorHardwareMTDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof MajorHardwareMTDto
   */
  defaultValue?: boolean;

  /**
   *
   * @type {number}
   * @memberof MajorHardwareMTDto
   */
  majorHardwareBuildAsisId?: number;
  /**
   *
   * @type {number}
   * @memberof MajorHardwareMTDto
   */
  orgEqpManuFacturerId?: number;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDto
   */
  hardwareSolution?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDto
   */
  hardwareSolutionReourceId?: string;
  /**
   *
   * @type {number}
   * @memberof MajorHardwareMTDto
   */
  platformId?: number;
  /**
   *
   * @type {number}
   * @memberof MajorHardwareMTDto
   */
  buildConstructionId?: number;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDto
   */
  hardwareType?: string;

  /**
   *
   * @type {Date}
   * @memberof MajorHardwareMTDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorHardwareMTDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorHardwareMTDto
   */
  majorHardwareMTResource?: { [key: string]: string };
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof MajorHardwareMTDto
   */
  buildConstructionResources?: Array<{ key: number; text: string }>;
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof MajorHardwareMTDto
   */
  orgEqpmanuFacturerResources?: Array<{ key: number; text: string }>;
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof MajorHardwareMTDto
   */
  platformResources?: Array<{ key: number; text: string }>;
}
/**
 *
 * @export
 * @interface MajorHardwareMTDtoGrid
 */
export interface MajorHardwareMTDtoGrid extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDtoGrid
   */
  orgEqpManuFacturerDesc?: string;
  /**
   *
   * @type {number}
   * @memberof MajorHardwareMTDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDtoGrid
   */
  intraDescription?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {number}
   * @memberof MajorHardwareMTDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof MajorHardwareMTDtoGrid
   */
  majorHardwareBuildAsisId?: number;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDtoGrid
   */
  orgEqpManuFacturerId?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDtoGrid
   */
  hardwareSolution?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDtoGrid
   */
  hardwareSolutionReourceId?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDtoGrid
   */
  platformId?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDtoGrid
   */
  buildConstructionId?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareMTDtoGrid
   */
  hardwareType?: string;
  /**
   *
   * @type {boolean}
   * @memberof MajorHardwareMTDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfMajorHardwareMTDtoGrid
 */
export interface CustomGridRenderOfMajorHardwareMTDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfMajorHardwareMTDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfMajorHardwareMTDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfMajorHardwareMTDtoGrid
 */
export interface QueryResultDtoOfMajorHardwareMTDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfMajorHardwareMTDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<MajorHardwareMTDtoGrid>}
   * @memberof QueryResultDtoOfMajorHardwareMTDtoGrid
   */
  items?: Array<MajorHardwareMTDtoGrid>;
  /**
   *
   * @type {Array<MajorHardwareMTDtoGrid>}
   * @memberof QueryResultDtoOfMajorHardwareMTDtoGrid
   */
  allItems?: Array<MajorHardwareMTDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfMajorHardwareMTDtoGrid}
   * @memberof QueryResultDtoOfMajorHardwareMTDtoGrid
   */
  gridRender?: CustomGridRenderOfMajorHardwareMTDtoGrid;
}
/**
 *
 * @export
 * @interface ResultDtoOfObject
 */
export interface ResultDtoOfObject {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfObject
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfObject
   */
  info?: string;
  /**
   *
   * @type {QueryResultDtoOfMajorHardwareMTDtoGrid}
   * @memberof ResultDtoOfObject
   */
  data?: QueryResultDtoOfMajorHardwareMTDtoGrid;
}

// ------------Not AutoGen---------

/**
 * This interface is used for QUERY/FILTER operations (can filter by multiple values)
 */
export interface MajorHardwareMTQueryObjectGrid extends QueryObject {
  majorHardwareBuildAsisId?: number[];
  orgEqpManuFacturerId?: number[];
  hardwareSolutionReourceId?: number[];
  platformId?: number[];
  buildConstructionId?: number[];

  orgEqpManuFacturerDesc?: string[];
  hardwareSolution?: string[];
  platformDesc?: string[];
  buildConstructionDesc?: string[];
  hardwareType?: string[];
}

export interface MajorHardwareMTEdit {
  LookUpDtoEdit: MajorHardwareMTDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface MajorHardwareMTCreate {
  LookUpDtoCreate: MajorHardwareMTDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface MajorHardwareMTGrid {
  LookUpGridResult: ResultDtoOfObject | null;
  LookUpGridResultAll: ResultDtoOfObject | null;
  filter: FilterValueDto[] | null;
}
