import { ResultDto } from "./CommonModels";
import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  CustomGridRender,
  GridDtoBase,
  QueryObject,
  DateFilter,
} from "./Common";
import { PlannedActivityDtoUpdate } from "./PlannedActivity";
import { ReasonCheckboxDto } from "./LookUp/ReasonCheckbox";
import { TipologicaGridDtoRule } from "./LookUp/LookUpGenericModel";
/**
 *
 * @export
 * @interface LcmEngineeringQueryDto
 */
export interface LcmEngineeringQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  lcmEngineeringId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  designComponent?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  opCo?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  operationalContact?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  softwareSupportProvider?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof LcmEngineeringQueryDto
   */
  softwareEndOfWarrantyDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  softwareSupportType?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  numberOfNodes?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  numberOfNodesInLab?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  subDomainSpoc?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  eduspoc?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof LcmEngineeringQueryDto
   */
  softwareEndOfSupportContract?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof LcmEngineeringQueryDto
   */
  hardwareEndOfSupportContract?: DateFilter;
  /**
   *
   * @type {Array<boolean>}
   * @memberof LcmEngineeringQueryDto
   */
  warranty?: Array<boolean>;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringQueryDto
   */
  onSoftwareOrHardware?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringQueryDto
   */
  renewalInProgress?: boolean;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  plannedActivity?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  hardwareSupportProvider?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  hardwareSupportType?: Array<string>;
}
export interface GenericReportAssociated {
  /**
   *
   * @type {string}
   * @memberof GenericReportAssociated
   */
  tableName?: string;
  /**
   *
   * @type {boolean}
   * @memberof GenericReportAssociated
   */
  checked?: boolean;
  /**
   *
   * @type {string}
   * @memberof GenericReportAssociated
   */
  columnName?: string;
  /**
   *
   * @type {string}
   * @memberof GenericReportAssociated
   */
  updatedColumnName?: string;
}
/**
 *
 * @export
 * @interface NetworkElementAssociated
 */
export interface NetworkElementAssociated {
  /**
   *
   * @type {number}
   * @memberof NetworkElementAssociated
   */
  id?: number;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAssociated
   */
  elementName?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAssociated
   */
  enviroment?: string;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAssociated
   */
  location?: string;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAssociated
   */
  enviromentId?: number;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAssociated
   */
  locationId?: number;
  /**
   *
   * @type {string}
   * @memberof NetworkElementAssociated
   */
  assetsStatus?: string;
  /**
   *
   * @type {number}
   * @memberof NetworkElementAssociated
   */
  assetsStatusId?: number;
  /**
   *
   * @type {boolean}
   * @memberof NetworkElementAssociated
   */
  isFinalAsset?: boolean;
}

/**
 *
 * @export
 * @interface LcmEngineeringDto
 */
export interface LcmEngineeringDto extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDto
   */
  operationalContact?: string;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  elementCount?: boolean;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDto
   */
  hardwareSheetIndex?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDto
   */
  softwareSheetIndex?: string;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  warranty?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  onSoftware?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  onHardware?: boolean;
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDto
   */
  numberOfNodes?: number;
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDto
   */
  numberOfNodesInLab?: number;
  /**
   *
   * @type {Date}
   * @memberof LcmEngineeringDto
   */
  softwareEndOfWarrantyDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof LcmEngineeringDto
   */
  softwareEndOfSupportContract?: Date;
  /**
   *
   * @type {Date}
   * @memberof LcmEngineeringDto
   */
  hardwareEndOfSupportContract?: Date;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDto
   */
  softwareSupportProvider?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDto
   */
  softwareSupportType?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDto
   */
  hardwareSupportProvider?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDto
   */
  hardwareSupportType?: string;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  renewalInProgress?: boolean;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDto
   */
  buildBagDescription?: string;
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDto
   */
  buildBagId?: number;
  /**
   *
   * @type {Date}
   * @memberof LcmEngineeringDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface LcmEngineeringDtoCreate
 */
export interface LcmEngineeringDtoCreate extends LcmEngineeringDto {
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  transientDesignComponentResource?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoCreate
   */
  hardwareSupportType?: string;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDtoCreate
   */
  sparesProvisioned?: boolean;
  /**
   *
   * @type {Array<NetworkElementAssociated>}
   * @memberof LcmEngineeringDtoCreate
   */
  networkElementAssociateds?: Array<NetworkElementAssociated>;
  /**
   *
   * @type {Array<PlannedActivityDtoUpdate>}
   * @memberof LcmEngineeringDtoCreate
   */
  plannedActivityDto?: Array<PlannedActivityDtoUpdate>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringDtoCreate
   */
  subDomainSpocIds?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringDtoCreate
   */
  eduSpocIds?: Array<number>;

  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringDtoCreate
   */
  operationContractsIds?: Array<number>;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  subDomainSpocResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  operationalContractResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  eduSpocResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  lcmDeploymentStatusResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  designComponentResource?: { [key: string]: string };
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  designComponentFamilyResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoCreate
   */
  designComponentId?: number;
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoCreate
   */
  designComponentFamilyid?: number;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoCreate
   */
  designComponentFamilyName?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  productImportanceResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoCreate
   */
  buildBagId?: number;
  /**
   *
   * @type {Array<{ key: number; text: string }>}
   * @memberof LcmEngineeringDtoCreate
   */
  buildBagResources?: Array<{ key: number; text: string }>;
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoCreate
   */
  productImportanceId?: number;
  /**
   *
   * @type {{ [key: string]: TipologicaGridDtoRule; }}
   * @memberof LcmEngineeringDtoCreate
   */
  supportedResource?: { [key: string]: TipologicaGridDtoRule };
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoCreate
   */
  hardwareSupportedId?: number;
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoCreate
   */
  softwareSupportedId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  fullorPartialSupportResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoCreate
   */
  fullorPartialSupportHWResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoCreate
   */
  fullorPartialSupportId?: number;

  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoCreate
   */
  fullorPartialSupportHWId?: number;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringDtoCreate
   */
  checkboxResourceLcmEngineeringSoftwares?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringDtoCreate
   */
  checkboxResourceLcmEngineeringHardwares?: Array<number>;
  /**
   *
   * @type {{ [key: string]: ReasonCheckboxDto; }}
   * @memberof LcmEngineeringDtoCreate
   */
  checkboxResourceResource?: { [key: string]: ReasonCheckboxDto };

  isExtendedSupportOfferedByVendor?: boolean;
  hwIsExtendedSupportOfferedByVendor?: boolean;

  resourceKey?: string;
  previousResourceKey?: string;
  commentOnProjectStatus?: string;
  reasonForNoPlan?: string;
  /**
   *
   * @type {Array<boolean>}
   * @memberof LcmEngineeringDtoCreate
   */
  isReleaseDetailUnKnown?: boolean;
}
/**
 *
 * @export
 * @interface LcmEngineeringDtoGrid
 */
export interface LcmEngineeringDtoGrid extends LcmEngineeringDto {
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoGrid
   */
  lcmEngineeringId?: number;
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoGrid
   */
  designComponentFamilyId?: number;
  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoGrid
   */
  opCoId?: number;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  hardwareSupportedId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  softwareSupportedId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  fullorPartialSupportId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  checkboxResourceLcmEngineeringSoftwares?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  checkboxResourceLcmEngineeringHardwares?: string;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoGrid
   */
  plannedActivity?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof LcmEngineeringDtoGrid
   */
  originalLcm?: { [key: string]: string };
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  designComponent?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  opCo?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  productImportanceId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  subDomainSpoc?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  eduspoc?: string;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDtoGrid
   */
  isLcmAncillaryData?: boolean;
}
/**
 *
 * @export
 * @interface LcmEngineeringDtoUpdate
 */
export interface LcmEngineeringDtoUpdate extends LcmEngineeringDtoCreate {
  lcmEngineeringId?: number;

  /**
   *
   * @type {number}
   * @memberof LcmEngineeringDtoUpdate
   */
  lcmDeploymentStatusId?: number;
  /**
   *
   * @type {Array<NetworkElementAssociated>}
   * @memberof LcmEngineeringDtoUpdate
   */
  selectedNodes?: Array<NetworkElementAssociated>;
  /**
   *
   * @type {Array<NetworkElementAssociated>}
   * @memberof LcmEngineeringDtoUpdate
   */
  unselectedNodes?: Array<NetworkElementAssociated>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfLcmEngineeringDtoGrid
 */
export interface QueryResultDtoOfLcmEngineeringDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfLcmEngineeringDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<LcmEngineeringDtoGrid>}
   * @memberof QueryResultDtoOfLcmEngineeringDtoGrid
   */
  items?: Array<LcmEngineeringDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

//------------------------------>no auto gen

export interface LcmEngineringQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  lcmEngineeringId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  designComponent?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  productImportanceId?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  opCo?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  operationalContact?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  softwareSupportProvider?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof LcmEngineeringQueryDto
   */
  softwareEndOfWarrantyDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  softwareSupportType?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  numberOfNodes?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  numberOfNodesInLab?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  subDomainSpoc?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  isLcmAncillaryData?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  verticalName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  eduspoc?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof LcmEngineeringQueryDto
   */
  softwareEndOfSupportContract?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof LcmEngineeringQueryDto
   */
  hardwareEndOfSupportContract?: DateFilter;
  /**
   *
   * @type {Array<boolean>}
   * @memberof LcmEngineeringQueryDto
   */
  warranty?: Array<boolean>;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringQueryDto
   */
  onSoftwareOrHardware?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringQueryDto
   */
  renewalInProgress?: boolean;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringQueryDto
   */
  plannedActivity?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  hardwareSupportProvider?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringQueryDto
   */
  hardwareSupportType?: Array<string>;

  /**
   *
   * @type {Array<boolean>}
   * @memberof LcmEngineeringQueryDto
   */
  archived?: boolean;
}

export interface LcmEngineeringEdit {
  LcmEngineeringDtoEdit: LcmEngineeringDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface LcmEngineeringCreate {
  LcmEngineeringDtoCreate: LcmEngineeringDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}
export interface LcmEngineeringGrid {
  LcmEngineeringGridResult: QueryResultDtoOfLcmEngineeringDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_LCM_ENGINEERING = "GET_CREATE_LCM_ENGINEERING";
export const GET_EDIT_LCM_ENGINEERING = "GET_EDIT_LCM_ENGINEERING";
export const GET_GRID_LCM_ENGINEERING = "GET_GRID_LCM_ENGINEERING";
export const GET_FILTER_LCM_ENGINEERING = "GET_FILTER_LCM_ENGINEERING";
export const CREATE_LCM_ENGINEERING = "CREATE_LCM_ENGINEERING";
export const EDIT_LCM_ENGINEERING = "EDIT_LCM_ENGINEERING";
export const DELETE_LCM_ENGINEERING = "DELETE_LCM_ENGINEERING";
export const RESTORE_LCM_ENGINEERING = "RESTORE_LCM_ENGINEERING";
