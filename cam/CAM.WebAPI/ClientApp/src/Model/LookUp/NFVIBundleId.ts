import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObject } from "../Common";
import { ResultDto } from "../CommonModels";



/**
 * 
 * @export
 * @interface NFVIBundleIDDtoGrid
 */
 export interface NFVIBundleIDDtoGrid extends GridDtoBase {
    /**
     * 
     * @type {number}
     * @memberof NFVIBundleIDDtoGrid
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof NFVIBundleIDDtoGrid
     */
    description?: string;
    /**
     * 
     * @type {number}
     * @memberof NFVIBundleIDDtoGrid
     */
    order?: number;
    /**
     * 
     * @type {Date}
     * @memberof NFVIBundleIDDtoGrid
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof NFVIBundleIDDtoGrid
     */
    lastModifiedBy?: string;
}

/**
 * 
 * @export
 * @interface QueryResultDtoOfNFVIBundleIDDtoGrid
 */
export interface QueryResultDtoOfNFVIBundleIDDtoGrid {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfNFVIBundleIDDtoGrid
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<NFVIBundleIDDtoGrid>}
     * @memberof QueryResultDtoOfNFVIBundleIDDtoGrid
     */
    items?: Array<NFVIBundleIDDtoGrid>;
    /**
     * 
     * @type {CustomGridRenderOfNFVIBundleIDDtoGrid}
     * @memberof QueryResultDtoOfNFVIBundleIDDtoGrid
     */
    gridRender?: CustomGridRender;
}

export interface NFVIBundleIDQueryObjectGrid extends QueryObject {

    id?: Array<number>,
    description?: Array<string>,
    order?: Array<number>,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    principalId?: number,
}