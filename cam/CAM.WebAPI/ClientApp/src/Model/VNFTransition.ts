import { RelatedResource, ResultDto } from "./CommonModels";
import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObject, QueryObjectGrid } from "./Common";

/**
 * 
 * @export
 * @interface QueryResultDtoOfVNFTransitionDtoGrid
 */
export interface QueryResultDtoOfVNFTransitionDtoGrid {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfVNFTransitionDtoGrid
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<VNFTransitionDtoGrid>}
     * @memberof QueryResultDtoOfVNFTransitionDtoGrid
     */
    items?: Array<VNFTransitionDtoGrid>;
    /**
     * 
     * @type {CustomGridRenderOfVNFTransitionDtoGrid}
     * @memberof QueryResultDtoOfVNFTransitionDtoGrid
     */
    gridRender?: CustomGridRender;
}

/**
 * 
 * @export
 * @interface VNFTransitionDto
 */
 export interface VNFTransitionDto extends GridDtoBase {
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDto
     */
    vnfType: string;
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDto
     */
    currentRelease: string;
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDto
     */
    plannedRelease: string;
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDto
     */
    elementName: string;
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDto
     */
    spare1Json?: string;
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDto
     */
    location: string;
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDto
     */
    nfviSiteDesignation: string;
    /**
     * 
     * @type {Date}
     * @memberof VNFTransitionDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDto
     */
    lastModifiedBy?: string;
}
/**
 * 
 * @export
 * @interface VNFTransitionDtoGrid
 */
export interface VNFTransitionDtoGrid extends VNFTransitionDto {
    /**
     * 
     * @type {number}
     * @memberof VNFTransitionDtoGrid
     */
    vnfTransitionId?: number;
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDtoGrid
     */
    vnfDesignComponent?: string;
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDtoGrid
     */
    opCo?: string;
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDtoGrid
     */
    equipmentStatus?: string;
    /**
     * 
     * @type {string}
     * @memberof VNFTransitionDtoGrid
     */
    nfviBundleID?: string;
}
/**
 * 
 * @export
 * @interface VNFTransitionDtoUpdate
 */
export interface VNFTransitionDtoUpdate extends VnfTransitionDtoCreate {
    /**
     * 
     * @type {number}
     * @memberof VNFTransitionDtoUpdate
     */
    vnfTransitionId?: number;
}
/**
 * 
 * @export
 * @interface VnfTransitionDtoCreate
 */
export interface VnfTransitionDtoCreate extends VNFTransitionDto {
    /**
     * 
     * @type {{ [key: string]: string; }}
     * @memberof VnfTransitionDtoCreate
     */
    vnfDesignComponentResource?: { [key: string]: string; };
    /**
     * 
     * @type {{ [key: string]: string; }}
     * @memberof VnfTransitionDtoCreate
     */
    equipmentStatusResource?: { [key: string]: string; };
    /**
     * 
     * @type {number}
     * @memberof VnfTransitionDtoCreate
     */
    vnfDesignComponentId: number;
    /**
     * 
     * @type {number}
     * @memberof VnfTransitionDtoCreate
     */
    equipmentStatusId: number;
    /**
     * 
     * @type {{ [key: string]: string; }}
     * @memberof VnfTransitionDtoCreate
     */
    opCoResource?: { [key: string]: string; };
    /**
     * 
     * @type {number}
     * @memberof VnfTransitionDtoCreate
     */
    opCoId: number;
    /**
     * 
     * @type {{ [key: string]: RelatedResource; }}
     * @memberof VnfTransitionDtoCreate
     */
    nfviBundleIDResource?: { [key: string]: RelatedResource; };
    /**
     * 
     * @type {number}
     * @memberof VnfTransitionDtoCreate
     */
    nfviBundleIDId: number;
    /**
     * 
     * @type {Array<string>}
     * @memberof VnfTransitionDtoCreate
     */
    vnfTypeResource?: Array<string>;
    /**
     * 
     * @type {string}
     * @memberof VnfTransitionDtoCreate
     */
    vnfType: string;
    /**
     * 
     * @type {Array<string>}
     * @memberof VnfTransitionDtoCreate
     */
    nfviSiteDesignationResource?: Array<string>;
    /**
     * 
     * @type {string}
     * @memberof VnfTransitionDtoCreate
     */
    nfviSiteDesignation: string;
}


// ------------------------ Not generated  ------------------------ 


export interface VNFTransitionEdit {
    VNFTransitionDtoEdit: VNFTransitionDtoUpdate | null,
    ResultDtoEdit: ResultDto | null,
}

export interface VNFTransitionCreate {
    VNFTransitionDtoCreate: VnfTransitionDtoCreate | null,
    ResultDtoCreate: ResultDto | null,
}
export interface VNFTransitionGrid {
    VNFTransitionGridResult: QueryResultDtoOfVNFTransitionDtoGrid | null,
    filter: FilterValueDto[] | null
}
export interface VNFTransitionQueryObjectGrid extends QueryObject {
    /**
     * 
     * @type {Array<number>}
     * @memberof VnfTransitionQueryDto
     */
    vnfTransitionId?: Array<number>;
    /**
     * 
     * @type {Array<number>}
     * @memberof VnfTransitionQueryDto
     */
    vnfDesignComponent?: Array<number>;
    /**
     * 
     * @type {Array<number>}
     * @memberof VnfTransitionQueryDto
     */
    opCo?: Array<number>;
    /**
     * 
     * @type {Array<string>}
     * @memberof VnfTransitionQueryDto
     */
    vnfType?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof VnfTransitionQueryDto
     */
    currentRelease?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof VnfTransitionQueryDto
     */
    plannedRelease?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof VnfTransitionQueryDto
     */
    elementName?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof VnfTransitionQueryDto
     */
    location?: Array<string>;
    /**
     * 
     * @type {Array<number>}
     * @memberof VnfTransitionQueryDto
     */
    nfviBundleID?: Array<number>;
    /**
     * 
     * @type {Array<string>}
     * @memberof VnfTransitionQueryDto
     */
    spare1Json?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof VnfTransitionQueryDto
     */
    nfviSiteDesignation?: Array<string>;
    /**
     * 
     * @type {Array<number>}
     * @memberof VnfTransitionQueryDto
     */
    equipmentStatus?: Array<number>;
    /**
     * 
     * @type {Array<string>}
     * @memberof VnfTransitionQueryDto
     */
    lastModifiedBy?: Array<string>;
}



export const GET_CREATE_VNF_TRANSITION = "GET_CREATE_VNF_TRANSITION";
export const GET_EDIT_VNF_TRANSITION = "GET_EDIT_VNF_TRANSITION";
export const GET_GRID_VNF_TRANSITION = "GET_GRID_VNF_TRANSITION";
export const GET_FILTER_VNF_TRANSITION = "GET_FILTER_VNF_TRANSITION";
export const CREATE_VNF_TRANSITION = "CREATE_VNF_TRANSITION";
export const EDIT_VNF_TRANSITION = "EDIT_VNF_TRANSITION";
export const DELETE_VNF_TRANSITION = "DELETE_VNF_TRANSITION";
export const RESTORE_VNF_TRANSITION = "RESTORE_VNF_TRANSITION";
