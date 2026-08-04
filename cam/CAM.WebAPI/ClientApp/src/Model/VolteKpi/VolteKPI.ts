import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObjectGrid, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 *
 * @export
 * @interface CustomGridRenderOfVolteKPIDtoGrid
 */
export interface CustomGridRenderOfVolteKPIDtoGrid {
	/**
	 *
	 * @type {string}
	 * @memberof CustomGridRenderOfVolteKPIDtoGrid
	 */
	className?: string;
	/**
	 *
	 * @type {Array<RenderDetail>}
	 * @memberof CustomGridRenderOfVolteKPIDtoGrid
	 */
	render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfVolteKPIDtoGrid
 */
export interface QueryResultDtoOfVolteKPIDtoGrid {
	/**
	 *
	 * @type {number}
	 * @memberof QueryResultDtoOfVolteKPIDtoGrid
	 */
	totalItems?: number;
	/**
	 *
	 * @type {Array<VolteKPIDtoGrid>}
	 * @memberof QueryResultDtoOfVolteKPIDtoGrid
	 */
	items?: Array<VolteKPIDtoGrid>;
	/**
	 *
	 * @type {CustomGridRenderOfVolteKPIDtoGrid}
	 * @memberof QueryResultDtoOfVolteKPIDtoGrid
	 */
	gridRender?: CustomGridRenderOfVolteKPIDtoGrid;
}

/**
 *
 * @export
 * @interface VolteKPIColumn
 */
export interface VolteKPIColumn {
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIColumn
	 */
	value?: string;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIColumn
	 */
	backgroundColor?: string;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIColumn
	 */
	volteKPIId?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIColumn
	 */
	opCoId?: number;
	/**
	 *
	 * @type {VolteKPIType}
	 * @memberof VolteKPIColumn
	 */
	volteKPIType?: VolteKPIType;
}
/**
 *
 * @export
 * @interface VolteKPIDashboardDto
 */
export interface VolteKPIDashboardDto {
	/**
	 *
	 * @type {Array<VolteKPIDtoGrid>}
	 * @memberof VolteKPIDashboardDto
	 */
	items?: Array<VolteKPIDtoGrid>;
	/**
	 *
	 * @type {{ [key: string]: string; }}
	 * @memberof VolteKPIDashboardDto
	 */
	opCoResource?: { [key: string]: string };
	/**
	 *
	 * @type {{ [key: string]: string; }}
	 * @memberof VolteKPIDashboardDto
	 */
	volteKPITypeResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface VolteKPIDto
 */
export interface VolteKPIDto extends GridDtoBase {
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	month?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	year?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiOneEoYTarget?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiOneMonthlyTarget?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiOneActualValue?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiOneTargetValueChangeProposal?: number;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIDto
	 */
	kpiOneComment?: string;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiTwoActualNumberOfRegisteredSubscribers?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiTwoActualNumberOfProvisionedSubscriber?: number;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIDto
	 */
	kpiTwoComment?: string;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiThreeEoYTarget?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiThreeMonthlyTarget?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiThreeActualValue?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiThreeTargetValueChangeProposal?: number;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIDto
	 */
	kpiThreeComment?: string;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiFourEoYTarget?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiFourMonthlyTarget?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiFourActualValue?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDto
	 */
	kpiFourTargetValueChangeProposal?: number;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIDto
	 */
	kpiFourComment?: string;
}
/**
 *
 * @export
 * @interface VolteKPIDtoCreate
 */
export interface VolteKPIDtoCreate extends VolteKPIDto {
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDtoCreate
	 */
	volteKPIId?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDtoCreate
	 */
	opCoId?: number;
	/**
	 *
	 * @type {VolteKPIType}
	 * @memberof VolteKPIDtoCreate
	 */
	volteKPIType?: VolteKPIType;
	/**
	 *
	 * @type {{ [key: string]: string; }}
	 * @memberof VolteKPIDtoCreate
	 */
	opCoResource?: { [key: string]: string };
	/**
	 *
	 * @type {{ [key: string]: string; }}
	 * @memberof VolteKPIDtoCreate
	 */
	volteKPITypeResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface VolteKPIDtoGrid
 */
export interface VolteKPIDtoGrid extends GridDtoBase {
	/**
	 *
	 * @type {VolteKPIType}
	 * @memberof VolteKPIDtoGrid
	 */
	type?: VolteKPIType;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIDtoGrid
	 */
	program?: string;
	/**
	 *
	 * @type {Array<VolteKPIColumn>}
	 * @memberof VolteKPIDtoGrid
	 */
	opCoList?: Array<VolteKPIColumn>;
	/**
	 *
	 * @type {Array<string>}
	 * @memberof VolteKPIDtoGrid
	 */
	opCoColumns?: Array<string>;
}
/**
 *
 * @export
 * @interface VolteKPIDtoUpdate
 */
export interface VolteKPIDtoUpdate extends VolteKPIDtoCreate {
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIDtoUpdate
	 */
	volteKPIId?: number;
}
/**
 *
 * @export
 * @enum {string}
 */
export enum VolteKPIType {
	NUMBER_1 = <any>1,
	NUMBER_2 = <any>2,
	NUMBER_3 = <any>3,
	NUMBER_4 = <any>4,
}

// -------------- Not auto Generated----------------

export interface VolteKPIEdit {
	VolteKPIDtoEdit: VolteKPIDtoUpdate | null;
	ResultDtoEdit: ResultDto | null;
}

export interface VolteKPICreate {
	VolteKPIDtoCreate: VolteKPIDtoCreate | null;
	ResultDtoCreate: ResultDto | null;
}
export interface VolteKPIGrid {
	VolteKPIGridResult: VolteKPIDashboardDto | null;
	filter: FilterValueDto[] | null;
}

export interface VolteKPIQueryObjectGrid extends QueryObjectGrid {
	volteKPIId?: Array<number>;
	opCo?: Array<number>;
	volteKPITypes?: Array<VolteKPIType>;
	month?: number;
	year?: number;
	sortBy?: string;
	isSortAscending?: boolean;
	page?: number;
	pageSize?: number;
	lastModifiedStartDate?: Date;
	lastModifiedEndDate?: Date;
	principalId?: number;
}

export const GET_CREATE_VOLTE_KPI = "GET_CREATE_VOLTE_KPI";
export const GET_EDIT_VOLTE_KPI = "GET_EDIT_VOLTE_KPI";
export const GET_GRID_VOLTE_KPI = "GET_GRID_VOLTE_KPI";
export const GET_FILTER_VOLTE_KPI = "GET_FILTER_VOLTE_KPI";
export const CREATE_VOLTE_KPI = "CREATE_VOLTE_KPI";
export const EDIT_VOLTE_KPI = "EDIT_VOLTE_KPI";
export const DELETE_VOLTE_KPI = "DELETE_VOLTE_KPI";
export const RESTORE_VOLTE_KPI = "RESTORE_VOLTE_KPI";
