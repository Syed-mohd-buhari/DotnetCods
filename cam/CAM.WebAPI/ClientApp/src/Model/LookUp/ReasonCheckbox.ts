import { CustomGridRender, GridDtoBase, QueryObject } from "../Common";

/**
 * 
 * @export
 * @interface ReasonCheckboxDto
 */
 export interface ReasonCheckboxDto extends GridDtoBase {
    /**
     * 
     * @type {number}
     * @memberof ReasonCheckboxDto
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof ReasonCheckboxDto
     */
    description?: string;
    /**
     * 
     * @type {boolean}
     * @memberof ReasonCheckboxDto
     */
    isHardware?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof ReasonCheckboxDto
     */
    isSoftware?: boolean;
    /**
     * 
     * @type {string}
     * @memberof ReasonCheckboxDto
     */
    lastModifiedBy?: string;
}


/**
 * 
 * @export
 * @interface QueryResultDtoOfReasonCheckboxDto
 */
 export interface QueryResultDtoOfReasonCheckboxDto {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfReasonCheckboxDto
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<ReasonCheckboxDto>}
     * @memberof QueryResultDtoOfReasonCheckboxDto
     */
    items?: Array<ReasonCheckboxDto>;
    /**
     * 
     * @type {CustomGridRenderOfReasonCheckboxDto}
     * @memberof QueryResultDtoOfReasonCheckboxDto
     */
    gridRender?: CustomGridRender;
}


export interface ReasonCheckboxQueryObjectGrid extends QueryObject {
    id?: Array<number>,
    description?: Array<string>,
    isHardware?: Array<boolean>, 
    isSoftware?: Array<boolean>,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    principalId?: number,
}