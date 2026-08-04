/* eslint-disable @typescript-eslint/consistent-type-assertions */
import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { DateFilter, QueryObject, QueryObjectGrid, RenderDetail } from "../Common";
import { ResultDto } from "../CommonModels";


/**
 * 
 * @export
 * @interface SkipManager
 */
 export interface SkipManager {
    /**
     * 
     * @type {number}
     * @memberof SkipManager
     */
    page?: number;
    /**
     * 
     * @type {number}
     * @memberof SkipManager
     */
    skipAmount?: number;
}


/**
 * 
 * @export
 * @interface ViaExport
 */
 export interface ViaExport {
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meSourceAssetId?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meName?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meType?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    lcmProdName?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    vendor?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    eoslContractDate?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    eeoslContractDate?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    locationName?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meDescription?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meStatus?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meIpAddress?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meVirtualFlg?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    organisationName?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    osName?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    osEoslContractDate?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    osEeoslContractDate?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meCniBcServiceFlg?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meCriticality?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meDeploymentType?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meEnvironment?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meExternalConnectionFlg?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meFqdn?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meGdprRelevantFlg?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meInstallationDate?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meLastMajorUpgradeDate?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meLastScanDate?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meMacAddress?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meSerialNumber?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meServiceType?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meCyberarkIntegrationFlg?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meIdmIntegrationFlg?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meSecurityTier?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    me2FaIntegrationFlg?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    meSiemIntegrationFlg?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    supportContract?: string;
    /**
     * 
     * @type {string}
     * @memberof ViaExport
     */
    lcmStatus?: string;
}

/**
 * 
 * @export
 * @interface ViaExportQuery
 */
 export interface ViaExportQuery extends QueryObject {
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meSourceAssetId?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meName?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meType?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    lcmProdName?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    vendor?: Array<string>;
    /**
     * 
     * @type {DateFilter}
     * @memberof ViaExportQuery
     */
    eoslContractDate?: DateFilter;
    /**
     * 
     * @type {DateFilter}
     * @memberof ViaExportQuery
     */
    eeoslContractDate?: DateFilter;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    locationName?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meDescription?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meStatus?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meIpAddress?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meVirtualFlg?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    organisationName?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    osName?: Array<string>;
    /**
     * 
     * @type {DateFilter}
     * @memberof ViaExportQuery
     */
    osEoslContractDate?: DateFilter;
    /**
     * 
     * @type {DateFilter}
     * @memberof ViaExportQuery
     */
    osEeoslContractDate?: DateFilter;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meCniBcServiceFlg?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meCriticality?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meDeploymentType?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meEnvironment?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meExternalConnectionFlg?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meFqdn?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meGdprRelevantFlg?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meInstallationDate?: Array<string>;
    /**
     * 
     * @type {DateFilter}
     * @memberof ViaExportQuery
     */
    meLastMajorUpgradeDate?: DateFilter;
    /**
     * 
     * @type {DateFilter}
     * @memberof ViaExportQuery
     */
    meLastScanDate?: DateFilter;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meMacAddress?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meSerialNumber?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meServiceType?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meCyberarkIntegrationFlg?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meIdmIntegrationFlg?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meSecurityTier?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    me2FaIntegrationFlg?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    meSiemIntegrationFlg?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    supportContract?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof ViaExportQuery
     */
    lcmStatus?: Array<string>;
    /**
     * 
     * @type {number}
     * @memberof ViaExportQuery
     */
    skipAmount?: number;
    /**
     * 
     * @type {Array<SkipManager>}
     * @memberof ViaExportQuery
     */
    skipManager?: Array<SkipManager>;
}

/**
 * 
 * @export
 * @interface CustomGridRenderOfViaExport
 */
 export interface CustomGridRenderOfViaExport {
    /**
     * 
     * @type {string}
     * @memberof CustomGridRenderOfViaExport
     */
    className?: string;
    /**
     * 
     * @type {Array<RenderDetail>}
     * @memberof CustomGridRenderOfViaExport
     */
    render?: Array<RenderDetail>;
}

/**
 * 
 * @export
 * @interface QueryResultDtoOfViaExport
 */
 export interface QueryResultDtoOfViaExport {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoOfViaExport
     */
    totalItems?: number;
    /**
     * 
     * @type {Array<ViaExport>}
     * @memberof QueryResultDtoOfViaExport
     */
    items?: Array<ViaExport>;
    /**
     * 
     * @type {CustomGridRenderOfViaExport}
     * @memberof QueryResultDtoOfViaExport
     */
    gridRender?: CustomGridRenderOfViaExport;
}

/**
 * 
 * @export
 * @interface QueryResultDtoVai
 */
 export interface QueryResultDtoVai extends QueryResultDtoOfViaExport {
    /**
     * 
     * @type {number}
     * @memberof QueryResultDtoVai
     */
    skipAmount?: number;
    /**
     * 
     * @type {Array<SkipManager>}
     * @memberof QueryResultDtoVai
     */
    skipManager?: Array<SkipManager>;
}

export interface ViaExportHardwareGrid {
	ViaExportHardwareGridResult: QueryResultDtoVai | null;
	filter: FilterValueDto[] | null;
}
export interface ViaExportSoftwareGrid {
	ViaExportSoftwareGridResult: QueryResultDtoOfViaExport | null;
	filter: FilterValueDto[] | null;
}

export const GET_GRID_VIA_EXPORT_HARDWARE = "GET_GRID_VIA_EXPORT_HARDWARE";
export const GET_FILTER_VIA_EXPORT_HARDWARE = "GET_FILTER_VIA_EXPORT_HARDWARE";

export const GET_GRID_VIA_EXPORT_SOFTWARE = "GET_GRID_VIA_EXPORT_SOFTWARE";
export const GET_FILTER_VIA_EXPORT_SOFTWARE = "GET_FILTER_VIA_EXPORT_SOFTWARE";