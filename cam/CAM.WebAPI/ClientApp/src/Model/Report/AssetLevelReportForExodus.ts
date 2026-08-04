import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, DateFilter, QueryObject } from "../Common";

export interface GetAssetLevelReportForExodusDTO extends QueryObject {
  lastModifiedValue?: DateFilter;
  daAssetMigrationId?: number[];
  plannedActivityId?: number[];
  networkElementAsPlannedId?: number[];
  newelEmentName?: string[];
  targetDesignComponenetId?: number[];
  environmentDesc?: number[];
  deploymentStatusDesc?: number[];
  opcoDesc?: number[];
  locationDesc?: number[];
  rfoDate?: DateFilter;
  rfsDate?: DateFilter;
  migrationCompletionDate?: DateFilter;
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
  opcoId?: number[];
  isPagination?: boolean;
  hwPoRaisedDate?: DateFilter;
  hwPoArrivedDate?: DateFilter;
  bomSubmittedDate?: DateFilter;
  rfaDate?: DateFilter;
  verticalName?: string[];
}

export interface ExodusAssetDetail {
  daAssetMigrationId: number;
  plannedActivityId: number;
  networkElementAsPlannedId: number;
  currentDcId: number | null;
  currentDesignComponenet: string | null;
  newelEmentName: string | null;
  targetDesignComponenet: string | null;
  targetDesignComponenetId: number | null;
  opcoId: number;
  opcoDesc: string | null;
  locationId: number;
  location: string | null;
  rfoDate: string | null;
  rfsDate: string | null;
  migrationCompletionDate: string | null;
  trafficNodePercentage: string | null;
  bomSubmittedDate: string | null;
  hwPoRaisedDate: string | null;
  hwPoArrivedDate: string | null;
  rfaDate: string | null;
  bomStartDate: string | null;
  plannedStartDate: string | null;
  pLannedDcfName: string | null;
  vecDate: string | null;
  startOfAppIntegration: string | null;
  migrationStart: string | null;
}

export interface ExodusLocationGroup {
  location: string | null;
  selectedOpco: number | null;
  selectedPlannedDcf: number | null;
  assetDetails: ExodusAssetDetail[];
}

export interface AssetLevelReportForExodusData {
  daAssetMigrationDetails: ExodusLocationGroup[];
}

export interface AssetLevelReportForExodusData {
  daAssetMigrationDetails: ExodusLocationGroup[];
  filteredOpcoAndDcfDropdown?: OpcoDcfDropdownResponse;
}

export interface AssetLevelReportForExodusResponse {
  warning: boolean;
  info: string;
  data: AssetLevelReportForExodusData;
}

export const GET_OPCO_DCF_DROPDOWN = "GET_OPCO_DCF_DROPDOWN";

export interface DropdownResource {
  key: number;
  text: string;
}

export interface OpcoDcfDropdownData {
  plannedDcfDropdown: DropdownResource[];
  opcoDropdown: DropdownResource[];
}

export interface OpcoDcfDropdownResponse {
  warning: boolean;
  info: string;
  data: OpcoDcfDropdownData;
}

export interface AssetLevelExodusGrid {
  AssetLevelExodusResult: AssetLevelReportForExodusResponse | null;
  OpcoDcfDropdownResult: OpcoDcfDropdownResponse | null;
  filter: FilterValueDto[] | null;
}

export const GET_GRID_ASSET_LEVEL_EXODUS = "GET_GRID_ASSET_LEVEL_EXODUS";
export const GET_FILTER_ASSET_LEVEL_EXODUS = "GET_FILTER_ASSET_LEVEL_EXODUS";

export interface AssetLevelExodusGrid {
  AssetLevelExodusResult: AssetLevelReportForExodusResponse | null;
  filter: FilterValueDto[] | null;
}
