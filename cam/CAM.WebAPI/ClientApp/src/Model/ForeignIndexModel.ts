import { RelatedResource, ResultDto } from "./CommonModels";
import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObjectGrid } from "./Common";
import { PlannedActivityDtoGrid } from "./PlannedActivity";

/**
 * 
 * @export
 * @interface ForeignIndexDto
 */
export interface ForeignIndexDto {
    /**
     * 
     * @type {FISessionDto}
     * @memberof ForeignIndexDto
     */
    session?: FISessionDto;
    /**
     * 
     * @type {Array<FIDesignComponentDto>}
     * @memberof ForeignIndexDto
     */
    designComponents?: Array<FIDesignComponentDto>;
    /**
     * 
     * @type {Array<FISystemTypesDto>}
     * @memberof ForeignIndexDto
     */
    systemTypes?: Array<FISystemTypesDto>;
    /**
     * 
     * @type {Array<FIGroupKey>}
     * @memberof ForeignIndexDto
     */
    groups?: Array<FIGroupKey>;
    /**
     * 
     * @type {Array<FIGroupRecord>}
     * @memberof ForeignIndexDto
     */
    groupToUpdate?: Array<FIGroupRecord>;

    /**
 * 
 * @type {Array<ImpactCheckDto>}
 * @memberof ForeignIndexDto
 */
    impactChecks?: Array<ImpactCheckDto>;
}

export interface PlannedActivityImpactResult {
    opco?: string,
    designComponent?: string,
    numberOfNodes?: number,
    numberOfNodesInLab?: number,
    productImportance?: string,
    plannedAction?: string,
    activityIndex?: string,
    eduspoc?: string,
    subDomainSpoc?: string
}

/**
 * 
 * @export
 * @interface FIDesignComponentDto
 */
export interface FIDesignComponentDto {
    /**
     * 
     * @type {number}
     * @memberof FIDesignComponentDto
     */
    fIDesignComponentId?: number;
    /**
     * 
     * @type {number}
     * @memberof FIDesignComponentDto
     */
    sessionId?: number;
    /**
     * 
     * @type {number}
     * @memberof FIDesignComponentDto
     */
    designComponentId?: number;
    /**
     * 
     * @type {string}
     * @memberof FIDesignComponentDto
     */
    description?: string;
    /**
     * 
     * @type {boolean}
     * @memberof FIDesignComponentDto
     */
    toDelete?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof FIDesignComponentDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof FIDesignComponentDto
     */
    lastModifiedBy?: string;
}
/**
 * 
 * @export
 * @interface FIGroupKey
 */
export interface FIGroupKey {
    /**
     * 
     * @type {number}
     * @memberof FIGroupKey
     */
    oem?: number;
    /**
     * 
     * @type {Date}
     * @memberof FIGroupKey
     */
    eom?: Date;
    /**
     * 
     * @type {{ [key: string]: FIGroupRecord; }}
     * @memberof FIGroupKey
     */
    records?: { [key: string]: FIGroupRecord; };
    /**
     * 
     * @type {Array<{ [key: string]: FIGroupRecord; }>}
     * @memberof FIGroupKey
     */
    similarityGroups?: Array<{ [key: string]: FIGroupRecord; }>;
    /**
     * 
     * @type {Array<{ [key: string]: FIGroupRecord; }>}
     * @memberof FIGroupKey
     */
    charDiffGroups?: Array<{ [key: string]: FIGroupRecord; }>;
    /**
     * 
     * @type {{ [key: string]: string; }}
     * @memberof FIGroupKey
     */
    dropDownResource?: { [key: string]: string; };
}
/**
 * 
 * @export
 * @interface FIGroupRecord
 */
export interface FIGroupRecord {
    /**
     * 
     * @type {number}
     * @memberof FIGroupRecord
     */
    key?: number;
    /**
     * 
     * @type {string}
     * @memberof FIGroupRecord
     */
    description?: string;
    /**
     * 
     * @type {boolean}
     * @memberof FIGroupRecord
     */
    selected?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof FIGroupRecord
     */
    correct?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof FIGroupRecord
     */
    modificationDate?: Date;
    /**
     * 
     * @type {number}
     * @memberof FIGroupRecord
     */
    count?: number;

    fromResource?: boolean;
    fromCharDiff?: boolean;
}

/**
 * 
 * @export
 * @interface ImpactCheckDto
 */
 export interface ImpactCheckDto {
    /**
     * 
     * @type {string}
     * @memberof ImpactCheckDto
     */
    key?: string;
    /**
     * 
     * @type {Array<string>}
     * @memberof ImpactCheckDto
     */
    lcmName?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ImpactCheckDto
     */
    lcmNameNew?: Array<string>;
    /**
     * 
     * @type {string}
     * @memberof ImpactCheckDto
     */
    opco?: string;
    /**
     * 
     * @type {boolean}
     * @memberof ImpactCheckDto
     */
    correct?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof ImpactCheckDto
     */
    notToShow?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof ImpactCheckDto
     */
    toDelete?: boolean;
    /**
     * 
     * @type {Array<PlannedActivityImpactResult>}
     * @memberof ImpactCheckDto
     */
    plannedActivity?: Array<PlannedActivityImpactResult>;
    /**
     * 
     * @type {string}
     * @memberof ImpactCheckDto
     */
    type?: string;
    /**
     * 
     * @type {SystemTypeChecker}
     * @memberof ImpactCheckDto
     */
    systemTypeChecker?: SystemTypeChecker;
    /**
     * 
     * @type {number}
     * @memberof ImpactCheckDto
     */
    designComponentId?: number;
    /**
     * 
     * @type {number}
     * @memberof ImpactCheckDto
     */
    lcmEngeeneringId?: number;
    /**
     * 
     * @type {number}
     * @memberof ImpactCheckDto
     */
    networkElementId?: number;
}

/**
 * 
 * @export
 * @interface SystemTypeChecker
 */
 export interface SystemTypeChecker {
    /**
     * 
     * @type {number}
     * @memberof SystemTypeChecker
     */
    majorHardwareId?: number;
    /**
     * 
     * @type {number}
     * @memberof SystemTypeChecker
     */
    majorSoftwareId?: number;
    /**
     * 
     * @type {string}
     * @memberof SystemTypeChecker
     */
    systemTypeNameOem?: string;
}
/**
 * 
 * @export
 * @interface FISessionDto
 */
export interface FISessionDto {
    /**
     * 
     * @type {number}
     * @memberof FISessionDto
     */
    sessionId?: number;
    /**
     * 
     * @type {ForeignIndexStatus}
     * @memberof FISessionDto
     */
    status?: ForeignIndexStatus;
    /**
     * 
     * @type {ForeignIndexSource}
     * @memberof FISessionDto
     */
    source?: ForeignIndexSource;
    /**
     * 
     * @type {Date}
     * @memberof FISessionDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof FISessionDto
     */
    lastModifiedBy?: string;
}
/**
 * 
 * @export
 * @interface FISystemTypesDto
 */
export interface FISystemTypesDto {
    /**
     * 
     * @type {number}
     * @memberof FISystemTypesDto
     */
    fISystemTypesId?: number;
    /**
     * 
     * @type {number}
     * @memberof FISystemTypesDto
     */
    sessionId?: number;
    /**
     * 
     * @type {number}
     * @memberof FISystemTypesDto
     */
    designComponentId?: number;
    /**
     * 
     * @type {number}
     * @memberof FISystemTypesDto
     */
    systemTypesId?: number;
    /**
     * 
     * @type {string}
     * @memberof FISystemTypesDto
     */
    description?: string;
    /**
     * 
     * @type {boolean}
     * @memberof FISystemTypesDto
     */
    toDelete?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof FISystemTypesDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof FISystemTypesDto
     */
    lastModifiedBy?: string;
}

/**
 * 
 * @export
 * @enum {string}
 */
export enum ForeignIndexSource {
    NUMBER_0 = <any>0,
    NUMBER_1 = <any>1,
    NUMBER_2 = <any>2,
    NUMBER_3 = <any>3
}

/**
 * 
 * @export
 * @enum {string}
 */
export enum ForeignIndexStatus {
    NUMBER_0 = <any>0,
    NUMBER_1 = <any>1,
    NUMBER_2 = <any>2,
    NUMBER_3 = <any>3,
    NUMBER_4 = <any>4,
    NUMBER_5 = <any>5
}

/**
 * 
 * @export
 * @interface ResultDtoOfForeignIndexDto
 */
export interface ResultDtoOfForeignIndexDto {
    /**
     * 
     * @type {boolean}
     * @memberof ResultDtoOfForeignIndexDto
     */
    warning?: boolean;
    /**
     * 
     * @type {string}
     * @memberof ResultDtoOfForeignIndexDto
     */
    info?: string;
    /**
     * 
     * @type {ForeignIndexDto}
     * @memberof ResultDtoOfForeignIndexDto
     */
    data?: ForeignIndexDto;
}



export const GET_CREATE_VNF_TRANSITION = "GET_CREATE_VNF_TRANSITION";
export const GET_EDIT_VNF_TRANSITION = "GET_EDIT_VNF_TRANSITION";
export const GET_GRID_VNF_TRANSITION = "GET_GRID_VNF_TRANSITION";
export const GET_FILTER_VNF_TRANSITION = "GET_FILTER_VNF_TRANSITION";
export const CREATE_VNF_TRANSITION = "CREATE_VNF_TRANSITION";
export const EDIT_VNF_TRANSITION = "EDIT_VNF_TRANSITION";
export const DELETE_VNF_TRANSITION = "DELETE_VNF_TRANSITION";
export const RESTORE_VNF_TRANSITION = "RESTORE_VNF_TRANSITION";
