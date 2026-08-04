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
 * @interface VbomDisaggregatedView
 */
export interface VbomDisaggregatedView {
  /**
   *
   * @type {string}
   * @memberof VbomDisaggregatedView
   */
  vnfName: string;
  /**
   *
   * @type {string}
   * @memberof VbomDisaggregatedView
   */
  vmTypeName: string;
  /**
   *
   * @type {number}
   * @memberof VbomDisaggregatedView
   */
  vCpu: number;
  /**
   *
   * @type {number}
   * @memberof VbomDisaggregatedView
   */
  ram: number;
  /**
   *
   * @type {number}
   * @memberof VbomDisaggregatedView
   */
  dataDisk: number;
}

/**
 *
 * @export
 * @interface VBOMReportDtoGrid
 */
export interface VBOMReportDtoGrid extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof VBOMReportDtoGrid
   */
  vnfName: string;
  /**
   *
   * @type {string}
   * @memberof VBOMReportDtoGrid
   */
  vmTypeName: string;
  /**
   *
   * @type {number}
   * @memberof VBOMReportDtoGrid
   */
  numberOfVmsPerType: number;
  /**
   *
   * @type {number}
   * @memberof VBOMReportDtoGrid
   */
  numberOfvCpuPerType: number;
  /**
   *
   * @type {number}
   * @memberof VBOMReportDtoGrid
   */
  ramPerGb: number;
  /**
   *
   * @type {number}
   * @memberof VBOMReportDtoGrid
   */
  storagePerVmDataDisk: number;
  /**
   *
   * @type {string}
   * @memberof VBOMReportDtoGrid
   */
  hardwareType: string;
  /**
   *
   * @type {Array<VbomDisaggregatedView>}
   * @memberof VBOMReportDtoGrid
   */
  vbomDisaggregatedViews: VbomDisaggregatedView[];
}
/**
 *
 * @export
 * @interface QueryResultDtoOfVBOMReportDtoGrid
 */
export interface QueryResultDtoOfVBOMReportDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfVBOMReportDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<VBOMReportDtoGrid>}
   * @memberof QueryResultDtoOfVBOMReportDtoGrid
   */
  items?: Array<VBOMReportDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface VBOMReportGrid {
  VBOMReportGridResult: any | null;
  filter: FilterValueDto[] | null;
}
export const GET_GRID_VBOM_REPORT = "GET_GRID_VBOM_REPORT";
