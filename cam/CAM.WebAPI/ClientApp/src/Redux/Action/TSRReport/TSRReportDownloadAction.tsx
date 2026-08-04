import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { TSRReportApi } from "../../../Business/TSRReportBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { TSRReportQueryObjectGrid } from "../../../Model/TSRReport";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTSRReport(
  queryFilter?: TSRReportQueryObjectGrid,
  fileType: "excel" | "csv" = "excel"
) {
  setLoader("ADD", "GetTSRReport");

  let res: ReturnFile | undefined;
  let api = new TSRReportApi();

  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.TSRReportExportReport(queryFilter ?? {}, fileType)
    );

    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);

    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });

    setLoader("REMOVE", "GetTSRReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }

  setLoader("REMOVE", "GetTSRReport");
}

export async function GetNonTemsTSRReport(
  queryFilter?: TSRReportQueryObjectGrid,
  fileType: "excel" | "csv" = "excel"
) {
  setLoader("ADD", "GetNonTemsTSRReport");

  let res: ReturnFile | undefined;
  let api = new TSRReportApi();

  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.TSRNonTemsReportExportReport(queryFilter ?? {}, fileType)
    );

    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);

    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });

    setLoader("REMOVE", "GetNonTemsTSRReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }

  setLoader("REMOVE", "GetNonTemsTSRReport");
}
