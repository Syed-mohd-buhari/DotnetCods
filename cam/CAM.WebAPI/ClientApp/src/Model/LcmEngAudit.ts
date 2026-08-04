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
 * @interface LcmEngAuditQueryDto
 */
export interface LcmEngAuditQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringDtoGrid
   */
  lcmAncillaryDataId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  originalHwLcmId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  originalSwLcmId?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof LcmEngineeringDtoGrid
   */
  lcmEngineeringId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  opCo?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  designComponent?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  productCode?: Array<string>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof LcmEngineeringDto
   */
  handedOverToOperation?: Array<boolean>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  contractRenewalPlan?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  reasonForNoPlan?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  commentOnProjectStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  scopeOfSimplification?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  dataSource?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  incidentClass?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  occurenceProbability?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  newopsRiskEvaluation?: Array<string>;
  // /**
  //  *
  //  * @type {Array<string>}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // securityRiskPotential?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  securityRiskEffective?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  vulnerabilityRating?: Array<string>;

  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  securityMitigation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  securityRiskOverall?: Array<string>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof LcmEngineeringDto
   */
  includedInSecurityScanning?: Array<boolean>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  raId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  cyberRiskRequestId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  lastPenTestReferenceNumber?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  lastScanRefNumber?: Array<string>;
  riskComment?: Array<string>;
  qId?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  requestId?: Array<string>;
  /**
   *
   * @type {Date}
   * @memberof LcmEngineeringDto
   */
  lastScanDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof LcmEngineeringDto
   */
  lastUpgradeDate?: Date;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  lastPenTestDate?: Date;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  assetOutOfScope?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  regulatoryFields?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  infrastructureLocation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  eomControl?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  engUpdateTracker?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  opsUpdateTracker?: Array<string>;
  // /**
  //  *
  //  * @type {Array<string>}
  //  * @memberof LcmEngineeringDtoGrid
  //*/
  // custom2?: Array<string>;
  // /**
  //  *
  //  * @type {Array<string>}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // kpiStatusService?: Array<string>;
  // /**
  //  *
  //  * @type {Array<string>}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // custom?: Array<string>;
  // /**
  //  *
  //  * @type {Array<string>}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // custom1?: Array<string>;
  // /**
  //  *
  //  * @type {Array<string>}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  //idNew?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  exNetworks?: Array<string>;
  // /**
  //  *
  //  * @type {Array<string>}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // productImportanceHistory2?: Array<string>;
  // /**
  //  *
  //  * @type {Array<string>}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // cloudVersion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  labSwRelease?: Array<string>;
  // /**
  //  *
  //  * @type {Array<string>}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // certifiedSWRealeseForNfviBundle?: Array<string>;
  // /**
  //  *
  //  * @type {Array<string>}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // lcmStatus?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof LcmEngineeringDtoGrid
   */
  originalLCMID?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof GenericReportDtoGrid
   */
  creationUser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof GenericReportDtoGrid
   */
  creationDate?: DateFilter;
  /**
   *
   * @type {string}
   * @memberof GenericReportDtoGrid
   */
  modificationuser?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof GenericReportDtoGrid
   */
  modificationDate?: DateFilter;
  /**
   *
   * @type {boolean}
   * @memberof GenericReportDtoGrid
   */
  warranty?: Array<boolean>;
}

/**
 *
 * @export
 * @interface LcmEngAuditDto
 */
export interface LcmEngAuditDto extends GridDtoBase {
  // /**
  //  *
  //  * @type {Date}
  //  * @memberof LcmEngAuditDto
  //  */
  // lastModified?: Date;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngAuditDto
  //  */
  // lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface LcmEngAuditDtoGrid
 */
export interface LcmEngAuditDtoGrid extends LcmEngAuditDto {
  /**
   *
   * @type {number | null}
   * @memberof LcmEngineeringDtoGrid
   */
  lcmAncillaryDataId?: number | null;
  /**
   *
   * @type {number | null}
   * @memberof LcmEngineeringDtoGrid
   */
  lcmEngineeringId?: number | null;
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
  designComponent?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  productCode?: string;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  handedOverToOperation?: boolean;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  contractRenewalPlan?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  originalHwLcmId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  originalSwLcmId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  reasonForNoPlan?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  commentOnProjectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  scopeOfSimplification?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  dataSource?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  incidentClass?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  occurenceProbability?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  newopsRiskEvaluation?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // securityRiskPotential?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  securityRiskEffective?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  vulnerabilityRating?: string;

  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  securityMitigation?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  securityRiskOverall?: string;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  includedInSecurityScanning?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  exposedEdgeFlag?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  externalFacingFlag?: boolean;

  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  raId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  cyberRiskRequestId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  lastPenTestReferenceNumber?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  lastScanRefNumber?: string;
  qId?: string;
  riskComment?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  requestId?: string;
  /**
   *
   * @type {Date | null}
   * @memberof LcmEngineeringDto
   */
  lastScanDate?: Date | null;
  /**
   *
   * @type {Date | null}
   * @memberof LcmEngineeringDto
   */
  lastUpgradeDate?: Date | null;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  lastPenTestDate?: Date | null;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  assetOutOfScope?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  regulatoryFields?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  infrastructureLocation?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  eomControl?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  engUpdateTracker?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  opsUpdateTracker?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // custom2?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // kpiStatusService?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // custom?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // custom1?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  //idNew?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  exNetworks?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // productImportanceHistory2?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // cloudVersion?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  labSwRelease?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // certifiedSWRealeseForNfviBundle?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  lcmStatusJune2021?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  originalLCMID?: string;
}
/**
 *
 * @export
 * @interface LcmEngAuditDtoCreate
 */
export interface LcmEngAuditDtoCreate extends LcmEngAuditDto {
  /**
   *
   * @type {number | null}
   * @memberof LcmEngAuditDtoUpdate
   */
  lcmEngineeringId?: number | null;
  /**
   *
   * @type {number | null}
   * @memberof LcmEngineeringDtoGrid
   */
  lcmAncillaryDataId?: number | null;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  productCode?: string;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  handedOverToOperation?: boolean;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  contractRenewalPlan?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  reasonForNoPlan?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  commentOnProjectStatus?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  scopeOfSimplification?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  dataSource?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  incidentClass?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  occurenceProbability?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // securityRiskPotential?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  securityRiskEffective?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  vulnerabilityRating?: string;

  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  securityMitigation?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  securityRiskOverall?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  assetOutOfScope?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  regulatoryFields?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  infrastructureLocation?: string;

  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  includedInSecurityScanning?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  exposedEdgeFlag?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof LcmEngineeringDto
   */
  externalFacingFlag?: boolean;

  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  raId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  cyberRiskRequestId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  lastPenTestReferenceNumber?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  lastScanRefNumber?: string;
  qId?: string;
  riskComment?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  requestId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  originalHwLcmId?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  originalSwLcmId?: string;
  /**
   *
   * @type {Date | null | string}
   * @memberof LcmEngineeringDto
   */
  lastScanDate?: Date | null | string;
  /**
   *
   * @type {Date | null | string}
   * @memberof LcmEngineeringDto
   */
  lastUpgradeDate?: Date | null | string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  lastPenTestDate?: Date | null | string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  eomControl?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  engUpdateTracker?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  opsUpdateTracker?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // custom2?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // kpiStatusService?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // custom?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // custom1?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  //idNew?: string;
  /**
   *
   * @type {string}
   * @memberof LcmEngineeringDtoGrid
   */
  exNetworks?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // productImportanceHistory2?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // cloudVersion?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // certifiedSWRealeseForNfviBundle?: string;
  // /**
  //  *
  //  * @type {string}
  //  * @memberof LcmEngineeringDtoGrid
  //  */
  // lcmStatus?: string;
}
/**
 *
 * @export
 * @interface LcmEngAuditDtoUpdate
 */
export interface LcmEngAuditDtoUpdate extends LcmEngAuditDtoCreate {}
/**
 *
 * @export
 * @interface QueryResultDtoOfLcmEngAuditDtoGrid
 */
export interface QueryResultDtoOfLcmEngAuditDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfLcmEngAuditDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<LcmEngAuditDtoGrid>}
   * @memberof QueryResultDtoOfLcmEngAuditDtoGrid
   */
  items?: Array<LcmEngAuditDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface LcmEngAuditEdit {
  LcmEngAuditDtoEdit: LcmEngAuditDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface LcmEngAuditCreate {
  LcmEngAuditDtoCreate: LcmEngAuditDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface LcmEngAuditGrid {
  LcmEngAuditGridResult: QueryResultDtoOfLcmEngAuditDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_LCM_ENG_AUDIT = "GET_CREATE_LCM_ENG_AUDIT";
export const GET_EDIT_LCM_ENG_AUDIT = "GET_EDIT_LCM_ENG_AUDIT";
export const GET_GRID_LCM_ENG_AUDIT = "GET_GRID_LCM_ENG_AUDIT";
export const GET_FILTER_LCM_ENG_AUDIT = "GET_FILTER_LCM_ENG_AUDIT";
export const CREATE_LCM_ENG_AUDIT = "CREATE_LCM_ENG_AUDIT";
export const EDIT_LCM_ENG_AUDIT = "EDIT_LCM_ENG_AUDIT";
export const DELETE_LCM_ENG_AUDIT = "DELETE_LCM_ENG_AUDIT";
export const RESTORE_LCM_ENG_AUDIT = "RESTORE_LCM_ENG_AUDIT";
