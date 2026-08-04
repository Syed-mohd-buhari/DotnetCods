import { RelatedResource, ResultDto } from "./CommonModels";
import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObject, QueryObjectGrid } from "./Common";

/**
 * 
 * @export
 * @interface NFVITransitionDto
 */
 export interface NFVITransitionDto extends GridDtoBase {
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDto
     */
    nfviSiteDesignation: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDto
     */
    nextStep?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDto
     */
    spare1Json?: string;
    /**
     * 
     * @type {Date}
     * @memberof NFVITransitionDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDto
     */
    lastModifiedBy?: string;
}
/**
 * 
 * @export
 * @interface NFVITransitionDtoCreate
 */
export interface NFVITransitionDtoCreate extends NFVITransitionDto {
    /**
     * 
     * @type {{ [key: string]: string; }}
     * @memberof NFVITransitionDtoCreate
     */
    opCoResource?: { [key: string]: string; };
    /**
     * 
     * @type {number}
     * @memberof NFVITransitionDtoCreate
     */
    opCoId: number;
    /**
     * 
     * @type {{ [key: string]: RelatedResource; }}
     * @memberof NFVITransitionDtoCreate
     */
    statusLabMCResource?: { [key: string]: RelatedResource; };
    /**
     * 
     * @type {number}
     * @memberof NFVITransitionDtoCreate
     */
    statusLabMCId: number;
    /**
     * 
     * @type {{ [key: string]: RelatedResource; }}
     * @memberof NFVITransitionDtoCreate
     */
    statusLabSCResource?: { [key: string]: RelatedResource; };
    /**
     * 
     * @type {number}
     * @memberof NFVITransitionDtoCreate
     */
    statusLabSCId: number;
    /**
     * 
     * @type {{ [key: string]: RelatedResource; }}
     * @memberof NFVITransitionDtoCreate
     */
    statusLiveMCResource?: { [key: string]: RelatedResource; };
    /**
     * 
     * @type {number}
     * @memberof NFVITransitionDtoCreate
     */
    statusLiveMCId: number;
    /**
     * 
     * @type {{ [key: string]: RelatedResource; }}
     * @memberof NFVITransitionDtoCreate
     */
    statusLiveSCResource?: { [key: string]: RelatedResource; };
    /**
     * 
     * @type {number}
     * @memberof NFVITransitionDtoCreate
     */
    statusLiveSCId: number;
    /**
     * 
     * @type {{ [key: string]: RelatedResource; }}
     * @memberof NFVITransitionDtoCreate
     */
    status12KSwitchResource?: { [key: string]: RelatedResource; };
    /**
     * 
     * @type {number}
     * @memberof NFVITransitionDtoCreate
     */
    status12KSwitchId?: number;
}
/**
 * 
 * @export
 * @interface NFVITransitionDtoGrid
 */
export interface NFVITransitionDtoGrid extends NFVITransitionDto {
    /**
     * 
     * @type {number}
     * @memberof NFVITransitionDtoGrid
     */
    nfviTransitionId?: number;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDtoGrid
     */
    statusLabMCColor?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDtoGrid
     */
    statusLabSCColor?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDtoGrid
     */
    statusLiveMCColor?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDtoGrid
     */
    statusLiveSCColor?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDtoGrid
     */
    status12KSwitchColor?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDtoGrid
     */
    opCo?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDtoGrid
     */
    statusLabSC?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDtoGrid
     */
    statusLabMC?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDtoGrid
     */
    statusLiveMC?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDtoGrid
     */
    statusLiveSC?: string;
    /**
     * 
     * @type {string}
     * @memberof NFVITransitionDtoGrid
     */
    status12KSwitch?: string;
}
/**
 * 
 * @export
 * @interface NFVITransitionDtoUpdate
 */
export interface NFVITransitionDtoUpdate extends NFVITransitionDtoCreate {
    /**
     * 
     * @type {number}
     * @memberof NFVITransitionDtoUpdate
     */
    nfviTransitionId?: number;
}

/**
 * 
 * @export
 * @interface QueryResultDtoOfNFVITransitionDtoGrid
 */
export interface QueryResultDtoOfNFVITransitionDtoGrid {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfNFVITransitionDtoGrid
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<NFVITransitionDtoGrid>}
     * @memberof QueryResultDtoOfNFVITransitionDtoGrid
     */
    items?: Array<NFVITransitionDtoGrid>;
    /**
     * 
     * @type {CustomGridRenderOfNFVITransitionDtoGrid}
     * @memberof QueryResultDtoOfNFVITransitionDtoGrid
     */
    gridRender?: CustomGridRender;
}


// ------------------------ Not generated  ------------------------ 


export interface NFVITransitionEdit {
    NFVITransitionDtoEdit: NFVITransitionDtoUpdate | null,
    ResultDtoEdit: ResultDto | null,
}

export interface NFVITransitionCreate {
    NFVITransitionDtoCreate: NFVITransitionDtoCreate | null,
    ResultDtoCreate: ResultDto | null,
}
export interface NFVITransitionGrid {
    NFVITransitionGridResult: QueryResultDtoOfNFVITransitionDtoGrid | null,
    filter: FilterValueDto[] | null
}
export interface NFVITransitionQueryObjectGrid extends QueryObject {
    /**
     * 
     * @type {Array<number>}
     * @memberof NFVITransitionQueryDto
     */
    nfviTransitionId?: Array<number>;
    /**
     * 
     * @type {Array<number>}
     * @memberof NFVITransitionQueryDto
     */
    opCo?: Array<number>;
    /**
     * 
     * @type {Array<number>}
     * @memberof NFVITransitionQueryDto
     */
    statusLabMC?: Array<number>;
    /**
     * 
     * @type {Array<number>}
     * @memberof NFVITransitionQueryDto
     */
    statusLabSC?: Array<number>;
    /**
     * 
     * @type {Array<number>}
     * @memberof NFVITransitionQueryDto
     */
    statusLiveMC?: Array<number>;
    /**
     * 
     * @type {Array<number>}
     * @memberof NFVITransitionQueryDto
     */
    statusLiveSC?: Array<number>;
    /**
     * 
     * @type {Array<string>}
     * @memberof NFVITransitionQueryDto
     */
    nfviSiteDesignation?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof NFVITransitionQueryDto
     */
    nextStep?: Array<string>;
    /**
     * 
     * @type {Array<number>}
     * @memberof NFVITransitionQueryDto
     */
    status12KSwitch?: Array<number>;
    /**
     * 
     * @type {Array<string>}
     * @memberof NFVITransitionQueryDto
     */
    spare1Json?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof NFVITransitionQueryDto
     */
    lastModifiedBy?: Array<string>;
}



export const GET_CREATE_NFVI_TRAMSITION = "GET_CREATE_NFVI_TRAMSITION";
export const GET_EDIT_NFVI_TRAMSITION = "GET_EDIT_NFVI_TRAMSITION";
export const GET_GRID_NFVI_TRAMSITION = "GET_GRID_NFVI_TRAMSITION";
export const GET_FILTER_NFVI_TRAMSITION = "GET_FILTER_NFVI_TRAMSITION";
export const CREATE_NFVI_TRAMSITION = "CREATE_NFVI_TRAMSITION";
export const EDIT_NFVI_TRAMSITION = "EDIT_NFVI_TRAMSITION";
export const DELETE_NFVI_TRAMSITION = "DELETE_NFVI_TRAMSITION";
export const RESTORE_NFVI_TRAMSITION = "RESTORE_NFVI_TRAMSITION";
