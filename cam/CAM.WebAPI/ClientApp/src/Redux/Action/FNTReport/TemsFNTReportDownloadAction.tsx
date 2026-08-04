import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { FNTReportApi } from "../../../Business/FNTReportBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { FNTReportQueryObjectGrid } from "../../../Model/FNTReport";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTemsFNTReportExport(
  queryFilter?: FNTReportQueryObjectGrid
) {
  setLoader("ADD", "GetTemsFNTReportExport");

  let res: ReturnFile | undefined;
  let api = new FNTReportApi();

  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.FNTReportExportReport(queryFilter ?? {}, "excel")
    );

    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);

    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });

    setLoader("REMOVE", "GetTemsFNTReportExport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }

  setLoader("REMOVE", "GetTemsFNTReportExport");
}
