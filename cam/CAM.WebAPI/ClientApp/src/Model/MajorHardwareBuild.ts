import { ResultDto } from "./CommonModels";
import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  DateFilter,
  EOM_STATUS,
  GridDtoBase,
  QueryObject,
  QueryObjectGrid,
} from "./Common";
/**
 *
 * @export
 * @interface MajorHardwareBuildDto
 */
export interface MajorHardwareBuildDto extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildDto
   */
  hardwareSolution?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildDto
   */
  otherHardwareInfo?: string;
  /**
   *
   * @type {Date}
   * @memberof MajorHardwareBuildDto
   */
  lastTimeBuyNew?: Date;
  /**
   *
   * @type {Date}
   * @memberof MajorHardwareBuildDto
   */
  lastTimeBuyUpgrades?: Date;
  /**
   *
   * @type {Date}
   * @memberof MajorHardwareBuildDto
   */
  lastTimeBuyExpansions?: Date;
  /**
   *
   * @type {Date}
   * @memberof MajorHardwareBuildDto
   */
  endOfMaintenance: Date | null;

  /**
   *
   * @type {Date}
   * @memberof MajorHardwareBuildDto
   */
  generaAvailableDate?: Date | null;
  /**
   *
   * @type {Date}
   * @memberof MajorHardwareBuildDto
   */
  endOfsupport?: Date;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildDto
   */
  vulnerabilityStatus?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildDto
   */
  spareFieldsJson?: string;
  /**
   *
   * @type {boolean}
   * @memberof MajorHardwareBuildDto
   */
  proprietaryHardware: boolean;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildDto
   */
  description: string;
  /**
   *
   * @type {Date}
   * @memberof MajorHardwareBuildDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface MajorHardwareBuildDtoCreate
 */
export interface MajorHardwareBuildDtoCreate extends MajorHardwareBuildDto {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorHardwareBuildDtoCreate
   */
  originalEquipmentManufacturerResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof MajorHardwareBuildDtoCreate
   */
  originalEquipmentManufacturerId: number;
  /**
   *
   * @type {Array<number>}
   * @memberof MajorHardwareBuildDtoCreate
   */
  thirdPartyHardwareComponentsId?: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorHardwareBuildDtoCreate
   */
  hardwareSolutionReource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof MajorHardwareBuildDtoCreate
   */
  hardwareSolutionReourceId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorHardwareBuildDtoCreate
   */
  buildConstructionResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof MajorHardwareBuildDtoCreate
   */
  buildConstructionId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorHardwareBuildDtoCreate
   */
  platformResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof MajorHardwareBuildDtoCreate
   */
  platformId: number;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildDtoCreate
   */
  hardwareType: string;
  eomStatus?: EOM_STATUS;

  /**
   *
   * @type {Array<number>}
   * @memberof MajorHardwareBuildDtoCreate
   */
  designContactIds?: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorHardwareBuildDtoCreate
   */
  designContacts?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface MajorHardwareBuildDtoGrid
 */
export interface MajorHardwareBuildDtoGrid extends MajorHardwareBuildDto {
  /**
   *
   * @type {number}
   * @memberof MajorHardwareBuildDtoGrid
   */
  majorHardwareBuildId?: number;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildDtoGrid
   */
  originalEquipmentManufacturer?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorHardwareBuildDtoGrid
   */
  thirdPartyHardwareComponents?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildDtoGrid
   */
  buildConstruction?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildDtoGrid
   */
  hardwareType?: string;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildDtoGrid
   */
  platform?: string;
}
/**
 *
 * @export
 * @interface MajorHardwareBuildDtoUpdate
 */
export interface MajorHardwareBuildDtoUpdate
  extends MajorHardwareBuildDtoCreate {
  /**
   *
   * @type {number}
   * @memberof MajorHardwareBuildDtoUpdate
   */
  majorHardwareId?: number;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfMajorHardwareBuildDtoGrid
 */
export interface QueryResultDtoOfMajorHardwareBuildDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfMajorHardwareBuildDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<MajorHardwareBuildDtoGrid>}
   * @memberof QueryResultDtoOfMajorHardwareBuildDtoGrid
   */
  items?: Array<MajorHardwareBuildDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface MajorHardwareBuildGrid {
  MajorHardwareBuildGridResult: QueryResultDtoOfMajorHardwareBuildDtoGrid | null;
  filter: FilterValueDto[] | null;
}
export interface MajorHardwareBuildQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof MajorHardwareBuildQueryDto
   */
  majorHardwareBuildId?: Array<number>;
  /**
   *
   * @type {string}
   * @memberof MajorHardwareBuildQueryDto
   */
  name?: string;
  /**
   *
   * @type {Array<number>}
   * @memberof MajorHardwareBuildQueryDto
   */
  originalEquipmentManufacturer?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorHardwareBuildQueryDto
   */
  hardwareSolution?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof MajorHardwareBuildQueryDto
   */
  platform?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorHardwareBuildQueryDto
   */
  hardwareType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorHardwareBuildQueryDto
   */
  otherHardwareInfo?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof MajorHardwareBuildQueryDto
   */
  lastTimeBuyNew?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof MajorHardwareBuildQueryDto
   */
  lastTimeBuyUpgrades?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof MajorHardwareBuildQueryDto
   */
  lastTimeBuyExpansions?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof MajorHardwareBuildQueryDto
   */
  endOfMaintenance?: DateFilter | null;

  /**
   *
   * @type {DateFilter}
   * @memberof MajorHardwareBuildQueryDto
   */
  endOfsupport?: DateFilter;

  /**
   *
   * @type {DateFilter}
   * @memberof MajorHardwareBuildQueryDto
   */
  generaAvailableDate?: DateFilter | null;

  /**
   *
   * @type {Array<string>}
   * @memberof MajorHardwareBuildQueryDto
   */
  vulnerabilityStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorHardwareBuildQueryDto
   */
  spareFieldsJson?: Array<string>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof MajorHardwareBuildQueryDto
   */
  proprietaryHardware?: Array<boolean>;
  /**
   *
   * @type {Array<number>}
   * @memberof MajorHardwareBuildQueryDto
   */
  principalIdList?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorHardwareBuildQueryDto
   */
  buildConstruction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorHardwareBuildQueryDto
   */
  lastModifiedBy?: Array<string>;
}

export interface MajorHardwareBuildEdit {
  MajorHardwareBuildDtoEdit: MajorHardwareBuildDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface MajorHardwareBuildCreate {
  MajorHardwareBuildDtoCreate: MajorHardwareBuildDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface MajorHardwareBuildGrid {
  MajorHardwareBuildGridResult: QueryResultDtoOfMajorHardwareBuildDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_MAJOR_HARDWARE_BUILD =
  "GET_CREATE_MAJOR_HARDWARE_BUILD";
export const GET_EDIT_MAJOR_HARDWARE_BUILD = "GET_EDIT_MAJOR_HARDWARE_BUILD";
export const GET_GRID_MAJOR_HARDWARE_BUILD = "GET_GRID_MAJOR_HARDWARE_BUILD";
export const GET_FILTER_MAJOR_HARDWARE_BUILD =
  "GET_FILTER_MAJOR_HARDWARE_BUILD";
export const CREATE_MAJOR_HARDWARE_BUILD = "CREATE_MAJOR_HARDWARE_BUILD";
export const EDIT_MAJOR_HARDWARE_BUILD = "EDIT_MAJOR_HARDWARE_BUILD";
export const RESTORE_MAJOR_HARDWARE_BUILD = "RESTORE_MAJOR_HARDWARE_BUILD";
export const DELETE_MAJOR_HARDWARE_BUILD = "DELETE_MAJOR_HARDWARE_BUILD";
// export const GET_RELATED_RECORDS_MAJOR_HARDWARE_BUILD = "GET_RELATED_RECORDS_MAJOR_HARDWARE_BUILD";
