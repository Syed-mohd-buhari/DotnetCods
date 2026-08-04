import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  GridDtoBase,
  RenderDetail,
  QueryObject,
  DateFilter,
  CustomGridRender,
  QueryObjectGrid,
} from "./Common";
import { ResultDto } from "./CommonModels";

export interface BPTReportDtoGrid extends GridDtoBase {
  budgetProjectTackerId?: number;
  budgetLineCode?: string;
  uploadStatus?: string;
  uploadMode?: string;
  currentTrackingNumber?: string;
  newTrackingNumber?: string;
  wbs?: string;
  opcoId?: number;
  opco?: string;
  domain?: string;
  team?: string;
  teamId?: number;
  budgetOwner?: string;
  budgetOwnerid?: number;
  program?: string;
  budgetProject?: string;
  activity?: string;
  priority?: string;
  driver?: string;
  benefits?: string;
  risks?: string;
  category?: string;
  categoryId?: number;
  nwelement?: string;
  virtualizedNwElement?: string;
  vendor?: string;
  lcmCategoriesId?: number;
  lcmCategories?: string;
  ohpLev1?: string;
  ohpLev2?: string;
  hfmLev1?: string;
  hfmLev2?: string;
  fy?: string;
  operational?: string;
  transfers?: string;
  cost1sTest?: string;
  validation?: string;
  signOff?: string;
  sub?: string;
  finalReSub?: string;
  latestScenario?: string;
  currency?: string;
  adjustments?: string;
  ytdActuals?: string;
  plannedAbsorption?: string;
  deviation?: string;
  apr?: string;
  may?: string;
  jun?: string;
  jul?: string;
  aug?: string;
  sep?: string;
  oct?: string;
  nov?: string;
  dec?: string;
  jan?: string;
  feb?: string;
  mar?: string;
  approvedBudget?: string;
  commitment?: string;
  budgetProjectDependency?: string;
  localProgram?: string;
  localBudgetProject?: string;
  localDriver?: string;
  localPrioritization?: string;
  ppmId?: string;
  ppmBudgetProjectId?: string;
  costCentre?: string;
  wpId?: string;
  groupBudgetOpcoId?: string;
  internalProgram?: string;
  verticalProject?: string;
  domainSpecificLabels?: string;
  labelsMarketVsVertical?: string;
  otherMinorVendors?: string;
  externalDemandBudget?: string;
  opexImpact?: string;
  legalEntity?: string;
  yearlyTransfersTrack?: string;
  yearlyAdjustmentsTrack?: string;
  mcustom02?: string;
  mcustom03?: string;
  mcustom04?: string;
  mcustom05?: string;
  mcustom06?: string;
  mcustom07?: string;
  mcustom08?: string;
  mcustom09?: string;
  mcustom10?: string;
  vcustom01?: string;
  vcustom02?: string;
  vcustom03?: string;
  vcustom04?: string;
  vcustom05?: string;
  dcustom01?: string;
  dcustom02?: string;
  dcustom03?: string;
  dcustom04?: string;
  dcustom05?: string;
  lsdb?: string;
  ls012?: string;
  ls210?: string;
  ls57?: string;
  archive?: boolean;
  plannedActivityId?: number;
}

export interface QueryResultDtoOfBPTReportDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfBPTReportDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<BPTReportDtoGrid>}
   * @memberof QueryResultDtoOfBPTReportDtoGrid
   */
  items?: Array<BPTReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface BPTReportQueryObjectGrid extends QueryObject {}

export interface BPTReportGrid {
  BPTReportGridResult: QueryResultDtoOfBPTReportDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_GRID_BPT_REPORT = "GET_GRID__BPT_REPORT";
export const GET_FILTER_BPT_REPORT = "GET_FILTER_BPT_REPORT";
