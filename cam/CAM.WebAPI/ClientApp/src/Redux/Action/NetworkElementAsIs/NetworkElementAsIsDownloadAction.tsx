import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsIsApi } from "../../../Business/NetworkElementAsIsBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { NetworkElementAsIsQueryObjectGrid } from "../../../Model/NetworkElementAsIs";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetNetworkElementAsIsReport(
  queryFilter?: NetworkElementAsIsQueryObjectGrid
) {
  setLoader("ADD", "GetNetworkElementAsIsReport");

  let api = new NetworkElementAsIsApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.networkElementAsIsExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetNetworkElementAsIsReport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetNetworkElementAsIsReport");
}

export async function GetNewNetworkElementAsIsReport(
  queryFilter?: NetworkElementAsIsQueryObjectGrid
) {
  setLoader("ADD", "GetNewNetworkElementAsIsReport");

  let api = new NetworkElementAsIsApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.newnetworkElementAsIsExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetNewNetworkElementAsIsReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetNewNetworkElementAsIsReport");
}
