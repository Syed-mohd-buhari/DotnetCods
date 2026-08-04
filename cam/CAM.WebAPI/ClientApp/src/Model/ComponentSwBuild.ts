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
 * @interface ComponentSwBuildDto
 */
export interface ComponentSwBuildDto extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildDto
   */
  designContact: string;
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildDto
   */
  softwareVersion: string;
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildDto
   */
  productName: string;
  /**
   *
   * @type {Date}
   * @memberof ComponentSwBuildDto
   */
  lastTimeBuyNew?: Date;
  /**
   *
   * @type {Date}
   * @memberof ComponentSwBuildDto
   */
  lastTimeBuyUpgrades?: Date;
  /**
   *
   * @type {Date}
   * @memberof ComponentSwBuildDto
   */
  lastTimeBuyExpansions?: Date;
  /**
   *
   * @type {Date}
   * @memberof ComponentSwBuildDto
   */
  endOfMaintenance: Date | null;
  /**
   *
   * @type {Date}
   * @memberof ComponentSwBuildDto
   */
  endOfsupport?: Date | null;
  /**
   *
   * @type {Date}
   * @memberof ComponentSwBuildDto
   */
  generaAvailableDate: Date;
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildDto
   */
  deliveryMethod?: string;
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildDto
   */
  spareFieldsJSON?: string;
  /**
   *
   * @type {Date}
   * @memberof ComponentSwBuildDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface ComponentSwBuildDtoCreate
 */
export interface ComponentSwBuildDtoCreate extends ComponentSwBuildDto {
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof ComponentSwBuildDtoCreate
   */
  originalEquipmentManufacturerResource?: {
    key: number;
    text: string;
  }[];

  componentManufacturerResource?: {
    key: number;
    text: string;
  }[];

  /**
   *
   * @type {number}
   * @memberof ComponentSwBuildDtoCreate
   */
  originalEquipmentManufacturerId: number;
  componentManufacturerId:number;
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof ComponentSwBuildDtoCreate
   */
  criticalAssetTypeResource?: { key: number; text: string }[];
  /**
   *
   * @type {number}
   * @memberof ComponentSwBuildDtoCreate
   */
  criticalAssetTypeId?: number;
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildDtoCreate
   */
  designContactIds?: Array<number>;
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof ComponentSwBuildDtoCreate
   */
  designContacts?: { key: number; text: string }[];
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof ComponentSwBuildDtoCreate
   */
  productNamesResource?: { key: number; text: string }[];
  /**
   *
   * @type {number}
   * @memberof ComponentSwBuildDtoCreate
   */
  productNameId?: number;
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildDtoCreate
   */
  vulnerabilityStatus?: string;
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildDtoCreate
   */
  thirdPartySoftwareComponentsId?: Array<number>;
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof ComponentSwBuildDtoCreate
   */
  operatingSystemResource?: { key: number; text: string }[];

  /**
   *
   * @type {number}
   * @memberof ComponentSwBuildDtoCreate
   */
  operatingSystemId?: number;

  /**
   *
   * @type {EOM_STATUS}
   * @memberof ComponentSwBuildDtoCreate
   */
  eomStatus?: EOM_STATUS;

  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildDtoCreate
   */
  description?: string;
}
/**
 *
 * @export
 * @interface ComponentSwBuildDtoGrid
 */
export interface ComponentSwBuildDtoGrid extends ComponentSwBuildDto {
  /**
   *
   * @type {number}
   * @memberof ComponentSwBuildDtoGrid
   */
  componentSoftwareBuildId?: number;
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildDtoGrid
   */
  originalEquipmentManufacturer?: string;
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildDtoGrid
   */
  vulnerabilityStatus?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ComponentSwBuildDtoGrid
   */
  thirdPartySoftwareComponents?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildDtoGrid
   */
  operatingSystem?: string;
}
/**
 *
 * @export
 * @interface ComponentSwBuildDtoUpdate
 */
export interface ComponentSwBuildDtoUpdate extends ComponentSwBuildDtoCreate {
  /**
   *
   * @type {number}
   * @memberof ComponentSwBuildDtoUpdate
   */
  componentSoftwareBuildId?: number;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfComponentSwBuildDtoGrid
 */
export interface QueryResultDtoOfComponentSwBuildDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfComponentSwBuildDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<ComponentSwBuildDtoGrid>}
   * @memberof QueryResultDtoOfComponentSwBuildDtoGrid
   */
  items?: Array<ComponentSwBuildDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @export
 * @interface ResultDtoOfComponentSwBuildToCloneDto
 */
export interface ResultDtoOfComponentSwBuildToCloneDto {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfComponentSwBuildToCloneDto
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfComponentSwBuildToCloneDto
   */
  info?: string;
  /**
   *
   * @type {ComponentSwBuildToCloneDto}
   * @memberof ResultDtoOfComponentSwBuildToCloneDto
   */
  data?: ComponentSwBuildToCloneDto;
}
/**
 *
 * @export
 * @interface ComponentSwBuildToCloneDto
 */
export interface ComponentSwBuildToCloneDto {
  /**
   *
   * @type {number}
   * @memberof ComponentSwBuildToCloneDto
   */
  componentSoftwareBuildId?: number;
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildToCloneDto
   */
  originalEquipmentManufacturer?: string;
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildToCloneDto
   */
  productName: string;
  /**
   *
   * @type {string}
   * @memberof ComponentSwBuildToCloneDto
   */
  softwareVersion?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ComponentSwBuildToCloneDto
   */
  designComponentResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ComponentSwBuildToCloneDto
   */
  tcpBundleVersion?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildToCloneDto
   */
  tcpSoftwareCompatibilityIdList: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ComponentSwBuildToCloneDto
   */
  tciBundleVersion?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildToCloneDto
   */
  tciSoftwareCompatibilityIdList: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildToCloneDto
   */
  designContactIds: Array<number>;
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof ComponentSwBuildToCloneDto
   */
  designContacts: { key: number; text: string }[];
  opCoId?:number,
  designComponentFamilyId?:number,
}

/**
 *
 * @export
 * @interface CloneComponentSwBuildDto
 */
export interface CloneComponentSwBuildDto {
  /**
   *
   * @type {number}
   * @memberof CloneComponentSwBuildDto
   */
  componentSoftwareBuildId?: number;
  /**
   *
   * @type {string}
   * @memberof CloneComponentSwBuildDto
   */
  softwareVersion?: string;
  /**
   *
   * @type {Date}
   * @memberof CloneComponentSwBuildDto
   */
  endOfMaintenance?: Date | null;

  endOfsupport?: Date | null;

  eomStatus: EOM_STATUS;
  originalEquipmentManufacturer?: string;
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildDtoCreate
   */
  designContactIds?: Array<number>;
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof ComponentSwBuildDtoCreate
   */
  designContacts?: { key: number; text: string }[];
}

// ------------------------ Not generated  ------------------------

export interface ComponentSwBuildEdit {
  ComponentSwBuildDtoEdit: ComponentSwBuildDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface ComponentSwBuildCreate {
  ComponentSwBuildDtoCreate: ComponentSwBuildDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface ComponentSwBuildGrid {
  ComponentSwBuildGridResult: QueryResultDtoOfComponentSwBuildDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface ComponentSwBuildQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildQueryDto
   */
  originalEquipmentManufacturer?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildQueryDto
   */
  productName?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildQueryDto
   */
  componentSoftwareBuild?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildQueryDto
   */
  criticalAssetTypeId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ComponentSwBuildQueryDto
   */
  description?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ComponentSwBuildQueryDto
   */
  designContact?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ComponentSwBuildQueryDto
   */
  softwareVersion?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof ComponentSwBuildQueryDto
   */
  lastTimeBuyNew?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof ComponentSwBuildQueryDto
   */
  lastTimeBuyUpgrades?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof ComponentSwBuildQueryDto
   */
  lastTimeBuyExpansions?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof ComponentSwBuildQueryDto
   */
  endOfMaintenanceValue?: DateFilter | null;
  /**
   *
   * @type {DateFilter}
   * @memberof ComponentSwBuildQueryDto
   */
  endOfsupportValue?: DateFilter | null;
  /**
   *
   * @type {DateFilter}
   * @memberof ComponentSwBuildQueryDto
   */
  generaAvailableDateValue?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof ComponentSwBuildQueryDto
   */
  deliveryMethod?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof ComponentSwBuildQueryDto
   */
  vulnerabilityStatus?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildQueryDto
   */
  operatingSystem?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ComponentSwBuildQueryDto
   */
  spareFieldsJson?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof ComponentSwBuildQueryDto
   */
  networkFunction?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof ComponentSwBuildQueryDto
   */
  lastModifiedBy?: Array<string>;
}

export const GET_CREATE_COMPONENT_SW_BUILD = "GET_CREATE_COMPONENT_SW_BUILD";
export const GET_EDIT_COMPONENT_SW_BUILD = "GET_EDIT_COMPONENT_SW_BUILD";
export const GET_GRID_COMPONENT_SW_BUILD = "GET_GRID_COMPONENT_SW_BUILD";
export const GET_FILTER_COMPONENT_SW_BUILD = "GET_FILTER_COMPONENT_SW_BUILD";
export const CREATE_COMPONENT_SW_BUILD = "CREATE_COMPONENT_SW_BUILD";
export const EDIT_COMPONENT_SW_BUILD = "EDIT_COMPONENT_SW_BUILD";
export const DELETE_COMPONENT_SW_BUILD = "DELETE_COMPONENT_SW_BUILD";
export const RESTORE_COMPONENT_SW_BUILD = "RESTORE_COMPONENT_SW_BUILD";
