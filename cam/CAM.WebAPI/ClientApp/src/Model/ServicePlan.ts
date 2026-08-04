import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  QueryObjectGrid,
  GridDtoBase,
  QueryObject,
  DateFilter,
} from "./Common";
import { RelatedResource, ResultDto } from "./CommonModels";
import {
  GenericReportAssociated,
  NetworkElementAssociated,
} from "./LcmEngineering";
import {
  PlannedActivityResourceDto,
  PlannedActivityResourceDtoGrid,
} from "./LookUp/PlannedActivityResource";
import { SettingsUpdatePlannedActivityDtoUpdate } from "./SettingsUpdatePlannedActivity";
import {
  DaAssetMigrationDto,
  DeploymentStatus,
  TargetDesignComponentResource,
} from "./LookUp/AssetMigrationModels";
import { PlannedActivityDtoUpdate } from "./PlannedActivity";

export interface ServicePlanPageDto {
  deleted?: boolean;
  orphan?: boolean;
  lastModified?: Date;
  lastModifiedBy?: string;
  servicePlanId?: number;
  serviceMasterId?: number;
  serviceMaster?: string;
  opCoId?: number;
  opCo?: string;
  dcfId?: number;
  dcfName?: string;
  dcfIdList?: number[];
  program?: string;
  status?: string;
  serviceMasterResource?: { [key: string]: string };
  transientDesignComponentResource?: { [key: string]: string };
  plannedActivityCreateDto?: ServicePlanDtoCreate;
}

/**
 *
 * @export
 * @interface ServicePlanDto
 */
export interface ServicePlanDto extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof ServiceMasterDto
   */
  program?: string;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDto
   */
  serviceMasterId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDto
   */
  servicePlanId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDto
   */
  status?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDto
   */
  dcfId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDto
   */
  opCoId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDto
   */
  lcmEngineeringId?: number;

  /**
   *
   * @type {number}
   * @memberof ServicePlanDto
   */
  designAspectId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDto
   */
  networkElementAsPlannedId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDto
   */
  plannedImplementationYear?: number;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  plannedActivityDescription?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  activityDetails?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  deliveryProjectName?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  localApproval?: string;
  /**
   *
   * @type {Date}
   * @memberof ServicePlanDto
   */
  plannedCompletion?: Date;
  /**
   *
   * @type {Date}
   * @memberof ServicePlanDto
   */
  preBaseLineDate?: Date;

  /**
   *
   * @type {Date | string}
   * @memberof ServicePlanDto
   */
  startDate?: Date | string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  notes?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  riskEngineeringNotes?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  riskOperationalNotes?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  spareFieldsJson?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  deliveryProjectId?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  deliveryProjectPpmId?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  budgetTrackingId?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  currency?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  projectStatus?: string;
  /**
   *
   * @type {number | string}
   * @memberof ServicePlanDto
   */
  budgetValue?: number | string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  activityDetailsText?: string;
  /**
   *
   * @type {boolean}
   * @memberof ServicePlanDto
   */
  isNewServiceArchitecture?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof ServicePlanDto
   */
  isReplacementExistingSolution?: boolean;
  /**
   *
   * @type {Date}
   * @memberof ServicePlanDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDto
   */
  lastModifiedBy?: string;
}

/**
 *
 * @export
 * @interface ServiceLevelPlanDtoCreate
 */

export interface ServiceLevelPlanDtoCreate extends GridDtoBase {
  plannedActivityUpdateDto?: PlannedActivityDtoUpdate | null;
  plannedActivityCreateDto?: PlannedActivityDtoUpdate | null;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServiceLevelPlanDtoCreate
   */
  serviceMasterResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof ServiceLevelPlanDtoCreate
   */
  serviceMasterId?: number;
  /**
   *
   * @type {Array<number>}
   * @memberof ServiceLevelPlanDtoCreate
   */
  dcfIdList?: Array<number>;
  /**
   *
   * @type {number}
   * @memberof ServiceLevelPlanDtoCreate
   */
  servicePlanId?: number;
  /**
   *
   * @type {string}
   * @memberof ServiceLevelPlanDtoCreate
   */
  serviceMaster?: string;
  /**
   *
   * @type {number}
   * @memberof ServiceLevelPlanDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {string}
   * @memberof ServiceLevelPlanDtoCreate
   */
  opCo?: string;
  /**
   *
   * @type {number}
   * @memberof ServiceLevelPlanDtoCreate
   */
  dcfId?: number;
  /**
   *
   * @type {string}
   * @memberof ServiceLevelPlanDtoCreate
   */
  dcfName?: string;
  /**
   *
   * @type {string}
   * @memberof ServiceLevelPlanDtoCreate
   */
  program?: string;
  /**
   *
   * @type {string}
   * @memberof ServiceLevelPlanDtoCreate
   */
  status?: string;
  /**
   *
   * @type {number}
   * @memberof ServiceLevelPlanDtoCreate
   */
  plannedActivityId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServiceLevelPlanDtoCreate
   */
  dcfStatus?: { [key: string]: string };
}

/**
 *
 * @export
 * @interface ServiceLevelPlanDtoUpdate
 */

export interface ServiceLevelPlanDtoUpdate extends ServiceLevelPlanDtoCreate {
  /**
   *
   * @type {number}
   * @memberof ServiceLevelPlanDtoUpdate
   */
  servicePlanId?: number;
}
/**
 *
 * @export
 * @interface ServicePlanDtoCreate
 */

export interface ServicePlanDtoCreate extends ServicePlanDto {
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  deliveryStatusId?: number;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoCreate
   */
  deliveryStatusName?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoCreate
   */
  reasonForNoPlan?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoCreate
   */
  commentOnProjectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoCreate
   */
  currentBuildBagDescription?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoCreate
   */
  plannedBuildBagDescription?: string;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  activityStatusId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  planningActivityStatusId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  responsibilityPhaseId?: number;

  deliveryPlanAvailable?: boolean;
  isPAReleaseDetailUnknown?: boolean;

  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoCreate
   */
  plannedDesignComponentName?: string;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  plannedActivityResourceId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  relatesToId?: number;
  program?: string;
  projectOwner?: string;

  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  designComponentId?: number;
  designComponentId2?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  budgetAvailabilityId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  riskEngId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  opCoId?: number;

  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  designComponentFamilyId?: number;

  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  plannedDesignComponentFamilyId?: number;

  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  benefitId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  driverId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  planningRiskId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  riskOpeId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  linkedToPlannedActivityId?: number;
  /**
   *
   * @type {{ [key: string]: PlannedActivityResourceDto; }}
   * @memberof ServicePlanDtoCreate
   */
  plannedActivityResource?: { [key: string]: PlannedActivityResourceDto };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  deliveryStatusResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  activityStatusResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  planningActivityStatusResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  responsibilityPhaseResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  designComponentResource?: Array<{ key: number; value: string }>;
  designComponentFamilyResource?: any;
  plannedDcfResource?: any;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  transientDesignComponentResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: boolean; }}
   * @memberof ServicePlanDtoCreate
   */
  designComponentIsVirtualizedResource?: { [key: string]: boolean };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  budgetAvaibilityResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  riskResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  serviceMasterResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  driverResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  planningRiskResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof ServicePlanDtoCreate
   */
  benefitResource?: { [key: string]: string };

  /**
   *
   * @type {boolean}
   * @memberof ServicePlanDtoCreate
   */
  forAddAsset?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof ServicePlanDtoCreate
   */
  forLcmLink?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof ServicePlanDtoCreate
   */
  forDesignAspectLink?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof ServicePlanDtoCreate
   */
  forEditAsset?: boolean;

  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoCreate
   */
  plannedCompletionValue?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoCreate
   */
  startDateValue?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoCreate
   */
  preBaseLineDateValue?: string;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoCreate
   */
  buildBagId?: number;
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof ServicePlanDtoCreate
   */
  buildBagResources?: Array<{ key: number; text: string }>;

  budgetOwnerResource?: { [key: string]: string };
  budgetOwnerId?: number;
  plannedActivityTeamResource?: { [key: string]: string };
  plannedActivityTeamId?: number;
  plannedActivityCategoryResource?: { [key: string]: string };
  plannedActivityCategoryId?: number;
  lcmCategoryResource?: { [key: string]: string };
  lcmCategories?: number;
  priorityResource?: { [key: string]: string };
  priority?: string;
  plannedActivityCategoryName?: string;
  budgetOwner?: string;
  plannedActivityTeam?: string;
  programResource?: { [key: string]: string };
  programId?: number;
  designComponentFamilyIdList?: number[];
  activityStatusName?: string;
}
/**
 *
 * @export
 * @interface ServicePlanDtoGrid
 */
export interface ServicePlanDtoGrid extends ServicePlanDto {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoGrid
   */
  plannedActivity?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  program?: string;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoGrid
   */
  serviceMasterId?: number;
  servicePlanId?: any;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoGrid
   */
  status?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoGrid
   */
  dcfId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoGrid
   */
  opCoId?: number;
  /**
   *
   * @type {number}
   * @memberof ServicePlanDtoGrid
   */
  plannedActivityId?: number;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  deliveryStatusId?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  activityStatusId?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  planningActivityStatusId?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  responsibilityPhaseId?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  relatesToId?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  originalDesignComponent?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  designComponentId?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  lcmEngineering?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  plannedActivityResourceId?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  driver?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  benefits?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  planningRisk?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  budgetValueGrid?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  riskEngineeringEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  riskOperationalEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  budgetAvailability?: string;
  /**
   *
   * @type {string}
   * @memberof ServicePlanDtoGrid
   */
  opCo?: string;
  plannedActivityTypeFor?: number;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfPlannedActivityDtoGrid
 */
export interface QueryResultDtoOfServicePlanDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfServicePlanDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<ServicePlanDtoGrid>}
   * @memberof QueryResultDtoOfServicePlanDtoGrid
   */
  items?: Array<ServicePlanDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfPlannedActivityDtoGrid}
   * @memberof QueryResultDtoOfPlannedActivityDtoGrid
   */
  gridRender?: CustomGridRender;
}
export interface ServicePlanQueryObjectGrid extends QueryObject {
  serviceMasterId?: number[];
  servicePlanId?: number[];
  opCoId?: number[];
  dcfId?: number[];
  status?: number[];
  program?: string[];
}
export interface ServicePlanCreate {
  ServicePlanDtoCreate: ServiceLevelPlanDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface ServicePlanEdit {
  ServicePlanDtoEdit: ServiceLevelPlanDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}
export interface ServicePlanGrid {
  LookUpGridResult: QueryResultDtoOfServicePlanDtoGrid | null;
  LookUpGridResultAll: QueryResultDtoOfServicePlanDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_SERVICE_PLAN = "GET_CREATE_SERVICE_PLAN";
export const GET_EDIT_SERVICE_PLAN = "GET_EDIT_SERVICE_PLAN";
export const CREATE_SERVICE_PLAN = "CREATE_SERVICE_PLAN";
export const EDIT_SERVICE_PLAN = "EDIT_SERVICE_PLAN";
