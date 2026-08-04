import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { VolteKPIType } from "../VolteKpi/VolteKPI";
import { ReportQueryDto } from "./Export";

/**
 *
 * @export
 * @interface PATReportColumn
 */
export interface PATReportColumn {
  /**
   *
   * @type {string}
   * @memberof PATReportColumn
   */
  quarter?: string;
  /**
   *
   * @type {number}
   * @memberof PATReportColumn
   */
  month?: number;
  /**
   *
   * @type {string}
   * @memberof PATReportColumn
   */
  monthYearLabel?: string;
  /**
   *
   * @type {string}
   * @memberof PATReportColumn
   */
  value?: string;
  /**
   *
   * @type {string}
   * @memberof PATReportColumn
   */
  backgroundColor?: string;
}
/**
 *
 * @export
 * @interface VolteKPIReportDto
 */
export interface PATReportDto {
  gridRender?: any;
  items?: Array<{
    opcoName: string;
    opcoId: number;
    dcfId: number;
    dcfName: string;
    verticalNameId: number;
    verticalName: string;
    months: Array<string>;
    releases: Array<string>;
  }>;
  totalItems?: number;
}
/**
 *
 * @export
 * @interface PATReportRow
 */
export interface PATReportRow {
  /**
   *
   * @type {VolteKPIType}
   * @memberof PATReportRow
   */
  type?: VolteKPIType;
  /**
   *
   * @type {string}
   * @memberof PATReportRow
   */
  dcfName?: string;
  /**
   *
   * @type {string}
   * @memberof PATReportRow
   */
  verticalName?: string;
  /**
   *
   * @type {string}
   * @memberof PATReportRow
   */
  opcoName?: string;
  /**
   *
   * @type {Array<PATReportColumn>}
   * @memberof PATReportRow
   */
  monthValues?: Array<PATReportColumn>;
}

export interface ReportPATQueryObjectGrid extends ReportQueryDto {
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareQueryDto
   */
  dcfId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareQueryDto
   */
  opCoId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ReportHardwareQueryDto
   */
  verticalNameId?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ReportHardwareQueryDto
   */
  vendorIds?: Array<number>;
  /**
   *
   * @type {number}
   * @memberof ReportHardwareQueryDto
   */
  buildConstruction?: number | undefined;
  /**
   *
   * @type {boolean}
   * @memberof ReportHardwareQueryDto
   */
  isEosDateEnable?: boolean;
}
export interface ReportPATGrid {
  ReportPATGridResult: PATReportDto | null;
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

export const GET_GRID_REPORT_PAT = "GET_GRID_REPORT_PAT";
export const DOWNLOAD_PAT_REPORT = "DOWNLOAD_PAT_REPORT";
export const GET_FILTER_REPORT_PAT = "GET_FILTER_REPORT_PAT";
