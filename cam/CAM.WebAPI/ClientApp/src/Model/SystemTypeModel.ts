import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  DateFilter,
  EOM_STATUS,
  GridDtoBase,
  QueryObjectGrid,
  RenderDetail,
} from "./Common";
import { RelatedResource, ResultDto } from "./CommonModels";
import { MajorHardwareBuildDtoUpdate } from "./MajorHardwareBuild";
import { MajorSoftwareBuildDtoUpdate } from "./MajorSoftwareBuild";
import { AssetCategoryDto, AssetCategoryDtoGrid } from "./LookUp/AssetCategory";

export interface SystemTypeDtoGrouped extends SystemTypeDtoGrid {
  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoGrouped
   */
  keyGrouped?: string;
}
/**
 *
 * @export
 * @interface MajorHardwareBuildMainSystemTypeDto
 */
export interface MajorHardwareBuildMainSystemTypeDto {
  /**
   *
   * @type {number}
   * @memberof MajorHardwareBuildMainSystemTypeDto
   */
  majorHardwareBuildId?: number;
  /**
   *
   * @type {boolean}
   * @memberof MajorHardwareBuildMainSystemTypeDto
   */
  isMain?: boolean;
}
/**
 *
 * @export
 * @interface ConstraintInfoDto
 */
export interface ConstraintInfoDto {
  /**
   *
   * @type {string}
   * @memberof ConstraintInfoDto
   */
  lcmStatus?: string;
  /**
   *
   * @type {string}
   * @memberof ConstraintInfoDto
   */
  constraintScaling?: any;
  /**
   *
   * @type {string}
   * @memberof ConstraintInfoDto
   */
  constraintLcm?: string;
}

/**
 *
 * @export
 * @interface SystemTypeDto
 */
export interface SystemTypeDto extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof SystemTypeDto
   */
  systemTypeNameVodafone: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDto
   */
  systemTypeName3Gpp?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDto
   */
  systemTypeNameOem?: string;
  /**
   *
   * @type {Date}
   * @memberof SystemTypeDto
   */
  constraintScaling?: any;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDto
   */
  constraintLcm?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDto
   */
  endOfMaintenance?: Date | string | null;
  /**
   *
   * @type {number}
   * @memberof SystemTypeDto
   */
  productImportanceId: number;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDto
   */
  subDomainSpoc?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDto
   */
  assetType?: string;
  /**
   *
   * @type {Date}
   * @memberof SystemTypeDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDto
   */
  lastModifiedBy?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDto
   */
  spareFieldsJson?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDto
   */
  lcmStatus?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDto
   */
  systemSolution?: string;
}
/**
 *
 * @export
 * @interface SystemTypeDtoCreate
 */
export interface SystemTypeDtoCreate extends SystemTypeDto {
  /**
   *
   * @type {number}
   * @memberof SystemTypeDtoCreate
   */
  majorSoftwareBuildsId: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SystemTypeDtoCreate
   */
  verticalResponsibleResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof SystemTypeDtoCreate
   */
  verticalResponsibleId: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SystemTypeDtoCreate
   */
  subDomainResponsibleResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof SystemTypeDtoCreate
   */
  subDomainResponsibleId: number;
  /**
   *
   * @type {{ [key: string]: AssetCategory; }}
   * @memberof SystemTypeDtoCreate
   */
  assetCategoryResource?: { [key: string]: AssetCategoryDto };
  /**
   *
   * @type {number}
   * @memberof SystemTypeDtoCreate
   */
  assetCategoryId: number;
  /**
   *
   * @type {{ [key: string]: RelatedResource; }}
   * @memberof SystemTypeDtoCreate
   */
  assetClassResource?: { [key: string]: RelatedResource };
  /**
   *
   * @type {number}
   * @memberof SystemTypeDtoCreate
   */
  assetClassId: number;
  /**
   *
   * @type {{ [key: string]: RelatedResource; }}
   * @memberof SystemTypeDtoCreate
   */
  assetTypeResource?: { [key: string]: RelatedResource };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SystemTypeDtoCreate
   */
  productImportanceResource: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof SystemTypeDtoCreate
   */
  assetTypeId?: number;
  /**
   *
   * @type {Array<MajorHardwareBuildMainSystemTypeDto>}
   * @memberof SystemTypeDtoCreate
   */
  majorHardwareBuildId: Array<MajorHardwareBuildMainSystemTypeDto>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeDtoCreate
   */
  subDomainSpocIds: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SystemTypeDtoCreate
   */
  subDomainSpocResource?: { [key: string]: string };

  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoCreate
   */
  vodafoneName?: string;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SystemTypeDtoCreate
   */
  vodafoneNameResource?: { [key: string]: string };

  /**
   *
   * @type {number}
   * @memberof SystemTypeDtoCreate
   */
  vodafoneNameId?: number;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoCreate
   */
  assetClassDescription?: string;
}
/**
 *
 * @export
 * @interface SystemTypeDtoGrid
 */
export interface SystemTypeDtoGrid extends SystemTypeDto {
  /**
   *
   * @type {number}
   * @memberof SystemTypeDtoGrid
   */
  systemTypeId?: number;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoGrid
   */
  majorSoftwareBuild?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoGrid
   */
  verticalResponsible?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoGrid
   */
  subDomainResponsible?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoGrid
   */
  assetCategory?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoGrid
   */
  productImportance?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoGrid
   */
  assetClass?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoGrid
   */
  softwareOem?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoGrid
   */
  hardwareOem?: string;
  /**
   *
   * @type {string}
   * @memberof SystemTypeDtoGrid
   */
  majorHardwareBuildWithoutOem?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SystemTypeDtoGrid
   */
  majorHardwareBuild?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof SystemTypeDtoGrid
   */
  majorHardwareBuildId?: number;
  vodafoneName?:string;
  vodafoneNameId?:number;
  /**
   *
   * @type {number}
   * @memberof SystemTypeDtoGrid
   */
  majorSoftwareBuildId?: number;
}
/**
 *
 * @export
 * @interface SystemTypeDtoUpdate
 */
export interface SystemTypeDtoUpdate extends SystemTypeDtoCreate {
  /**
   *
   * @type {number}
   * @memberof SystemTypeDtoUpdate
   */
  systemTypeId?: number;
}

/**
 *
 * @export
 * @interface LifecycleConstraintDto
 */
export interface LifecycleConstraintDto {
  /**
   *
   * @type {SystemTypeDtoUpdate}
   * @memberof LifecycleConstraintDto
   */
  systemTypeDto?: SystemTypeDtoUpdate;
  /**
   *
   * @type {MajorHardwareBuildDtoUpdate}
   * @memberof LifecycleConstraintDto
   */
  majorHardwareBuildDto?: MajorHardwareBuildDtoUpdate;
  /**
   *
   * @type {MajorSoftwareBuildDtoUpdate}
   * @memberof LifecycleConstraintDto
   */
  majorSoftwareBuildDto?: MajorSoftwareBuildDtoUpdate;
}

/**
 *
 * @export
 * @interface LifecycleConstraintQueryDto
 */
export interface LifecycleConstraintQueryDto {
  /**
   *
   * @type {Date}
   * @memberof LifecycleConstraintQueryDto
   */
  msLastTimeBuyNew?: Date;
  /**
   *
   * @type {Date}
   * @memberof LifecycleConstraintQueryDto
   */
  msLastTimeBuyUpgrades?: Date;
  /**
   *
   * @type {Date}
   * @memberof LifecycleConstraintQueryDto
   */
  msLastTimeBuyExpansions?: Date;
  /**
   *
   * @type {Date}
   * @memberof LifecycleConstraintQueryDto
   */
  msEndOfMaintenance?: Date;
  /**
   *
   * @type {Date}
   * @memberof LifecycleConstraintQueryDto
   */
  msEndOfsupport?: Date;
  /**
   *
   * @type {Date}
   * @memberof LifecycleConstraintQueryDto
   */
  mhLastTimeBuyNew?: Date;
  /**
   *
   * @type {Date}
   * @memberof LifecycleConstraintQueryDto
   */
  mhLastTimeBuyUpgrades?: Date;
  /**
   *
   * @type {Date}
   * @memberof LifecycleConstraintQueryDto
   */
  mhLastTimeBuyExpansions?: Date;
  /**
   *
   * @type {Date}
   * @memberof LifecycleConstraintQueryDto
   */
  mhEndOfMaintenance?: Date;
  /**
   *
   * @type {Date}
   * @memberof LifecycleConstraintQueryDto
   */
  mhEndOfsupport?: Date;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfSystemTypeDtoGrid
 */
export interface QueryResultDtoOfSystemTypeDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfSystemTypeDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<SystemTypeDtoGrid>}
   * @memberof QueryResultDtoOfSystemTypeDtoGrid
   */
  items?: Array<SystemTypeDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface QueryResultDtoOfSystemTypeDtoGrouped {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfSystemTypeDtoGrouped
   */
  totalItems?: number;
  /**
   *
   * @type {Array<SystemTypeDtoGrouped>}
   * @memberof QueryResultDtoOfSystemTypeDtoGrouped
   */
  items?: Array<SystemTypeDtoGrouped>;
  /**
   *
   * @type {CustomGridRenderOfSystemTypeDtoGrouped}
   * @memberof QueryResultDtoOfSystemTypeDtoGrouped
   */
  gridRender?: CustomGridRenderOfSystemTypeDtoGrouped;
}

export interface CustomGridRenderOfSystemTypeDtoGrouped {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfSystemTypeDtoGrouped
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfSystemTypeDtoGrouped
   */
  render?: Array<RenderDetail>;
}

export interface SystemTypeQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  systemTypeId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  systemTypeNameVodafone?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  systemTypeName3Gpp?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  systemTypeNameOem?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  majorSoftwareBuild?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  majorHardwareBuild?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  systemSolution?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof SystemTypeQueryDto
   */
  constraintScaling?: any;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  constraintLcm?: Array<string>;
  /**
   *
   * @type {DateFilter | string}
   * @memberof SystemTypeQueryDto
   */
  endOfMaintenance?: DateFilter | string | null;
  /**
   *
   * @type {DateFilter}
   * @memberof SystemTypeQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  productImportance?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  verticalResponsible?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  subDomainResponsible?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  subDomainSpoc?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  assetCategory?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  assetClass?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  assetType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  spareFieldsJson?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  softwareOem?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  hardwareOem?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  vodafoneName?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeQueryDto
   */
  VodafoneNameIds?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  lastModifiedBy?: Array<string>;
}

export interface SystemTypeEdit {
  SystemTypeDtoEdit: SystemTypeDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface SystemTypeCreate {
  SystemTypeDtoCreate: SystemTypeDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface SystemTypeGrid {
  SystemTypeGridResult: QueryResultDtoOfSystemTypeDtoGrid | null;
  filter: FilterValueDto[] | null;
}
export interface SystemTypeCommonInfo {
  SystemSolution: string | null;
  ConstraintInfo: ConstraintInfoDto | null;
}

/**
 *
 * @export
 * @interface SystemTypeReleatedMajorEntity
 */
export interface SystemTypeReleatedMajorEntity {
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeReleatedMajorEntity
   */
  idMajorSoftwareSameAssetCategory?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeReleatedMajorEntity
   */
  idMajorSoftwareOtherAssetCategory?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeReleatedMajorEntity
   */
  idMajorHardwareSameAssetCategory?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof SystemTypeReleatedMajorEntity
   */
  idMajorHardwareOtherAssetCategory?: Array<number>;
}
/**
 *
 * @export
 * @interface ResultDtoOfSystemTypeReleatedMajorEntity
 */
export interface ResultDtoOfSystemTypeReleatedMajorEntity {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfSystemTypeReleatedMajorEntity
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfSystemTypeReleatedMajorEntity
   */
  info?: string;
  /**
   *
   * @type {SystemTypeReleatedMajorEntity}
   * @memberof ResultDtoOfSystemTypeReleatedMajorEntity
   */
  data?: SystemTypeReleatedMajorEntity;
}

export const GET_CREATE_SYSTEM_TYPE = "GET_CREATE_SYSTEM_TYPE";
export const GET_EDIT_SYSTEM_TYPE = "GET_EDIT_SYSTEM_TYPE";
export const GET_GRID_SYSTEM_TYPE = "GET_GRID_SYSTEM_TYPE";
export const GET_FILTER_SYSTEM_TYPE = "GET_FILTER_SYSTEM_TYPE";
export const CREATE_SYSTEM_TYPE = "CREATE_SYSTEM_TYPE";
export const EDIT_SYSTEM_TYPE = "EDIT_SYSTEM_TYPE";
export const DELETE_SYSTEM_TYPE = "DELETE_SYSTEM_TYPE";
export const RESTORE_SYSTEM_TYPE = "RESTORE_SYSTEM_TYPE";
export const GET_SYSTEM_TYPE_NAME = "GET_SYSTEM_TYPE_NAME";
export const GET_SYSTEM_TYPE_CONSTRAINT_INFO =
  "GET_SYSTEM_TYPE_CONSTRAINT_INFO";
