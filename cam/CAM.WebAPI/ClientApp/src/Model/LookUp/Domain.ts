import { GridDtoBase, QueryObject, RenderDetail } from "../Common";

/**
 * 
 * @export
 * @interface SystemNamesDto
 */
export interface SystemNamesDto {
    /**
     * 
     * @type {boolean}
     * @memberof SystemNamesDto
     */
    deleted?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof SystemNamesDto
     */
    orphan?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof SystemNamesDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof SystemNamesDto
     */
    lastModifiedBy?: string;
    /**
     * 
     * @type {number}
     * @memberof SystemNamesDto
     */
    systemNameId?: number;
    /**
     * 
     * @type {string}
     * @memberof SystemNamesDto
     */
    systemNameDescription?: string;
}

/**
 * 
 * @export
 * @interface SystemNamesDtoGrid
 */
export interface SystemNamesDtoGrid extends GridDtoBase {
    /**
     * 
     * @type {boolean}
     * @memberof SystemNamesDtoGrid
     */
    deleted?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof SystemNamesDtoGrid
     */
    orphan?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof SystemNamesDtoGrid
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof SystemNamesDtoGrid
     */
    lastModifiedBy?: string;
    /**
     * 
     * @type {number}
     * @memberof SystemNamesDtoGrid
     */
    systemNameId?: number;
    /**
     * 
     * @type {string}
     * @memberof SystemNamesDtoGrid
     */
    systemNameDescription?: string;
}

/**
 * 
 * @export
 * @interface SystemNameQuery 
 */
export interface SystemNameQuery extends QueryObject {
    deleted?: boolean;
    orphan?: boolean;
    lastModifiedStartDate?: Date;
    lastModifiedEndDate?: Date;
    lastModifiedBy?: Array<string>;
    systemNameId?: Array<number>;
    systemNameDescription?: Array<string>;
    sortBy?: string;
    isSortAscending?: boolean;
    page?: number;
    pageSize?: number;
    options?: any;
}



/**
 * 
 * @export
 * @interface CustomGridRenderOfSystemNamesDtoGrid
 */
export interface CustomGridRenderOfSystemNamesDtoGrid {
    /**
     * 
     * @type {string}
     * @memberof CustomGridRenderOfSystemNamesDtoGrid
     */
    className?: string;
    /**
     * 
     * @type {Array<RenderDetail>}
     * @memberof CustomGridRenderOfSystemNamesDtoGrid
     */
    render?: Array<RenderDetail>;
}

/**
 * 
 * @export
 * @interface QueryResultDtoOfSystemNamesDtoGrid
 */
export interface QueryResultDtoOfSystemNamesDtoGrid {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfSystemNamesDtoGrid
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<SystemNamesDtoGrid>}
     * @memberof QueryResultDtoOfSystemNamesDtoGrid
     */
    items?: Array<SystemNamesDtoGrid>;
    /**
     * 
     * @type {CustomGridRenderOfSystemNamesDtoGrid}
     * @memberof QueryResultDtoOfSystemNamesDtoGrid
     */
    gridRender?: CustomGridRenderOfSystemNamesDtoGrid;
}
