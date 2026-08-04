import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { DateFilter, QueryObject, RenderDetail } from "../Common";
import { QueryResultDtoOfNetworkElementAsPlannedDtoGrid } from "../NetworkElementAsPlanned";

export const GET_GRID_ASSETS_DETAILS = "GET_GRID_ASSETS_DETAILS";
export const CREATE_ASSET_MIGRATION = "CREATE_ASSET_MIGRATION";

export const GET_GRID_ASSETS_RESULT = "GET_GRID_ASSETS_RESULT";

export const GET_FILTER_ASSETS_GRID_RESULT = "GET_FILTER_ASSETS_GRID_RESULT";

export interface AssetsDetailsGrid {
  AssetsDetailsGridResult: AssetMigrationApiResponse | null;
  filter: any | null;
  ResultDtoCreate?: any | null;
  daAssetMigrationDtoCreate?: DaAssetMigrationDtoCreate | null;
}

export interface DaAssetMigrationDto {
  uniqueId?: string;
  isDecommissioned?: boolean;
  deleted: boolean;
  orphan: boolean;
  lastModified: string;
  lastModifiedBy: string;
  daAssetMigrationId: number;
  plannedActivityId: number;
  networkElementAsPlannedId: number;
  oldAssetName: string;
  newelEmentName: string;
  targetDesignComponenet: string;
  targetDesignComponenetId: number;
  newEnvironmentId: number;
  newEnvironment: string;
  deploymentStatusId: number;
  deploymentStatus: string;

  newDeploymentStatusId?: number;
  newDeploymentStatus?: string;

  opcoId: number;
  opcoDesc: string;
  locationId: number;
  location: string;

  rfoDate: string;
  rfsDate: string;
  rfaDate: string;
  hwPoRaisedDate: string;
  hwPoArrivedDate: string;
  bomSubmittedDate: string;
  migrationCompletionDate: string;
  migrationStart: string;

  startOfAppIntegration: string;

  vecDate: string;

  trafficNodePercentage: string;

  oldEnvironment: string;
  oldDeploymentType: string;
  oldDeploymentStatus: string;
  uniqueIdForUi?: number;
}

export interface DaAssetMigrationDtoCreate {
  daAssetMigrationDtoGrid: DaAssetMigrationDto[];
  environmentResource: Record<string, string>;
  deploymentStatusResource: Record<string, DeploymentStatus>;
  locationResource: Record<string, string>;
  targetDesignComponentResource: TargetDesignComponentResource[];
}

export interface TargetDesignComponentResource {
  key: number;
  value: string;
}

export interface DeploymentStatus {
  deploymentStatusId: number;
  deploymentStatusDescription: string;
  rule: number;
  plannedActivityResourceAllowedId: number[];
  readOnlyPlannedActivity: boolean;
  checkPlannedActivity: boolean;
  defaultValue: boolean;
  lastModified: string;
  lastModifiedBy: string;
  plannedActivityResources: Record<string, string>;
}

export interface AssetMigrationApiResponse {
  daAssetMigrationDtoGrid: DaAssetMigrationDto[];
  environmentReosurce: Record<string, string>;
  deploymentStatusReosurce: Record<string, DeploymentStatus>;
  locationReosurce: Record<string, string>;
  targetDesignComponentResource: TargetDesignComponentResource[];
  existingAssetResource?: { key: number; value: string }[];
}

export interface DaAssetMigrationGrid extends QueryObject {
  isDecommissioned?: boolean;
  uniqueIdForUi?: number;
  daAssetMigrationId?: number[];
  plannedActivityId?: number[];
  networkElementAsPlannedId?: number[];
  newelEmentName?: string[];
  targetDesignComponenetId?: number[];
  environmentDesc?: number[];
  deploymentStatusDesc?: number[];
  opcoDesc?: number[];
  locationDesc?: number[];
  opcoId?: number[];
  rfoDate?: DateFilter;
  rfsDate?: DateFilter;
  migrationCompletionDate?: DateFilter;
  startOfAppIntegration?: DateFilter;
  migrationStart?: DateFilter;

  vecDate?: DateFilter;

  trafficNodePercentage?: string[];
  oldAssetName?: string[];
  currentDesignComponenet?: string[];
  targetDesignComponenet?: string[];
  newEnvironment?: string[];
  newDeploymentStatus?: string[];
  location?: string[];
  oldEnvironment?: string[];
  oldDeploymentType?: string[];
  oldDeploymentStatus?: string[];
  currentDcfId?: number[];
  uniqueId?: string;
  isPagination?: boolean;
}

export interface QueryResultDtoOfDAAssetMigrationDtoGrid {
  totalItems?: number;

  items?: Array<DaAssetMigrationGrid>;

  gridRender?: CustomGridRenderOfDAAssetsMigrationDtoGrid;
}

export interface CustomGridRenderOfDAAssetsMigrationDtoGrid {
  className?: string;

  render?: Array<RenderDetail>;
}

export interface DaAssetsMigrationGridContainer {
  DAAssetsMigrationGridResult: QueryResultDtoOfDAAssetMigrationDtoGrid | null;
  filter: FilterValueDto[] | null;
}
