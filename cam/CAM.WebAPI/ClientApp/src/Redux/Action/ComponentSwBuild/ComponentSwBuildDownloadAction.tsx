import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ComponentSwBuildApi } from "../../../Business/ComponentSwBuildsBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { ComponentSwBuildQueryObjectGrid } from "../../../Model/ComponentSwBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetComponentSwBuildDownload(
  queryFilter?: ComponentSwBuildQueryObjectGrid
) {
  setLoader("ADD", "GetComponentSwBuildDownload");

  let res: ReturnFile | undefined;
  let api = new ComponentSwBuildApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.componentSwBuildExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetComponentSwBuildDownload");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    setLoader("REMOVE", "GetComponentSwBuildDownload");
  }
}
