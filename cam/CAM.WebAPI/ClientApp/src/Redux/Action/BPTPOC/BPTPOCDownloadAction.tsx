import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { AuditApi } from "../../../Business/BPTPOCBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { AuditQueryObjectGrid } from "../../../Model/Audit";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetBPTPOCExport(flag?: any) {
  setLoader("ADD", "GetBPTPOCExport");
  console.log("flag", flag);
  let api = new AuditApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.BPTPOCExport(flag ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf("=") + 1;
    var lastIndex = fileNameBase?.indexOf(".", index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetBPTPOCExport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetBPTPOCExport");
}
