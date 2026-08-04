import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignAspectApi } from "../../../Business/DesignAspectsBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { DesignAspectQueryObjectGrid } from "../../../Model/DesignAspects";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetDesignAspectReport(
  queryFilter?: DesignAspectQueryObjectGrid
) {
  let api = new DesignAspectApi();
  let res: ReturnFile | undefined;
  setLoader("ADD", "GetDesignApectReport");
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.designAspectExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetDesignApectReport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetDesignApectReport");
}
