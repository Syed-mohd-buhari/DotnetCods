/* eslint-disable @typescript-eslint/consistent-type-assertions */
import { ResultDto } from "./CommonModels";

/**
 *
 * @export
 * @interface DateFilter
 */
export interface DateFilter {
  /**
   *
   * @type {Date}
   * @memberof DateFilter
   */
  startDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof DateFilter
   */
  endDate?: Date;
}

export interface DataModalConfirm {
  title: string;
  button?: string;
  buttonSecond?: string;
  onlyOneButton?: boolean;
  cancelText?: string;
  message: string;
  item: number | string;
  isOpen: boolean;
  footerAlert?: boolean;
  actions: {
    confirm?(): any;
    confirmSecond?(): any;
    cancel?(): any;
    logout?(): any;
    changemode?(): any;
  };
}
export interface DataModeOrLogoutConfirm {
  title: string;
  button?: string;
  buttonSecond?: string;
  onlyOneButton?: boolean;
  cancelText?: string;
  message: string;
  item: number | string;
  isOpen: boolean;
  actions: {
    cancel(): any;
  };
}

export enum EOM_STATUS {
  "NOT_ANNOUNCED" = 0,
  "NOT_SPECIFIED" = 1,
  "DEFAULT" = 2,
}

/**
 *
 * @export
 * @interface ResultDtoOfLong
 */
export interface ResultDtoOfLong {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfLong
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfLong
   */
  info?: string;
  /**
   *
   * @type {number}
   * @memberof ResultDtoOfLong
   */
  data?: number;
}

export const stateConfirm = {
  title: "",
  button: "",
  message: "",
  item: "",
  isOpen: false,
  actions: {
    cancel: () => {
      return;
    },
    confirm: () => {
      return;
    },
  },
};
export const stateDataModeOrLogoutConfirm = {
  title: "",
  button: "",
  message: "",
  item: "",
  isOpen: false,
  actions: {
    cancel: () => {
      return;
    },
    logout: () => {
      return;
    },
    changemode: () => {
      return;
    },
  },
};

export const DataRemediationConfirm = {
  title: "Data Remediation",
  button: "Proceed",
  message:
    "Are you sure you want apply data remediation? all duplicates selected will be definitely removed",
  item: 0,
  isOpen: false,
  actions: {
    confirm: () => {
      return;
    },
    cancel: () => {
      return;
    },
  },
} as DataModalConfirm;

export const rtnConfirmMessage = (inner: string) => {
  switch (inner) {
    case "systemtype":
      return "Warning, the system will create the elements:<br/>- New Major Software Build";
    case "designcomponent":
      return "Warning, the system will create the elements:<br/>- New Major Software Build<br/>- New System Type";
    default:
      return "Info, the system will create the elements:<br/>- New Major Software Build<br/>- New System Type<br/>- New Design Component";
  }
};

export interface QueryObjectGrid {
  sortBy?: string;
  isSortAscending?: boolean;
  page?: number;
  pageSize?: number;
  principalId?: number;
  deleted?: boolean;
  orphan?: boolean;
  lastModifiedBy?: Array<string>;
  dynamicReportId?: Array<number>;
  userId?: Array<number>;
  userName?: Array<string>;
  email?: Array<string>;
  lcmExportDescription?: string;
  isHistorical?: boolean;
  isDefault?: boolean;
  isEosDateEnable?: boolean;
  cnfClusterInfoId?: Array<number>;
  vnfClusterInfoId?: Array<number>;
  vnfNameDescritpion?: Array<string>;
}

export interface ResourceDtoBase {
  resource: object;
  result: ResultDto;
}

export interface IOverrideBehavior {
  operation: Function;
  overrideProperty?: string;
}

/**
 *
 * @export
 * @interface QueryObject
 */
/**
 *
 * @export
 * @interface QueryObject
 */
export interface QueryObject {
  /**
   *
   * @type {string}
   * @memberof QueryObject
   */
  sortBy?: string;
  /**
   *
   * @type {boolean}
   * @memberof QueryObject
   */
  isSortAscending?: boolean;
  /**
   *
   * @type {number}
   * @memberof QueryObject
   */
  page?: number;
  /**
   *
   * @type {number}
   * @memberof QueryObject
   */
  pageSize?: number;
  /**
   *
   * @type {DateFilter}
   * @memberof QueryObject
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {number}
   * @memberof QueryObject
   */
  principalId?: number;
  /**
   *
   * @type {boolean}
   * @memberof QueryObject
   */
  deleted?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof QueryObject
   */
  orphan?: boolean;
  /**
   *
   * @type {string}
   * @memberof QueryObject
   */
  lastModifiedBy?: Array<string>;
}

export interface FileResult {
  file: Blob;
  fileName: string;
}
export interface ReturnFile {
  File: Promise<Blob>;
  FileName: string;
}

/**
 *
 * @export
 * @interface RenderDetail
 */
export interface RenderDetail {
  /**
   *
   * @type {string}
   * @memberof RenderDetail
   */
  propertyName: string;
  /**
   *
   * @type {boolean}
   * @memberof RenderDetail
   */
  show: boolean;

  /**
   *
   * @type {boolean}
   * @memberof RenderDetail
   */
  archive: boolean;
  /**
   *
   * @type {boolean}
   * @memberof RenderDetail
   */
  ignore: boolean;
  /**
   *
   * @type {number}
   * @memberof RenderDetail
   */
  order: number;
  /**
   *
   * @type {GridFilterType}
   * @memberof RenderDetail
   */
  type: GridFilterType;
  /**
   *
   * @type {string}
   * @memberof RenderDetail
   */
  tab?: string;
  /**
   *
   * @type {string}
   * @memberof RenderDetail
   */
  colorHeader?: string;
  /**
   *
   * @type {string}
   * @memberof RenderDetail
   */
  updatedPropertyName?: string;
  /**
   *
   * @type {string}
   * @memberof RenderDetail
   */
  tableName?: string;
}
/**
 *
 * @export
 * @enum {string}
 */
export enum GridFilterType {
  CheckBoxFilter = <any>0,
  CheckBoxFilterNumber = <any>1,
  DateRangeFilter = <any>2,
}

export interface CustomGridRender {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfDesignComponentDtoGrid
   */
  className: string;
  /**
   *
   * @type { Array<RenderDetail>}
   * @memberof RenderDetail
   */
  render: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface GridDtoBase
 */
export interface GridDtoBase {
  /**
   *
   * @type {boolean}
   * @memberof GridDtoBase
   */
  deleted?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof GridDtoBase
   */
  orphan?: boolean;
  /**
   *
   * @type {Date}
   * @memberof GridDtoBase
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof GridDtoBase
   */
  lastModifiedBy?: string;
}

/**
 *
 * @export
 * @interface ResultDtoOfBoolean
 */
export interface ResultDtoOfBoolean {
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfBoolean
   */
  warning?: boolean;
  /**
   *
   * @type {string}
   * @memberof ResultDtoOfBoolean
   */
  info?: string;
  /**
   *
   * @type {boolean}
   * @memberof ResultDtoOfBoolean
   */
  data?: boolean;
}

export enum PlannedActivityTypeForEnum {
  LcmEngineering = 0,
  AddAsset = 1,
  EditAsset = 2,
  DesignAspect = 3,
  ServicePlan = 4,
}

export enum ReportViewMode {
  Aggregated = 1,
  Disaggregated = 2,
}

export enum ReportViewModeR10 {
  "Subnetwork Boundary" = 1,
  "Network Element - Level 1" = 2,
  "Network Element - Level 2" = 3,
  "Hardware" = 4,
  "Full" = 5,
}

export interface getResourceObject {
  opCoId?: string | null;
  designComponentId?: string | null;
  assetId?: string | null;
  elementName?: string | null;
}

export interface getLcmIdObject {
  opCo?: number[] | null;
  designComponentId?: number[] | null;
  deploymentStatusId?: number[] | null;
}

export interface getOpCoAssetsId {
  opCoId?: number | null;
  assetId?: number | null;
}
