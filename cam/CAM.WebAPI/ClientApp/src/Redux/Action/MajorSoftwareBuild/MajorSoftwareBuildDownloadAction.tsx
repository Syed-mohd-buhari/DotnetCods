import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { MajorSoftwareBuildApi } from "../../../Business/MajorSoftwareBuildsBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { MajorSoftwareBuildQueryObjectGrid } from "../../../Model/MajorSoftwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetMajorSoftwareBuildDownload(
  queryFilter?: MajorSoftwareBuildQueryObjectGrid
) {
  setLoader("ADD", "GetMajorSoftwareBuildDownload");

  let res: ReturnFile | undefined;
  let api = new MajorSoftwareBuildApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.majorSoftwareBuildExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetMajorSoftwareBuildDownload");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    setLoader("REMOVE", "GetMajorSoftwareBuildDownload");
  }
}
