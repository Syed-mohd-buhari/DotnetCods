import { VolteKPIType } from "../VolteKpi/VolteKPI";

/**
 *
 * @export
 * @interface VolteKPIReportColumn
 */
export interface VolteKPIReportColumn {
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIReportColumn
	 */
	quarter?: string;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIReportColumn
	 */
	month?: number;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIReportColumn
	 */
	monthYearLabel?: string;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIReportColumn
	 */
	value?: string;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIReportColumn
	 */
	backgroundColor?: string;
}
/**
 *
 * @export
 * @interface VolteKPIReportDto
 */
export interface VolteKPIReportDto {
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIReportDto
	 */
	year?: number;
	/**
	 *
	 * @type {Array<VolteKPIReportRow>}
	 * @memberof VolteKPIReportDto
	 */
	provisioned?: Array<VolteKPIReportRow>;
	/**
	 *
	 * @type {Array<VolteKPIReportRow>}
	 * @memberof VolteKPIReportDto
	 */
	registered?: Array<VolteKPIReportRow>;
	/**
	 *
	 * @type {{ [key: string]: string; }}
	 * @memberof VolteKPIReportDto
	 */
	opCoResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface VolteKPIReportRow
 */
export interface VolteKPIReportRow {
	/**
	 *
	 * @type {VolteKPIType}
	 * @memberof VolteKPIReportRow
	 */
	type?: VolteKPIType;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIReportRow
	 */
	opCo?: string;
	/**
	 *
	 * @type {Array<VolteKPIReportColumn>}
	 * @memberof VolteKPIReportRow
	 */
	monthValues?: Array<VolteKPIReportColumn>;
}

export interface ReportVolteKPIGrid {
	ReportVolteKPIGridResult: VolteKPIReportDto | null;
	filter: number | null;
}

export const GET_GRID_REPORT_VOLTE_KPI = "GET_GRID_REPORT_VOLTE_KPI";
export const DOWNLOAD_VOLTE_KPI_REPORT = "DOWNLOAD_VOLTE_KPI_REPORT";
