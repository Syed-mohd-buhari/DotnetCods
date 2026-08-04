import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObjectGrid, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";
/**
 * 
 * @export
 * @interface OperationalRiskDto
 */
 export interface OperationalRiskDto extends GridDtoBase {
    /**
     * 
     * @type {number}
     * @memberof OperationalRiskDto
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof OperationalRiskDto
     */
    description?: string;
    /**
     * 
     * @type {number}
     * @memberof OperationalRiskDto
     */
    severity?: number;
    /**
     * 
     * @type {Date}
     * @memberof OperationalRiskDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof OperationalRiskDto
     */
    lastModifiedBy?: string;
}

/**
 * 
 * @export
 * @interface QueryResultDtoOfOperationalRiskDto
 */
export interface QueryResultDtoOfOperationalRiskDto {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfOperationalRiskDto
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<OperationalRiskDto>}
     * @memberof QueryResultDtoOfOperationalRiskDto
     */
    items?: Array<OperationalRiskDto>;
    /**
     * 
     * @type {CustomGridRenderOfOperationalRiskDto}
     * @memberof QueryResultDtoOfOperationalRiskDto
     */
    gridRender?: CustomGridRender;
}

export interface OperationalRiskQueryDto extends QueryObjectGrid {
    id?: Array<number>,
    severity?: Array<number>,
    description?: Array<string>,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    principalId?: number,
    deleted?: boolean,
    orphan?: boolean,
}


export interface LookUpEditRisk {
    LookUpDtoEdit: OperationalRiskDto | null,
    ResultDtoEdit: ResultDto | null,
}
export interface LookUpCreateRisk {
    LookUpDtoCreate: OperationalRiskDto | null,
    ResultDtoCreate: ResultDto | null,
}

export interface LookUpGridRisk {
    LookUpGridRiskResult: QueryResultDtoOfOperationalRiskDto | null,
    LookUpGridRiskResultAll: QueryResultDtoOfOperationalRiskDto | null,
    filter: FilterValueDto[] | null
}



