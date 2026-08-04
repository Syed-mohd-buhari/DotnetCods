import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { IdentityAsIsApi } from "../../../Business/IdentityAsIs";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { IdentityAsIsQueryObjectGrid } from "../../../Model/LookUp/Identities";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetIdentityAsIsReport(
  queryFilter?: IdentityAsIsQueryObjectGrid
) {
  setLoader("ADD", "GetIdentityAsIsReport");

  let api = new IdentityAsIsApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.IdentityAsIsExportReport(queryFilter ?? {})
    );

    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetIdentityAsIsReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    setLoader("REMOVE", "GetIdentityAsIsReport");
  }
}
