/**
 * 
 * @export
 * @interface OperatingSystemDto
 */
export interface OperatingSystemDto {
    /**
     * 
     * @type {number}
     * @memberof OperatingSystemDto
     */
    operatingSystemId?: number;
    /**
     * 
     * @type {string}
     * @memberof OperatingSystemDto
     */
    operatingSystemName?: string;
}

/**
 * 
 * @export
 * @interface QueryResultDtoOfOperatingSystemDto
 */
export interface QueryResultDtoOfOperatingSystemDto {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfOperatingSystemDto
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<OperatingSystemDto>}
     * @memberof QueryResultDtoOfOperatingSystemDto
     */
    items?: Array<OperatingSystemDto>;
}


export const GET_CREATE_OPERATING_SYSTEM = "GET_CREATE_OPERATING_SYSTEM";
export const GET_EDIT_MAJOR_ = "GET_EDIT_OPERATING_SYSTEM";
export const GET_GRID_OPERATING_SYSTEM = "GET_GRID_OPERATING_SYSTEM";
export const GET_FILTER_OPERATING_SYSTEM = "GET_FILTER_OPERATING_SYSTEM";
export const CREATE_OPERATING_SYSTEM = "CREATE_OPERATING_SYSTEM";
export const EDIT_OPERATING_SYSTEM = "EDIT_OPERATING_SYSTEM";
export const DELETE_OPERATING_SYSTEM = "DELETE_OPERATING_SYSTEM";