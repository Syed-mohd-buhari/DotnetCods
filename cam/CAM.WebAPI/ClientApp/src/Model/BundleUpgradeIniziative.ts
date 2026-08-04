import { ResultDto } from "./CommonModels";
import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObject, QueryObjectGrid } from "./Common";
/**
 * 
 * @export
 * @interface BundleUpgradeInitiativeDto
 */
 export interface BundleUpgradeInitiativeDto extends GridDtoBase {
    /**
     * 
     * @type {string}
     * @memberof BundleUpgradeInitiativeDto
     */
    vnfType: string;
    /**
     * 
     * @type {string}
     * @memberof BundleUpgradeInitiativeDto
     */
    verticalOwner: string;
    /**
     * 
     * @type {string}
     * @memberof BundleUpgradeInitiativeDto
     */
    oemCertifiedRelease?: string;
    /**
     * 
     * @type {string}
     * @memberof BundleUpgradeInitiativeDto
     */
    remarks?: string;
    /**
     * 
     * @type {string}
     * @memberof BundleUpgradeInitiativeDto
     */
    spare1Json?: string;
    /**
     * 
     * @type {Date}
     * @memberof BundleUpgradeInitiativeDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof BundleUpgradeInitiativeDto
     */
    lastModifiedBy?: string;
}

/**
 * 
 * @export
 * @interface BundleUpgradeInitiativeQueryDto
 */
 export interface BundleUpgradeInitiativeQueryDto extends QueryObject {
    /**
     * 
     * @type {Array<number>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    bundleUpgradeInitiativeId?: Array<number>;
    /**
     * 
     * @type {Array<number>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    originalEquipmentManufacturer?: Array<number>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    vnfType?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    verticalOwner?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    oemCertifiedRelease?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    remarks?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    spare1Json?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    lastModifiedBy?: Array<string>;
}

/**
 * 
 * @export
 * @interface BundleUpgradeInitiativeDtoCreate
 */
export interface BundleUpgradeInitiativeDtoCreate extends BundleUpgradeInitiativeDto {
    /**
     * 
     * @type {{ [key: string]: string; }}
     * @memberof BundleUpgradeInitiativeDtoCreate
     */
    originalEquipmentManufacturerResource?: { [key: string]: string; };
    /**
     * 
     * @type {number}
     * @memberof BundleUpgradeInitiativeDtoCreate
     */
    originalEquipmentManufacturerId: number;
}
/**
 * 
 * @export
 * @interface BundleUpgradeInitiativeDtoGrid
 */
export interface BundleUpgradeInitiativeDtoGrid extends BundleUpgradeInitiativeDto {
    /**
     * 
     * @type {number}
     * @memberof BundleUpgradeInitiativeDtoGrid
     */
    bundleUpgradeInitiativeId?: number;
    /**
     * 
     * @type {string}
     * @memberof BundleUpgradeInitiativeDtoGrid
     */
    originalEquipmentManufacturer?: string;
}
/**
 * 
 * @export
 * @interface BundleUpgradeInitiativeDtoUpdate
 */
export interface BundleUpgradeInitiativeDtoUpdate extends BundleUpgradeInitiativeDtoCreate {
    /**
     * 
     * @type {number}
     * @memberof BundleUpgradeInitiativeDtoUpdate
     */
    bundleUpgradeInitiativeId?: number;
}
/**
 * 
 * @export
 * @interface QueryResultDtoOfBundleUpgradeInitiativeDto
 */
export interface QueryResultDtoOfBundleUpgradeInitiativeDtoGrid {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfBundleUpgradeInitiativeDto
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<BundleUpgradeInitiativeDto>}
     * @memberof QueryResultDtoOfBundleUpgradeInitiativeDto
     */
    items?: Array<BundleUpgradeInitiativeDto>;
    /**
     * 
     * @type {CustomGridRenderOfBundleUpgradeInitiativeDto}
     * @memberof QueryResultDtoOfBundleUpgradeInitiativeDto
     */
    gridRender?: CustomGridRender;
}




// ------------------------ Not generated  ------------------------ 


export interface BundleUpgradeInitiativeEdit {
    BundleUpgradeInitiativeDtoEdit: BundleUpgradeInitiativeDtoUpdate | null,
    ResultDtoEdit: ResultDto | null,
}

export interface BundleUpgradeInitiativeCreate {
    BundleUpgradeInitiativeDtoCreate: BundleUpgradeInitiativeDtoCreate | null,
    ResultDtoCreate: ResultDto | null,
}
export interface BundleUpgradeInitiativeGrid {
    BundleUpgradeInitiativeGridResult: QueryResultDtoOfBundleUpgradeInitiativeDtoGrid | null,
    filter: FilterValueDto[] | null
}
export interface BundleUpgradeInitiativeQueryObjectGrid extends QueryObject {
    /**
     * 
     * @type {Array<number>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    bundleUpgradeInitiativeId?: Array<number>;
    /**
     * 
     * @type {Array<number>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    originalEquipmentManufacturer?: Array<number>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    vnfType?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    verticalOwner?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    oemCertifiedRelease?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    remarks?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    spare1Json?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof BundleUpgradeInitiativeQueryDto
     */
    lastModifiedBy?: Array<string>;
}



export const GET_CREATE_BUNDLE_UPGRADE_INIZIATIVE = "GET_CREATE_BUNDLE_UPGRADE_INIZIATIVE";
export const GET_EDIT_BUNDLE_UPGRADE_INIZIATIVE = "GET_EDIT_BUNDLE_UPGRADE_INIZIATIVE";
export const GET_GRID_BUNDLE_UPGRADE_INIZIATIVE = "GET_GRID_BUNDLE_UPGRADE_INIZIATIVE";
export const GET_FILTER_BUNDLE_UPGRADE_INIZIATIVE = "GET_FILTER_BUNDLE_UPGRADE_INIZIATIVE";
export const CREATE_BUNDLE_UPGRADE_INIZIATIVE = "CREATE_BUNDLE_UPGRADE_INIZIATIVE";
export const EDIT_BUNDLE_UPGRADE_INIZIATIVE = "EDIT_BUNDLE_UPGRADE_INIZIATIVE";
export const DELETE_BUNDLE_UPGRADE_INIZIATIVE = "DELETE_BUNDLE_UPGRADE_INIZIATIVE";
export const RESTORE_BUNDLE_UPGRADE_INIZIATIVE = "RESTORE_BUNDLE_UPGRADE_INIZIATIVE";
