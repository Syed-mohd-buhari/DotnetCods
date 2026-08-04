import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  DateFilter,
  GridDtoBase,
  QueryObject,
} from "../Common";
import { ResultDto } from "../CommonModels";
import { UserManagementRoleDtoGrid } from "../UserManagement";
import {
  DeploymentStatusDto,
  QueryResultDtoOfDeploymentStatusDtoGrid,
} from "./DeploymentStatus";

import { QueryResultDtoOfSystemNamesDtoGrid, SystemNamesDto } from "./Domain";

/**
 *
 * @export
 * @interface TipologicaGridDtoMultipleRule
 */
export interface TipologicaGridDtoMultipleRule extends TipologicaGridDto {
  /**
   *
   * @type {Array<number>}
   * @memberof TipologicaGridDtoMultipleRule
   */
  rule?: Array<number>;
}

/**
 *
 * @export
 * @interface TipologicaGridDto
 */
export interface TipologicaGridDto extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof TipologicaGridDto
   */
  id?: number;

  /**
   *
   * @type {number}
   * @memberof TipologicaGridDto
   */
  productNamesId?: number;
  /**
   *
   * @type {string}
   * @memberof TipologicaGridDto
   */
  description?: string;
  isHistorical?: boolean;
  isCloudHostedAssetBool?: boolean;
  isDefault?: boolean;
  problemCategoryId?: number;
  problemCategoryDescription?: string;
  practiceId?: number;
  practiceDescription?: string;
  mainOrganisationId?: number;
  mainOrganisationDescription?: string;
  isEdu?: boolean;
  isSubDomain?: boolean;
  practiceEmailId?: number;
  productNameResource?: Array<ProductNameObj>;
  productNames?: string;
  vodafoneNameId?: number;
  riskClusterId?: number;
  riskLevel?: string;
  settingsValue?: Array<string>;
  productNameIds?: Array<number>;
  appSettingsConfigurationId?: number;
  operatingSystemVersion?: number;
  isPlatformSoftware?: boolean;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof TipologicaGridDto
   */
  riskClusterResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof TipologicaGridDto
   */
  practiceResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof TipologicaGridDto
   */
  riskClusterSeverityResource?: { [key: string]: string };
  /**
   *
   * @type {Date}
   * @memberof TipologicaGridDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof TipologicaGridDto
   */
  lastModifiedBy?: string;

  /**
   *
   * @type {{ [key: string]: string; }}
   * * @memberof TipologicaGridDto
   */
  vodafoneNameResource?: { [key: string]: string };
  componentManufacturerId?: number;
  componentManufacturer?: string;
  componentName?: string;
  bptDriverDetails?: string;
}

export interface ProductNameObj {
  description: string;
  enableDelete: boolean;
  id: number;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfTipologicaGridDto
 */
export interface QueryResultDtoOfTipologicaGridDto {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfTipologicaGridDto
   */
  totalItems?: number;
  /**
   *
   * @type {Array<TipologicaGridDto>}
   * @memberof QueryResultDtoOfTipologicaGridDto
   */
  items?: Array<TipologicaGridDto>;
  /**
   *
   * @type {CustomGridRenderOfTipologicaGridDto}
   * @memberof QueryResultDtoOfTipologicaGridDto
   */
  gridRender?: CustomGridRender;
}

export interface TipologicheQueryObjectGrid extends QueryObject {
  id?: Array<number>;
  appSettingsId?: Array<number>;
  deploymentStatusId?: Array<number>;
  mainOrganisationDescription?: Array<string>;
  mainOrganisationId?: Array<number>;
  practiceDescription?: Array<string>;
  practiceId?: Array<number>;
  problemCategoryId?: Array<number>;
  problemCategoryDescription?: Array<string>;
  description?: Array<string>;
  isHistorical?: boolean | undefined;
  isDefault?: boolean | undefined;
  productName?: Array<number>;
  riskCluster?: Array<string>;
  sortBy?: string;
  isSortAscending?: boolean;
  page?: number;
  pageSize?: number;
  riskClusterId?: Array<number>;
  riskLevel?: Array<string>;
  lastModifiedStartDate?: Date;
  lastModifiedEndDate?: Date;
  principalId?: number;
  isEdu?: Array<boolean>;
  isSubDomain?: Array<boolean>;
  componentManufacturerId?: Array<number>;
  componentManufacturer?: Array<string>;
  componentName?: Array<string>;
  lastModifiedValue?: DateFilter;
  isPlatformSoftware?: boolean;
}

export interface TipologicaQueryDtoForVirtualized
  extends TipologicheQueryObjectGrid {
  forVirtualized: Array<boolean>;
}

export interface TipologicheQueryObjectGridRule
  extends TipologicheQueryObjectGrid {
  rule?: Array<number>;
  cloudTypeBuild?: string;
  isCloudHostedAsset?: string;
}

export interface TipologicheQueryObjectGridCombinationRule
  extends TipologicheQueryObjectGridRule {
  projectStatusCombinationRule?: Array<number>;
}

export interface TipologicheQueryObjectGridProjectStatusCombinationRule
  extends TipologicheQueryObjectGrid {
  projectStatusCombinationRule?: Array<number>;
  default?: Array<boolean>;
}

export interface LookUpEdit {
  LookUpDtoEdit: TipologicaGridDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface LookUpCreate {
  LookUpDtoCreate: TipologicaGridDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface LookUpGraphFilter {
  FilterData: [] | null;
}
export interface LookUpGrid {
  LookUpGridResult: QueryResultDtoOfTipologicaGridDto | null;
  LookUpGridResultAll: QueryResultDtoOfTipologicaGridDto | null;
  filter: FilterValueDto[] | null;
}

export interface LookUpGridForDeploymentStatus {
  LookUpGridResult: QueryResultDtoOfDeploymentStatusDtoGrid | null;
  LookUpGridResultAll: QueryResultDtoOfDeploymentStatusDtoGrid | null;
  filter: FilterValueDto[] | null;
}
export interface LookUpForDeploymentStatusCreate {
  LookUpDtoCreate: DeploymentStatusDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface LookUpForDeploymentStatusEdit {
  LookUpDtoEdit: DeploymentStatusDto | null;
  ResultDtoEdit: ResultDto | null;
}

export interface LookUpEditRule {
  LookUpDtoEdit: TipologicaGridDtoRule | null;
  ResultDtoEdit: ResultDto | null;
}
export interface LookUpCreateRule {
  LookUpDtoCreate: TipologicaGridDtoRule | null;
  ResultDtoCreate: ResultDto | null;
}
export interface LookUpCreateUserRole {
  LookUpDtoCreate: UserManagementRoleDtoGrid | null;
  ResultDtoCreate: ResultDto | null;
}
export interface LookUpGridRule {
  LookUpGridResult: QueryResultDtoOfTipologicaGridDtoRule | null;
  LookUpGridResultAll: QueryResultDtoOfTipologicaGridDtoRule | null;
  filter: FilterValueDto[] | null;
}

export interface LookUpForSystemNamesCreate {
  LookUpDtoCreate: SystemNamesDto | null;
  ResultDtoCreate: ResultDto | null;
}
export interface LookUpGridForSystemNames {
  LookUpGridResult: QueryResultDtoOfSystemNamesDtoGrid | null;
  LookUpGridResultAll: QueryResultDtoOfSystemNamesDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface LookUpForSystemNamesEdit {
  LookUpDtoEdit: SystemNamesDto | null;
  ResultDtoEdit: ResultDto | null;
}

/**
 *
 * @export
 * @interface TipologicaGridDtoRule
 */
export interface TipologicaGridDtoRule extends TipologicaGridDto {
  /**
   *
   * @type {number}
   * @memberof TipologicaGridDtoRule
   */
  rule?: number;
  /**
   *
   * @type {string}
   * @memberof TipologicaGridDtoRule
   */
  cloudTypeBuild?: string;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfTipologicaGridDtoRule
 */
export interface QueryResultDtoOfTipologicaGridDtoRule {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfTipologicaGridDtoRule
   */
  totalItems?: number;
  /**
   *
   * @type {Array<TipologicaGridDtoRule>}
   * @memberof QueryResultDtoOfTipologicaGridDtoRule
   */
  items?: Array<TipologicaGridDtoRule>;
  /**
   *
   * @type {CustomGridRenderOfTipologicaGridDtoRule}
   * @memberof QueryResultDtoOfTipologicaGridDtoRule
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @export
 * @interface TipologicaGridDtoCombinationRule
 */
export interface TipologicaGridDtoCombinationRule
  extends TipologicaGridDtoRule {
  /**
   *
   * @type {number}
   * @memberof TipologicaGridDtoCombinationRule
   */
  projectStatusCombinationRule?: number;
}

/**
 *
 * @export
 * @interface TipologicaGridDtoProjectStatusCombinationRule
 */
export interface TipologicaGridDtoProjectStatusCombinationRule
  extends TipologicaGridDto {
  /**
   *
   * @type {number}
   * @memberof TipologicaGridDtoProjectStatusCombinationRule
   */
  projectStatusCombinationRule?: number;

  /**
   *
   * @type {boolean}
   * @memberof TipologicaGridDtoProjectStatusCombinationRule
   */
  default?: boolean;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfTipologicaGridDtoCombinationRule
 */
export interface QueryResultDtoOfTipologicaGridDtoCombinationRule {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfTipologicaGridDtoCombinationRule
   */
  totalItems?: number;
  /**
   *
   * @type {Array<TipologicaGridDtoCombinationRule>}
   * @memberof QueryResultDtoOfTipologicaGridDtoCombinationRule
   */
  items?: Array<TipologicaGridDtoCombinationRule>;
  /**
   *
   * @type {CustomGridRenderOfTipologicaGridDtoCombinationRule}
   * @memberof QueryResultDtoOfTipologicaGridDtoCombinationRule
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfTipologicaGridDtoProjectStatusCombinationRule
 */
export interface QueryResultDtoOfTipologicaGridDtoProjectStatusCombinationRule {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfTipologicaGridDtoProjectStatusCombinationRule
   */
  totalItems?: number;
  /**
   *
   * @type {Array<TipologicaGridDtoProjectStatusCombinationRule>}
   * @memberof QueryResultDtoOfTipologicaGridDtoProjectStatusCombinationRule
   */
  items?: Array<TipologicaGridDtoProjectStatusCombinationRule>;
  /**
   *
   * @type {CustomGridRenderOfTipologicaGridDtoProjectStatusCombinationRule}
   * @memberof QueryResultDtoOfTipologicaGridDtoProjectStatusCombinationRule
   */
  gridRender?: CustomGridRender;
}
