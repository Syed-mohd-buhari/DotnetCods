import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PlannedActivityApi } from "../../../Business/PlannedActivityBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { PlannedActivityQueryObjectGrid } from "../../../Model/PlannedActivity";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
export async function GetPlannedActivityDownload(
  queryFilter?: PlannedActivityQueryObjectGrid
) {
  setLoader("ADD", "GetPlannedActivityDownload");

  let res: ReturnFile | undefined;
  let api = new PlannedActivityApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.plannedActivityExportReport(queryFilter ?? {})
    );

    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetPlannedActivityDownload");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPlannedActivityDownload");
}

export async function GetPlannedActivityArchivedDownload(
  queryFilter?: PlannedActivityQueryObjectGrid
) {
  setLoader("ADD", "GetPlannedActivityArchivedDownload");

  let res: ReturnFile | undefined;
  let api = new PlannedActivityApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.plannedActivityArchivedExportReport(queryFilter ?? {})
    );

    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetPlannedActivityArchivedDownload");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPlannedActivityArchivedDownload");
}
