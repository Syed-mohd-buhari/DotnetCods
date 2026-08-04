import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NFVISwCompatibleApi } from "../../../Business/NFVISoftwareCompatibleBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { NFVISwCompatibleQueryObjectGrid } from "../../../Model/NFVISoftwareCompatible";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetNFVISwCompatibleReport(
  queryFilter?: NFVISwCompatibleQueryObjectGrid
) {
  setLoader("ADD", "GetNFVISwCompatibleReport");

  let res: ReturnFile | undefined;
  let api = new NFVISwCompatibleApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.NFVISwCompatibleExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetNFVISwCompatibleReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetNFVISwCompatibleReport");
}
