import { ClusterInfoApi } from "../../../Business/ClusterInfoBusiness";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ClusterInfoQueryObjectGrid } from "../../../Model/ClusterInfo";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetClusterInfo(queryFilter?: ClusterInfoQueryObjectGrid) {
  setLoader("ADD", "GetClusterInfo");

  let api = new ClusterInfoApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.ClusterInfoExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetClusterInfo");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetClusterInfo");
}
