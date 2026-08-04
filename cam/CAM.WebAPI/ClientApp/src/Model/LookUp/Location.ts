import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface LocationDto
 */
export interface LocationDto {
  /**
   *
   * @type {number}
   * @memberof LocationDto
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof LocationDto
   */
  description?: string;
  /**
   *
   * @type {number}
   * @memberof LocationDto
   */
  opcoId?: number;
  /**
   *
   * @type {boolean}
   * @memberof LocationDto
   */
  defaultValue?: boolean;
  /**
   *
   * @type {number}
   * @memberof LocationDto
   */
  locationTypeId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof LocationDto
   */
  LocationTypeIdsList?: Array<number>;
  /**
   *
   * @type {Date}
   * @memberof LocationDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof LocationDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LocationDto
   */
  opcoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LocationDto
   */
  locationTypeResource?: { [key: string]: string };
/**
   *
   * @type {Array<number>}
   * @memberof LocationDto
   */
 locationTypeIdsList?: Array<number>;

}
/**
 *
 * @export
 * @interface LocationDtoGrid
 */
export interface LocationDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof LocationDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof LocationDtoGrid
   */
  description?: string;
  /**
   *
   * @type {string}
   * @memberof LocationDtoGrid
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof LocationDtoGrid
   */
  locationType?: string;
  /**
   *
   * @type {number}
   * @memberof LocationDtoGrid
   */
  opcoId?: number;
  /**
   *
   * @type {number}
   * @memberof LocationDtoGrid
   */
  locationTypeId?: number;
  /**
   *
   * @type {boolean}
   * @memberof LocationDtoGrid
   */
  defaultValue?: boolean;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfLocationDtoGrid
 */
export interface CustomGridRenderOfLocationDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfLocationDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfLocationDtoGrid
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfLocationDtoGrid
 */
export interface QueryResultDtoOfLocationDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfLocationDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<LocationDtoGrid>}
   * @memberof QueryResultDtoOfLocationDtoGrid
   */
  items?: Array<LocationDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfLocationDtoGrid}
   * @memberof QueryResultDtoOfLocationDtoGrid
   */
  gridRender?: CustomGridRenderOfLocationDtoGrid;
}

// ------------Not AutoGen---------

export interface LocationQueryObjectGrid extends QueryObject {
  id?: number[];
  description?: string[];
  opco?: number[];
  locationType?: number[];
  defaultValue?: boolean[];
}

export interface LocationEdit {
  LookUpDtoEdit: LocationDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface LocationCreate {
  LookUpDtoCreate: LocationDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface LocationGrid {
  LookUpGridResult: QueryResultDtoOfLocationDtoGrid | null;
  LookUpGridResultAll: QueryResultDtoOfLocationDtoGrid | null;
  filter: FilterValueDto[] | null;
}
