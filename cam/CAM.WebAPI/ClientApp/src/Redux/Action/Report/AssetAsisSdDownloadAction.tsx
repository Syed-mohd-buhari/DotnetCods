import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { AssetAsisSdApi } from "../../../Business/Report/AssetAsisSdBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { AssetAsisSdHardwareQueryDto } from "../../../Model/Report/AssetAsisSdExport";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function DownloadAssetAsisSd(query: AssetAsisSdHardwareQueryDto) {
  setLoader("ADD", "DownloadAssetAsisSd");
  let api = new AssetAsisSdApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.AssetAsisSdExportReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "DownloadAssetAsisSd");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadAssetAsisSd");
}
