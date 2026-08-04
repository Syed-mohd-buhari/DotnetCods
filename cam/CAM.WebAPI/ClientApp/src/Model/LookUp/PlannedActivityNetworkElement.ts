import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObjectGrid, RenderDetail } from "../Common";
import { PlannedActivityResourceDto, PlannedActivityResourceDtoGrid } from "./PlannedActivityResource";

/**
 *
 * @export
 * @interface PlannedActivityNetworkElementDto
 */
export interface PlannedActivityNetworkElementDto extends GridDtoBase {
	/**
	 *
	 * @type {string}
	 * @memberof PlannedActivityNetworkElementDto
	 */
	plannedActivityNetworkElementDescription?: string;
	/**
	 *
	 * @type {number}
	 * @memberof PlannedActivityNetworkElementDto
	 */
	rule?: number;
	/**
	 *
	 * @type {boolean}
	 * @memberof PlannedActivityNetworkElementDto
	 */
	forCreate?: boolean;
	/**
	 *
	 * @type {boolean}
	 * @memberof PlannedActivityNetworkElementDto
	 */
	forEdit?: boolean;
	/**
	 *
	 * @type {string}
	 * @memberof PlannedActivityNetworkElementDto
	 */
	benefitText?: string;
	/**
	 *
	 * @type {string}
	 * @memberof PlannedActivityNetworkElementDto
	 */
	driverText?: string;
	/**
	 *
	 * @type {Date}
	 * @memberof PlannedActivityNetworkElementDto
	 */
	lastModified?: Date;
	/**
	 *
	 * @type {string}
	 * @memberof PlannedActivityNetworkElementDto
	 */
	lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface PlannedActivityNetworkElementDtoCreate
 */
export interface PlannedActivityNetworkElementDtoCreate extends PlannedActivityNetworkElementDto {
	/**
	 *
	 * @type {number}
	 * @memberof PlannedActivityNetworkElementDtoCreate
	 */
	plannedActivityResourceId?: number;
	/**
	 *
	 * @type {{ [key: string]: PlannedActivityResourceDto; }}
	 * @memberof PlannedActivityNetworkElementDtoCreate
	 */
	plannedActivityResource?: { [key: string]: PlannedActivityResourceDto };
}
/**
 *
 * @export
 * @interface PlannedActivityNetworkElementDtoGrid
 */
export interface PlannedActivityNetworkElementDtoGrid extends PlannedActivityNetworkElementDto {
	/**
	 *
	 * @type {number}
	 * @memberof PlannedActivityNetworkElementDtoGrid
	 */
	plannedActivityNetworkElementId?: number;
	/**
	 *
	 * @type {string}
	 * @memberof PlannedActivityNetworkElementDtoGrid
	 */
	plannedActivityNetworkElementDescription?: string;
	/**
	 *
	 * @type {number}
	 * @memberof PlannedActivityNetworkElementDtoGrid
	 */
	rule?: number;
	/**
	 *
	 * @type {boolean}
	 * @memberof PlannedActivityNetworkElementDtoGrid
	 */
	forCreate?: boolean;
	/**
	 *
	 * @type {boolean}
	 * @memberof PlannedActivityNetworkElementDtoGrid
	 */
	forEdit?: boolean;
	/**
	 *
	 * @type {string}
	 * @memberof PlannedActivityNetworkElementDtoGrid
	 */
	plannedActivityResource?: string;
	/**
	 *
	 * @type {string}
	 * @memberof PlannedActivityNetworkElementDtoGrid
	 */
	benefitText?: string;
	/**
	 *
	 * @type {string}
	 * @memberof PlannedActivityNetworkElementDtoGrid
	 */
	driverText?: string;
	/**
	 *
	 * @type {Date}
	 * @memberof PlannedActivityNetworkElementDtoGrid
	 */
	lastModified?: Date;
	/**
	 *
	 * @type {string}
	 * @memberof PlannedActivityNetworkElementDtoGrid
	 */
	lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface PlannedActivityNetworkElementDtoUpdate
 */
export interface PlannedActivityNetworkElementDtoUpdate extends PlannedActivityNetworkElementDtoCreate {
	/**
	 *
	 * @type {number}
	 * @memberof PlannedActivityNetworkElementDtoUpdate
	 */
	plannedActivityNetworkElementId?: number;
}

//#region no auto gen

export interface PlannedActivityNetworkElementQueryObjectGrid extends QueryObjectGrid {
	plannedActivityResourceId?: Array<number>;
	plannedActivityNetworkElementId?: Array<number>;
	plannedActivityResourceDescription?: Array<string>;
	plannedActivityNetworkElementDescription?: Array<string>;
	benefitText?: Array<string>;
	driverText?: Array<string>;
	forCreate?: Array<boolean>;
	forEdit?: Array<boolean>;
	rule?: Array<number>;
	sortBy?: string;
	isSortAscending?: boolean;
	page?: number;
	pageSize?: number;
	lastModifiedStartDate?: Date;
	lastModifiedEndDate?: Date;
	principalId?: number;
	deleted?: boolean;
	orphan?: boolean;
	lastModifiedBy?: Array<string>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfPlannedActivityNetworkElementDtoGrid
 */
export interface QueryResultDtoOfPlannedActivityNetworkElementDtoGrid {
	/**
	 *
	 * @type {number}
	 * @memberof QueryResultDtoOfPlannedActivityNetworkElementDtoGrid
	 */
	totalItems?: number;
	/**
	 *
	 * @type {Array<PlannedActivityNetworkElementDtoGrid>}
	 * @memberof QueryResultDtoOfPlannedActivityNetworkElementDtoGrid
	 */
	items?: Array<PlannedActivityNetworkElementDtoGrid>;
	/**
	 *
	 * @type {CustomGridRenderOfPlannedActivityNetworkElementDtoGrid}
	 * @memberof QueryResultDtoOfPlannedActivityNetworkElementDtoGrid
	 */
	gridRender?: CustomGridRenderOfPlannedActivityNetworkElementDtoGrid;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfPlannedActivityNetworkElementDtoGrid
 */
export interface CustomGridRenderOfPlannedActivityNetworkElementDtoGrid {
	/**
	 *
	 * @type {string}
	 * @memberof CustomGridRenderOfPlannedActivityNetworkElementDtoGrid
	 */
	className?: string;
	/**
	 *
	 * @type {Array<RenderDetail>}
	 * @memberof CustomGridRenderOfPlannedActivityNetworkElementDtoGrid
	 */
	render?: Array<RenderDetail>;
}

export interface LookUpGridPlannedActivityNetworkElement {
	LookUpGridResult: QueryResultDtoOfPlannedActivityNetworkElementDtoGrid | null;
	LookUpGridResultAll: QueryResultDtoOfPlannedActivityNetworkElementDtoGrid | null;
	filter: FilterValueDto[] | null;
}

//#endregion
