
import { ViaExportQuery } from "./ViaExport";



/**
 * 
 * @export
 * @interface ReportViaQueryAllDto
 */
 export interface ReportViaQueryAllDto {
    /**
     * 
     * @type {ViaExportQuery}
     * @memberof ReportViaQueryAllDto
     */
    queryHardware?: ViaExportQuery;
    /**
     * 
     * @type {ViaExportQuery}
     * @memberof ReportViaQueryAllDto
     */
    querySoftware?: ViaExportQuery;
}

export interface ExportDownload {
	file: Blob | null;
}

export const DOWNLOAD_VIA_EXPORT = "DOWNLOAD_VIA_EXPORT";