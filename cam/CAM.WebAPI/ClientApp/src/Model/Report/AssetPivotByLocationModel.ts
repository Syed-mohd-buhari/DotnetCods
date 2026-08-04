import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { VolteKPIType } from "../VolteKpi/VolteKPI";
import { ReportQueryDto } from "./Export";

/**
 *
 * @export
 * @interface AssetPivotByLocationColumn
 */
export interface AssetPivotByLocationColumn {
  /**
   *
   * @type {string}
   * @memberof AssetPivotByLocationColumn
   */
  quarter?: string;
  /**
   *
   * @type {number}
   * @memberof AssetPivotByLocationColumn
   */
  month?: number;
  /**
   *
   * @type {string}
   * @memberof AssetPivotByLocationColumn
   */
  monthYearLabel?: string;
  /**
   *
   * @type {string}
   * @memberof AssetPivotByLocationColumn
   */
  value?: string;
  /**
   *
   * @type {string}
   * @memberof AssetPivotByLocationColumn
   */
  backgroundColor?: string;
}
/**
 *
 * @export
 * @interface VolteKPIReportDto
 */
export interface AssetPivotByLocationDto {
  gridRender?: any;
  items?: Array<{
    paImplementaionYear: string;
    opcoName: string;
    opCo: number;
    hardwareType: number;
    dcId: number;
    dcfName: string;
    designComponent: string;
    verticalNameId: number;
    deploymentStatus: string;
    total: string;
    months: Array<string>;
    locations: Array<{ [key: string]: string }>;
    releases: Array<string>;
    verticalName?: string;
  }>;
  totalItems?: number;
}
/**
 *
 * @export
 * @interface AssetPivotByLocationRow
 */
export interface AssetPivotByLocationRow {
  /**
   *
   * @type {VolteKPIType}
   * @memberof AssetPivotByLocationRow
   */
  type?: VolteKPIType;
  /**
   *
   * @type {string}
   * @memberof AssetPivotByLocationRow
   */
  dcfName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetPivotByLocationRow
   */
  designComponent?: string;
  /**
   *
   * @type {string}
   * @memberof AssetPivotByLocationRow
   */
  verticalName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetPivotByLocationRow
   */
  opcoName?: string;
  /**
   *
   * @type {string}
   * @memberof AssetPivotByLocationRow
   */
  paImplementaionYear?: string;
  /**
   *
   * @type {Array<AssetPivotByLocationColumn>}
   * @memberof AssetPivotByLocationRow
   */
  monthValues?: Array<AssetPivotByLocationColumn>;
}

export interface AssetPivotByLocationQueryObjectGrid extends ReportQueryDto {
  /**
   *
   * @type {Array<number>}
   * @memberof AssetPivotByLocationQueryObjectGrid
   */
  dcId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetPivotByLocationQueryObjectGrid
   */
  opCo?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetPivotByLocationQueryObjectGrid
   */
  hardwareType?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof AssetPivotByLocationQueryObjectGrid
   */
  deploymentStatus?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetPivotByLocationQueryObjectGrid
   */
  vendorIds?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof AssetPivotByLocationQueryObjectGrid
   */
  paImplementaionYear?: Array<number>;
  /**
   *
   * @type {boolean}
   * @memberof AssetPivotByLocationQueryObjectGrid
   */
  isEosDateEnable?: boolean;
}
export interface AssetPivotByLocationGrid {
  AssetPivotByLocationGridResult: AssetPivotByLocationDto | null;
  filter: FilterValueDto[] | null;
}

export interface SWOEM_MODEL {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SWOEM_MODEL
   */
  SWOemResources?: { [key: string]: string };
}

export const GET_ASSET_BY_LOCATION = "GET_ASSET_BY_LOCATION";
export const DOWNLOAD_PAT_REPORT = "DOWNLOAD_PAT_REPORT";
export const GET_FILTER_ASSET_BY_LOCATION = "GET_FILTER_ASSET_BY_LOCATION";
