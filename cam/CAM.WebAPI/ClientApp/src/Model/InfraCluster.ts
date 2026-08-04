import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { CustomGridRender, GridDtoBase, QueryObject } from "./Common";
import { ResultDto } from "./CommonModels";

/**
 *
 * @export
 * @interface InfraClusterDtoCreate
 */
export interface InfraClusterDtoCreate extends GridDtoBase {
  clusterUpgradeAddUpdateDto?: any;
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof LcmEngineeringDtoCreate
   */
  opcoReosurce?: Array<{ key: number; text: string }>;
  /**
   *
   * @type {Array<number>}
   * @memberof InfraClusterDtoCreate
   */
  opCoId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoCreate
   */
  opCoValue?: Array<string>;
  /**
   *
   * @type {Array<any>}
   * @memberof LcmEngineeringDtoCreate
   */
  locationReosurce?: Array<any>;
  /**
   *
   * @type {Array<number>}
   * @memberof InfraClusterDtoCreate
   */
  locationId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoCreate
   */
  locationValue?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof InfraClusterDtoCreate
   */
  site?: string;
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof LcmEngineeringDtoCreate
   */
  platfromResource?: Array<{ key: number; text: string }>;
  /**
   *
   * @type {Array<number>}
   * @memberof InfraClusterDtoCreate
   */
  platformId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoCreate
   */
  platformValue?: Array<string>;
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof LcmEngineeringDtoCreate
   */
  clusterTypeMswResource?: Array<{ key: number; text: string }>;
  /**
   *
   * @type {Array<number>}
   * @memberof InfraClusterDtoCreate
   */
  clustertypeId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoCreate
   */
  clustertypeValue?: Array<string>;
  /**
   *
   * @type {string}
   * @memberof InfraClusterDtoCreate
   */
  clusterName?: string;
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof LcmEngineeringDtoCreate
   */
  hardwareMhwResource?: Array<{ key: number; text: string }>;
  /**
   *
   * @type {Array<number>}
   * @memberof InfraClusterDtoCreate
   */
  hardwaretypeId?: Array<number>;
  plannedHardwareTypeId?: number | null;

  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoCreate
   */
  hardwaretypeValue?: Array<string>;
  /**
   *
   * @type {Array<{ key: number; value: string }>}
   * @memberof LcmEngineeringDtoCreate
   */
  deploymentStatusReosurce?: Array<{ key: number; value: string }>;
  /**
   *
   * @type {Array<number>}
   * @memberof InfraClusterDtoCreate
   */
  deploymentStatusId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoCreate
   */
  deploymentStatusValue?: Array<string>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof VolteKPIReportDto
   */
  verticalResponsibleResource?: { [key: string]: string };
  /**
   *
   * @type {Array<number>}
   * @memberof InfraClusterDtoCreate
   */
  verticalResponsibleId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoCreate
   */
  verticalResponsibleValue?: Array<string>;
  /**
   *
   * @type {number | null}
   * @memberof InfraClusterDtoCreate
   */
  paId?: number | null;
  deploymentStatuesResources?: any;
  applicationNames?: any;
}
/**
 *
 * @export
 * @interface InfraClusterDtoUpdate
 */
export interface InfraClusterDtoUpdate extends InfraClusterDtoCreate {
  /**
   *
   * @type {Array<number>}
   * @memberof InfraClusterDtoUpdate
   */
  infraClusterAsPlannedId?: Array<number>;
}
/**
 *
 * @export
 * @interface InfraClusterQueryDto
 */
export interface InfraClusterQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof InfraClusterDtoGrid
   */
  infraClusterAsPlannedId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoGrid
   */
  opCoValue?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoGrid
   */
  locationValue?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoGrid
   */
  site?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoGrid
   */
  platformValue?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoGrid
   */
  clustertypeValue?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoGrid
   */
  clusterName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoGrid
   */
  hardwaretypeValue?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoGrid
   */
  deploymentStatusValue?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof InfraClusterDtoGrid
   */
  verticalResponsibleValue?: Array<string>;
  /**
   *
   * @type {number}
   * @memberof InfraClusterDtoGrid
   */
  paId?: number;
}
/**
 *
 * @export
 * @interface InfraClusterDtoGrid
 */
export interface InfraClusterDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number | null}
   * @memberof InfraClusterDtoGrid
   */
  infraClusterAsPlannedId?: number | null;
  /**
   *
   * @type {number | null}
   * @memberof InfraClusterDtoGrid
   */
  opCoId?: number | null;
  /**
   *
   * @type {string | null}
   * @memberof InfraClusterDtoGrid
   */
  opCoValue?: string | null;
  /**
   *
   * @type {number | null}
   * @memberof InfraClusterDtoGrid
   */
  locationId?: number | null;
  /**
   *
   * @type {string | null}
   * @memberof InfraClusterDtoGrid
   */
  locationValue?: string | null;
  /**
   *
   * @type {string | undefined}
   * @memberof InfraClusterDtoGrid
   */
  site?: string | undefined;
  /**
   *
   * @type {number | null}
   * @memberof InfraClusterDtoGrid
   */
  platformId?: number | null;
  /**
   *
   * @type {string | null}
   * @memberof InfraClusterDtoGrid
   */
  platformValue?: string | null;
  /**
   *
   * @type {number | null}
   * @memberof InfraClusterDtoGrid
   */
  clustertypeId?: number | null;
  /**
   *
   * @type {string | null}
   * @memberof InfraClusterDtoGrid
   */
  clustertypeValue?: string | null;
  /**
   *
   * @type {string | undefined}
   * @memberof InfraClusterDtoGrid
   */
  clusterName?: string | undefined;
  /**
   *
   * @type {number | null}
   * @memberof InfraClusterDtoGrid
   */
  hardwaretypeId?: number | null;
  /**
   *
   * @type {string | null}
   * @memberof InfraClusterDtoGrid
   */
  hardwaretypeValue?: string | null;
  plannedHardwareTypeId?: number | null;
  /**
   *
   * @type {number | null}
   * @memberof InfraClusterDtoGrid
   */
  deploymentStatusId?: number | null;
  /**
   *
   * @type {string | null}
   * @memberof InfraClusterDtoGrid
   */
  deploymentStatusValue?: string | null;
  /**
   *
   * @type {number | null}
   * @memberof InfraClusterDtoGrid
   */
  verticalResponsibleId?: number | null;
  /**
   *
   * @type {string | null}
   * @memberof InfraClusterDtoGrid
   */
  verticalResponsibleValue?: string | null;
  /**
   *
   * @type {number | null}
   * @memberof InfraClusterDtoGrid
   */
  paId?: number | null;
}

// export interface InfraClusterGroup {
//   site: string;
//   infraClusterAsPlannedDtoGrid: InfraClusterDtoGrid[];
// }
// export interface ApiResponse {
//   InfraClusterGridResult: InfraClusterGroup[];
// }
/**
 *
 * @export
 * @interface QueryResultDtoOfInfraClusterDtoGrid
 */
export interface QueryResultDtoOfInfraClusterDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfInfraClusterDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<InfraClusterDtoGrid>}
   * @memberof QueryResultDtoOfInfraClusterDtoGrid
   */
  items?: Array<InfraClusterDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
  infraClusterAsPlannedDtoGrid?: Array<InfraClusterDtoGrid>;
}

export interface InfraClusterEdit {
  InfraClusterDtoEdit: InfraClusterDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface InfraClusterCreate {
  InfraClusterDtoCreate: InfraClusterDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface InfraClusterGrid {
  InfraClusterGridResult: QueryResultDtoOfInfraClusterDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_INFRA_CLUSTER = "GET_CREATE_INFRA_CLUSTER";
export const GET_EDIT_INFRA_CLUSTER = "GET_EDIT_INFRA_CLUSTER";
export const GET_GRID_INFRA_CLUSTER = "GET_GRID_INFRA_CLUSTER";
export const GET_FILTER_INFRA_CLUSTER = "GET_FILTER_INFRA_CLUSTER";
export const CREATE_INFRA_CLUSTER = "CREATE_INFRA_CLUSTER";
export const EDIT_INFRA_CLUSTER = "EDIT_INFRA_CLUSTER";
export const DELETE_INFRA_CLUSTER = "DELETE_INFRA_CLUSTER";
export const RESTORE_INFRA_CLUSTER = "RESTORE_INFRA_CLUSTER";
export const GET_GRID_UPDATE_HARDWARE_CLUSTER =
  "GET_GRID_UPDATE_HARDWARE_CLUSTER";
export const GET_CREATE_INFRA_CLUSTER_HARDWARE =
  "GET_CREATE_INFRA_CLUSTER_HARDWARE";
export const GET_GRID_PROGRAM_CLUSTER = "GET_GRID_PROGRAM_CLUSTER";
export const GET_CREATE_PROGRAM_CLUSTER = "GET_CREATE_PROGRAM_CLUSTER";
