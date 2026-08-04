import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VodafoneNameApi } from "../../../../Business/LookUp/VodafoneNameBusiness";
import { FileResult, ReturnFile } from "../../../../Model/Common";
import { TipologicheQueryObjectGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetVodafoneNameDownload(
  queryFilter?: TipologicheQueryObjectGrid
) {
  setLoader("ADD", "GetVodafoneNameDownload");

  let res: ReturnFile | undefined;
  let api = new VodafoneNameApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.vodafoneNameExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetVodafoneNameDownload");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    setLoader("REMOVE", "GetVodafoneNameDownload");
  }
}
