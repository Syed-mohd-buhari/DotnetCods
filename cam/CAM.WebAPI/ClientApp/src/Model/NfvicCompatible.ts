import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  GridDtoBase,
  QueryObject,
  DateFilter,
  CustomGridRender,
  QueryObjectGrid,
} from "./Common";
import { ResultDto } from "./CommonModels";

/**
 * Grid Query DTO for NFVI Compatibility
 */
export interface NFVICCompatibilityQueryDto extends QueryObjectGrid {
  lastModified?: DateFilter;
  lastModifiedValue?: DateFilter;
  plannedUpgrade?: DateFilter;
  oemId?: number[];
  market?: number[];
  application?: number[];
  domain?: number[];
  designComponent?: number[];
  currentVNF?: number[];
  minimumVNF?: number[];
  plannedVNF?: number[];
  status?: string[];
  deleiveryStatus?: number[];
  eduSpoc?: number[];
  subDomainSpoc?: number[];
  vodafoneNameId?:number[];
}

/**
 * Grid DTO for display
 */
export interface NFVICCompatibilityDtoGrid extends GridDtoBase {
  market: string;
  application: string;
  domain?: number[];
  designComponent: string;
  currentVNF: number;
  minimumVNF: number;
  plannedVNF: number;
  plannedUpgrade: string; // stored as string (ISO or display format)
  status: string;
  deleiveryStatus: string;
  eduSpoc: string;
  subDomainSpoc: string;
  oemId?: number[];
  vodafoneNameId?: number[];
}

/**
 * Query Result for Grid
 */
export interface QueryResultDtoOfNFVICCompatibilityDtoGrid {
  totalItems?: number;
  items?: NFVICCompatibilityDtoGrid[];
  gridRender?: CustomGridRender;
}

/**
 * DTO for creating entries
 */
export interface NFVICCompatibilityDtoCreate extends GridDtoBase {
  market: string;
  application: string;
  domain: string;
  designComponent: string;
  currentVNF: number;
  minimumVNF: number;
  plannedVNF: number;
  plannedUpgrade: string;
  status: string;
  deleiveryStatus: string;
  eduSpoc: string;
  subDomainSpoc: string;
}

/**
 * DTO for updating entries
 */
export interface NFVICCompatibilityDtoUpdate extends NFVICCompatibilityDtoCreate {}

/**
 * Store state for Redux
 */


/**
 * Redux Action Types
 */
export const GET_CREATE_NFVIC_COMPATIBILITY = "GET_CREATE_NFVIC_COMPATIBILITY";
export const GET_EDIT_NFVIC_COMPATIBILITY = "GET_EDIT_NFVIC_COMPATIBILITY";
export const GET_GRID_NFVIC_COMPATIBILITY = "GET_GRID_NFVIC_COMPATIBILITY";
export const GET_FILTER_NFVIC_COMPATIBILITY = "GET_FILTER_NFVIC_COMPATIBILITY";
export const CREATE_NFVIC_COMPATIBILITY = "CREATE_NFVIC_COMPATIBILITY";
export const EDIT_NFVIC_COMPATIBILITY = "EDIT_NFVIC_COMPATIBILITY";
export const DELETE_NFVIC_COMPATIBILITY = "DELETE_NFVIC_COMPATIBILITY";
export const RESTORE_NFVIC_COMPATIBILITY = "RESTORE_NFVIC_COMPATIBILITY";

/**
 * Represents a single VMware dropdown item
 */
export interface VmWareDropDown {
    key: number;
    value: string | null;
  }
  

  export interface VmWareDropdownInnerData {
    VmWareDropDown: VmWareDropDown[];
  }
  
 
  export interface VmWareDropdownPayload {
    warning: boolean;
    info: string;
    data: VmWareDropdownInnerData;
  }
  
  /**
   * Full response structure from /api/test/GetAllVmware
   */


  export interface VmWareDropdowns {
    vmwareMswPlaftform: Record<string, string>;
    opcoDropDown: Record<string, string>;
    verticalDropDown: Record<string, string>;
    oemDropDown: Record<string, string>;
    vfDropDown: Record<string, string>;
    plannedComplentionDropDown: Record<string, string>;
  }
  
  export interface VmWareDropdownResponse {
    warning: boolean;
    info: string;
    data: VmWareDropdowns;
  }
  

  export const GET_VMWARE_DROPDOWN = "GET_VMWARE_DROPDOWN";

  export interface NFVICCompatibilityGrid {
    NFVICCompatibilityGridResult: QueryResultDtoOfNFVICCompatibilityDtoGrid | null;
    filter: FilterValueDto[] | null;
    dropdowns?: VmWareDropdowns | null;
  }

  export interface NFVICompatibilityReportRequest extends QueryObjectGrid {
    lastModified?: DateFilter;
    lastModifiedValue?: DateFilter;
    plannedUpgrade?: DateFilter;
    market?: number[];
    application?: number[];
    domain?: number[];
    designComponent?: number[];
    currentVNF?: number[];
    minimumVNF?: number[];
    plannedVNF?: number[];
    status?: string[];
    deleiveryStatus?: number[];
    eduSpoc?: number[];
    subDomainSpoc?: number[];
    principalId?: number;
    deleted?: boolean;
    orphan?: boolean;
    lastModifiedBy?: string[];
    oemId?: number[];              
  vodafoneNameId?: number[];
  }
  interface verticalIdDto {
    value: string;
    text: string;
  }

  export interface NFVIStatus {
    opCoId: number;
    opco: string | null;
    vendorId: number;
    vendorName: string | null;
    verticalId: number;
    verticalName: string | null;
    vodafoneNameId: number;
    vodafoneName: string | null;
    plannedCompletionDataProvided: number;
    plannedCompletionDataNotProvided: number;
    complaint: number;
    swUpgradePlanOk: number;
    swUpgradeNoPlan: number;
    noMinVnfProvided: number;
    decommissioning: number;
    swUpgradePlanNotOk: number;
    noMinVnfProvidedForPlannedDc: number;
    overAllStatusCount: number;
    verticalIdDto?: verticalIdDto[];
  }
  
  export interface DropdownItem {
    value: string;
    text: string;
  }
  
  export interface NFVICompatibilityReportData {
    overAllNFVIStatus: NFVIStatus;
    opcoBasedNFVIStatus: NFVIStatus[];
    vendorBasedNFVIStatus: NFVIStatus[];
    verticalBasedNFVIStatus: NFVIStatus[];
    vfBasedNFVIStatus: NFVIStatus[];
    plannedCompletionBasedNFVIStatus: NFVIStatus;
    opCoFilterDropDown: DropdownItem[];
    oemFilterDropDown: DropdownItem[];
    verticalFilterDropDown: DropdownItem[];
    vfFilterDropDown: DropdownItem[];
    plannedCompletionFilterDropDown: DropdownItem[];
  }
  
  export interface NFVICompatibilityReportPayload {
    warning: boolean;
    info: string;
    data: NFVICompatibilityReportData;
  }
  
  export interface NFVICompatibilityReportResponse {
    warning: boolean;
    info: string;
    data: NFVICompatibilityReportPayload;
  }
  

  export const NFVIC_COMPATIBILITY_REPORT_REQUEST = "NFVIC_COMPATIBILITY_REPORT_REQUEST";
export const NFVIC_COMPATIBILITY_REPORT_SUCCESS = "NFVIC_COMPATIBILITY_REPORT_SUCCESS";
export const NFVIC_COMPATIBILITY_REPORT_FAILURE = "NFVIC_COMPATIBILITY_REPORT_FAILURE";
