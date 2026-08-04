import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";
import { TipologicaQueryDtoForVirtualized } from "./LookUpGenericModel";
import { TipologicaGridDtoForVirtualized } from "./PlannedActivityResource";

/**
 *
 * @export
 * @interface ServiceBoundaryGridDto
 */
export interface ServiceBoundaryGridDto extends GridDtoBase {
	/**
	 *
	 * @type {number}
	 * @memberof ServiceBoundaryGridDto
	 */
	serviceBoundaryId?: number;
	/**
	 *
	 * @type {string}
	 * @memberof ServiceBoundaryGridDto
	 */
	serviceBoundaryDescription?: string;
	/**
	 *
	 * @type {Date}
	 * @memberof ServiceBoundaryGridDto
	 */
	lastModified?: Date;
	/**
	 *
	 * @type {string}
	 * @memberof ServiceBoundaryGridDto
	 */
	lastModifiedBy?: string;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfServiceBoundaryGridDto
 */
export interface CustomGridRenderOfServiceBoundaryGridDto {
	/**
	 *
	 * @type {string}
	 * @memberof CustomGridRenderOfServiceBoundaryGridDto
	 */
	className?: string;
	/**
	 *
	 * @type {Array<RenderDetail>}
	 * @memberof CustomGridRenderOfServiceBoundaryGridDto
	 */
	render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfServiceBoundaryGridDto
 */
export interface QueryResultDtoOfServiceBoundaryGridDto {
	/**
	 *
	 * @type {number}
	 * @memberof QueryResultDtoOfServiceBoundaryGridDto
	 */
	totalItems?: number;
	/**
	 *
	 * @type {Array<ServiceBoundaryGridDto>}
	 * @memberof QueryResultDtoOfServiceBoundaryGridDto
	 */
	items?: Array<ServiceBoundaryGridDto>;
	/**
	 *
	 * @type {CustomGridRenderOfServiceBoundaryGridDto}
	 * @memberof QueryResultDtoOfServiceBoundaryGridDto
	 */
	gridRender?: CustomGridRenderOfServiceBoundaryGridDto;
}

// not auto gen

export interface ServiceBoundaryQueryDto extends QueryObject {
	/**
	 *
	 * @type {Array<number>}
	 * @memberof ServiceBoundaryQueryDto
	 */
	serviceBoundaryId?: Array<number>;
	/**
	 *
	 * @type {Array<number>}
	 * @memberof ServiceBoundaryQueryDto
	 */
	serviceBoundaryDescription?: Array<string>;
	/**
	 *
	 * @type {Array<number>}
	 * @memberof ServiceBoundaryQueryDto
	 */
	lastModifiedStartDate?: Date;
	/**
	 *
	 * @type {Array<number>}
	 * @memberof ServiceBoundaryQueryDto
	 */
	lastModifiedEndDate?: Date;
}

export interface LookUpEditServiceBoundary {
	LookUpDtoEdit: ServiceBoundaryGridDto | null;
	ResultDtoEdit: ResultDto | null;
}

export interface LookUpCreateServiceBoundary {
	LookUpDtoCreate: ServiceBoundaryGridDto | null;
	ResultDtoCreate: ResultDto | null;
}

export interface LookUpServiceBoundaryGrid {
	LookUpGridResult: QueryResultDtoOfServiceBoundaryGridDto | null;
	LookUpGridResultAll: QueryResultDtoOfServiceBoundaryGridDto | null;
	filter: FilterValueDto[] | null;
}
