import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { CustomGridRender, DateFilter, QueryObject } from "../Common";

export interface ExodusDTO extends QueryObject {
  lastModifiedValue?: DateFilter;

  opCo?: number[];
  site?: string[];
  siteName?: number[];
  vendor?: number[];
  vnfCnf?: number[];
  vendorNf?: string[];
  xnfInstance?: string[];
  usageOptional?: string[];
  xnfSizeCore?: number[];
  environment?: number[];
  status?: number[];
  iaasCaasInfraStackInitialGoLive?: number[];
  stackNameInitialGoLive?: number[];

  rfoReqdBy?: DateFilter;
  rfsReqdBy?: DateFilter;

  poRaised?: string[];
  poReqdByIfNotNa?: string[];
  clusterName?: string[];
  hardwareTypeOld?: string[];
  hardwareType?: string[];
  xnfSizeVcpu?: number[];
  iaasCaasInfraStackTarget?: number[];
  stackNameTarget?: number[];
  poReqdBy?: string[];

  startRfo?: DateFilter;
  rfs?: DateFilter;
  migrationComplete?: DateFilter;

  broadcomRelease?: string[];
  hardwareTypeTarget?: string[];
  nfSizeExpansionDcekpiValue?: number[];
  nfSizeDcekpiSauGbps?: number[];
  aciAvailable?: string[];
  power?: string[];
}

export interface ExodusGrid {
  opCoId?: number;
  locationId?: number;
  oemId?: number;
  livePlatformId?: number;
  productNameId?: number;
  stausId?: number;
  targetPlatformId?: number;
  environmentId?: number;
  fyQ1_24_25?: string;
  fyQ2_24_25?: string;
  fyQ3_24_25?: string;
  fyQ4_24_25?: string;

  fyQ1_25_26?: string;
  fyQ2_25_26?: string;
  fyQ3_25_26?: string;
  fyQ4_25_26?: string;

  fyQ1_26_27?: string;
  fyQ2_26_27?: string;
  fyQ3_26_27?: string;
  fyQ4_26_27?: string;

  fyQ1_27_28?: string;
  fyQ2_27_28?: string;
  fyQ3_27_28?: string;
  fyQ4_27_28?: string;

  fyQ1_28_29?: string;
  fyQ2_28_29?: string;
  fyQ3_28_29?: string;
  fyQ4_28_29?: string;

  fyQ1_29_30?: string;
  fyQ2_29_30?: string;
  fyQ3_29_30?: string;
  fyQ4_29_30?: string;
  opCo?: string;
  site?: string;
  siteName?: string;
  vendor?: string;
  vnfCnf?: string;
  vendorNf?: string;
  xnfInstance?: string;
  usageOptional?: string;

  xnfSizeCore?: number;
  environment?: string;
  status?: string;

  laasCaasInfraStackInitialGoLive?: string;
  stackNameInitialGoLive?: string;
  rfoReqdBy?: Date;
  rfsReqdBy?: Date;

  poRaised?: string;
  poReqdByIfNotNa?: string;
  clusterName?: string;
  hardwareTypeOld?: string;
  hardwareType?: string;

  xnfSizeVcpu?: number;
  laasCaasInfraStackTarget?: string;
  stackNameTarget?: string;
  poReqdBy?: string;
  startRfo?: Date;
  rfs?: Date;
  migrationComplete?: Date;

  broadcomRelease?: string;
  hardwareTypeTarget?: string;

  nfSizeExpansionDcekpiValue?: number;
  nfSizeDcekpiSauGbps?: number;

  aciAvailable?: string;
  power?: string;
}

export interface QueryResultDtoOfExodusReportDtoGrid {
  totalItems?: number;

  items?: Array<ExodusGrid>;

  gridRender?: CustomGridRender;
}

export interface ExodusReportGrid {
  ExodusReportGridResult: QueryResultDtoOfExodusReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_GRID_EXODUS_REPORT = "GET_GRID_EXODUS_REPORT";
export const GET_FILTER_EXODUS_REPORT = "GET_FILTER_EXODUS_REPORT";
