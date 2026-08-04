import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { CBOMApi } from "../../../Business/CBOMBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { CBOMQueryObjectGrid } from "../../../Model/CBOM";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetCBOMReport(queryFilter?: CBOMQueryObjectGrid) {
  setLoader("ADD", "GetCBOMReport");

  let res: ReturnFile | undefined;
  let api = new CBOMApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.CBOMExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetCBOMReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCBOMReport");
}
