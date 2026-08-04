import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { BuildBagApi } from "../../../Business/BuildBagsBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { BuildBagQueryObjectGrid } from "../../../Model/BuildBag";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetBuildBagDownload(
  queryFilter?: BuildBagQueryObjectGrid
) {
  setLoader("ADD", "GetBuildBagDownload");

  let res: ReturnFile | undefined;
  let api = new BuildBagApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.buildBagExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetBuildBagDownload");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    setLoader("REMOVE", "GetBuildBagDownload");
  }
}
