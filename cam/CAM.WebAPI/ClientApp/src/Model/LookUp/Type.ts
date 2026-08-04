import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";
import { ResultDto } from "../CommonModels";
/**
 *
 * @export
 * @interface TypeDto
 */
export interface TypeDto {
  /**
   *
   * @type {number}
   * @memberof TypeDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof TypeDtoGrid
   */
  description?: string;

  /**
   *
   * @type {number}
   * @memberof TypeDtoGrid
   */
  classId?: number;
  categoryId?:number;

  /**
   *
   * @type {string}
   * @memberof TypeDto
   */
  classDescription?: string;
  categoryResource?: { [key: string]: string };
  classResource?: { [key: string]: string };

  categoryDescription?: string;
  /**
   *
   * @type {Date}
   * @memberof TypeDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof TypeDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface TypeDtoGrid
 */
export interface TypeDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof TypeDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof TypeDtoGrid
   */
  description?: string;

  /**
   *
   * @type {number}
   * @memberof TypeDtoGrid
   */
  classId?: number;

  /**
   *
   * @type {string}
   * @memberof TypeDto
   */
  classDescription?: string;

  categoryDescription?: string;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfTypeDtoGrid
 */
export interface QueryResultDtoOfTypeDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfTypeDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<TypeDtoGrid>}
   * @memberof QueryResultDtoOfTypeDtoGrid
   */
  items?: Array<TypeDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfTypeDtoGrid}
   * @memberof QueryResultDtoOfTypeDtoGrid
   */
  gridRender?: CustomGridRender;
}

export interface TypeQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {number}
   * @memberof TypeDtoGrid
   */
  id?: Array<number>;
  /**
   *
   * @type {string}
   * @memberof TypeDtoGrid
   */
  description?: Array<string>;

  /**
   *
   * @type {number}
   * @memberof TypeDtoGrid
   */
  classId?: Array<number>;

  /**
   *
   * @type {string}
   * @memberof TypeDto
   */
  classDescription?:  Array<string>;

  sortBy?: string;
  isSortAscending?: boolean;
  page?: number;
  pageSize?: number;
  lastModifiedStartDate?: Date;
  lastModifiedEndDate?: Date;
  categoryDescription?: Array<string>;
}

export interface TypeGrid {
  LookUpGridResult: QueryResultDtoOfTypeDtoGrid | null;
  LookUpGridResultAll: QueryResultDtoOfTypeDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface TypeEdit {
  LookUpDtoEdit: TypeDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface TypeCreate {
  LookUpDtoCreate: TypeDto | null;
  ResultDtoCreate: ResultDto | null;
}
