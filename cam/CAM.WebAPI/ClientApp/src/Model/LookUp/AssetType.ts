import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "../Common";
import { ResultDto } from "../CommonModels";
/**
 * 
 * @export
 * @interface AssetTypeDto
 */
export interface AssetTypeDto {
    /**
     * 
     * @type {number}
     * @memberof AssetTypeDto
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof AssetTypeDto
     */
    description?: string;
    /**
     * 
     * @type {string}
     * @memberof AssetTypeDto
     */
    assetCategoryId?: string;
    /**
 * 
 * @type {number}
 * @memberof AssetTypeDto
 */
    idAssetCategory?: number;
    /**
     * 
     * @type {Date}
     * @memberof AssetTypeDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof AssetTypeDto
     */
    lastModifiedBy?: string;
    /**
     * 
     * @type {{ [key: string]: string; }}
     * @memberof AssetTypeDto
     */
    assetCategoryResource?: { [key: string]: string; };
}
/**
 * 
 * @export
 * @interface AssetTypeDtoGrid
 */
export interface AssetTypeDtoGrid extends GridDtoBase {
    /**
     * 
     * @type {number}
     * @memberof AssetTypeDtoGrid
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof AssetTypeDtoGrid
     */
    description?: string;
    /**
     * 
     * @type {string}
     * @memberof AssetTypeDtoGrid
     */
    idAssetCategory?: number;

    /**
 * 
 * @type {string}
 * @memberof AssetTypeDtoGrid
 */
    assetCategoryId?: string;

}
/**
 * 
 * @export
 * @interface QueryResultDtoOfAssetTypeDtoGrid
 */
export interface QueryResultDtoOfAssetTypeDtoGrid {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfAssetTypeDtoGrid
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<AssetTypeDtoGrid>}
     * @memberof QueryResultDtoOfAssetTypeDtoGrid
     */
    items?: Array<AssetTypeDtoGrid>;
    /**
     * 
     * @type {CustomGridRenderOfAssetTypeDtoGrid}
     * @memberof QueryResultDtoOfAssetTypeDtoGrid
     */
    gridRender?: CustomGridRender;
}

export interface AssetTypeQueryObjectGrid extends QueryObjectGrid {
    id?: Array<number>,
    description?: Array<string>,
    assetCategoryId?: Array<number>,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    principalId?: number
}

export interface AssetTypeGrid {
    LookUpGridResult: QueryResultDtoOfAssetTypeDtoGrid | null;
    LookUpGridResultAll: QueryResultDtoOfAssetTypeDtoGrid | null;
    filter: FilterValueDto[] | null;
}

export interface AssetTypeEdit {
	LookUpDtoEdit: AssetTypeDto | null;
	ResultDtoEdit: ResultDto | null;
}
export interface AssetTypeCreate {
	LookUpDtoCreate: AssetTypeDto | null;
	ResultDtoCreate: ResultDto | null;
}