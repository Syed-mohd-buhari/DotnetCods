import { ResultDto } from "./CommonModels";
import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  GridDtoBase,
  QueryObject,
  DateFilter,
} from "./Common";
import { PlannedActivityDtoUpdate } from "./PlannedActivity";
import { ReasonCheckboxDto } from "./LookUp/ReasonCheckbox";
import { TipologicaGridDtoRule } from "./LookUp/LookUpGenericModel";
/**
 *
 * @export
 * @interface DesignAspectQueryDto
 */
export interface DesignAspectQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectQueryDto
   */
  designAspectId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectQueryDto
   */
  designComponent?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectQueryDto
   */
  opCo?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  operationalContact?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  softwareSupportProvider?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof DesignAspectQueryDto
   */
  softwareEndOfWarrantyDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  softwareSupportType?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectQueryDto
   */
  numberOfNodes?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectQueryDto
   */
  numberOfNodesInLab?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  subDomainSpoc?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  eduspoc?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof DesignAspectQueryDto
   */
  softwareEndOfSupportContract?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof DesignAspectQueryDto
   */
  hardwareEndOfSupportContract?: DateFilter;
  /**
   *
   * @type {Array<boolean>}
   * @memberof DesignAspectQueryDto
   */
  warranty?: Array<boolean>;
  /**
   *
   * @type {boolean}
   * @memberof DesignAspectQueryDto
   */
  onSoftwareOrHardware?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof DesignAspectQueryDto
   */
  renewalInProgress?: boolean;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectQueryDto
   */
  plannedActivity?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  hardwareSupportProvider?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  hardwareSupportType?: Array<string>;
}
/**
 *
 * @export
 * @interface NetworkElementAssociated
 */
export interface NetworkElementAssociated {
  /**
   *
   * @type {number}
   * @memberof NetworkElementAssociated
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAssociated
   */
  elementName?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAssociated
   */
  enviroment?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAssociated
   */
  location?: string;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAssociated
   */
  enviromentId?: number;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAssociated
   */
  locationId?: number;
}

/**
 *
 * @export
 * @interface DesignAspectDto
 */
export interface DesignAspectDto extends GridDtoBase {
  designComponentFamilyId: number;
  designComponentFamilyName: string;
  subNetworkBoundary: string;
  opCoId: number;
  opCoName: string;
  description: string;
  authenicationTypeId: number;
  authenicationTypeName: string;
  securityManagerId: number;
  securityManagerName: string;
  licenseModelId: number;
  licenseModelName: string;
  thirdPartyAccessId: number;
  thirdPartyAccessName: string;
  siteResilienceId: number;
  siteResilienceName: string;
  swDeliveryLifeCycleId: number;
  swDeliveryLifeCycleName: string;
  instanceResilienceId: number;
  instanceResilienceName: string;
  businessContinuityMethodId: number;
  businessContinuityMethodName: string;
  countrySpecificCriticality?: boolean;
  criticalNationalInfrastructure?: boolean;
  criticalNationalInfrastructureName: string;
  securityTireZoneId: number;
  securityTireZoneName: string;
  /**
   *
   * @type {Date}
   * @memberof DesignAspectDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof DesignAspectDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface DesignAspectDtoCreate
 */
export interface DesignAspectDtoCreate extends DesignAspectDto {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  serviceMasterResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectDtoCreate
   */
  usedNetworkFunctionsIds?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectDtoCreate
   */
  supportedServicesIds?: Array<number>;

  /**
   *
   * @type {Boolean}
   * @memberof DesignAspectDtoCreate
   */
  isSupportedAllServices?: boolean;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  securityTireZoneResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  thirdPartyAccessResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  licenseModelsResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  swDeliveryLifeCyclesResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  securityManagersResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  dcFsResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  opcosResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  instanseResiliencesResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  siteResilienceMethodsResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  siteResiliencesResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  authenicationTypesResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  usedNetworkFunctionsResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoCreate
   */
  supportedServicesResource?: { [key: string]: string };

  nominalCapacityLimit?: string;
  designedCapacityLimit?: string;
  maxAllowedLoading?: string;
  platformSoftware?: boolean;
}
/**
 *
 * @export
 * @interface DesignAspectDtoGrid
 */
export interface DesignAspectDtoGrid extends DesignAspectDto {
  /**
   *
   * @type {number}
   * @memberof DesignAspectDtoGrid
   */
  designAspectId?: number;

  /**
   *
   * @type {number}
   * @memberof DesignAspectDtoGrid
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof DesignAspectDtoGrid
   */
  hardwareSupportedId?: string;
  /**
   *
   * @type {string}
   * @memberof DesignAspectDtoGrid
   */
  softwareSupportedId?: string;
  /**
   *
   * @type {string}
   * @memberof DesignAspectDtoGrid
   */
  fullorPartialSupportId?: string;
  /**
   *
   * @type {string}
   * @memberof DesignAspectDtoGrid
   */
  checkboxResourceDesignAspectSoftwares?: string;
  /**
   *
   * @type {string}
   * @memberof DesignAspectDtoGrid
   */
  checkboxResourceDesignAspectHardwares?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignAspectDtoGrid
   */
  plannedActivity?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof DesignAspectDtoGrid
   */
  designComponent?: string;
  /**
   *
   * @type {string}
   * @memberof DesignAspectDtoGrid
   */
  opCo?: string;
  /**
   *
   * @type {string}
   * @memberof DesignAspectDtoGrid
   */
  productImportanceId?: string;
  /**
   *
   * @type {string}
   * @memberof DesignAspectDtoGrid
   */
  subDomainSpoc?: string;
  /**
   *
   * @type {string}
   * @memberof DesignAspectDtoGrid
   */
  eduspoc?: string;
}

interface MigrationStatus {
  daMigrationStatusId?: number;
  opco?: string;
  location?: string;
  status?: string;
}

interface AssetMigrationStatus {
  oldAssetName?: string;
  newelEmentName?: string;
  location?: string;
  rfoDate?: string;
  rfsDate?: string;
  oldDeploymentStatus?: string;
  migrationCompletionDate?: string;
  trafficNodePercentage?: string;
  targetDesignComponenet?: string;
  newEnvironment?: string;
}
/**
 *
 * @export
 * @interface DesignAspectDtoUpdate
 */
export interface DesignAspectDtoUpdate extends DesignAspectDtoCreate {
  /**
   *
   * @type {number}
   * @memberof DesignAspectDtoUpdate
   */
  designAspectId?: number;
  id?: number;

  /**
   *
   * @type {Array<PlannedActivityDtoUpdate>}
   * @memberof LcmEngineeringDtoCreate
   */
  plannedActivityDto?: Array<PlannedActivityDtoUpdate>;
  daMigrationStatusEntity?: MigrationStatus[];
  daAssetMigrationEntity?: AssetMigrationStatus[];
}

/**
 *
 * @export
 * @interface QueryResultDtoOfDesignAspectDtoGrid
 */
export interface QueryResultDtoOfDesignAspectDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfDesignAspectDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<DesignAspectDtoGrid>}
   * @memberof QueryResultDtoOfDesignAspectDtoGrid
   */
  items?: Array<DesignAspectDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

//------------------------------>no auto gen

export interface DesignAspectQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectQueryDto
   */
  designAspectId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectQueryDto
   */
  designComponentFamily?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  subNetworkBoundary?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectQueryDto
   */
  opcoId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  authenicationTypeId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  securityManagerId?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof DesignAspectQueryDto
   */
  softwareEndOfWarrantyDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  thirdPartyAccessId?: Array<number>;

  /**
   *
   * @type {Array<any>}
   * @memberof DesignAspectQueryDto
   */
  designComponentFamilyName?: Array<any>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectQueryDto
   */
  siteResilienceId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignAspectQueryDto
   */
  swDeliveryLifeCycleId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  businessContinuityMethodId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignAspectQueryDto
   */
  instanceResilienceId?: Array<number>;

  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  lastModifiedBy?: Array<string>;

  /**
   *
   * @type {Array<boolean>}
   * @memberof LcmEngineeringQueryDto
   */
  archived?: boolean;
}

export interface DesignAspectEdit {
  DesignAspectDtoEdit: DesignAspectDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface DesignAspectCreate {
  DesignAspectDtoCreate: DesignAspectDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface DesignAspectGrid {
  DesignAspectGridResult: QueryResultDtoOfDesignAspectDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_DESIGN_ASPECT = "GET_CREATE_DESIGN_ASPECT";
export const GET_EDIT_DESIGN_ASPECT = "GET_EDIT_DESIGN_ASPECT";
export const GET_GRID_DESIGN_ASPECT = "GET_GRID_DESIGN_ASPECT";
export const GET_FILTER_DESIGN_ASPECT = "GET_FILTER_DESIGN_ASPECT";
export const CREATE_DESIGN_ASPECT = "CREATE_DESIGN_ASPECT";
export const EDIT_DESIGN_ASPECT = "EDIT_DESIGN_ASPECT";
export const DELETE_DESIGN_ASPECT = "DELETE_DESIGN_ASPECT";
export const RESTORE_DESIGN_ASPECT = "RESTORE_DESIGN_ASPECT";
