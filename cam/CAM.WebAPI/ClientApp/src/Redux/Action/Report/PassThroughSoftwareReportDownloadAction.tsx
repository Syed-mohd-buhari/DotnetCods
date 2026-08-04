import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PassThroughSoftwareReportApi } from "../../../Business/Report/PassThroughSoftwareBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { ReportSoftwareQueryDto } from "../../../Model/Report/Export";
import { ReportNetworkLevel2QueryGrid } from "../../../Model/Report/LcmExportReport";
import { ReportHardwareQueryObjectGrid } from "../../../Model/Report/ReportHardwareModel";
import { ReportNetworkLevel2Grid } from "../../../Model/Report/ReportLcmExportModel";
import { ReportSoftwareQueryObjectGrid } from "../../../Model/Report/ReportSoftwareModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function DownloadPassThroughSoftwareReport(
  query: ReportSoftwareQueryDto
) {
  setLoader("ADD", "DownloadPassThroughSoftwareReport");
  let api = new PassThroughSoftwareReportApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.PassThroughSoftwareReportExportReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "DownloadPassThroughSoftwareReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadPassThroughSoftwareReport");
}
