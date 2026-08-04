import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";
import { ResultDto } from "../CommonModels";
/**
 *
 * @export
 * @interface ClassDto
 */
export interface ClassDto {
  /**
   *
   * @type {number}
   * @memberof ClassDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof ClassDto
   */
  description?: string;
  /**
   *
   * @type {number}
   * @memberof ClassDto
   */
  categoryId?: number;
  /**
   *
   * @type {string}
   * @memberof ClassDto
   */
  categoryDescription?: string;
  /**
   *
   * @type {Date}
   * @memberof ClassDto
   */
  lastModified?: Date;
  categoryResource?: { [key: string]: string };

  /**
   *
   * @type {string}
   * @memberof ClassDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface ClassDtoGrid
 */
export interface ClassDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof ClassDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof ClassDtoGrid
   */
  description?: string;

  /**
   *
   * @type {number}
   * @memberof ClassDtoGrid
   */
  categoryId?: number;

  /**
   *
   * @type {string}
   * @memberof ClassDto
   */
  categoryDescription?: string;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfClassDtoGrid
 */
export interface QueryResultDtoOfClassDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfClassDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<ClassDtoGrid>}
   * @memberof QueryResultDtoOfClassDtoGrid
   */
  items?: Array<ClassDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfClassDtoGrid}
   * @memberof QueryResultDtoOfClassDtoGrid
   */
  gridRender?: CustomGridRender;
}

export interface ClassQueryObjectGrid extends QueryObjectGrid {
  id?: Array<number>;
  description?: Array<string>;
  categoryId?: Array<number>;
  // categoryDescription?: Array<string>;
  sortBy?: string;
  isSortAscending?: boolean;
  page?: number;
  pageSize?: number;
  lastModifiedStartDate?: Date;
  lastModifiedEndDate?: Date;
  categoryDescription?:  Array<string>;
}

export interface ClassGrid {
  LookUpGridResult: QueryResultDtoOfClassDtoGrid | null;
  LookUpGridResultAll: QueryResultDtoOfClassDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface ClassEdit {
  LookUpDtoEdit: ClassDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface ClassCreate {
  LookUpDtoCreate: ClassDto | null;
  ResultDtoCreate: ResultDto | null;
}
