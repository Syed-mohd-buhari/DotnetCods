import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";
import { TipologicaQueryDtoForVirtualized } from "./LookUpGenericModel";
import { TipologicaGridDtoForVirtualized } from "./PlannedActivityResource";
import { SupportedServiceDto } from "./SupportedService";

/**
 *
 * @export
 * @interface SubNetworkBoundaryGridDto
 */

export interface SubNetworkBoundaryGridDto extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof SubNetworkBoundaryGridDto
   */
  subNetworkBoundaryId?: number;
  /**
   *
   * @type {string}
   * @memberof SubNetworkBoundaryGridDto
   */
  alias?: string;

  /**
   *
   * @type {Array<number>}
   * @memberof SubNetworkBoundaryGridDto
   */
  supportedServicesIDs?: Array<number>;
  /**
   *
   * @type {string}
   * @memberof SubNetworkBoundaryGridDto
   */
  subNetworkBoundaryDescription?: string;
  /**
   *
   * @type {boolean}
   * @memberof SubNetworkBoundaryGridDto
   */
  default?: boolean;
  /**
   *
   * @type {Date}
   * @memberof SubNetworkBoundaryGridDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof SubNetworkBoundaryGridDto
   */
  lastModifiedBy?: string;

  supportedServices?: Array<SupportedServiceDto>;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SubNetworkBoundaryGridDto
   */
  swApplicationTypes?: { [key: string]: string };

  /**
   *
   * @type {string}
   * @memberof SubNetworkBoundaryGridDto
   */
  swApplicationName?: string;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SubNetworkBoundaryGridDto
   */
  systemFunctionsResource?: { [key: string]: string };

  /**
   *
   * @type {Array<number>}
   * @memberof SubNetworkBoundaryGridDto
   */
  systemFunctionsIds?: Array<number>;

  /**
   *
   * @type {nummber}
   * @memberof SubNetworkBoundaryGridDto
   */
  gdprClassification?: number;

  /**
   *
   * @type {string}
   * @memberof SubNetworkBoundaryGridDto
   */
  gdprClassificationValue?: string;

  /**
   *
   * @type {boolean}
   * @memberof SubNetworkBoundaryGridDto
   */
  missionCritical?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof SubNetworkBoundaryGridDto
   */
  c3C4?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof SubNetworkBoundaryGridDto
   */
  pcisox?: boolean;
  /**
   *
   * @type {boolean}
   * @memberof SubNetworkBoundaryGridDto
   */
  gdprRelevant?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof DesignComponentFamilyDto
   */
  internetFacing?: boolean;

  /**
   *
   * @type {boolean}
   * @memberof SubNetworkBoundaryGridDto
   */
  lcmPolicy?: number;

  /**
   *
   * @type {string}
   * @memberof SubNetworkBoundaryGridDto
   */
  criticality?: string;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SubNetworkBoundaryGridDto
   */
  customerWheelResource?: { [key: string]: string };

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SubNetworkBoundaryGridDto
   */
  productNamesResource?: any;

  /**
   *
   * @type {number}
   * @memberof SubNetworkBoundaryGridDto
   */
  productNameId?: number;

  /**
   *
   * @type {Array<number>}
   * @memberof SubNetworkBoundaryGridDto
   */
  customerWheelsIds?: Array<number>;

  /**
   *
   * @type {boolean}
   * @memberof SubNetworkBoundaryGridDto
   */
  securityElement?: boolean;

  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof SubNetworkBoundaryGridDto
   */
  vodafoneNAmesResource?: { [key: string]: string };

  /**
   *
   * @type {number}
   * @memberof SubNetworkBoundaryGridDto
   */
  vodafoneName?: string;
  vodafoneNameId?: number;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfSubNetworkBoundaryGridDto
 */
export interface CustomGridRenderOfSubNetworkBoundaryGridDto {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfSubNetworkBoundaryGridDto
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfSubNetworkBoundaryGridDto
   */
  render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfSubNetworkBoundaryGridDto
 */
export interface QueryResultDtoOfSubNetworkBoundaryGridDto {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfSubNetworkBoundaryGridDto
   */
  totalItems?: number;
  /**
   *
   * @type {Array<SubNetworkBoundaryGridDto>}
   * @memberof QueryResultDtoOfSubNetworkBoundaryGridDto
   */
  items?: Array<SubNetworkBoundaryGridDto>;
  /**
   *
   * @type {CustomGridRenderOfSubNetworkBoundaryGridDto}
   * @memberof QueryResultDtoOfSubNetworkBoundaryGridDto
   */
  gridRender?: CustomGridRenderOfSubNetworkBoundaryGridDto;
}

// not auto gen

export interface SubNetworkBoundaryQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof SubNetworkBoundaryQueryDto
   */
  subNetworkBoundaryId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof SubNetworkBoundaryQueryDto
   */
  subNetworkBoundaryDescription?: Array<string>;

  /**
   *
   * @type {Array<string>}
   * @memberof SubNetworkBoundaryQueryDto
   */
  swApplicationName?: Array<string>;

  /**
   *
   * @type {Array<string>}
   * @memberof SubNetworkBoundaryQueryDto
   */
  vodafoneName?: Array<number>;

  alias?: Array<string>;
  allSupportedServices?: Array<string>;
  AllSupportedServices?: Array<number>;
  gdprRelevant?: Array<number>;
  internetFacing?: Array<boolean>;
  lcmPolicy?: Array<number>;
  criticality?: Array<string>;
  gdrpClassificationValue?: Array<number>;
  pcisox?: Array<boolean>;
  c3C4?: Array<boolean>;
  missionCritical?: Array<boolean>;
  criticalAssetType?: Array<number>;
  systemFunction?: Array<number>;
  customerWheel?: Array<number>;
  productName?: Array<number>;
  securityElement?: Array<boolean>;

  /**
   *
   * @type {Array<number>}
   * @memberof SubNetworkBoundaryQueryDto
   */
  lastModifiedValueStartDate?: Date;
  /**
   *
   * @type {Array<number>}
   * @memberof SubNetworkBoundaryQueryDto
   */
  lastModifiedValueEndDate?: Date;
}

export interface LookUpEditSubNetworkBoundary {
  LookUpDtoEdit: SubNetworkBoundaryGridDto | null;
  ResultDtoEdit: ResultDto | null;
}

export interface LookUpCreateSubNetworkBoundary {
  LookUpDtoCreate: SubNetworkBoundaryGridDto | null;
  ResultDtoCreate: ResultDto | null;
}

export interface LookUpSubNetworkBoundaryGrid {
  LookUpGridResult: QueryResultDtoOfSubNetworkBoundaryGridDto | null;
  LookUpGridResultAll: QueryResultDtoOfSubNetworkBoundaryGridDto | null;
  filter: FilterValueDto[] | null;
}
