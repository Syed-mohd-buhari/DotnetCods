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
 * @interface MajorSoftwareBuildDto
 */
export interface MajorSoftwareBuildDto extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildDto
   */
  softwareVersion: string;
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildDto
   */
  productName: string;
  /**
   *
   * @type {Date}
   * @memberof MajorSoftwareBuildDto
   */
  lastTimeBuyNew?: Date;
  /**
   *
   * @type {Date}
   * @memberof MajorSoftwareBuildDto
   */
  lastTimeBuyUpgrades?: Date;
  /**
   *
   * @type {Date}
   * @memberof MajorSoftwareBuildDto
   */
  lastTimeBuyExpansions?: Date;
  /**
   *
   * @type {Date}
   * @memberof MajorSoftwareBuildDto
   */
  endOfMaintenance: Date | null;
  /**
   *
   * @type {Date}
   * @memberof MajorSoftwareBuildDto
   */
  endOfsupport?: Date | null;
  /**
   *
   * @type {Date}
   * @memberof MajorSoftwareBuildDto
   */
  generaAvailableDate: Date;
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildDto
   */
  deliveryMethod?: string;
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildDto
   */
  spareFieldsJSON?: string;
  /**
   *
   * @type {Date}
   * @memberof MajorSoftwareBuildDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface MajorSoftwareBuildDtoCreate
 */
export interface MajorSoftwareBuildDtoCreate extends MajorSoftwareBuildDto {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  originalEquipmentManufacturerResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  originalEquipmentManufacturerId: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  tcpBundleVersion?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  tcpSoftwareCompatibilityIdList: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  tciBundleVersion?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  tciSoftwareCompatibilityIdList: Array<number>;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  networkFunctionsResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  networkFunctionsIds: Array<number>;

  criticalAssetTypeResource?: { [key: string]: string };
  criticalAssetTypeId?: number;
  /**
   *
   * @type {number}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  vodafoneNameId?: number;
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  designContactIds?: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  designContacts?: { [key: string]: string };

  /**
   *
   * @type {number}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  productNameId?: number;
  /**
   *
   * @type {number}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  existSystemTypeId?: number;
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  vulnerabilityStatus?: string;
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  thirdPartySoftwareComponentsId?: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  operatingSystemResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  productNamesResource?: { key: number; value: string; isSelected: boolean }[];

  /**
   *
   * @type {number}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  operatingSystemId?: number;

  /**
   *
   * @type {EOM_STATUS}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  eomStatus?: EOM_STATUS;

  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  description?: string;

  isPlatform?: boolean;
}
/**
 *
 * @export
 * @interface MajorSoftwareBuildDtoGrid
 */
export interface MajorSoftwareBuildDtoGrid extends MajorSoftwareBuildDto {
  /**
   *
   * @type {number}
   * @memberof MajorSoftwareBuildDtoGrid
   */
  majorSoftwareBuildId?: number;
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildDtoGrid
   */
  originalEquipmentManufacturer?: string;
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildDtoGrid
   */
  vulnerabilityStatus?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildDtoGrid
   */
  thirdPartySoftwareComponents?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildDtoGrid
   */
  operatingSystem?: string;
}
/**
 *
 * @export
 * @interface MajorSoftwareBuildDtoUpdate
 */
export interface MajorSoftwareBuildDtoUpdate
  extends MajorSoftwareBuildDtoCreate {
  /**
   *
   * @type {number}
   * @memberof MajorSoftwareBuildDtoUpdate
   */
  majorSoftwareBuildsId?: number;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfMajorSoftwareBuildDtoGrid
 */
export interface QueryResultDtoOfMajorSoftwareBuildDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfMajorSoftwareBuildDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<MajorSoftwareBuildDtoGrid>}
   * @memberof QueryResultDtoOfMajorSoftwareBuildDtoGrid
   */
  items?: Array<MajorSoftwareBuildDtoGrid>;
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
 * @interface ResultDtoOfMajorSoftwareBuildToCloneDto
 */
export interface ResultDtoOfMajorSoftwareBuildToCloneDto {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfMajorSoftwareBuildToCloneDto
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfMajorSoftwareBuildToCloneDto
   */
  info?: string;
  /**
   *
   * @type {MajorSoftwareBuildToCloneDto}
   * @memberof ResultDtoOfMajorSoftwareBuildToCloneDto
   */
  data?: MajorSoftwareBuildToCloneDto;
}
/**
 *
 * @export
 * @interface MajorSoftwareBuildToCloneDto
 */
export interface MajorSoftwareBuildToCloneDto {
  /**
   *
   * @type {number}
   * @memberof MajorSoftwareBuildToCloneDto
   */
  majorSoftwareBuildsId?: number;
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildToCloneDto
   */
  originalEquipmentManufacturer?: string;
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildToCloneDto
   */
  productName: string;
  /**
   *
   * @type {string}
   * @memberof MajorSoftwareBuildToCloneDto
   */
  softwareVersion?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildToCloneDto
   */
  designComponentResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildToCloneDto
   */
  tcpBundleVersion?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildToCloneDto
   */
  tcpSoftwareCompatibilityIdList: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildToCloneDto
   */
  tciBundleVersion?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildToCloneDto
   */
  tciSoftwareCompatibilityIdList: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildToCloneDto
   */
  designContactIds: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildToCloneDto
   */
  designContacts: { [key: string]: string };
  isvmware?: boolean;
}

/**
 *
 * @export
 * @interface CloneMajorSoftwareBuildDto
 */
export interface CloneMajorSoftwareBuildDto {
  /**
   *
   * @type {number}
   * @memberof CloneMajorSoftwareBuildDto
   */
  majorSoftwareBuildsId?: number;
  /**
   *
   * @type {string}
   * @memberof CloneMajorSoftwareBuildDto
   */
  softwareVersion?: string;
  /**
   *
   * @type {Date}
   * @memberof CloneMajorSoftwareBuildDto
   */
  endOfMaintenance?: Date | null;

  endOfsupport?: Date | null;

  eomStatus: EOM_STATUS;
  originalEquipmentManufacturer?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  tcpBundleVersion?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  tcpSoftwareCompatibilityIdList: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  tciBundleVersion?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  tciSoftwareCompatibilityIdList: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  designContactIds?: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof MajorSoftwareBuildDtoCreate
   */
  designContacts?: { [key: string]: string };
}

// ------------------------ Not generated  ------------------------

export interface MajorSoftwareBuildEdit {
  MajorSoftwareBuildDtoEdit: MajorSoftwareBuildDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface MajorSoftwareBuildCreate {
  MajorSoftwareBuildDtoCreate: MajorSoftwareBuildDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface MajorSoftwareBuildGrid {
  MajorSoftwareBuildGridResult: QueryResultDtoOfMajorSoftwareBuildDtoGrid | null;
  filter: FilterValueDto[] | null;
}
export interface MajorSoftwareBuildProductBasedQueryObjectGrid
  extends QueryObject {
  /**
   *
   * @type {number}
   * @memberof MajorSoftwareBuildProductBasedQueryObjectGrid
   */
  productId?: number;
  /**
   *
   * @type {number}
   * @memberof MajorSoftwareBuildProductBasedQueryObjectGrid
   */
  currentVersionSwId?: number;

  /**
   *
   * @type {Array<string>}
   * @memberof MajorSoftwareBuildProductBasedQueryObjectGrid
   */
  lastModifiedBy?: Array<string>;
}
export interface MajorSoftwareBuildQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildQueryDto
   */
  majorSoftwareBuildId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildQueryDto
   */
  originalEquipmentManufacturer?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorSoftwareBuildQueryDto
   */
  productNamesId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorSoftwareBuildQueryDto
   */
  softwareVersion?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof MajorSoftwareBuildQueryDto
   */
  lastTimeBuyNew?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof MajorSoftwareBuildQueryDto
   */
  lastTimeBuyUpgrades?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof MajorSoftwareBuildQueryDto
   */
  lastTimeBuyExpansions?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof MajorSoftwareBuildQueryDto
   */
  endOfMaintenance?: DateFilter | null;
  /**
   *
   * @type {DateFilter}
   * @memberof MajorSoftwareBuildQueryDto
   */
  endOfsupport?: DateFilter | null;
  /**
   *
   * @type {DateFilter}
   * @memberof MajorSoftwareBuildQueryDto
   */
  generaAvailableDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorSoftwareBuildQueryDto
   */
  deliveryMethod?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorSoftwareBuildQueryDto
   */
  vulnerabilityStatus?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof MajorSoftwareBuildQueryDto
   */
  operatingSystem?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorSoftwareBuildQueryDto
   */
  spareFieldsJson?: Array<string>;
  globalSearchKeyword?: string;
  /**
   *
   * @type {Array<string>}
   * @memberof MajorSoftwareBuildQueryDto
   */
  lastModifiedBy?: Array<string>;
}

export const GET_CREATE_MAJOR_SOFTWARE_BUILD =
  "GET_CREATE_MAJOR_SOFTWARE_BUILD";
export const GET_EDIT_MAJOR_SOFTWARE_BUILD = "GET_EDIT_MAJOR_SOFTWARE_BUILD";
export const GET_GRID_MAJOR_SOFTWARE_BUILD = "GET_GRID_MAJOR_SOFTWARE_BUILD";
export const GET_FILTER_MAJOR_SOFTWARE_BUILD =
  "GET_FILTER_MAJOR_SOFTWARE_BUILD";
export const GET_GRID_MAJOR_SOFTWARE_BUILD_PRODUCT_BASED =
  "GET_GRID_MAJOR_SOFTWARE_BUILD_PRODUCT_BASED";
export const GET_FILTER_MAJOR_SOFTWARE_BUILD_PRODUCT_BASED =
  "GET_FILTER_MAJOR_SOFTWARE_BUILD_PRODUCT_BASED";
export const CREATE_MAJOR_SOFTWARE_BUILD = "CREATE_MAJOR_SOFTWARE_BUILD";
export const EDIT_MAJOR_SOFTWARE_BUILD = "EDIT_MAJOR_SOFTWARE_BUILD";
export const DELETE_MAJOR_SOFTWARE_BUILD = "DELETE_MAJOR_SOFTWARE_BUILD";
export const RESTORE_MAJOR_SOFTWARE_BUILD = "RESTORE_MAJOR_SOFTWARE_BUILD";
