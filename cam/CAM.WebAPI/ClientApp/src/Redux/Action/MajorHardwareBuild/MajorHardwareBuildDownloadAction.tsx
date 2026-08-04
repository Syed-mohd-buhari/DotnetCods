import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { MajorHardwareBuildApi } from "../../../Business/MajorHardwareBuildBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { MajorHardwareBuildQueryObjectGrid } from "../../../Model/MajorHardwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetMajorHardwareBuildReport(
  queryFilter?: MajorHardwareBuildQueryObjectGrid
) {
  setLoader("ADD", "GetMajorHardwareBuildReport");

  let api = new MajorHardwareBuildApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.majorHardwareBuildExportReport(queryFilter ?? {})
    );

    console.log("res => ", res);
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetMajorHardwareBuildReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    setLoader("REMOVE", "GetMajorHardwareBuildReport");
  }
}
