import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";

/**
 * 
 * @export
 * @interface AssetClassDto
 */
 export interface AssetClassDto {
    /**
     * 
     * @type {number}
     * @memberof AssetClassDto
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof AssetClassDto
     */
    description?: string;
    /**
     * 
     * @type {Date}
     * @memberof AssetClassDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof AssetClassDto
     */
    lastModifiedBy?: string;
}
/**
 * 
 * @export
 * @interface AssetClassDtoGrid
 */
export interface AssetClassDtoGrid extends GridDtoBase {
    /**
     * 
     * @type {number}
     * @memberof AssetClassDtoGrid
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof AssetClassDtoGrid
     */
    description?: string;
    /**
     * 
     * @type {string}
     * @memberof AssetClassDtoGrid
     */
    lastModifiedBy?: string;
}


/**
 * 
 * @export
 * @interface QueryResultDtoOfAssetClassDtoGrid
 */
export interface QueryResultDtoOfAssetClassDtoGrid {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfAssetClassDtoGrid
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<AssetClassDtoGrid>}
     * @memberof QueryResultDtoOfAssetClassDtoGrid
     */
    items?: Array<AssetClassDtoGrid>;
    /**
     * 
     * @type {CustomGridRenderOfAssetClassDtoGrid}
     * @memberof QueryResultDtoOfAssetClassDtoGrid
     */
    gridRender?: CustomGridRender;
}



export interface AssetClassQueryObjectGrid extends QueryObjectGrid {
    id?: Array<number>, 
    description?: Array<string>, 
    sortBy?: string, 
    isSortAscending?: boolean, 
    page?: number, 
    pageSize?: number, 
    lastModifiedStartDate?: Date, 
    lastModifiedEndDate?: Date, 
    principalId?: number,
}