import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { GridDtoBase, RenderDetail, QueryObject, DateFilter } from "./Common";
import { ResultDto } from "./CommonModels";

/**
 *
 * @export
 * @interface GlossaryDto
 */
export interface GlossaryDto extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  elementDeploymentName?: string;

  linkedToNetworkAsIs: boolean;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  softwareProductNumber?: string;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  elementManager?: string;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  patchDetails?: string;
  /**
   *
   * @type {Date}
   * @memberof GlossaryDto
   */
  softwareProductionDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof GlossaryDto
   */
  softwareInstallDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof GlossaryDto
   */
  hardwareInstallDate?: Date;
  /**
   *
   * @type {Date}
   * @memberof GlossaryDto
   */
  dataAcquisitionDate?: Date;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  dataAcquisitionMethod?: string;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  hardwareAcquisition?: string;
  /**
   *
   * @type {boolean}
   * @memberof GlossaryDto
   */
  manualOverride?: boolean;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  nodeType?: string;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  elementManagerExportFileFormat?: string;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  networkFunction?: string;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  hardwareSolution?: string;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  platform?: string;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  hardwareType?: string;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  softwareReleaseInformation?: string;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  otherHardwareInfo?: string;
  /**
   *
   * @type {Date}
   * @memberof GlossaryDto
   */
  lastModified?: Date;
  /**
   *
   * @type {string}
   * @memberof GlossaryDto
   */
  lastModifiedBy?: string;
}
/**
 *
 * @export
 * @interface glossaryDtoCreate
 */
export interface glossaryDtoCreate extends GlossaryDto {
  /**
   *
   * @type {number}
   * @memberof glossaryDtoCreate
   */
  originalEquipmentManufacturerId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof glossaryDtoCreate
   */
  originalEquipmentManufacturerResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof glossaryDtoCreate
   */
  networkElementAsPlannedId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof glossaryDtoCreate
   */
  networkElementAsPlannedResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof glossaryDtoCreate
   */
  locationId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof glossaryDtoCreate
   */
  locationResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof glossaryDtoCreate
   */
  opCoId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof glossaryDtoCreate
   */
  opCoResource?: { [key: string]: string };
  /**
   *
   * @type {number}
   * @memberof glossaryDtoCreate
   */
  systemTypeId?: number;
  /**
   *
   * @type {{ [key: string]: string; }}
   * @memberof glossaryDtoCreate
   */
  systemTypeResource?: { [key: string]: string };
}
/**
 *
 * @export
 * @interface glossaryDtoGrid
 */
export interface glossaryDtoGrid extends GlossaryDto {
  /**
   *
   * @type {number}
   * @memberof glossaryDtoGrid
   */
  GlossaryId?: number;
  /**
   *
   * @type {string}
   * @memberof glossaryDtoGrid
   */
  opCo?: string;
  /**
   *
   * @type {string}
   * @memberof glossaryDtoGrid
   */
  systemType?: string;
  /**
   *
   * @type {string}
   * @memberof glossaryDtoGrid
   */
  location?: string;
}
/**
 *
 * @export
 * @interface glossaryDtoUpdate
 */
export interface glossaryDtoUpdate extends glossaryDtoCreate {
  /**
   *
   * @type {number}
   * @memberof glossaryDtoUpdate
   */
  GlossaryId?: number;
}
/**
 *
 * @export
 * @interface QueryResultDtoOfGlossaryDtoGrid
 */
export interface QueryResultDtoOfGlossaryDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfGlossaryDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<NetworkElementAsPlannedDtoGrid>}
   * @memberof QueryResultDtoOfGlossaryDtoGrid
   */
  items?: Array<glossaryDtoGrid>;
  /**
   *
   * @type {CustomGridRenderOfGlossaryDtoGrid}
   * @memberof QueryResultDtoOfGlossaryDtoGrid
   */
  gridRender?: CustomGridRenderOfGlossaryDtoGrid;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfGlossaryDtoGrid
 */
export interface CustomGridRenderOfGlossaryDtoGrid {
  /**
   *
   * @type {string}
   * @memberof CustomGridRenderOfGlossaryDtoGrid
   */
  className?: string;
  /**
   *
   * @type {Array<RenderDetail>}
   * @memberof CustomGridRenderOfGlossaryDtoGrid
   */
  render?: Array<RenderDetail>;
}

// -------------------------No auto Generated---------------------------------

export interface GlossaryQueryObjectGrid extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  GlossaryId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  opCo?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  oem?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  networkFunction?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  nodeType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  elementDeploymentName?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  location?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  systemTypeId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  softwareReleaseInformationSystemLevel?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  softwareProductNumberSystemLevel?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof NetworkElementsAsIsQueryDto
   */
  softwareProductionDate?: DateFilter;
  /**
   *
   * @type {DateFilter}
   * @memberof NetworkElementsAsIsQueryDto
   */
  softwareInstallDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  hardwareSolution?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  platform?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  hardwareType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  otherHardwareInfo?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  hardwareAcquisition?: Array<string>;
  /**
   *
   * @type {Array<boolean>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  manualOverride?: Array<boolean>;
  /**
   *
   * @type {Array<number>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  hardwareSystemId?: Array<number>;
  /**
   *
   * @type {DateFilter}
   * @memberof NetworkElementsAsIsQueryDto
   */
  dataAcquisitionDate?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  dataAcquisitionMethod?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  elementManager?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  elementManagerExportFileFormat?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof NetworkElementsAsIsQueryDto
   */
  spareFieldsJson?: Array<string>;
}

/**
 *
 * @param {Array<number>} [GlossaryId]
 * @param {Array<number>} [opCo]
 * @param {Array<number>} [oem]
 * @param {Array<string>} [networkFunction]
 * @param {Array<string>} [nodeType]
 * @param {Array<string>} [elementDeploymentName]
 * @param {Array<number>} [location]
 * @param {Array<number>} [systemTypeId]
 * @param {Array<string>} [softwareReleaseInformationSystemLevel]
 * @param {Array<string>} [softwareProductNumberSystemLevel]
 * @param {Date} [softwareProductionDateStartDate]
 * @param {Date} [softwareProductionDateEndDate]
 * @param {Date} [softwareInstallDateStartDate]
 * @param {Date} [softwareInstallDateEndDate]
 * @param {Array<string>} [hardwareSolution]
 * @param {Array<string>} [platform]
 * @param {Array<string>} [hardwareType]
 * @param {Array<string>} [otherHardwareInfo]
 * @param {Array<string>} [hardwareAcquisition]
 * @param {Array<boolean>} [manualOverrideHardwareSolution]
 * @param {Array<number>} [hardwareSystemId]
 * @param {Date} [dataAcquisitionDateStartDate]
 * @param {Date} [dataAcquisitionDateEndDate]
 * @param {Array<string>} [dataAcquisitionMethod]
 * @param {Array<string>} [elementManager]
 * @param {Array<string>} [elementManagerExportFileFormat]
 * @param {Array<string>} [spareFieldsJson]
 * @param {string} [sortBy]
 * @param {boolean} [isSortAscending]
 * @param {number} [page]
 * @param {number} [pageSize]
 * @param {Date} [lastModifiedStartDate]
 * @param {Date} [lastModifiedEndDate]
 * @param {number} [principalId]
 * @param {boolean} [deleted]
 * @param {boolean} [orphan]
 * @param {Array<string>} [lastModifiedBy]
 * @param {*} [options] Override http request option.
 * @throws {RequiredError}
 */

export interface GlossaryEdit {
  GlossaryDtoEdit: glossaryDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface GlossaryCreate {
  glossaryDtoCreate: glossaryDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface GlossaryGrid {
  GlossaryGridResult: QueryResultDtoOfGlossaryDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface NewGlossaryGrid {
  NewGlossaryGridResult: QueryResultDtoOfGlossaryDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface GlossarySystemTypeInfo {
  hardwareSolution?: string;
  platform?: string;
  hardwareType?: string;
  otherHardwareInfo?: string;
  softwareReleaseInformation?: string;
  networkFunction?: string;
}
