import { BPTReportApi } from "../../../Business/BPTReportBuisnes";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { BPTReportQueryObjectGrid } from "../../../Model/BPTReport";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetBPTReport(queryFilter?: BPTReportQueryObjectGrid) {
  setLoader("ADD", "GetBPTReport");

  let api = new BPTReportApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.BPTReportExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetBPTReport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetBPTReport");
}
