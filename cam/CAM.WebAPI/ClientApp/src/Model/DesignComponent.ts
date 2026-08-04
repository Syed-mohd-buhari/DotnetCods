import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  QueryObjectGrid,
  GridDtoBase,
  QueryObject,
} from "./Common";
import { ResultDto } from "./CommonModels";
import { CustomGridRenderOfDesignComponentFamilyDtoGrid } from "./DesignComponentFamily";
import { LcmEngineeringDtoGrid } from "./LcmEngineering";
import { NetworkElementAsPlannedDtoGrid } from "./NetworkElementAsPlanned";
import { PlannedActivityDtoGrid } from "./PlannedActivity";
/**
 *
 * @export
 * @interface DesignComponentQueryDto
 */
export interface DesignComponentQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  designComponentFamily?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  designComponentFamilyId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  systemType?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  designComponentId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  equipmentManufacturer?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentQueryDto
   */
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentQueryDto
   */
  softwareReleaseNumber?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  hardwarePlatform?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentQueryDto
   */
  hardwareType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentQueryDto
   */
  serviceBoundary?: Array<string>;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfDesignComponentDtoGrid
 */
export interface QueryResultDtoOfDesignComponentDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfDesignComponentDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<DesignComponentDtoGrid>}
   * @memberof QueryResultDtoOfDesignComponentDtoGrid
   */
  items?: Array<DesignComponentDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfDesignComponentDtoGrid}
   * @memberof QueryResultDtoOfDesignComponentDtoGrid
   */
  gridRender?: CustomGridRenderOfDesignComponentFamilyDtoGrid;
}
/**
 *
 * @export
 * @interface DesignComponentDto
 */
export interface DesignComponentDto extends GridDtoBase {
  /**
   *
   * @type {boolean}
   * @memberof DesignComponentDto
   */
  gdprRelevant?: boolean;
}
/**
 *
 * @export
 * @interface DesignComponentDtoCreate
 */
export interface DesignComponentDtoCreate extends DesignComponentDto {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignComponentDtoCreate
   */
  systemTypeResource?: Array<{ key: number; value: string }>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof DesignComponentDtoCreate
   */
  subNetworkBoundaryResource?: Array<UnUsedSubnetworkBoundary>;
  /**
   *
   * @type {number}
   * @memberof DesignComponentDtoCreate
   */
  systemTypeId?: number;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentDtoCreate
   */
  subNetworkBoundaryIds?: Array<number>;
  /**
   *
   * @type {number}
   * @memberof DesignComponentDtoCreate
   */
  designComponentFamilyId?: number;

  /**
   *
   * @type {boolean}
   * @memberof DesignComponentDtoCreate
   */
  supportedAllServices: boolean;
}
/**
 *
 * @export
 * @interface DesignComponentDtoGrid
 */
export interface DesignComponentDtoGrid extends DesignComponentDto {
  /**
   *
   * @type {number}
   * @memberof DesignComponentDtoGrid
   */
  designComponentId?: number;
  /**
   *
   * @type {number}
   * @memberof DesignComponentDtoGrid
   */
  designComponentFamilyId?: number;
  /**
   *
   * @type {string}
   * @memberof DesignComponentDtoGrid
   */
  systemType?: string;
  /**
   *
   * @type {string}
   * @memberof DesignComponentDtoGrid
   */
  serviceBoundary?: string;
  /**
   *
   * @type {string}
   * @memberof DesignComponentDtoGrid
   */
  equipmentManufacturer?: string;
  /**
   *
   * @type {string}
   * @memberof DesignComponentDtoGrid
   */
  /**
   *
   * @type {string}
   * @memberof DesignComponentDtoGrid
   */
  softwareReleaseNumber?: string;
  /**
   *
   * @type {string}
   * @memberof DesignComponentDtoGrid
   */
  hardwarePlatform?: string;
  /**
   *
   * @type {string}
   * @memberof DesignComponentDtoGrid
   */
  hardwareType?: string;
  /**
   *
   * @type {Date}
   * @memberof DesignComponentDtoGrid
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof DesignComponentDtoGrid
   */
  lastModifiedBy?: string;
  vodafoneName?: string;
}
/**
 *
 * @export
 * @interface DesignComponentDtoUpdate
 */
export interface DesignComponentDtoUpdate extends DesignComponentDtoCreate {
  /**
   *
   * @type {number}
   * @memberof DesignComponentDtoUpdate
   */
  designComponentId?: number;

  supportedServiceIds?: Array<number>;

  /**
   *
    @type {{ [key: number]: string; }}
   * @memberof DesignComponentDtoUpdate
   */
  subNetworkSupportedServices?: { [key: number]: string };
}

/**
 *
 * @export
 * @interface ResultDtoOfListOfSystemAndServiceBoundary
 */
export interface ResultDtoOfListOfSystemAndServiceBoundary {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfListOfSystemAndServiceBoundary
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfListOfSystemAndServiceBoundary
   */
  info?: string;
  /**
   *
   * @type {Array<SystemAndServiceBoundary>}
   * @memberof ResultDtoOfListOfSystemAndServiceBoundary
   */
  data?: {
    supportedAllServices: boolean;
    swApplicationName: string;
    systemAndSubNetworkBoundaries: Array<SystemAndServiceBoundary>;
  };
}

/**
 *
 * @export
 * @interface SystemAndServiceBoundary
 */
export interface SystemAndServiceBoundary {
  /**
   *
   * @type {number}
   * @memberof SystemAndServiceBoundary
   */
  systemTypeId?: number;
  /**
   *
   * @type {string}
   * @memberof SystemAndServiceBoundary
   */
  systemSolutionName?: string;
  /**
   *
   * @type {number}
   * @memberof SystemAndServiceBoundary
   */
  subNetworkBoundaryId?: number;
  /**
   *
   * @type {string}
   * @memberof SystemAndServiceBoundary
   */
  subNetworkBoundaryName?: string;

  /**
   *
   * @type {string}
   * @memberof SystemAndServiceBoundary
   */
  subNetworkBoundaryAlias?: string;
}

export interface DesignComponentQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  designComponentFamily?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  designComponentFamilyId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  systemType?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  designComponentId?: Array<number>;
  productName?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  equipmentManufacturer?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentQueryDto
   */

  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentQueryDto
   */
  softwareReleaseNumber?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof DesignComponentQueryDto
   */
  hardwarePlatform?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentQueryDto
   */
  hardwareType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentQueryDto
   */
  systemTypeIdBasedVerticalValue?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof DesignComponentQueryDto
   */
  serviceBoundary?: Array<string>;
  //softwareApplicationType?: Array<number>;
}

/**
 *
 * @export
 * @interface ResultDtoOfImpactServiceBoundaryChanged
 */
export interface ResultDtoOfImpactServiceBoundaryChanged {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfImpactServiceBoundaryChanged
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfImpactServiceBoundaryChanged
   */
  info?: string;
  /**
   *
   * @type {ImpactServiceBoundaryChanged}
   * @memberof ResultDtoOfImpactServiceBoundaryChanged
   */
  data?: ImpactServiceBoundaryChanged;
}

export interface ResultDtoOfServiceBoundaryChanged {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfImpactServiceBoundaryChanged
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfImpactServiceBoundaryChanged
   */
  info?: string;
  /**
   *
   * @type {ImpactServiceBoundaryChanged}
   * @memberof ResultDtoOfImpactServiceBoundaryChanged
   */
  data?: SystemSolutionResultDto;
}

/**
 *
 * @export
 * @interface ImpactServiceBoundaryChanged
 */
export interface ImpactServiceBoundaryChanged {
  /**
   *
   * @type {Array<LcmEngineeringDtoGrid>}
   * @memberof ImpactServiceBoundaryChanged
   */
  engineeringDtoGrids?: Array<LcmEngineeringDtoGrid>;
  /**
   *
   * @type {Array<PlannedActivityDtoGrid>}
   * @memberof ImpactServiceBoundaryChanged
   */
  plannedActivityDtoGrids?: Array<PlannedActivityDtoGrid>;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof ImpactServiceBoundaryChanged
   */
  networkElementAsPlannedDtoGrids?: Array<NetworkElementAsPlannedDtoGrid>;
}

export interface SystemSolutionResultDto {
  supportedAllServices?: boolean;
  vodafoneName?: string;
  vodafoneNameId?: number;
  unUsedSubNetworkBoundaries?: Array<UnUsedSubnetworkBoundary>;
  usedSubNetworkBoundaries?: Array<UsedSubnetworkBoundary>;
}

export interface UsedSubnetworkBoundary {
  subNetworkBoundaryAlias?: string;
  subNetworkBoundaryId?: number;
  subNetworkBoundaryName?: string;
  systemSolutionName?: string;
  systemTypeId?: number;
}

export interface UnUsedSubnetworkBoundary {
  subNetworkBoundaryAlias: string;
  subNetworkBoundaryId: number;
  subNetworkBoundaryName: string;
  systemSolutionName: string;
  systemTypeId: number;
}
/**
 *
 * @param {Array<number>} [designComponentFamily]
 * @param {Array<number>} [systemType]
 * @param {Array<number>} [designComponentId]
 * @param {Array<number>} [equipmentManufacturer]
 * @param {Array<string>} [softwareReleaseNumber]
 * @param {Array<number>} [hardwarePlatform]
 * @param {Array<string>} [hardwareType]
 * @param {Array<string>} [lastModifiedBy]
 * @param {string} [sortBy]
 * @param {boolean} [isSortAscending]
 * @param {number} [page]
 * @param {number} [pageSize]
 * @param {Date} [lastModifiedStartDate]
 * @param {Date} [lastModifiedEndDate]
 * @param {number} [principalId]
 * @param {boolean} [deleted]
 * @param {boolean} [orphan]
 * @param {*} [options] Override http request option.
 * @throws {RequiredError}
 * @memberof DesignComponentApi
 */

export interface DesignComponentEdit {
  DesignComponentDtoEdit: DesignComponentDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface DesignComponentCreate {
  DesignComponentDtoCreate: DesignComponentDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface DesignComponentGrid {
  DesignComponentGridResult: QueryResultDtoOfDesignComponentDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_DESIGN_COMPONENT = "GET_CREATE_DESIGN_COMPONENT";
export const GET_EDIT_DESIGN_COMPONENT = "GET_EDIT_DESIGN_COMPONENT";
export const GET_GRID_DESIGN_COMPONENT = "GET_GRID_DESIGN_COMPONENT";
export const GET_FILTER_DESIGN_COMPONENT = "GET_FILTER_DESIGN_COMPONENT";
export const CREATE_DESIGN_COMPONENT = "CREATE_DESIGN_COMPONENT";
export const EDIT_DESIGN_COMPONENT = "EDIT_DESIGN_COMPONENT";
export const DELETE_DESIGN_COMPONENT = "DELETE_DESIGN_COMPONENT";
export const RESTORE_DESIGN_COMPONENT = "RESTORE_DESIGN_COMPONENT";
export const GET_DESIGN_COMPONENT_NAME = "GET_DESIGN_COMPONENT_NAME";
export const GET_DESIGN_COMPONENT_CONSTRAINT_INFO =
  "GET_DESIGN_COMPONENT_CONSTRAINT_INFO";
