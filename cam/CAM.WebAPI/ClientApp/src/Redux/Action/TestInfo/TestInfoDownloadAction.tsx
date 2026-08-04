import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { TestInfoApi } from "../../../Business/TestInfoBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { TestInfoQueryObjectGrid } from "../../../Model/TestInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTestInfoReport(queryFilter?: TestInfoQueryObjectGrid) {
  setLoader("ADD", "GetTestInfoReport");

  let res: ReturnFile | undefined;
  let api = new TestInfoApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.testInfoExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetTestInfoReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetTestInfoReport");
}
