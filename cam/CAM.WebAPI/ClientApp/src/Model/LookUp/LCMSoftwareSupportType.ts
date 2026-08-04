import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";
import { ResultDto } from "../CommonModels";

/**
 * 
 * @export
 * @interface LCMSoftwareSupportTypeDto
 */
 export interface LCMSoftwareSupportTypeDto {
    /**
     * 
     * @type {number}
     * @memberof LCMSoftwareSupportTypeDto
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof LCMSoftwareSupportTypeDto
     */
    description?: string;
    /**
     * 
     * @type {boolean}
     * @memberof LCMSoftwareSupportTypeDto
     */
    oemSupport?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof LCMSoftwareSupportTypeDto
     */
    warranty?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof LCMSoftwareSupportTypeDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof LCMSoftwareSupportTypeDto
     */
    lastModifiedBy?: string;
}
/**
 * 
 * @export
 * @interface LCMSoftwareSupportTypeDtoGrid
 */
export interface LCMSoftwareSupportTypeDtoGrid extends GridDtoBase {
    /**
     * 
     * @type {number}
     * @memberof LCMSoftwareSupportTypeDtoGrid
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof LCMSoftwareSupportTypeDtoGrid
     */
    description?: string;
    /**
     * 
     * @type {boolean}
     * @memberof LCMSoftwareSupportTypeDtoGrid
     */
    oemSupport?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof LCMSoftwareSupportTypeDtoGrid
     */
    warranty?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof LCMSoftwareSupportTypeDtoGrid
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof LCMSoftwareSupportTypeDtoGrid
     */
    valueToShow?: string;
    /**
     * 
     * @type {string}
     * @memberof LCMSoftwareSupportTypeDtoGrid
     */
    lastModifiedBy?: string;
}

/**
 * 
 * @export
 * @interface QueryResultDtoOfLCMSoftwareSupportTypeDtoGrid
 */
export interface QueryResultDtoOfLCMSoftwareSupportTypeDtoGrid {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfLCMSoftwareSupportTypeDtoGrid
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<LCMSoftwareSupportTypeDtoGrid>}
     * @memberof QueryResultDtoOfLCMSoftwareSupportTypeDtoGrid
     */
    items?: Array<LCMSoftwareSupportTypeDtoGrid>;
    /**
     * 
     * @type {CustomGridRenderOfLCMSoftwareSupportTypeDtoGrid}
     * @memberof QueryResultDtoOfLCMSoftwareSupportTypeDtoGrid
     */
    gridRender?: CustomGridRender;
}

export interface LCMSoftwareSupportTypeQueryObjectGrid extends QueryObjectGrid {
    id?: Array<number>,
    description?: Array<string>,
    oemSupport?: Array<boolean>,
    warranty?: Array<boolean>,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    principalId?: number,
}


export interface LCMSoftwareSupportTypOperational {
    Create: LCMSoftwareSupportTypeDto | null,
    ResultDtoCreate: ResultDto | null,
}
export interface LCMSoftwareSupportTypOperationalEdit {
    Create: LCMSoftwareSupportTypeDto | null,
    ResultDtoEdit: ResultDto | null,
}
export interface LCMSoftwareSupportTypGrid {
    GridResult: QueryResultDtoOfLCMSoftwareSupportTypeDtoGrid | null,
    GridResultAll: QueryResultDtoOfLCMSoftwareSupportTypeDtoGrid | null,
    filter: FilterValueDto[] | null
}
