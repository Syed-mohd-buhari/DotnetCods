import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  GridDtoBase,
  QueryObjectGrid,
  RenderDetail,
} from "../Common";
import { ResultDto } from "../CommonModels";
import { TipologicaGridDto } from "./LookUpGenericModel";

/**
 *
 * @export
 * @interface PlannedActivityResourceDto
 */
export interface PlannedActivityResourceDto {
  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  plannedActivityResourceId?: number;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  lcmLabelSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  lcmLabelHardware?: string;

  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  designAspectLabelSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  designAspectLabelHardware?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  networkElementLabelSoftware?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  networkElementLabelHardware?: string;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  lcmHardware?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  lcmSoftware?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  networkElementHardware?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  networkElementSoftware?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  addAssetHardware?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  editAssetHardware?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  addAssetSoftware?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  editAssetSoftware?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  exportable?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  serviceExportable?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  designAspectExportable?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  forLcm?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  forserviceplan?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  forDesignAspect?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  forEditAsset?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  forAddAsset?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  forNetworkElement?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  plannedDesignComponentRequiredNetworkElement?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  plannedDesignComponentRequiredAddAsset?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  plannedDesignComponentRequiredEditAsset?: boolean;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  ruleActicvityDetails?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  ruleDesignAspect?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  ruleService?: number;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  ruleLinkedDc?: number;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  ruleLinkedDcPlannedActivityTypeDescription?: string;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  plannedActivityTypesId?: number;

  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  plannedActivityResourceDescription?: string;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  ruleNetworkElement?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  ruleAddAsset?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  ruleEditAsset?: number;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  driverTextLcm?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  driverTextDesignAspect?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  driverTextServicePlan?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  driverTextEditAsset?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  driverTextAddAsset?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  benefitTextLcm?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  benefitTextDesignAspect?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  benefitTextServicePlan?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  benefitTextAddAsset?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  benefitTextEditAsset?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  planningRisksLcm?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  planningRisksDesignAspect?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  planningRiskServicePlan?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  driverTextNetworkElement?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  benefitTextNetworkElement?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  planningRisksNetworkElement?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */
  planningRisksAddAsset?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof PlannedActivityResourceDto
   */

  planningRisksAEditAsset?: Array<number>;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  onBareMetalNetworkElement?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  onBareMetalAddAsset?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  onVirtualizedAddAsset?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  onVirtualizedEditAsset?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  onBareMetalEditAsset?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  designAspectSoftware?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  designAspectHardware?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  onVirtualizedNetworkElement?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  forCreateNetworkElement?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof PlannedActivityResourceDto
   */
  forEditNetworkElement?: boolean;
  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  ruleActicvityDetailsNetworkElement?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  ruleActicvityDetailsAddAsset?: number;

  /**
   *
   * @type {number}
   * @memberof PlannedActivityResourceDto
   */
  ruleActicvityDetailsEditAsset?: number;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  activityDetailsNetworkElement?: string;

  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  activityDetailsAddAsset?: string;

  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  activityDetailsEditAsset?: string;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  activityDetailsLcm?: string;

  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  activityDetailsDesignAspect?: string;

  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  activityDetailsService?: string;

  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  activityDetailsForVirtualizedNetworkElement?: string;

  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  activityDetailsForVirtualizedAddAsset?: string;

  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  activityDetailsForVirtualizedEditAsset?: string;
  /**
   *
   * @type {{ [key: string]: TipologicaGridDtoForVirtualized; }}
   * @memberof PlannedActivityResourceDto
   */
  activityDetailsResource?: { [key: string]: TipologicaGridDtoForVirtualized };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityResourceDto
   */
  benefitResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityResourceDto
   */
  driverResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof PlannedActivityResourceDto
   */
  planningRiskResource?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  jsonForm?: string;
  /**
   *
   * @type {Date}
   * @memberof PlannedActivityResourceDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof PlannedActivityResourceDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface PlannedActivityResourceDtoGrid
 */
export interface PlannedActivityResourceDtoGrid extends GridDtoBase {
  plannedActivityResourceId?: number;
  plannedActivityResourceDescription?: string;
  forLcm?: boolean;
  forAddAsset?: boolean;
  forDesignAspect?: boolean;
  exportable?: boolean;
  ruleLinkedDc?: number;
  ruleLinkedDcPlannedActivityTypeDescription?: string;
  ruleActicvityDetails?: number;
  activityDetailsLcm?: string;
  lcmHardware?: boolean;
  lcmLabelHardware?: string;
  lcmSoftware?: boolean;
  lcmLabelSoftware?: string;
  driverTextLcm?: string;
  benefitTextLcm?: string;
  planningRisksLcm?: string;
  ruleActicvityDetailsAddAsset?: number;
  activityDetailsAddAsset?: string;
  activityDetailsForVirtualizedAddAsset?: string;
  ruleAddAsset?: number;
  plannedDesignComponentRequiredAddAsset?: boolean;
  addAssetHardware?: boolean;
  addAssetLabelHardware?: string;
  addAssetSoftware?: boolean;
  addAssetLabelSoftware?: string;
  driverTextAddAsset?: string;
  benefitTextAddAsset?: string;
  planningRisksAddAsset?: string;
  forCreateAddAsset?: boolean;
  forEditAddAsset?: boolean;
  onBareMetalAddAsset?: boolean;
  onVirtualizedAddAsset?: boolean;
  forEditAsset?: boolean;
  ruleActicvityDetailsEditAsset?: number;
  activityDetailsEditAsset?: string;
  activityDetailsForVirtualizedEditAsset?: string;
  ruleEditAsset?: number;
  plannedDesignComponentRequiredEditAsset?: boolean;
  editAssetHardware?: boolean;
  editAssetLabelHardware?: string;
  editAssetSoftware?: boolean;
  editAssetLabelSoftware?: string;
  driverTextEditAsset?: string;
  benefitTextEditAsset?: string;
  planningRisksEditAsset?: string;
  forCreateEditAsset?: boolean;
  forEditEditAsset?: boolean;
  onBareMetalEditAsset?: boolean;
  onVirtualizedEditAsset?: boolean;
  designAspectHardware?: boolean;
  designAspectSoftware?: boolean;
  activityDetailsDesignAspect?: string;
  ruleDesignAspect?: number;
  designAspectLabelSoftware?: string;
  designAspectLabelHardware?: string;
  driverTextDesignAspect?: string;
  benefitTextDesignAspect?: string;
  planningRisksDesignAspect?: string;
  designAspectExportable?: boolean;
  lastModified?: Date;
  lastModifiedBy?: string;
  jsonFormResource?: string;
}

/**
 *
 * @export
 * @interface TipologicaGridDtoForVirtualized
 */
export interface TipologicaGridDtoForVirtualized extends TipologicaGridDto {
  /**
   *
   * @type {boolean}
   * @memberof TipologicaGridDtoForVirtualized
   */
  forVirtualized?: boolean;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfPlannedActivityResourceDtoGrid
 */
export interface QueryResultDtoOfPlannedActivityResourceDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfPlannedActivityResourceDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<PlannedActivityResourceDtoGrid>}
   * @memberof QueryResultDtoOfPlannedActivityResourceDtoGrid
   */
  items?: Array<PlannedActivityResourceDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfPlannedActivityResourceDtoGrid}
   * @memberof QueryResultDtoOfPlannedActivityResourceDtoGrid
   */
  gridRender?: CustomGridRenderOfPlannedActivityResourceDtoGrid;
}
/**
 *
 * @export
 * @interface CustomGridRenderOfPlannedActivityResourceDtoGrid
 */
export interface CustomGridRenderOfPlannedActivityResourceDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfPlannedActivityResourceDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfPlannedActivityResourceDtoGrid
   */
  render?: Array<RenderDetail>;
}

//#region no auto gen

export interface PlannedActivityResourceQueryObjectGrid
  extends QueryObjectGrid {
  plannedActivityResourceId?: Array<number>;
  plannedActivityResourceDescription?: Array<string>;
  activityDetailsNetworkElement?: Array<string>;
  activityDetailsAddAsset?: Array<string>;
  activityDetailsEditAsset?: Array<string>;
  activityDetailsLcm?: Array<string>;
  activityDetailsForVirtualizedNetworkElement?: Array<string>;
  activityDetailsForVirtualizedAddAsset?: Array<string>;
  activityDetailsForVirtualizedEditAsset?: Array<string>;
  jsonFormResource?: Array<string>;
  exportable?: Array<boolean>;
  lcmHardware?: Array<boolean>;
  lcmSoftware?: Array<boolean>;
  networkElementHardware?: Array<boolean>;
  networkElementSoftware?: Array<boolean>;
  addAssetHardware?: Array<boolean>;
  editAssetHardware?: Array<boolean>;
  addAssetSoftware?: Array<boolean>;
  editAssetSoftware?: Array<boolean>;
  plannedDesignComponentRequiredNetworkElement?: Array<boolean>;
  plannedDesignComponentRequiredAddAsset?: Array<boolean>;
  plannedDesignComponentRequiredEditAsset?: Array<boolean>;
  ruleActicvityDetails?: Array<number>;
  ruleLinkedDc?: Array<number>;
  ruleLinkedDcPlannedActivityTypeDescription?: Array<string>;
  ruleNetworkElement?: Array<number>;
  ruleAddAsset?: Array<number>;
  ruleEditAsset?: Array<number>;
  ruleActicvityDetailsNetworkElement?: Array<number>;
  ruleActicvityDetailsAddAsset?: Array<number>;
  ruleActicvityDetailsEditAsset?: Array<number>;
  forLcm?: boolean[];
  ruleDesignAspect?: Array<number>;
  activityDetailsDesignAspect?: Array<string>;
  designAspectExportable?: boolean[];
  forDesignAspect?: boolean[];
  designAspectHardware?: boolean[];
  forNetworkElement?: boolean[];
  forCreateNetworkElement?: boolean[];
  forEditNetworkElement?: boolean[];
  forAddAsset?: boolean[];
  forEditAsset?: boolean[];
  onBareMetalNetworkElement?: boolean[];
  onBareMetalAddAsset?: boolean[];
  onBareMetalEditAsset?: boolean[];
  addAssetLabelSoftware?: Array<string>;
  addAssetLabelHardware?: Array<string>;

  editAssetLabelSoftware?: Array<string>;
  editAssetLabelHardware?: Array<string>;
  forCreateAddAsset?: Array<boolean>;
  forEditAddAsset?: Array<boolean>;
  forCreateEditAsset?: Array<boolean>;
  forEditEditAsset?: Array<boolean>;

  onVirtualizedNetworkElement?: boolean[];
  onVirtualizedAddAsset?: boolean[];
  onVirtualizedEditAsset?: boolean[];
  lcmLabelSoftware?: Array<string>;
  lcmLabelHardware?: Array<string>;
  networkElementLabelSoftware?: Array<string>;
  networkElementLabelHardware?: Array<string>;

  designAspectLabelSoftware?: Array<string>;
  designAspectLabelHardware?: Array<string>;
  benefitTextDesignAspect?: Array<number>;
  benefitTextAddAsset?: Array<number>;
  benefitTextEditAsset?: Array<number>;
  driverTextDesignAspect?: Array<number>;

  driverTextEditAsset?: Array<number>;
  driverTextAddAsset?: Array<number>;

  designAspectSoftware?: Array<boolean>;

  benefitTextNetworkElement?: Array<number>;
  planningRisksNetworkElement?: Array<number>;
  planningRisksAddAsset?: Array<number>;
  planningRisksAEditAsset?: Array<number>;
  driverTextNetworkElement?: Array<number>;
  benefitTextLcm?: Array<number>;

  planningRisksLcm?: Array<number>;
  planningRisksDesignAspect?: Array<number>;
  driverTextLcm?: Array<number>;
  sortBy?: string;
  isSortAscending?: boolean;
  page?: number;
  pageSize?: number;
  lastModifiedStartDate?: Date;
  lastModifiedEndDate?: Date;
  principalId?: number;
  deleted?: boolean;
  orphan?: boolean;
}

export interface LookUpGridPlannedActivityResource {
  LookUpGridResult: QueryResultDtoOfPlannedActivityResourceDtoGrid | null;
  LookUpGridResultAll: QueryResultDtoOfPlannedActivityResourceDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface LookUpEditPlannedActivityResource {
  LookUpDtoEdit: PlannedActivityResourceDto | null;
  ResultDtoEdit: ResultDto | null;
}
export interface LookUpCreatePlannedActivityResource {
  LookUpDtoCreate: PlannedActivityResourceDto | null;
  ResultDtoCreate: ResultDto | null;
}

//#endregion

/**
 *
 * @export
 * @interface ResultDtoOfIDictionaryOfShortAndPlannedActivityResourceDto
 */
export interface ResultDtoOfIDictionaryOfShortAndPlannedActivityResourceDto {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfIDictionaryOfShortAndPlannedActivityResourceDto
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfIDictionaryOfShortAndPlannedActivityResourceDto
   */
  info?: string;
  /**
   *
   * @type {{ [key: string]: PlannedActivityResourceDto; }}
   * @memberof ResultDtoOfIDictionaryOfShortAndPlannedActivityResourceDto
   */
  data?: { [key: string]: PlannedActivityResourceDto };
}
