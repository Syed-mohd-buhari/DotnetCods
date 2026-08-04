import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PassThroughHardwareReportApi } from "../../../Business/Report/PassThroughHardwareBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { PassThroughHardwareReportQueryObjectGrid } from "../../../Model/Report/PassThroughHardwareReportExport";
import { ReportNetworkLevel2QueryGrid } from "../../../Model/Report/LcmExportReport";
import { ReportHardwareQueryObjectGrid } from "../../../Model/Report/ReportHardwareModel";
import { ReportNetworkLevel2Grid } from "../../../Model/Report/ReportLcmExportModel";
import { ReportSoftwareQueryObjectGrid } from "../../../Model/Report/ReportSoftwareModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function DownloadPassThroughHardwareReport(
  query: PassThroughHardwareReportQueryObjectGrid
) {
  setLoader("ADD", "DownloadPassThroughHardwareReport");
  let api = new PassThroughHardwareReportApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.PassThroughHardwareReportExportReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "DownloadPassThroughHardwareReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadPassThroughHardwareReport");
}
