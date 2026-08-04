import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObject } from "../Common";
import { ResultDto } from "../CommonModels";



/**
 * 
 * @export
 * @interface NFVIStatusDtoGrid
 */
 export interface NFVIStatusDtoGrid extends GridDtoBase {
    /**
     * 
     * @type {number}
     * @memberof NFVIStatusDtoGrid
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof NFVIStatusDtoGrid
     */
    description?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVIStatusDtoGrid
     */
    color?: string;
    /**
     * 
     * @type {Date}
     * @memberof NFVIStatusDtoGrid
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof NFVIStatusDtoGrid
     */
    lastModifiedBy?: string;
}

/**
 * 
 * @export
 * @interface QueryResultDtoOfNFVIStatusDtoGrid
 */
export interface QueryResultDtoOfNFVIStatusDtoGrid {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfNFVIStatusDtoGrid
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<NFVIStatusDtoGrid>}
     * @memberof QueryResultDtoOfNFVIStatusDtoGrid
     */
    items?: Array<NFVIStatusDtoGrid>;
    /**
     * 
     * @type {CustomGridRenderOfNFVIStatusDtoGrid}
     * @memberof QueryResultDtoOfNFVIStatusDtoGrid
     */
    gridRender?: CustomGridRender;
}

export interface NFVIStatusQueryObjectGrid extends QueryObject {

    id?: Array<number>,
    description?: Array<string>,
    color?: Array<string>,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    principalId?: number,
}