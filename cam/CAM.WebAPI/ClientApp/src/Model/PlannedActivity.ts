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

/**
 *
 * @export
 * @interface CrossSettingsDto
 */
export interface CrossSettingsDto {
  /**
   *
   * @type {number}
   * @memberof CrossSettingsDto
   */
  input?: number;
  /**
   *
   * @type {number}
   * @memberof CrossSettingsDto
   */
  output?: number;
  /**
   *
   * @type {number}
   * @memberof CrossSettingsDto
   */
  rule?: number;
}

export interface GeneraticReportDto {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof GeneraticReportDto
   */
  opCoId?: number;
  elementName?: string;
  /**
   *
   * @type {Array<GenericReportAssociated>}
   * @memberof PlannedActivityForLinkDto
   */
  startGenericReportAssociated?: Array<GenericReportAssociated>;
  /**
   *
   * @type {Array<GenericReportAssociated>}
   * @memberof PlannedActivityForLinkDto
   */
  endGenericReportAssociated?: Array<GenericReportAssociated>;
}

export interface DaPlannedActivityDcfDto {
  dcfStatus: string;
  daPlannedActivityDcfId: number;
  plannedActivityId: number;
  designComponentfamilyId: number;
  designComponentFamilyName: string;
}

/**
 *
 * @export
 * @interface UpdatePlannedActivityStatusDto
 */
export interface UpdatePlannedActivityStatusDto {
  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  opCoId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof UpdatePlannedActivityStatusDto
   */
  opCoResource?: { [key: string]: string };

  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  plannedActivityTypeId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof UpdatePlannedActivityStatusDto
   */
  plannedActivityTypeResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  designComponentId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof UpdatePlannedActivityStatusDto
   */
  designComponentResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  plannedActivityId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof UpdatePlannedActivityStatusDto
   */
  plannedActivityResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  settingsUpdatePlannedActivityId?: number;
  /**
   *
   * @type {{ [key: string]: SettingsUpdatePlannedActivityDtoUpdate; }}
   * @memberof UpdatePlannedActivityStatusDto
   */
  settingsUpdatePlannedActivityResource?: {
    [key: string]: SettingsUpdatePlannedActivityDtoUpdate;
  };

  plannedActivityTypeFor?: number;

  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  planningActivityDetailsResourceId?: number;
  /**
   *
   * @type {{ [key: string]: SettingsUpdatePlannedActivityDtoUpdate; }}
   * @memberof UpdatePlannedActivityStatusDto
   */
  planningActivityDetailsResource?: {
    [key: string]: string;
  };
  /**
   *
   * @type {boolean}
   * @memberof UpdatePlannedActivityStatusDto
   */
  inEngineeringPhase?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof UpdatePlannedActivityStatusDto
   */
  elementCount?: boolean;
  /**
   *
   * @type {Array<NetworkElementAssociated>}
   * @memberof UpdatePlannedActivityStatusDto
   */
  startNodesInProd?: Array<NetworkElementAssociated>;
  /**
   *
   * @type {Array<NetworkElementAssociated>}
   * @memberof UpdatePlannedActivityStatusDto
   */
  startNodesInLab?: Array<NetworkElementAssociated>;
  /**
   *
   * @type {Array<NetworkElementAssociated>}
   * @memberof UpdatePlannedActivityStatusDto
   */
  endNodesInProd?: Array<NetworkElementAssociated>;
  /**
   *
   * @type {Array<NetworkElementAssociated>}
   * @memberof UpdatePlannedActivityStatusDto
   */
  endNodesInLab?: Array<NetworkElementAssociated>;
  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  ruleElementCount?: number;
  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  numberOfNodesInput?: number;
  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  numberOfNodesOutput?: number;
  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  numberOfNodesInLabInput?: number;
  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  numberOfNodesInLabOutput?: number;
  /**
   *
   * @type {boolean}
   * @memberof UpdatePlannedActivityStatusDto
   */
  isCrossSettings?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof UpdatePlannedActivityStatusDto
   */
  needPlannedAsset?: boolean;
  /**
   *
   * @type {number}
   * @memberof UpdatePlannedActivityStatusDto
   */
  ruleLinkedDc?: number;
  /**
   *
   * @type {Date}
   * @memberof UpdatePlannedActivityStatusDto
   */
  assetLiveStatusDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof UpdatePlannedActivityStatusDto
   */
  assetDecommissionedDate?: Date;
  /**
   *
   * @type {{ [key: string]: CrossSettingsDto; }}
   * @memberof UpdatePlannedActivityStatusDto
   */
  crossSettingscResource?: { [key: string]: CrossSettingsDto };

  daPlannedActivtyDcfDto?: DaPlannedActivityDcfDto[];
  infraClusterClusterUpgradeUpsertDto?: any;
  servicePlanDcfDto?: any;
}

/**
 *
 * @export
 * @interface ResultDtoOfUpdatePlannedActivityStatusDto
 */
export interface ResultDtoOfUpdatePlannedActivityStatusDto {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfUpdatePlannedActivityStatusDto
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfUpdatePlannedActivityStatusDto
   */
  info?: string;
  /**
   *
   * @type {UpdatePlannedActivityStatusDto}
   * @memberof ResultDtoOfUpdatePlannedActivityStatusDto
   */
  data?: UpdatePlannedActivityStatusDto;
}

/**
 *
 * @export
 * @interface PlannedActivityDto
 */
export interface PlannedActivityDto extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDto
   */
  lcmEngineeringId?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityDto
   */
  servicePlanid?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityDto
   */
  designAspectId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDto
   */
  networkElementAsPlannedId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDto
   */
  plannedImplementationYear?: number;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  plannedActivityDescription?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  activityDetails?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  deliveryProjectName?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  localApproval?: string;
  /**
   *
   * @type {Date}
   * @memberof PlannedActivityDto
   */
  plannedCompletion?: Date;
  /**
   *
   * @type {Date}
   * @memberof PlannedActivityDto
   */
  preBaseLineDate?: Date;

  /**
   *
   * @type {Date | string}
   * @memberof PlannedActivityDto
   */
  startDate?: Date | string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  notes?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  riskEngineeringNotes?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  riskOperationalNotes?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  spareFieldsJson?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  deliveryProjectId?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  deliveryProjectPpmId?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  budgetTrackingId?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  currency?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  projectStatus?: string;
  /**
   *
   * @type {number | string}
   * @memberof PlannedActivityDto
   */
  budgetValue?: number | string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  activityDetailsText?: string;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityDto
   */
  isNewServiceArchitecture?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityDto
   */
  isReplacementExistingSolution?: boolean;
  /**
   *
   * @type {Date}
   * @memberof PlannedActivityDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface PlannedActivityDtoCreate
 */
export interface PlannedActivityDtoCreate extends PlannedActivityDto {
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  deliveryStatusId?: number;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoCreate
   */
  deliveryStatusName?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoCreate
   */
  reasonForNoPlan?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoCreate
   */
  commentOnProjectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoCreate
   */
  currentBuildBagDescription?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoCreate
   */
  plannedBuildBagDescription?: string;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  activityStatusId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  planningActivityStatusId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  responsibilityPhaseId?: number;

  deliveryPlanAvailable?: boolean;
  isPAReleaseDetailUnknown?: boolean;

  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoCreate
   */
  plannedDesignComponentName?: string;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  plannedActivityResourceId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  relatesToId?: number;
  program?: string;
  projectOwner?: string;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  designComponentId?: number;
  designComponentId2?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  budgetAvailabilityId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  riskEngId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  opCoId?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  designComponentFamilyId?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  plannedDesignComponentFamilyId?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  benefitId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  driverId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  planningRiskId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  riskOpeId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  linkedToPlannedActivityId?: number;
  /**
   *
   * @type {{ [key: string]: PlannedActivityResourceDto; }}
   * @memberof PlannedActivityDtoCreate
   */
  plannedActivityResource?: { [key: string]: PlannedActivityResourceDto };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  deliveryStatusResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  activityStatusResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  planningActivityStatusResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  responsibilityPhaseResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  designComponentResource?: { [key: string]: string };
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
   * @memberof PlannedActivityDtoCreate
   */
  designComponentIsVirtualizedResource?: { [key: string]: boolean };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  budgetAvaibilityResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  riskResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  serviceMasterResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  driverResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  planningRiskResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoCreate
   */
  benefitResource?: { [key: string]: string };

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityDtoCreate
   */
  forAddAsset?: string[] | [];

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityDtoCreate
   */
  forLcmLink?: string[] | [];
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityDtoCreate
   */
  forDesignAspectLink?: string[] | [];

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityQueryDto
   */
  forServicePlanLink?: string[] | [];

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityDtoCreate
   */
  forEditAsset?: string[] | [];

  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoCreate
   */
  plannedCompletionValue?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoCreate
   */
  startDateValue?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoCreate
   */
  preBaseLineDateValue?: string;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoCreate
   */
  buildBagId?: number;
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof PlannedActivityDtoCreate
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
}

/**
 *
 * @export
 * @interface PlannedActivityDtoGrid
 */
export interface PlannedActivityDtoGrid extends PlannedActivityDto {
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoGrid
   */
  plannedActivityId?: number;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  deliveryStatusId?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  activityStatusId?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  planningActivityStatusId?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  responsibilityPhaseId?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  relatesToId?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  originalDesignComponent?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  designComponentId?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  lcmEngineering?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  plannedActivityResourceId?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  driver?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  benefits?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  planningRisk?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  budgetValueGrid?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  riskEngineeringEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  riskOperationalEvaluation?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  budgetAvailability?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityDtoGrid
   */
  opCo?: string;
  plannedActivityTypeFor?: number;
}
/**
 *
 * @export
 * @interface PlannedActivityDtoUpdate
 */

export interface PlaftformMigrationDcfResources {
  daAssetMigrationDtoGrid?: DaAssetMigrationDto[];
  environmentReosurce?: Record<string, string>;
  deploymentStatusReosurce?: Record<string, DeploymentStatus>;
  locationReosurce?: Record<string, string>;
  targetDesignComponentResource?: TargetDesignComponentResource[];
  existingAssetResource?: { key: number; value: string }[];
}

export interface PlannedActivityDtoUpdate extends PlannedActivityDtoCreate {
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoUpdate
   */
  serviceMasterId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityDtoUpdate
   */
  serviceMasterResource?: { [key: string]: string };
  isServicePlan?: boolean;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityDtoUpdate
   */
  plannedActivityId?: number;
  /**
   *
   * @type {Array<NetworkElementAssociated>}
   * @memberof PlannedActivityDtoUpdate
   */
  selectedNodes?: Array<NetworkElementAssociated>;
  projectDescription?: string;
  plaftformMigrationDcfResources?: PlaftformMigrationDcfResources;
  infraClusterClusterUpgradeUpsertDto?: any;
  plannedHardwareTypeId?: any;
  servicePlanDetails?: any;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfPlannedActivityDtoGrid
 */
export interface QueryResultDtoOfPlannedActivityDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfPlannedActivityDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<PlannedActivityDtoGrid>}
   * @memberof QueryResultDtoOfPlannedActivityDtoGrid
   */
  items?: Array<PlannedActivityDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfPlannedActivityDtoGrid}
   * @memberof QueryResultDtoOfPlannedActivityDtoGrid
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @export
 * @interface ResultDtoOfPlannedActivityForLinkDto
 */
export interface ResultDtoOfPlannedActivityForLinkDto {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfPlannedActivityForLinkDto
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfPlannedActivityForLinkDto
   */
  info?: string;
  /**
   *
   * @type {PlannedActivityForLinkDto}
   * @memberof ResultDtoOfPlannedActivityForLinkDto
   */
  data?: PlannedActivityForLinkDto;
}

/**
 *
 * @export
 * @interface PlannedActivityMigrationsDto
 */
export interface PlannedActivityMigrationsDto {
  /**
   *
   * @type {number}
   * @memberof PlannedActivityMigrationsDto
   */
  plannedActivityId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityMigrationsDto
   */
  linkedPlannedActivityId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityMigrationsDto
   */
  numberOfNodes?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityMigrationsDto
   */
  numberOfLabNodes?: number;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityMigrationsDto
   */
  networkElementStart?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityMigrationsDto
   */
  networkElementEnd?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityMigrationsDto
   */
  designComponentIdStart?: number;
}

/**
 *
 * @export
 * @interface PlannedActivityForLinkDto
 */
export interface PlannedActivityForLinkDto {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityForLinkDto
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof PlannedActivityForLinkDto
   */
  opCoId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityForLinkDto
   */
  designComponentResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof PlannedActivityForLinkDto
   */
  designComponentId?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityForLinkDto
   */
  numberOfNodes?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityForLinkDto
   */
  numberOfLabNodes?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityForLinkDto
   */
  plannedActivityId?: number;
  /**
   *
   * @type {{ [key: string]: PlannedActivityToConnect; }}
   * @memberof PlannedActivityForLinkDto
   */
  plannedActivityResource?: { [key: string]: PlannedActivityToConnect };
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityForLinkDto
   */
  elementCount?: boolean;
  /**
   *
   * @type {Array<NetworkElementAssociated>}
   * @memberof PlannedActivityForLinkDto
   */
  startNetworkElementAssociateds?: Array<NetworkElementAssociated>;
  /**
   *
   * @type {Array<NetworkElementAssociated>}
   * @memberof PlannedActivityForLinkDto
   */
  endNetworkElementAssociateds?: Array<NetworkElementAssociated>;
}

/**
 *
 * @export
 * @interface PlannedActivityToConnect
 */
export interface PlannedActivityToConnect {
  /**
   *
   * @type {string}
   * @memberof PlannedActivityToConnect
   */
  plannedActivityName?: string;
  /**
   *
   * @type {{ [key: string]: PlannedActivityToConnectData; }}
   * @memberof PlannedActivityToConnect
   */
  linkedToPlannedActivity?: { [key: string]: PlannedActivityToConnectData };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityToConnect
   */
  linkedDesignComponent?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof PlannedActivityForLinkDto
   */
  numberOfNodes?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityForLinkDto
   */
  numberOfLabNodes?: number;
}

/**
 *
 * @export
 * @interface PlannedActivityToConnectData
 */
export interface PlannedActivityToConnectData {
  /**
   *
   * @type {string}
   * @memberof PlannedActivityToConnectData
   */
  name?: string;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityToConnectData
   */
  numberOfNode?: number;
}

/**
 *
 * @export
 * @interface ResultDtoOfDictionaryOfLongAndPlannedActivityToConnectData
 */
export interface ResultDtoOfDictionaryOfLongAndPlannedActivityToConnectData {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfDictionaryOfLongAndPlannedActivityToConnectData
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfDictionaryOfLongAndPlannedActivityToConnectData
   */
  info?: string;
  /**
   *
   * @type {{ [key: string]: PlannedActivityToConnectData; }}
   * @memberof ResultDtoOfDictionaryOfLongAndPlannedActivityToConnectData
   */
  data?: { [key: string]: PlannedActivityToConnectData };
}

// ------------------------ Not generated  ------------------------

export interface PlannedActivityEdit {
  PlannedActivityDtoEdit: PlannedActivityDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface PlannedActivityCreate {
  PlannedActivityDtoCreate: PlannedActivityDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface PlannedActivityGrid {
  PlannedActivityGridResult: QueryResultDtoOfPlannedActivityDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface PlannedActivityQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  plannedImplementationYear?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  activityStatusId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  buildBagId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  planningActivityStatusId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  plannedActivityResourceId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  designComponentId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  designComponentFamilyid?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  plannedDesignComponentFamilyId?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  originalDesignComponent?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  driver?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  benefits?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  plannedActivityDescription?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  activityDetailsText?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  budgetAvailability?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  deliveryProjectName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  localApproval?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  deliveryStatusId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  responsibilityPhaseId?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof PlannedActivityQueryDto
   */
  plannedCompletion?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof PlannedActivityQueryDto
   */
  preBaseLineDate?: DateFilter;

  /**
   *
   * @type {DateFilter}
   * @memberof PlannedActivityQueryDto
   */
  startDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof PlannedActivityQueryDto
   */
  assetLiveStatusDateValue?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof PlannedActivityQueryDto
   */
  dateAssetDecommissionedAssetValue?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  notes?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  lcmEngineeringId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  riskEngineeringEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  riskEngineeringNotes?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  riskOperationalEvaluation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  riskOperationalNotes?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  relatesToId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  budgetValueGrid?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  planningRisk?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  deliveryProjectId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  deliveryProjectPpmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  budgetTrackingId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  projectStatus?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityQueryDto
   */
  opCo?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof PlannedActivityQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   ** @type {boolean}
   * @memberof PlannedActivityQueryDto
   */

  forDesignAspect?: boolean;
  /**
   ** @type {boolean}
   * @memberof PlannedActivityQueryDto
   */

  forServicePlan?: boolean;
  /**
   * @type {boolean}
   * @memberof PlannedActivityQueryDto
   */

  forLcm?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityQueryDto
   */
  forNetwork?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityQueryDto
   */
  forAddAsset?: string[] | [];
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityQueryDto
   */
  forLcmLink?: string[] | [];
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityQueryDto
   */
  forDesignAspectLink?: string[] | [];
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityQueryDto
   */
  AddEditAssetFilter?: string[] | [];

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityQueryDto
   */
  forServicePlanLink?: string[] | [];

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityQueryDto
   */
  forEditAsset?: string[] | [];
  /**
   *
   * @type {Array<boolean>}
   * @memberof PlannedActivityQueryDto
   */
  isNewServiceArchitecture?: Array<boolean>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof PlannedActivityQueryDto
   */
  isReplacementExistingSolution?: Array<boolean>;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityQueryDto
   */
  archived?: boolean;

  plannedDesignComponent?: Array<number>;
}

export interface PlannedActivityConfirmationDto {
  designComponentName: string;
  dcfId: number[];
  opcoId: number;
  opcoName: string;
  lcmId: string;
  lcmPaId: string;
}

export interface StatusKeyPairValue {
  [key: number]: string;
}

export interface DaMigrationStatusDtoGrid {
  daMigrationStatusId: number;
  opco: string;
  opcoId: number;
  location: string;
  locationId: number;
  status: string;
  statusId: number;
  migratonStatus: StatusKeyPairValue;
  plannedActivityId: number;
  deleted: boolean | null;
  orphan: boolean | null;
  lastModified: string;
  lastModifiedBy: string;
}

export interface PaWithDaMigrationApiResponse {
  statusKeyPairValue: StatusKeyPairValue;
  daMigrationStatusDtoGrids: DaMigrationStatusDtoGrid[];
}

export interface PaWhenDaMigrationCompleteApiResponse {
  daAssetMigrationDtoGrid: DaMigrationStatusDtoGrid[];
}

export const GET_CREATE_PLANNED_ACTIVITY = "GET_CREATE_PLANNED_ACTIVITY";
export const GET_EDIT_PLANNED_ACTIVITY = "GET_EDIT_PLANNED_ACTIVITY";
export const GET_GRID_PLANNED_ACTIVITY = "GET_GRID_PLANNED_ACTIVITY";
export const GET_FILTER_PLANNED_ACTIVITY = "GET_FILTER_PLANNED_ACTIVITY";
export const CREATE_PLANNED_ACTIVITY = "CREATE_PLANNED_ACTIVITY";
export const EDIT_PLANNED_ACTIVITY = "EDIT_PLANNED_ACTIVITY";
export const DELETE_PLANNED_ACTIVITY = "DELETE_PLANNED_ACTIVITY";
export const RESTORE_PLANNED_ACTIVITY = "RESTORE_PLANNED_ACTIVITY";
export const GET_UPDATE_DA_MigrationRecords = "GET_UPDATE_DA_MigrationRecords";
