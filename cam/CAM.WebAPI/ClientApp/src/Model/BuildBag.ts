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
 * @interface BuildBagDto
 */
export interface BuildBagDto extends GridDtoBase {
  /**
   *
   * @type {Date}
   * @memberof BuildBagDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof BuildBagDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface BuildBagDtoCreate
 */
export interface BuildBagDtoCreate extends BuildBagDto {
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof BuildBagDtoCreate
   */
  originalEquipmentManufacturerResource?: {
    key: number;
    text: string;
  }[];
  opcoResource?: {
    key: number;
    text: string;
  }[];

  dcfResource?: {
    key: number;
    text: string;
  }[];
  /**
   *
   * @type {Array<any>}
   * @memberof BuildBagDtoCreate
   */
  componentSofwareBuild?: Array<any>;
  /**
   *
   * @type {Array<number>}
   * @memberof BuildBagDtoCreate
   */
  componentSoftwareBuildId?: Array<number>;
  /**
   *
   * @type {number}
   * @memberof BuildBagDtoCreate
   */
  originalEquipmentManufacturerId?: number;
  /**
   *
   * @type {string}
   * @memberof BuildBagDtoCreate
   */
  productName?: string;
  /**
   *
   * @type {number}
   * @memberof BuildBagDtoCreate
   */
  productNameId?: number;
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof BuildBagDtoCreate
   */
  productNamesResource?: { key: number; text: string }[];

  /**
   *
   * @type {string}
   * @memberof BuildBagDtoCreate
   */
  componentBagDescription?: string;

  /**
   *
   * @type {string}
   * @memberof BuildBagDtoCreate
   */
  buildBagDescription?: string;

  /**
   *
   * @type {number}
   * @memberof BuildBagDtoCreate
   */
  lcmEngineeringId?: number;
  /**
   *
   * @type {number}
   * @memberof BuildBagDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {number}
   * @memberof BuildBagDtoCreate
   */
  designComponentFamilyId?: number;
}
/**
 *
 * @export
 * @interface BuildBagDtoGrid
 */
export interface BuildBagDtoGrid extends BuildBagDto {
  /**
   *
   * @type {number}
   * @memberof BuildBagDtoGrid
   */
  buildBagId?: number;
  /**
   *
   * @type {string}
   * @memberof BuildBagDtoGrid
   */
  buildBagDescription?: string;
  /**
   *
   * @type {string}
   * @memberof BuildBagDtoGrid
   */
  mappedComponentSoftwareBuild?: string;
  /**
   *
   * @type {string}
   * @memberof BuildBagDtoGrid
   */
  associatedWithLcm?: string;
}
/**
 *
 * @export
 * @interface BuildBagToCloneDto
 */
export interface BuildBagToCloneDto {
  /**
   *
   * @type {number}
   * @memberof BuildBagToCloneDto
   */
  componentSoftwareBuildId?: number;
  /**
   *
   * @type {string}
   * @memberof BuildBagToCloneDto
   */
  originalEquipmentManufacturer?: string;
  /**
   *
   * @type {string}
   * @memberof BuildBagToCloneDto
   */
  productName: string;
  /**
   *
   * @type {string}
   * @memberof BuildBagToCloneDto
   */
  softwareVersion?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof BuildBagToCloneDto
   */
  designComponentResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof BuildBagToCloneDto
   */
  tcpBundleVersion?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof BuildBagToCloneDto
   */
  tcpSoftwareCompatibilityIdList: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof BuildBagToCloneDto
   */
  tciBundleVersion?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof BuildBagToCloneDto
   */
  tciSoftwareCompatibilityIdList: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof BuildBagToCloneDto
   */
  designContactIds: Array<number>;
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof BuildBagToCloneDto
   */
  designContacts: { key: number; text: string }[];
}

/**
 *
 * @export
 * @interface CloneBuildBagDto
 */
export interface CloneBuildBagDto {
  /**
   *
   * @type {number}
   * @memberof CloneBuildBagDto
   */
  componentSoftwareBuildId?: number;
  /**
   *
   * @type {string}
   * @memberof CloneBuildBagDto
   */
  softwareVersion?: string;
  /**
   *
   * @type {Date}
   * @memberof CloneBuildBagDto
   */
  endOfMaintenance?: Date | null;

  endOfsupport?: Date | null;

  eomStatus: EOM_STATUS;
  originalEquipmentManufacturer?: string;
  /**
   *
   * @type {Array<number>}
   * @memberof BuildBagDtoCreate
   */
  designContactIds?: Array<number>;
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof BuildBagDtoCreate
   */
  designContacts?: { key: number; text: string }[];
}
/**
 *
 * @export
 * @interface BuildBagDtoUpdate
 */
export interface BuildBagDtoUpdate extends BuildBagDtoCreate {
  /**
   *
   * @type {number}
   * @memberof BuildBagDtoUpdate
   */
  buildBagId?: number;
  /**
   *
   * @type {number}
   * @memberof BuildBagDtoUpdate
   */
  bagVersion?: number;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfBuildBagDtoGrid
 */
export interface QueryResultDtoOfBuildBagDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfBuildBagDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<BuildBagDtoGrid>}
   * @memberof QueryResultDtoOfBuildBagDtoGrid
   */
  items?: Array<BuildBagDtoGrid>;
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
 * @interface ResultDtoOfBuildBagToCloneDto
 */
export interface ResultDtoOfBuildBagToCloneDto {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfBuildBagToCloneDto
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfBuildBagToCloneDto
   */
  info?: string;
  /**
   *
   * @type {BuildBagToCloneDto}
   * @memberof ResultDtoOfBuildBagToCloneDto
   */
  data?: BuildBagToCloneDto;
}

// ------------------------ Not generated  ------------------------

export interface BuildBagEdit {
  BuildBagDtoEdit: BuildBagDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface BuildBagCreate {
  BuildBagDtoCreate: BuildBagDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface BuildBagGrid {
  BuildBagGridResult: QueryResultDtoOfBuildBagDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface BuildBagQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof BuildBagQueryDto
   */
  buildBagDescription?: Array<number>;
  /**
   *
   * @type {Array<string | number>}
   * @memberof BuildBagQueryDto
   */
  buildBagId?: Array<string | number>;
  /**
   *
   * @type {Array<string>}
   * @memberof BuildBagQueryDto
   */
  lastModifiedBy?: Array<string>;
}

export const GET_CREATE_BUILD_BAG = "GET_CREATE_BUILD_BAG";
export const GET_EDIT_BUILD_BAG = "GET_EDIT_BUILD_BAG";
export const GET_GRID_BUILD_BAG = "GET_GRID_BUILD_BAG";
export const GET_FILTER_BUILD_BAG = "GET_FILTER_BUILD_BAG";
export const CREATE_BUILD_BAG = "CREATE_BUILD_BAG";
export const EDIT_BUILD_BAG = "EDIT_BUILD_BAG";
export const DELETE_BUILD_BAG = "DELETE_BUILD_BAG";
export const RESTORE_BUILD_BAG = "RESTORE_BUILD_BAG";
