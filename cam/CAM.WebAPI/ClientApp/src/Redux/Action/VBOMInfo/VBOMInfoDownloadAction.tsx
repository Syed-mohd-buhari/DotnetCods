import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { VBOMInfoApi } from "../../../Business/VBOMInfoBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { VBOMInfoQueryObjectGrid } from "../../../Model/VBOMInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetVBOMInfoReport(queryFilter?: VBOMInfoQueryObjectGrid) {
  setLoader("ADD", "GetVBOMInfoReport");

  let res: ReturnFile | undefined;
  let api = new VBOMInfoApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.VBOMInfoExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetVBOMInfoReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVBOMInfoReport");
}
