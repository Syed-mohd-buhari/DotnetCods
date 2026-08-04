import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DCFLifeCycleApi } from "../../../Business/DCFLifeCycleBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { DCFLifeCycleQueryObjectGrid } from "../../../Model/DCFLifeCycle";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetDCFLifeCycleReport(
  queryFilter?: DCFLifeCycleQueryObjectGrid
) {
  setLoader("ADD", "GetDCFLifeCycleReport");

  let api = new DCFLifeCycleApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.dcfLifeCycleExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetDCFLifeCycleReport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetDCFLifeCycleReport");
}
