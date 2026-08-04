import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { ResultDto } from "./CommonModels";
import { GridDtoBase } from './Common';


/**
 * 
 * @export
 * @interface QueryResultDtoOfThirdPartyHardwareComponentDtoGrid
 */
export interface QueryResultDtoOfThirdPartyHardwareComponentDtoGrid {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfThirdPartyHardwareComponentDtoGrid
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<ThirdPartyHardwareComponentDtoGrid>}
     * @memberof QueryResultDtoOfThirdPartyHardwareComponentDtoGrid
     */
    items?: Array<ThirdPartyHardwareComponentDtoGrid>;
}

/**
 * 
 * @export
 * @interface ThirdPartyHardwareComponentDto
 */
export interface ThirdPartyHardwareComponentDto extends GridDtoBase {
    /**
     * 
     * @type {number}
     * @memberof ThirdPartyHardwareComponentDto
     */
    thirdPartyHardwareComponentId?: number;
    /**
     * 
     * @type {string}
     * @memberof ThirdPartyHardwareComponentDto
     */
    hardwareComponentType?: string;
    /**
     * 
     * @type {string}
     * @memberof ThirdPartyHardwareComponentDto
     */
    hardwareComponent?: string;
    /**
     * 
     * @type {string}
     * @memberof ThirdPartyHardwareComponentDto
     */
    hardwareReleaseInformation?: string;
    /**
     * 
     * @type {number}
     * @memberof ThirdPartyHardwareComponentDto
     */
    vulnerabilityStatusId?: number;
    /**
     * 
     * @type {Date}
     * @memberof ThirdPartyHardwareComponentDto
     */
    endOfSale?: Date;
    /**
     * 
     * @type {Date}
     * @memberof ThirdPartyHardwareComponentDto
     */
    endOfSupport?: Date;
    /**
     * 
     * @type {number}
     * @memberof ThirdPartyHardwareComponentDto
     */
    originalEquipmentManufacturerId?: number;
    /**
     * 
     * @type {string}
     * @memberof ThirdPartyHardwareComponentDto
     */
    hardwareSolution?: string;
    /**
     * 
     * @type {string}
     * @memberof ThirdPartyHardwareComponentDto
     */
    hardwareType?: string;
    /**
     * 
     * @type {Date}
     * @memberof ThirdPartyHardwareComponentDto
     */
    lastModified?: Date;
}
/**
 * 
 * @export
 * @interface ThirdPartyHardwareComponentDtoCreate
 */
export interface ThirdPartyHardwareComponentDtoCreate extends ThirdPartyHardwareComponentDto {
    /**
     * 
     * @type {{ [key: string]: string; }}
     * @memberof ThirdPartyHardwareComponentDtoCreate
     */
    originalEquipmentManufacturerResource?: { [key: string]: string; };
    /**
     * 
     * @type {{ [key: string]: string; }}
     * @memberof ThirdPartyHardwareComponentDtoCreate
     */
    vulnerabilityStatusResource?: { [key: string]: string; };
}
/**
 * 
 * @export
 * @interface ThirdPartyHardwareComponentDtoGrid
 */
export interface ThirdPartyHardwareComponentDtoGrid extends ThirdPartyHardwareComponentDto {
    /**
     * 
     * @type {string}
     * @memberof ThirdPartyHardwareComponentDtoGrid
     */
    vulnerabilityStatus?: string;
    /**
     * 
     * @type {string}
     * @memberof ThirdPartyHardwareComponentDtoGrid
     */
    originalEquipmentManufacturer?: string;
}

export interface ThirdPartyHardwareComponentQueryObjectGrid {
    propertyName?: string,
    propertyFilter?: string,
    hardwareComponentType?: Array<string>,
    hardwareComponent?: Array<string>,
    hardwareReleaseInformation?: Array<string>,
    vulnerabilityStatusId?: Array<number>,
    majorHardwareBuildId?: Array<number>,
    endOfSaleStartDate?: Date,
    endOfSaleEndDate?: Date,
    endOfSupportStartDate?: Date,
    endOfSupportEndDate?: Date,
    originalEquipmentManufacturerId?: Array<number>,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    principalId?: number
}


/**
 * 
 * @export
 * @interface ThirdPartyHardwareComponentDtoUpdate
 */
export interface ThirdPartyHardwareComponentDtoUpdate extends ThirdPartyHardwareComponentDtoCreate {
    /**
     * 
     * @type {number}
     * @memberof ThirdPartyHardwareComponentDtoUpdate
     */
    thirdPartyHardwareComponentId?: number;
}
export interface ThirdPartyHardwareComponentEdit {
    ThirdPartyHardwareComponentDtoEdit: ThirdPartyHardwareComponentDtoUpdate | null,
    ResultDtoEdit: ResultDto | null,
}

export interface ThirdPartyHardwareComponentCreate {
    ThirdPartyHardwareComponentDtoCreate: ThirdPartyHardwareComponentDtoCreate | null,
    ResultDtoCreate: ResultDto | null,
}
export interface ThirdPartyHardwareComponentGrid {
    ThirdPartyHardwareComponentGridResult: QueryResultDtoOfThirdPartyHardwareComponentDtoGrid | null,
    filter: FilterValueDto[] | null
}

export const GET_CREATE_THIRD_PARTY_HARDWARE_COMPONENT = "GET_CREATE_THIRD_PARTY_HARDWARE_COMPONENT";
export const GET_EDIT_THIRD_PARTY_HARDWARE_COMPONENT = "GET_EDIT_THIRD_PARTY_HARDWARE_COMPONENT";
export const GET_GRID_THIRD_PARTY_HARDWARE_COMPONENT = "GET_GRID_THIRD_PARTY_HARDWARE_COMPONENT";
export const GET_FILTER_THIRD_PARTY_HARDWARE_COMPONENT = "GET_FILTER_THIRD_PARTY_HARDWARE_COMPONENT";
export const CREATE_THIRD_PARTY_HARDWARE_COMPONENT = "CREATE_THIRD_PARTY_HARDWARE_COMPONENT";
export const EDIT_THIRD_PARTY_HARDWARE_COMPONENT = "EDIT_THIRD_PARTY_HARDWARE_COMPONENT";
export const DELETE_THIRD_PARTY_HARDWARE_COMPONENT = "DELETE_THIRD_PARTY_HARDWARE_COMPONENT";
export const RESTORE_THIRD_PARTY_HARDWARE_COMPONENT = "RESTORE_THIRD_PARTY_HARDWARE_COMPONENT";
export const GET_THIRD_PARTY_HARDWARE_COMPONENT_NAME = "GET_THIRD_PARTY_HARDWARE_COMPONENT_NAME";
export const GET_THIRD_PARTY_HARDWARE_COMPONENT_CONSTRAINT_INFO = "GET_THIRD_PARTY_HARDWARE_COMPONENT_CONSTRAINT_INFO";

