import { RenderDetail } from "./Common";

/**
 *
 * @export
 * @interface ResultDto
 */
export interface ResultDto {
  /**
   *
   * @type {boolean}
   * @memberof ResultDto
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDto
   */
  info?: string;
  /**
   *
   * @type {ModelObject}
   * @memberof ResultDto
   */
  data?: any;
}
/**
 *
 * @export
 * @interface RelatedResource
 */
export interface RelatedResource {
  /**
   *
   * @type {string}
   * @memberof RelatedResource
   */
  value?: string;
  /**
   *
   * @type {string}
   * @memberof RelatedResource
   */
  id?: string;
}

/**
 *
 * @export
 * @interface DataRemediationSystemTypeDto
 */
export interface DataRemediationSystemTypeDto extends DataRemediationDto {
  /**
   *
   * @type {number}
   * @memberof DataRemediationSystemTypeDto
   */
  keyGrouped?: string;
}
/**
 *
 * @export
 * @interface DataRemediationDto
 */
export interface DataRemediationDto {
  /**
   *
   * @type {number}
   * @memberof DataRemediationDto
   */
  correctId?: number;
  /**
   *
   * @type {Array<number>}
   * @memberof DataRemediationDto
   */
  duplicatesId?: Array<number>;
}

/**
 *
 * @export
 * @interface ResultDtoOfListOfShort
 */
export interface ResultDtoOfListOfShort {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfListOfShort
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfListOfShort
   */
  info?: string;
  /**
   *
   * @type {Array<number>}
   * @memberof ResultDtoOfListOfShort
   */
  data?: Array<number>;
}

export interface ResultDtoOfResultDataRemediationDto {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfResultDataRemediationDto
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfResultDataRemediationDto
   */
  info?: string;
  /**
   *
   * @type {ResultDataRemediationDto}
   * @memberof ResultDtoOfResultDataRemediationDto
   */
  data?: ResultDataRemediationDto;
}

export interface ResultDataRemediationDto {
  /**
   *
   * @type {Array<number>}
   * @memberof ResultDataRemediationDto
   */
  id?: Array<number>;
}

/**
 *
 * @export
 * @interface ChangeGridOrderDto
 */
export interface ChangeGridOrderDto {
  /**
   *
   * @type {number}
   * @memberof ChangeGridOrderDto
   */
  id?: number;
  /**
   *
   * @type {number}
   * @memberof ChangeGridOrderDto
   */
  order?: number;
}

/**
 *
 * @export
 * @interface SaveGrid
 */
export interface SaveGrid {
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof SaveGrid
   */
  render?: Array<RenderDetail>;
  /**
   *
   * @type {string}
   * @memberof SaveGrid
   */
  className?: string;
}
/**
 *
 * @export
 * @interface NetworkElementGraph
 */
export interface NetworkElementGraph {
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof NetworkElementGraph
   */
  render?: Array<RenderDetail>;
  /**
   *
   * @type {string}
   * @memberof NetworkElementGraph
   */
  className?: string;
}
export interface NetworkVisualizerGraph {
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof NetworkElementGraph
   */
  render?: Array<RenderDetail>;
  /**
   *
   * @type {string}
   * @memberof NetworkElementGraph
   */
  className?: string;
}

export interface LCMAtGlanceGraph {
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof LCMAtGlanceGraph
   */
  render?: Array<RenderDetail>;
  /**
   *
   * @type {string}
   * @memberof LCMAtGlanceGraph
   */
  className?: string;
}

export interface ExodusAtGlanceGraph {
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof ExodusAtGlanceGraph
   */
  render?: Array<RenderDetail>;
  /**
   *
   * @type {string}
   * @memberof ExodusAtGlanceGraph
   */
  className?: string;
}

/**
 *
 * @export
 * @interface RelatedResourceListValue
 */
export interface RelatedResourceListValue {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof RelatedResourceListValue
   */
  value?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof RelatedResourceListValue
   */
  id?: string;
}

export interface GetRelatedRecordsResult {
  entityName: string;
  recordName: string;
  dataRelatedList: {
    title: string;
    values: string[];
  }[];
}

export interface RelatedRecordsResultDto {
  entityName: string;
  recordName: string;
  dataRelatedList: Array<ResultMessageDto>;
}

export interface ResultMessageDto {
  table: string;
  values: string[];
}
