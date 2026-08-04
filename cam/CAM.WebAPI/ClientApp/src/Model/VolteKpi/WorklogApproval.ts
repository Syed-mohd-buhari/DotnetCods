import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { DateFilter, GridDtoBase, QueryObjectGrid, RenderDetail, QueryObject } from '../Common';
import { ResultDto } from "../CommonModels";
import { VolteKPIType } from "./VolteKPI";

/**
 *
 * @export
 * @interface VolteKPIWorklogDto
 */
export interface VolteKPIWorklogDto extends GridDtoBase {
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	volteKPIWorklogId?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	volteKPIId?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	opCoId?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	month?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	year?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	volteKPIType?: number;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIWorklogDto
	 */
	opCo?: string;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIWorklogDto
	 */
	 kpiIdName?: string;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIWorklogDto
	 */
	monthYear?: string;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	targetMonthlyValueOld?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	targetMonthlyValueProposed?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	targetMonthlyValueNew?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	eoyTargetOld?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	eoyTargetNew?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	actualMonthlyValueOld?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	actualMonthlyValueNew?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	actualNumberOfRegisteredOld?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	actualNumberOfRegisteredNew?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	actualNumberOfProvisionedOld?: number;
	/**
	 *
	 * @type {number}
	 * @memberof VolteKPIWorklogDto
	 */
	actualNumberOfProvisionedNew?: number;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIWorklogDto
	 */
	comments?: string;
	/**
	 *
	 * @type {boolean}
	 * @memberof VolteKPIWorklogDto
	 */
	approved?: boolean;
	/**
	 *
	 * @type {Date}
	 * @memberof VolteKPIWorklogDto
	 */
	submissionDate?: Date;
	/**
	 *
	 * @type {string}
	 * @memberof VolteKPIWorklogDto
	 */
	submittedBy?: string;
	/**
	 *
	 * @type {boolean}
	 * @memberof VolteKPIWorklogDto
	 */
	isStored?: boolean;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfVolteKPIWorklogDto
 */
export interface QueryResultDtoOfVolteKPIWorklogDto {
	/**
	 *
	 * @type {number}
	 * @memberof QueryResultDtoOfVolteKPIWorklogDto
	 */
	totalItems?: number;
	/**
	 *
	 * @type {Array<VolteKPIWorklogDto>}
	 * @memberof QueryResultDtoOfVolteKPIWorklogDto
	 */
	items?: Array<VolteKPIWorklogDto>;
	/**
	 *
	 * @type {CustomGridRenderOfVolteKPIWorklogDto}
	 * @memberof QueryResultDtoOfVolteKPIWorklogDto
	 */
	gridRender?: CustomGridRenderOfVolteKPIWorklogDto;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfVolteKPIWorklogDto
 */
export interface CustomGridRenderOfVolteKPIWorklogDto {
	/**
	 *
	 * @type {string}
	 * @memberof CustomGridRenderOfVolteKPIWorklogDto
	 */
	className?: string;
	/**
	 *
	 * @type {Array<RenderDetail>}
	 * @memberof CustomGridRenderOfVolteKPIWorklogDto
	 */
	render?: Array<RenderDetail>;
}



// ----Not Auto Generated

export interface WorklogApprovalEdit {
	worklogApprovalDtoEdit: QueryResultDtoOfVolteKPIWorklogDto | null;
	ResultDtoEdit: ResultDto | null;
}
export interface WorklogApprovalGrid {
	worklogApprovalGridResult: QueryResultDtoOfVolteKPIWorklogDto | null;
	filter: FilterValueDto[] | null;
}

export interface WorklogApprovalQueryObjectGrid extends QueryObject {
	/**
     * 
     * @type {Array<number>}
     * @memberof VolteKPIWorklogQueryDto
     */
	 opCo?: Array<number>;
	 /**
	  * 
	  * @type {Array<number>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 kpiIdName?: Array<number>;
	 /**
	  * 
	  * @type {Array<string>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 monthYear?: Array<string>;
	 /**
	  * 
	  * @type {Array<boolean>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 targetMonthlyValueOld?: Array<boolean>;
	 /**
	  * 
	  * @type {Array<boolean>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 targetMonthlyValueProposed?: Array<boolean>;
	 /**
	  * 
	  * @type {Array<boolean>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 targetMonthlyValueNew?: Array<boolean>;
	 /**
	  * 
	  * @type {Array<boolean>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 eoyTargetOld?: Array<boolean>;
	 /**
	  * 
	  * @type {Array<boolean>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 eoyTargetNew?: Array<boolean>;
	 /**
	  * 
	  * @type {Array<boolean>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 actualMonthlyValueOld?: Array<boolean>;
	 /**
	  * 
	  * @type {Array<boolean>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 actualMonthlyValueNew?: Array<boolean>;
	 /**
	  * 
	  * @type {Array<boolean>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 actualNumberOfRegisteredOld?: Array<boolean>;
	 /**
	  * 
	  * @type {Array<boolean>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 actualNumberOfRegisteredNew?: Array<boolean>;
	 /**
	  * 
	  * @type {Array<boolean>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 actualNumberOfProvisionedOld?: Array<boolean>;
	 /**
	  * 
	  * @type {Array<boolean>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 actualNumberOfProvisionedNew?: Array<boolean>;
	 /**
	  * 
	  * @type {Array<string>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 comments?: Array<string>;
	 /**
	  * 
	  * @type {Array<number>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 approved?: Array<number>;
	 /**
	  * 
	  * @type {DateFilter}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 submissionDate?: DateFilter;
	 /**
	  * 
	  * @type {Array<string>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 submittedBy?: Array<string>;
	 /**
	  * 
	  * @type {Array<number>}
	  * @memberof VolteKPIWorklogQueryDto
	  */
	 isStored?: Array<number>;
}

export const GET_WORKLOG_APPROVALS = "GET_WORKLOG_APPROVALS";
export const EDIT_WORKLOG_APPROVALS = "EDIT_WORKLOG_APPROVALS";
export const GET_FILTER_WORKLOG_APPROVAL = "GET_FILTER_WORKLOG_APPROVAL";
