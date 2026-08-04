import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { AssetAsisSdSwitchApi } from "../../../Business/Report/AssetAsisSdSwitchBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { AssetAsisSdSwitchQueryObjectGrid } from "../../../Model/Report/AssetAsisSdSwitchExport";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function DownloadAssetAsisSdSwitch(
  query: AssetAsisSdSwitchQueryObjectGrid
) {
  setLoader("ADD", "DownloadAssetAsisSdSwitch");
  let api = new AssetAsisSdSwitchApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.AssetAsisSdSwitchExportReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "DownloadAssetAsisSdSwitch");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadAssetAsisSdSwitch");
}
