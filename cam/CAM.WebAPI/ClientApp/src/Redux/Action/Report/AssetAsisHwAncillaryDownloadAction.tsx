import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { AssetAsisHwAncillaryApi } from "../../../Business/Report/AssetAsisHwAncillaryBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { ReportSoftwareQueryDto } from "../../../Model/Report/Export";
import { AssetAsisHwAncillaryHardwareQueryDto } from "../../../Model/Report/AssetAsisHwAncillaryExport";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function DownloadAssetAsisHwAncillary(
  query: AssetAsisHwAncillaryHardwareQueryDto
) {
  setLoader("ADD", "DownloadAssetAsisHwAncillary");
  let api = new AssetAsisHwAncillaryApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.AssetAsisHwAncillaryExportReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "DownloadAssetAsisHwAncillary");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadAssetAsisHwAncillary");
}
