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

/**
 *
 * @export
 * @interface CbomDisaggregatedView
 */
export interface CbomDisaggregatedView {
  /**
   *
   * @type {string}
   * @memberof CbomDisaggregatedView
   */
  cnfName: string;
  /**
   *
   * @type {string}
   * @memberof CbomDisaggregatedView
   */
  podTypeName: string;
  /**
   *
   * @type {number}
   * @memberof CbomDisaggregatedView
   */
  memRequestForPodType: number;
  /**
   *
   * @type {number}
   * @memberof CbomDisaggregatedView
   */
  nonPersistentStoragePerPodType: number;
  /**
   *
   * @type {number}
   * @memberof CbomDisaggregatedView
   */
  numberOfCnfInstancesPerSite: number;
  /**
   *
   * @type {number}
   * @memberof CbomDisaggregatedView
   */
  numberOfPodsPerPodType: number;
}

/**
 *
 * @export
 * @interface CBOMReportDtoGrid
 */
export interface CBOMReportDtoGrid extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof CBOMReportDtoGrid
   */
  cnfName: string;
  /**
   *
   * @type {string}
   * @memberof CBOMReportDtoGrid
   */
  podTypeName: string;
  /**
   *
   * @type {number}
   * @memberof CBOMReportDtoGrid
   */
  numberOfPodsPerPodType: number;
  /**
   *
   * @type {number}
   * @memberof CBOMReportDtoGrid
   */
  vcpuRequestForPodType: number;
  /**
   *
   * @type {number}
   * @memberof CBOMReportDtoGrid
   */
  memRequestForPodType: number;
  /**
   *
   * @type {number}
   * @memberof CBOMReportDtoGrid
   */
  numberOfCnfInstancesPerSite: number;
  /**
   *
   * @type {number}
   * @memberof CBOMReportDtoGrid
   */
  nonPersistentStoragePerPodType: number;
  /**
   *
   * @type {string}
   * @memberof CBOMReportDtoGrid
   */
  hardwareType: string;
  /**
   *
   * @type {Array<CbomDisaggregatedView>}
   * @memberof CBOMReportDtoGrid
   */
  cbomDisaggregatedView: CbomDisaggregatedView[];
}
/**
 *
 * @export
 * @interface QueryResultDtoOfCBOMReportDtoGrid
 */
export interface QueryResultDtoOfCBOMReportDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfCBOMReportDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<CBOMReportDtoGrid>}
   * @memberof QueryResultDtoOfCBOMReportDtoGrid
   */
  items?: Array<CBOMReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface CBOMReportGrid {
  CBOMReportGridResult: any | null;
  filter: FilterValueDto[] | null;
}
export const GET_GRID_CBOM_REPORT = "GET_GRID_CBOM_REPORT";
